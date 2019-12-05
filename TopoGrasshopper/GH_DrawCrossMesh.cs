using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using TopoRhino;
namespace TopoGrasshopper
{
    public class GH_DrawCrossMesh : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GH_DrawCrossMesh()
          : base("DrawCrossMesh", "DrawCrossMesh",
            "Create the Geometry of the cross Mesh",
            "TopoCreator", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("TopoPtr", "TopoPtr", "Pointer of a TopoCreator", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PolyLines", "PolyLines", "CrossMesh composed of polylines", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IntPtr topoData = IntPtr.Zero;
            if (!DA.GetData(0, ref topoData)) return;

            List<Polyline> polylines = new List<Polyline>();
            IntPtr crossMesh = TopoCreator.initCrossMeshPtr(topoData);
            TopoCreator.getPolyLines(polylines, crossMesh);
            TopoCreator.deletePolyLineRhino(crossMesh);
            DA.SetDataList(0, polylines);
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
            get { return new Guid("e1cd869c-4894-4f7d-ac98-cb7015403627"); }
        }
    }
}
