using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using TopoRhino;

namespace TopoGrasshopper
{
    public class GH_ModifyCrossMesh : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GH_ModifyCrossMesh()
          : base("ModifyCrossMesh", "ModifyCrossMesh",
            "Modify the cross mesh of the TopoCreator",
            "TopoCreator", "Utility")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddGenericParameter("a pointer of TopoCreator", "TopoPtr", "a pointer of TopoCreator", GH_ParamAccess.item);

            pManager.AddGenericParameter("Crosses", "Parts", "a list of rhino polylines which are not boundary", GH_ParamAccess.list);

            pManager.AddGenericParameter("Crosses", "Boundaries", "a list of rhino polylines which at at boundary of the structure", GH_ParamAccess.list);

            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("a pointer of TopoCreator", "TopoPtr", "a pointer of TopoCreator", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            IntPtr topoData = IntPtr.Zero;
            List<Curve> parts = new List<Curve>();
            List<Curve> boundaries = new List<Curve>();

            if (!DA.GetData(0, ref topoData)) return;
            if (!DA.GetDataList(1, parts)) return;
            DA.GetDataList(2, boundaries);

            CPolyLines polylines = new CPolyLines();
            List<float> points = new List<float>();
            List<int> sta_ends = new List<int>();
            List<int> atBoundary = new List<int>();

            //Add Parts
            int count = 0;
            foreach(var curve in parts)
            {
                int sta = count;
                Polyline polyline;
                curve.TryGetPolyline(out polyline);

                for (int id = 0; id < polyline.Count - 1; id++)
                {
                    Point3d pt = polyline[id];
                    //flip YZ because the coordianate systems of rhino and C++ are different
                    points.Add((float)pt.X);
                    points.Add((float)pt.Z);
                    points.Add(-(float)pt.Y);
                    count++;
                }
                int end = count - 1;
                sta_ends.Add(sta);
                sta_ends.Add(end);
                atBoundary.Add(0);
            }


            //Add Boundries
            foreach (var curve in boundaries)
            {
                int sta = count;
                Polyline polyline;
                curve.TryGetPolyline(out polyline);

                for (int id = 0; id < polyline.Count - 1; id++)
                {
                    Point3d pt = polyline[id];
                    //flip YZ because the coordianate systems of rhino and C++ are different
                    points.Add((float)pt.X);
                    points.Add((float)pt.Z);
                    points.Add(-(float)pt.Y);
                    count++;
                }
                int end = count - 1;
                sta_ends.Add(sta);
                sta_ends.Add(end);
                atBoundary.Add(1);
            }

            polylines.n_polyline = sta_ends.Count / 2;
            polylines.n_points = points.Count / 3;

            polylines.points = new float[points.Count];
            polylines.sta_ends = new int[sta_ends.Count];

            for (int id = 0; id < points.Count; id++)
                polylines.points[id] = points[id];

            for (int id = 0; id < sta_ends.Count; id++)
                polylines.sta_ends[id] = sta_ends[id];


            if (boundaries.Count == 0)
            {
                TopoCreator.setCrossMesh(ref polylines, topoData, false);
            }
            else
            {
                polylines.atBoundary = new int[atBoundary.Count];
                for (int id = 0; id < atBoundary.Count; id++)
                    polylines.atBoundary[id] = atBoundary[id];

                TopoCreator.setCrossMesh(ref polylines, topoData, true);
            }

            DA.SetData(0, topoData);
            return;
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
            get { return new Guid("a56c72d8-0e89-42d7-b04c-d6776d8a9204"); }
        }
    }
}
