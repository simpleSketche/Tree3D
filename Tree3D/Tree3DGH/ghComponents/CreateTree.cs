using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Tree3D;

namespace Tree3DGH
{
    public class CreateTree : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateTree class.
        /// </summary>
        public CreateTree()
          : base("CreateTree", "CreateTree",
              "CreateTree",
              Constants.tabName, Constants.subCategory)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Central Leader Curve", "CentralLeaderCrv", "The central leader curve representation", GH_ParamAccess.item);
            pManager.AddNumberParameter("Spacing", "Spacing", "The  spacing between branches, one spacing corresponding to one leader", GH_ParamAccess.item);
            pManager.AddGenericParameter("Gypsophila", "Gypsophila", "Gypsophila Preset", GH_ParamAccess.item);
            pManager.AddGenericParameter("Fern", "Fern", "Fern Preset", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Seed", "Seed", "Random Seed", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Grow On Ends?", "IsGrowOnEnds", "Decide whether the Branch objects grow at the ends of the leader axis", GH_ParamAccess.item);
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
            Curve leader = null;
            double spacing = 0;

            IBlend Gypsophila = null;
            IBlend Fern = null;

            int Seed = 0;

            bool isGrowOnEnd = false;

            if (!DA.GetData(0, ref leader)) { return; }
            if (!DA.GetData(1, ref spacing)) { return; }
            if (!DA.GetData(2, ref Gypsophila)) { return; }
            if (!DA.GetData(3, ref Fern)){ return; }
            if (!DA.GetData(4, ref Seed)) { return; }
            if (!DA.GetData(5, ref isGrowOnEnd)) { return; }


            List<ICentralLeader> allLeaders = new List<ICentralLeader>();

            List<IBlend> uniqueBlends = new List<IBlend>()
            {
                Gypsophila,
                Fern
            };

            ICentralLeader leaderObj = new CentralLeader(leader, spacing, Seed);
            leaderObj.GrowBranches(uniqueBlends, isGrowOnEnd);

            ITree treeOutput = new Tree()
            {
                CentralLeaders = leaderObj,
            };

            DA.SetData(0, treeOutput);
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
            get { return new Guid("8f2608c5-05df-4b17-8570-5659c939a94e"); }
        }
    }
}