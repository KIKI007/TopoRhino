using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using TopoRhino;
namespace TopoGrasshopper
{
    public class GH_ModifyPattern : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GH_ModifyPattern()
          : base("ModifyPattern", "ModifyPattern",
            "Modify the 2D pattern",
            "TopoCreator", "Utility")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("a pointer of TopoCreator", "TopoPtr", "a pointer of TopoCreator", GH_ParamAccess.item);
            pManager.AddGenericParameter("pattern line", "PolyLines", "a list of pattern line", GH_ParamAccess.list);
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
            List<Curve> pattern = new List<Curve>();

            if (!DA.GetData(0, ref topoData)) return;
            if (!DA.GetDataList(1, pattern)) return;

            CPolyLines polylines = new CPolyLines();
            List<float> points = new List<float>();
            List<int> sta_ends = new List<int>();

            //Add Parts
            int count = 0;
            foreach (var curve in pattern)
            {
                int sta = count;
                Polyline polyline;
                curve.TryGetPolyline(out polyline);

                for (int id = 0; id < polyline.Count - 1; id++)
                {
                    Point3d pt = polyline[id];
                    points.Add((float)pt.X);
                    points.Add((float)pt.Y);
                    points.Add((float)pt.Z);
                    count++;
                }
                int end = count - 1;
                sta_ends.Add(sta);
                sta_ends.Add(end); 
            }

            if(sta_ends.Count > Constants.MAXIMUM_POLYLINE_FACE ||
                points.Count > Constants.MAXIMUM_POLYLINE_POINTS)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Pattern size exceeds the software's capacity");
                return;
            }

            polylines.n_polyline = sta_ends.Count / 2;
            polylines.n_points = points.Count / 3;

            polylines.points = new float[Constants.MAXIMUM_POLYLINE_FACE];
            polylines.sta_ends = new int[Constants.MAXIMUM_POLYLINE_POINTS];

            for (int id = 0; id < points.Count; id++)
                polylines.points[id] = points[id];

            for (int id = 0; id < sta_ends.Count; id++)
                polylines.sta_ends[id] = sta_ends[id];

            

            TopoCreator.setPattern(ref polylines, topoData);

            DA.SetData(0, topoData);
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
            get { return new Guid("c72882d3-383a-4378-a203-28665ba44620"); }
        }
    }
}
