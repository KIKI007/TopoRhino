using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using TopoRhino;
namespace TopoGrasshopper
{
    public class GH_ModifyParameters : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GH_ModifyParameters()
          : base("ModifyPara", "ModifyPara",
            "Modify parameter of a TopoCreator",
            "TopoCreator", "Utility")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("a pointer of TopoCreator", "TopoPtr", "a pointer of TopoCreator", GH_ParamAccess.item);
            pManager.AddTextParameter("name", "name", "variable name", GH_ParamAccess.item);
            pManager.AddGenericParameter("value", "value", "variable value", GH_ParamAccess.item);
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
            string name = "";
            if (!DA.GetData(0, ref topoData)) return;
            if (!DA.GetData(1, ref name)) return;

            if(name == "tiltAngle")
            {
                double tilt_angle = 0;
                if (!DA.GetData(2, ref tilt_angle)) return;
                if (tilt_angle < 0 || tilt_angle > 90)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid Tilt Angle");
                    return;
                }
                TopoCreator.setParaDouble(name, tilt_angle, topoData);
            }
            else if (name == "cutUpper" || name == "cutLower")
            {
                double cut_height = 0;
                if (!DA.GetData(2, ref cut_height)) return;
                if(cut_height < 0 || cut_height > 0.2)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid " + name);
                    return;
                }
                TopoCreator.setParaDouble(name, cut_height, topoData);
            }
            else if(name == "patternID")
            {
                double type = 0;
                if (!DA.GetData(2, ref type)) return;
                if (type < 0 || type > 15)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid " + name);
                    return;
                }
                TopoCreator.setParaInt(name, (int)type, topoData);
            }
            else if (name == "patternAngle")
            {
                double angle = 0;
                if (!DA.GetData(2, ref angle)) return;
                if(angle < -180 || angle > 180)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid " + name);
                    return;
                }
                TopoCreator.setPatternAngle(angle, topoData);
            }
            else if (name == "patternXY")
            {
                Rhino.Geometry.Vector3d pos = new Rhino.Geometry.Vector3d();
                if (!DA.GetData(2, ref pos)) return;
                TopoCreator.setPatternXY(pos.X, pos.Y, topoData);
            }
            else if(name == "patternScale")
            {
                double scale = 0;
                if (!DA.GetData(2, ref scale)) return;
                if (scale < 0 || scale > 2)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid " + name);
                    return;
                }
                TopoCreator.setPatternScale(scale, topoData);
            }

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
            get { return new Guid("4135de4f-ca68-43a7-adb5-ef9a3c90a9b1"); }
        }
    }
}
