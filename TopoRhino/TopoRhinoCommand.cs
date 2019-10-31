using System;
using Rhino;
using Rhino.Commands;

namespace TopoRhino
{
    [CommandStyle(Style.ScriptRunner)]
    public class TopoRhinoCommand : Command
  {
        private String xmlFile { set; get; }

        /// <returns>
        /// The command name as it appears on the Rhino command line.
        /// </returns>
        public override string EnglishName
        {
            get { return "TopoRhino"; }
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
            TopoCreator.readXML(xmlfile);
            int n_part = TopoCreator.partNumber();

<<<<<<< HEAD
            //double tile_angle = 0;
            //Rhino.Input.RhinoGet.GetNumber("tiltAngle", false, ref tile_angle, 0, 90);
            //TopoCreator.setParaDouble("tiltAngle", tile_angle);
            //TopoCreator.refresh();

            int n_part = TopoCreator.partNumber();

            //Part Layer
            Rhino.DocObjects.Layer childlayer = new Rhino.DocObjects.Layer();
            childlayer.ParentLayerId = current_layer_guid;
            childlayer.Name = String.Format("Part");
            int childpart_index = doc.Layers.Add(childlayer);

            //Part Layer
            childlayer = new Rhino.DocObjects.Layer();
            childlayer.ParentLayerId = current_layer_guid;
            childlayer.Name = String.Format("Boundary");
            int childboundary_index = doc.Layers.Add(childlayer);
=======
            //Tilt Angle
            {
                //double tile_angle = 0;
                //Rhino.Input.RhinoGet.GetNumber("tiltAngle", false, ref tile_angle, 0, 90);
                //TopoCreator.setParaDouble("tiltAngle", tile_angle);
                //TopoCreator.refresh();
            }


            //Layer
            Guid current_layer_guid = doc.Layers.CurrentLayer.Id;
            Rhino.DocObjects.Layer childlayer = new Rhino.DocObjects.Layer();
            childlayer.ParentLayerId = current_layer_guid;
            childlayer.Name = "Topo NURBS";
            int childindex = doc.Layers.Add(childlayer);
            doc.Layers.SetCurrentLayerIndex(childindex, true);
>>>>>>> af77a0f5dcd769cfe33c124fa21e2101e0728112


            for (int partID = 0; partID < n_part; partID++)
            {
                Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
                TopoCreator.getMesh(partID, mesh);

                Guid mesh_guid = doc.Objects.AddMesh(mesh);

                //error: exit
                if (mesh_guid == Guid.Empty)
                {
                    TopoCreator.deleteStructure();
                    return Result.Failure;
                }

                Rhino.DocObjects.RhinoObject obj = doc.Objects.Find(mesh_guid);
                int mat_index;
                if (TopoCreator.isBoundary(partID))
                {
                    mat_index = doc.Materials.Find("PlasterBoundary", false);
                    obj.Attributes.LayerIndex = childboundary_index;
                    //RhinoApp.WriteLine("{0}, {1}", partID, "PlasterBoundary");
                }
                else
                {
                    mat_index = doc.Materials.Find("PlasterPart", false);
                    obj.Attributes.LayerIndex = childpart_index;
                    //RhinoApp.WriteLine("{0}, {1}", partID, "PlasterPart");
                }

                if (mat_index != -1)
                {
                    obj.Attributes.MaterialIndex = mat_index;
                    obj.Attributes.MaterialSource = Rhino.DocObjects.ObjectMaterialSource.MaterialFromObject;
                    obj.CommitChanges();
                }
            }

            Rhino.RhinoApp.RunScript("_SelMesh", false);
            Rhino.RhinoApp.RunScript("_MeshToNURB", false);
            Rhino.RhinoApp.RunScript("_SelMesh", false);
            Rhino.RhinoApp.RunScript("_Delete", false);
            Rhino.RhinoApp.RunScript("_SelPolysrf", false);
            Rhino.RhinoApp.RunScript("_MergeAllFaces", false);

            doc.Objects.UnselectAll();
            TopoCreator.deleteStructure();
            return Result.Success;
        }
  }
}
