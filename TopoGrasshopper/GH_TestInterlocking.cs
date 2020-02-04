using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using TopoRhino;
namespace TopoGrasshopper
{
    public class GH_TestInterlocking : GH_Component
    {

        private bool result;
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GH_TestInterlocking()
          : base("GH_TestInterlocking", "GH_TestInterlocking",
            "GH_TestInterlocking",
            "TopoCreator", "Utility")
        {
            result = false;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Meshes", "Parts", "a list of rhino meshes which are not boundary", GH_ParamAccess.list);

            pManager.AddMeshParameter("Meshes", "Boundaries", "a list of rhino meshes which at at boundary of the structure", GH_ParamAccess.list);

            pManager.AddBooleanParameter("Trigger", "Trigger", "Trigger", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Interlocking", "Interlocking", "Interlocking", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> parts = new List<Mesh>();
            List<Mesh> brdies = new List<Mesh>();
            bool runProgram = false;
            if (!DA.GetDataList(0, parts)) return;
            if (!DA.GetDataList(1, brdies)) return;
            if (!DA.GetData(2, ref runProgram)) return;

            if (runProgram)
            {
                IntPtr graphData = TopoCreator.initContactGraph();

                for (int kd = 0; kd < parts.Count; kd++)
                {
                    var mesh = parts[kd];
                    addToContactGraph(mesh, false, graphData);
                }

                for (int kd = 0; kd < brdies.Count; kd++)
                {
                    var mesh = brdies[kd];
                    addToContactGraph(mesh, true, graphData);
                }

                result = (TopoCreator.testInterlocking(graphData) == 1);
                DA.SetData(0, result);

                TopoCreator.deleteContactGraph(graphData);
            }
            DA.SetData(0, result);
            return;
        }

        private void addToContactGraph(Mesh mesh, bool atBoundary, IntPtr graphData)
        {
            CMesh cmesh = new CMesh();
            String errorMessage;
            if (TopoCreator.RhinoMeshToCMesh(mesh, ref cmesh, false, out errorMessage))
            {
                TopoCreator.addMeshesToContactGraph(graphData, ref cmesh, atBoundary);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, errorMessage);
                return;
            }
            Marshal.FreeHGlobal(cmesh.points);
            Marshal.FreeHGlobal(cmesh.faces);
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
            get { return new Guid("ae9f0530-24b4-464a-a063-32670427d3c4"); }
        }
    }
}
