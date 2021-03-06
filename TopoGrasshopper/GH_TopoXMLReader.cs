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
    public class GH_TopoXMLReader : GH_Component
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
        public GH_TopoXMLReader()
          : base("TopoXMLReader", "TopoXMLReader",
            "Construct an Topological Interlocking by XML Filename.",
            "TopoCreator", "IO")
        {
            xmlPath = "";
        }

        ~GH_TopoXMLReader()
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

            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.

            pManager.AddGenericParameter("a pointer of TopoCreator", "TopoPtr", "a pointer of TopoCreator", GH_ParamAccess.item);

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

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetData(0, ref xml_file)) return;
            
            // We should now validate the data and warn the user if invalid data is supplied.
            if (Path.GetExtension(xml_file.ToString()) != ".xml")
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input File must be xml file");
                return;
            }

            //if (tilt_angle < 0 || tilt_angle > 90)
            //{
            //    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid Tilt Angle");
            //    return;
            //}

            if(xmlPath != xml_file || xmlPath == "")
            {
                if(topoData != IntPtr.Zero)
                {
                    TopoCreator.deleteStructure(topoData);
                }
                topoData = TopoCreator.readXML(xml_file);
                xmlPath = xml_file;
            }
                
            DA.SetData(0, topoData);
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
