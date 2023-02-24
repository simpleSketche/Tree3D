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
            pManager.AddCurveParameter("Central Leader Curve", "CentralLeaderCrv", "The list of central leader curve representations", GH_ParamAccess.list);
            pManager.AddNumberParameter("Spacing", "Spacing", "The list of spacings between branches, one spacing corresponding to one leader", GH_ParamAccess.list);
            pManager.AddGenericParameter("Gypsophila", "Gypsophila", "Gypsophila Preset", GH_ParamAccess.item);
            pManager.AddGenericParameter("Fern", "Fern", "Fern Preset", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Seed", "Seed", "Random Seed", GH_ParamAccess.item);
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
            List<Curve> leaders = new List<Curve>();
            List<double> spacings = new List<double>();

            IBlend Gypsophila = null;
            IBlend Fern = null;

            int Seed = 0;

            if (!DA.GetDataList(0, leaders)) { return; }
            if (!DA.GetDataList(1, spacings)) { return; }
            if (!DA.GetData(2, ref Gypsophila)) { return; }
            if (!DA.GetData(3, ref Fern)){ return; }
            if (!DA.GetData(4, ref Seed)) { return; }

            int numLeaders = leaders.Count;
            int numSpacings = spacings.Count;

            if(numLeaders != numSpacings)
            {
                return;
            }

            List<ICentralLeader> allLeaders = new List<ICentralLeader>();

            for(int i=0; i<numLeaders; i++)
            {
                ICentralLeader leaderObj = new CentralLeader(leaders[i], spacings[i], Seed);
                allLeaders.Add(leaderObj);
            }

            ITree treeOutput = new Tree()
            {
                CentralLeaders = allLeaders,
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