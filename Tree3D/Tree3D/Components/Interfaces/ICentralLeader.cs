using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree3D
{
    public interface ICentralLeader
    {
        /// <summary>
        /// The list of branches grow on the central leader
        /// </summary>
        List<IBranch> Branches { get; set; }

        /// <summary>
        /// The spacing inbetween branches
        /// </summary>
        double Spacing { get; set; }

        /// <summary>
        /// The curve representation of the leader
        /// </summary>
        Curve LeaderCrv { get; set; }

        /// <summary>
        /// The start point (point in the lower z axis)
        /// </summary>
        Point3d StartPt { get; set; }

        /// <summary>
        /// The end point (point in the higher z axis)
        /// </summary>
        Point3d EndPt { get; set; }

        /// <summary>
        /// The random generator
        /// </summary>
        Random Rand { get; set; }

        /// <summary>
        /// Grow branches along the central leader
        /// </summary>
        /// <param name="uniqueBranches"></param>
        /// <param name="includeEnds"></param>
        void GrowBranches(List<IBlend> uniqueBlends, bool includeEnds);

        /// <summary>
        /// duplicate the Leader object
        /// </summary>
        ICentralLeader Clone();
    }
}
