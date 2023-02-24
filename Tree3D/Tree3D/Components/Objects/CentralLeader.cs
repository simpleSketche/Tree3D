using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree3D
{
    public class CentralLeader : ICentralLeader
    {
        public List<IBranch> Branches { get; set; }
        public double Spacing { get; set; }
        public Curve LeaderCrv { get; set; }
        public Point3d StartPt { get; set; }
        public Point3d EndPt { get; set; }
        
        public Random Rand { get; set; }

        public CentralLeader(Curve leaderCrv, double spacing, int seed)
        {
            LeaderCrv = leaderCrv;
            Spacing = spacing;
            Rand = new Random(seed);
            InitializeCentralLeader();
        }

        /// <summary>
        /// Initialize the central leader and assign the start and end points of the central leader
        /// </summary>
        private void InitializeCentralLeader()
        {
            Branches = new List<IBranch>();

            List<Point3d> endPts = new List<Point3d>();
            endPts.Add(LeaderCrv.PointAtNormalizedLength(0));
            endPts.Add(LeaderCrv.PointAtNormalizedLength(1));

            endPts = endPts.OrderBy(o => o.Z).ToList();

            StartPt = endPts[0];
            EndPt = endPts[1];
        }

        public void GrowBranches(List<IBlend> uniqueBlends, bool includeEnds)
        {

            int numUniqueBlends = uniqueBlends.Count;
            int numBranchesToGrow = Convert.ToInt32(Math.Floor(LeaderCrv.GetLength() / Spacing));

            for(int i = 0; i < numBranchesToGrow; i++)
            {
                // randomly select an unique branch
                int uniqueBranchIndex = Rand.Next(0, numUniqueBlends);

                // find perpendicular frame at the point along the curve
                Plane targetPlane = new Plane();
                double t = LeaderCrv.Domain.ParameterAt((double)i / (double)numBranchesToGrow);
                Point3d branchBasePt = LeaderCrv.PointAt(t);
                LeaderCrv.FrameAt(t, out targetPlane);


                if (!includeEnds && (i == 0 || i == uniqueBranchIndex - 1))
                {
                    continue;
                }
                else
                {
                    IBlend selBlend = uniqueBlends[uniqueBranchIndex].Clone();

                    IBranch selBranch = new Branch(selBlend);

                    // initialize Branch with random number of Blend objects (max 4 by default)
                    selBranch.InitBranch(Rand);

                    Transform trans = Transform.PlaneToPlane(selBranch.orientation, targetPlane);
                    selBranch.UpdateLocation(trans);

                    if (Branches.Count > 0)
                    {
                        IBranch upStreamNeighbor = Branches.Last();
                        selBranch.UpdateNeighbors(upStreamNeighbor);
                    }
                    Branches.Add(selBranch);
                }
            }
        }

        public ICentralLeader Clone()
        {
            ICentralLeader leader = new CentralLeader(LeaderCrv.Duplicate() as Curve, Spacing, 0)
            {

                Branches = Branches.Select(b => b.Clone()).ToList(),
                Rand = Rand,
                StartPt = new Point3d(StartPt.X, StartPt.Y, StartPt.Z),
                EndPt = new Point3d(EndPt.X, EndPt.Y, EndPt.Z),
            };
            return leader;
        }
    }
}
