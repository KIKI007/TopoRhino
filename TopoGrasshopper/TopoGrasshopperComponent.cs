using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using TopoRhino;

namespace TopoGrasshopper
{
    public class TopoGrasshopperComponent : GH_Component
    {
        //
        private IntPtr topoData;
        private String xmlPath;


        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public TopoGrasshopperComponent()
          : base("TopoGrasshopperComponent", "TopoCreator",
            "Construct an Topological Interlocking by XML Filename.",
            "Surface", "Freeform")
        {
            xmlPath = "";
        }

        ~TopoGrasshopperComponent()
        {
            if (topoData != IntPtr.Zero)
            {
          
                TopoCreator.deleteStructure(topoData);
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Use the pManager object to register your input parameters.
            // You can often supply default values when creating parameters.
            // All parameters must have the correct access type. If you want 
            // to import lists or trees of values, modify the ParamAccess flag.
            pManager.AddTextParameter("XMLFile", "xml", "XML Filename", GH_ParamAccess.item);

            pManager.AddNumberParameter("tilt Angle", "tilt", "tilt angle for the model", GH_ParamAccess.item);

            pManager.AddBooleanParameter("Preview", "preview", "preview mode", GH_ParamAccess.item, true);

            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            pManager.AddNumberParameter("Number of Part", "N", "Number of Parts", GH_ParamAccess.item);

            pManager.AddMeshParameter("Meshes", "M", "Topological Interlocking Parts", GH_ParamAccess.list);


            //pManager.AddGenericParameter("TopoLock", "Topo", "TopoLocker Class", GH_ParamAccess.item);

            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            String xml_file = "";
            Double tilt_angle = 0;
            Boolean preview_mode = true;
            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref xml_file)) return;
            if (!DA.GetData(1, ref tilt_angle)) return;
            if (!DA.GetData(2, ref preview_mode)) return;
            // We should now validate the data and warn the user if invalid data is supplied.
            if (Path.GetExtension(xml_file.ToString()) != ".xml")
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input File must be xml file");
                return;
            }

            if (tilt_angle < 0 || tilt_angle > 90)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid Tilt Angle");
                return;
            }

            if(xmlPath != xml_file || xmlPath == "")
            {
                if(topoData != IntPtr.Zero)
                {
                    TopoCreator.deleteStructure(topoData);
                }
                topoData = TopoCreator.readXML(xml_file);
                xmlPath = xml_file;
            }
                

            int n_part = TopoCreator.partNumber(topoData);
            TopoCreator.setParaDouble("tiltAngle", tilt_angle, topoData);

            if (preview_mode)
                TopoCreator.preview(topoData);
            else
                TopoCreator.refresh(topoData);

            DA.SetData(0, n_part);
            List<Mesh> meshes = new List<Mesh>();
            for(int partID = 0; partID < n_part; partID++)
            {
                Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
                TopoCreator.getMesh(partID, mesh, topoData);
                meshes.Add(mesh);  
            }

            DA.SetDataList(1, meshes);
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.primary;


        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("334B8253-FE16-46CA-8C71-0D1287E9D48B");
    }
}
