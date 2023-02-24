using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Tree3D;

namespace Tree3DGH.ghComponents
{
    public class ModifyBlend : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModifyBlend class.
        /// </summary>
        public ModifyBlend()
          : base("Modify Blend", "ModifyBlend",
              "ModifyBlend",
              Constants.tabName, Constants.subCategory)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Blend", "Blend", "Blend", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotation", "Rotation", "Rotation", GH_ParamAccess.item);
            pManager.AddGenericParameter("Tree", "Tree", "Tree", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "Tree", "Tree", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IBlend blend = null;
            double rotation = 0;
            ITree tree = null;

            if(!DA.GetData(0, ref blend)) { return; }
            if(!DA.GetData(1, ref rotation)) { return; }
            if(!DA.GetData(2, ref tree)) { return; }

            ITree outputTree = tree.Clone();

            foreach(IBranch branch in tree.CentralLeaders.Branches)
            {
                foreach(IBlend curBlend in branch.PlantParts)
                {
                    if (curBlend.Equals(blend))
                    {
                        blend = curBlend;
                        break;
                    }
                }
            }

            Transform trans = Transform.Rotation(RhinoMath.ToRadians(rotation), blend.direction, blend.orientation.Origin);

            blend.Move(trans);

            DA.SetData(0, outputTree);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("945027a2-a1bb-46c0-a805-ec4308e9df35"); }
        }
    }
}