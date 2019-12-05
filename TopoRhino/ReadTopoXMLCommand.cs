using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;

namespace TopoRhino
{
    [CommandStyle(Style.ScriptRunner)]
    public class ReadTopoXMLCommand : Command
  {
        private String xmlFile { set; get; }

        /// <returns>
        /// The command name as it appears on the Rhino command line.
        /// </returns>
        public override string EnglishName
        {
            get { return "ReadTopoXML"; }
        }


        private int getLayerOrCreate(Rhino.RhinoDoc doc, string name)
        {
            int layer_index = 0;
            Rhino.DocObjects.Layer layer = doc.Layers.FindName(name);
            Guid current_layer_guid = doc.Layers.CurrentLayer.Id;
            if (layer == null)
            {
                Rhino.DocObjects.Layer newlayer = new Rhino.DocObjects.Layer();
                newlayer.ParentLayerId = current_layer_guid;
                newlayer.Name = name;
                layer_index = doc.Layers.Add(newlayer);
            }
            else
            {
                layer_index = layer.Index;
            }
            return layer_index;
        }

        /// <summary>
        /// Called by Rhino to "run" your command.
        /// </summary>
        protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
        {
            doc.Objects.Clear();

            Rhino.UI.OpenFileDialog fd = new Rhino.UI.OpenFileDialog { Filter = "XML File (*.xml)|*.xml" };

            if (!fd.ShowOpenDialog())
            {
                return Result.Cancel;
            }


            //Read TopoCreator
            String xmlfile = fd.FileName;
            IntPtr topoData = TopoCreator.readXML(xmlfile);
            int n_part = TopoCreator.partNumber(topoData);

            //Part Layer
            int partlayer_index = getLayerOrCreate(doc, "Part");

            //Boundary Layer
            int boundarylayer_index = getLayerOrCreate(doc, "Boundary");

            for (int partID = 0; partID < n_part; partID++)
            {
                Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
                TopoCreator.getPartMesh(partID, mesh, topoData);

                Guid mesh_guid = doc.Objects.AddMesh(mesh);

                //error: exit
                if (mesh_guid == Guid.Empty)
                {
                    TopoCreator.deleteStructure(topoData);
                    return Result.Failure;
                }

                Rhino.DocObjects.RhinoObject obj = doc.Objects.Find(mesh_guid);
                int mat_index;
                if (TopoCreator.isBoundary(partID, topoData) == 1)
                {
                    mat_index = doc.Materials.Find("PlasterBoundary", false);
                    obj.Attributes.LayerIndex = boundarylayer_index;
                }
                else
                {
                    mat_index = doc.Materials.Find("PlasterPart", false);
                    obj.Attributes.LayerIndex = partlayer_index;
                }

                if (mat_index != -1)
                {
                    obj.Attributes.MaterialIndex = mat_index;
                    obj.Attributes.MaterialSource = Rhino.DocObjects.ObjectMaterialSource.MaterialFromObject;
                }
                obj.CommitChanges();
            }

            //CrossMesh

            int crossMeshlayer_index = getLayerOrCreate(doc, "CrossMesh");

            List<Rhino.Geometry.Polyline> polylines = new List<Rhino.Geometry.Polyline>();
            IntPtr crossMesh = TopoCreator.initCrossMeshPtr(topoData);
            TopoCreator.getPolyLines(polylines, crossMesh);
            TopoCreator.deletePolyLineRhino(crossMesh);
            for (int id = 0; id < polylines.Count; id++)
            {
                Guid polyline_guid = doc.Objects.AddPolyline(polylines[id]);

                Rhino.DocObjects.RhinoObject obj = doc.Objects.Find(polyline_guid);
                obj.Attributes.LayerIndex = crossMeshlayer_index;
                obj.CommitChanges();
            }
                

            //Rhino.RhinoApp.RunScript("_SelMesh", false);
            //Rhino.RhinoApp.RunScript("_MeshToNURB", false);
            //Rhino.RhinoApp.RunScript("_SelMesh", false);
            //Rhino.RhinoApp.RunScript("_Delete", false);
            //Rhino.RhinoApp.RunScript("_SelPolysrf", false);
            //Rhino.RhinoApp.RunScript("_MergeAllFaces", false);

            doc.Objects.UnselectAll();
            TopoCreator.deleteStructure(topoData);
            return Result.Success;
        }
  }
}
