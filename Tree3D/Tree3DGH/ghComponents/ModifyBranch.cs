using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Tree3D;

namespace Tree3DGH.ghComponents
{
    public class ModifyBranch : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModifyBranch class.
        /// </summary>
        public ModifyBranch()
          : base("Modify Branch", "ModifyBranch",
              "Modify the Branch and its neighbor Branches",
              Constants.tabName, Constants.subCategory)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch", "Branch", "Target Branch to modify", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Number of Blends", "numBlends", "Change the number of Blends in the Branch", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Number of Neighbors", "numNeighbors", "Number of neighbors to modify in both Up and Down streams", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rotation", "Rotation", "Rotate the branches to the given directions", GH_ParamAccess.list);
            pManager.AddGenericParameter("Tree", "Tree", "Tree", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "Tree", "Tree", GH_ParamAccess.item);
            pManager.AddGenericParameter("Blends", "Blends", "Blends", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            IBranch targetBranch = null;
            int numNeighbors = 0;
            List<int> numBlends = new List<int>();
            List<double> rotations = new List<double>() ;
            ITree tree = null;

            if (!DA.GetData(0, ref targetBranch)) { return; }
            if(!DA.GetDataList(1, numBlends)) { return; }
            if (!DA.GetData(2, ref numNeighbors)) { return; }
            if (!DA.GetDataList(3, rotations)) { return; }
            if (!DA.GetData(4, ref tree)) { return; }

            int numRotations = rotations.Count;

            List<IBranch> neighbors = new List<IBranch>();
            

            targetBranch.UpdateNumBlends(numBlends[0]);

            ITree outputTree = tree.Clone();

            targetBranch = outputTree.CentralLeaders.Branches.Where(b => b.Equals(targetBranch)).First();

            // update neighbor rotation
            if (numNeighbors != null)
            {
                Transform targetRotation = Transform.Rotation(RhinoMath.ToRadians(rotations[0]), targetBranch.orientation.XAxis, targetBranch.orientation.Origin);
                targetBranch.UpdateLocation(targetRotation);

                for (int i = 0; i < numNeighbors; i++)
                {
                    IBranch upNeighbor = null;
                    IBranch downNeighbor = null;

                    if (targetBranch.UpstreamNeighbors != null && targetBranch.UpstreamNeighbors.Count > 0)
                    {
                        upNeighbor = targetBranch.UpstreamNeighbors[i];
                        upNeighbor = UpdateNeighbor(upNeighbor, rotations, i, numRotations);
                        targetBranch.UpstreamNeighbors[i] = upNeighbor;
                        neighbors.Add(upNeighbor);
                    }

                    if (targetBranch.DownstreamNeighbors != null && targetBranch.DownstreamNeighbors.Count > 0)
                    {
                        downNeighbor = targetBranch.DownstreamNeighbors[i];
                        downNeighbor = UpdateNeighbor(downNeighbor, rotations, i, numRotations);
                        targetBranch.DownstreamNeighbors[i] = downNeighbor;
                        neighbors.Add(downNeighbor);
                    }
                }
            }

            // update neighbor number of blends
            if(numBlends != null)
            {
                int numBlendsCount = numBlends.Count;
                for (int i = 0; i < numNeighbors; i++)
                {
                    if(numBlendsCount > i)
                    {
                        neighbors[i].UpdateNumBlends(numBlends[i]);
                    }
                    else
                    {
                        neighbors[i].UpdateNumBlends(numBlends.Last());
                    }
                }
            }


            DA.SetData(0, outputTree);
            DA.SetDataList(1, targetBranch.PlantParts);
            
        }

        private IBranch UpdateNeighbor(IBranch neighborBranch, List<double> rotations, int index, int numRotations)
        {
            double rotation = 0;

            if(numRotations > index)
            {
                rotation = RhinoMath.ToRadians(rotations[index]);
            }
            else
            {
                rotation = RhinoMath.ToRadians(rotations.Last());
            }
            Transform trans = Transform.Rotation(RhinoMath.ToRadians(rotation), neighborBranch.orientation.XAxis, neighborBranch.orientation.Origin);

            neighborBranch.UpdateLocation(trans);

            return neighborBranch;
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
            get { return new Guid("b875c262-d79f-44ce-87ed-8b9cee4adea6"); }
        }
    }
}