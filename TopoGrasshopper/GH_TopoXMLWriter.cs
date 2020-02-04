using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using TopoRhino;

namespace TopoGrasshopper
{
    public class GH_TopoXMLWriter : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GH_TopoXMLWriter()
          : base("TopoXMLWriter", "TopoXMLWriter",
            "Write to a XML Filename.",
            "TopoCreator", "IO")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("TopoPtr", "TopoPtr", "TopoPtr", GH_ParamAccess.item);
            pManager.AddTextParameter("XMLFile", "xml", "XML Filename", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Trigger", "Trigger", "Trigger", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            String xml_file = "";
            IntPtr topoData = IntPtr.Zero;
            bool runProgram = false;
            if (!DA.GetData(0, ref topoData)) return;
            if (!DA.GetData(1, ref xml_file)) return;
            if (!DA.GetData(2, ref runProgram)) return;

            if (runProgram)
            {
                TopoCreator.writeXML(topoData, xml_file);
            }
        }

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
        public override Guid ComponentGuid
        {
            get { return new Guid("e9b478c6-f4bd-49f1-abf4-7995521d5806"); }
        }
    }
}
