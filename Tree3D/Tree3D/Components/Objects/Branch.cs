using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree3D

{
    public class Branch : IBranch
    {
        public List<IBlend> PlantParts { get; set; }

        public IBlend SelUniqueBlend { get; set; }

        public List<IBranch> UpstreamNeighbors { get; set; }

        public List<IBranch> DownstreamNeighbors { get; set; }

        public Plane orientation { get; set; }

        public Vector3d direction { get; set; }

        public BoundingBox Bbx { get; set; }


        public Branch(IBlend selUniqueBlend)
        {
            UpstreamNeighbors = new List<IBranch>();
            DownstreamNeighbors = new List<IBranch>();
            PlantParts = new List<IBlend>();
            SelUniqueBlend = selUniqueBlend;
        }

        public void InitBranch(Random rand)
        {
            int numPlantParts = rand.Next(1, Constants.defaultMaxNumPlantParts);

            List<IBlend> tempPlantParts = CreateBlends(numPlantParts);

            PlantParts = tempPlantParts;

            // get the orientation plane
            CalculateOrientation(PlantParts);
        }

        public void UpdateNumBlends(int num)
        {
            PlantParts = CreateBlends(num);
        }

        private List<IBlend> CreateBlends(int numBlends)
        {
            List<IBlend> tempPlantParts = new List<IBlend>();


            double rotationAngle = RhinoMath.ToRadians(360 / numBlends);

            for (int i = 0; i < numBlends; i++)
            {
                IBlend primaryPlantPart = SelUniqueBlend.Clone();


                double curAngleToRotate = rotationAngle * i;
                Transform trans = Transform.Rotation(curAngleToRotate, primaryPlantPart.direction, primaryPlantPart.orientation.Origin);
                primaryPlantPart.Move(trans);

                tempPlantParts.Add(primaryPlantPart);
            }

            return tempPlantParts;
        }

        /// <summary>
        /// Calculate the boundingbox and orientation of the Branch object based on the input Blend objects
        /// </summary>
        /// <param name="plantParts"></param>
        private void CalculateOrientation(List<IBlend> plantParts)
        {
            List<BoundingBox> childBbxes = plantParts.Select(p => p.Bbx).ToList();

            Bbx = GetBranchBoundingBox(childBbxes);

            Point3d BbxCnt = Bbx.Center;
            Point3d basePt = new Point3d(BbxCnt.X, BbxCnt.Y, Bbx.Min.Z);

            direction = Vector3d.ZAxis;

            orientation = new Plane(basePt, direction);
        }

        /// <summary>
        /// Get the corners of the boundingbox of each geometry, and calculate the parent boundingbox based on the corners
        /// </summary>
        /// <param name="geos"></param>
        /// <returns></returns>
        private BoundingBox GetBranchBoundingBox(List<BoundingBox> childBbx)
        {
            List<Point3d> corners = childBbx.SelectMany(bbx => bbx.GetCorners()).ToList();
            BoundingBox branchBbx = new BoundingBox(corners);

            return branchBbx;
        }


        public void UpdateNeighbors(IBranch neighbor)
        {
            UpstreamNeighbors.Add(neighbor);
            neighbor.DownstreamNeighbors.Add(this);
        }

        public void UpdateLocation(Transform trans)
        {
            Plane tempOrientation = orientation.Clone();
            Vector3d tempDirection = new Vector3d(direction.X, direction.Y, direction.Z);

            tempOrientation.Transform(trans);
            tempDirection.Transform(trans);

            orientation = tempOrientation;
            direction = tempDirection;

            foreach (IBlend part in PlantParts)
            {
                // plane to plane blend to new orientation of the branch
                Transform blendTrans = Transform.PlaneToPlane(part.orientation, orientation);
                part.Move(blendTrans);
            }
        }

        public IBranch Clone()
        {
            List<IBlend> clonePlantParts = PlantParts.Select(p => p.Clone()).ToList();

            return new Branch(SelUniqueBlend)
            {
                PlantParts = clonePlantParts,
                UpstreamNeighbors = UpstreamNeighbors,
                DownstreamNeighbors = DownstreamNeighbors,
                orientation = orientation.Clone(),
                direction = new Vector3d(direction.X, direction.Y, direction.Z)
            };
        }

        public bool Equals(IBranch otherBranch)
        {
            bool sameOrientation = orientation == otherBranch.orientation;

            return sameOrientation;
        }
    }
}
