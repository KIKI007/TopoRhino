using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using TopoRhino;
namespace TopoGrasshopper
{
    public class GH_ComputeContacts : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GH_ComputeContacts()
          : base("DrawContacts", "DrawContacts",
            "Compute the contacts from a list of rhino mesh",
            "TopoCreator", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Meshes", "Parts", "a list of rhino meshes which are not boundary", GH_ParamAccess.list);

            pManager.AddMeshParameter("Meshes", "Boundaries", "a list of rhino meshes which at at boundary of the structure", GH_ParamAccess.list);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Contact", "Contact", "A mesh for contact", GH_ParamAccess.item);
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
            if (!DA.GetDataList(0, parts)) return;
            DA.GetDataList(1, brdies);

            IntPtr graphData = TopoCreator.initContactGraph();

            for (int kd = 0; kd < parts.Count; kd++)
            {
                var mesh = parts[kd];
                addToContactGraph(mesh, false, graphData);
            }

            for(int kd = 0; kd < brdies.Count; kd++)
            {
                var mesh = brdies[kd];
                addToContactGraph(mesh, true, graphData);
            }

            Rhino.Geometry.Mesh rhmesh = new Rhino.Geometry.Mesh();
            TopoCreator.getContactMesh(rhmesh, graphData);
            DA.SetData(0, rhmesh);

            TopoCreator.deleteContactGraph(graphData);

            return;
        }

        private void addToContactGraph(Mesh mesh, bool atBoundary, IntPtr graphData)
        {
            CMesh cmesh = new CMesh();

            cmesh.n_vertices = mesh.Vertices.Count;
            cmesh.n_faces = mesh.Faces.Count;

            if (cmesh.n_vertices * 3 >= Constants.MAXIMUM_MESHSIZE || cmesh.n_faces * 3 >= Constants.MAXIMUM_MESHSIZE)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Exceed maximum mesh size!");
                return;
            }

            cmesh.points = new float[cmesh.n_vertices * 3];
            cmesh.faces = new int[cmesh.n_faces * 3];

            for (int id = 0; id < mesh.Vertices.Count; id++)
            {
                cmesh.points[id * 3] = mesh.Vertices[id].X;
                cmesh.points[id * 3 + 1] = mesh.Vertices[id].Y;
                cmesh.points[id * 3 + 2] = mesh.Vertices[id].Z;
            }

            for (int id = 0; id < mesh.Faces.Count; id++)
            {
                if (mesh.Faces[id].IsTriangle)
                {
                    cmesh.faces[id * 3] = mesh.Faces[id].A;
                    cmesh.faces[id * 3 + 1] = mesh.Faces[id].B;
                    cmesh.faces[id * 3 + 2] = mesh.Faces[id].C;
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mesh has Quad Faces!");
                    return;
                }
            }
            TopoCreator.addMeshesToContactGraph(graphData, ref cmesh, atBoundary);
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
            get { return new Guid("5cd6133b-aae6-40d0-8aa9-e165c0a3a4b3"); }
        }
    }
}
