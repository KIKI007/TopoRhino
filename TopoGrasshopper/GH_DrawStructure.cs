﻿using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using TopoRhino;

namespace TopoGrasshopper
{
    public class GH_DrawStructure : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GH_DrawStructure()
          : base("DrawStructure", "DrawStructure",
            "Create the mesh of the structure",
            "TopoCreator", "Geometry")
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("TopoPtr", "TopoPtr", "Pointer of a TopoCreator", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Preview", "preview", "preview mode", GH_ParamAccess.item, true);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Part", "Part", "Internal Parts", GH_ParamAccess.list);
            pManager.AddMeshParameter("Boundary", "Boundary", "Boundary Parts", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            IntPtr topoData = IntPtr.Zero;
            bool preview_mode = false;
            if (!DA.GetData(0, ref topoData)) return;
            if (!DA.GetData(1, ref preview_mode)) return;

            int n_part = TopoCreator.partNumber(topoData);
   
            if (preview_mode)
                TopoCreator.preview(topoData);
            else
                TopoCreator.refresh(topoData);

            List<Mesh> parts = new List<Mesh>();
            List<Mesh> boundary = new List<Mesh>();
            for (int partID = 0; partID < n_part; partID++)
            {
                Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
                TopoCreator.getPartMesh(partID, mesh, topoData);
                if(TopoCreator.isBoundary(partID, topoData) == 1){
                    boundary.Add(mesh);
                }
                else{
                    parts.Add(mesh);
                }
            }

            DA.SetDataList(0, parts);
            DA.SetDataList(1, boundary);
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
            get { return new Guid("271761eb-8da5-4f36-8f99-1d33318577fe"); }
        }
    }
}
