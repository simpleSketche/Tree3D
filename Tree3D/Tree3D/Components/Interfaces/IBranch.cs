using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree3D
{
    public interface IBranch
    {
        /// <summary>
        /// The list of plant parts to grow on this branch
        /// </summary>
        List<IBlend> PlantParts { get; set; }

        /// <summary>
        /// The selected Blend type in this Branch
        /// </summary>
        IBlend SelUniqueBlend { get; set; }

        /// <summary>
        /// The upstream neighbors of this current branch
        /// </summary>
        List<IBranch> UpstreamNeighbors { get; set; }

        /// <summary>
        /// The downstream neighbors of this current branch
        /// </summary>
        List<IBranch> DownstreamNeighbors { get; set; }


        /// <summary>
        /// The base location point of the Branch
        /// </summary>
        Plane orientation { get; set; }

        /// <summary>
        /// The growth direction of the branch
        /// </summary>
        Vector3d direction { get; set; }

        /// <summary>
        /// The boundingbox of the Branch object
        /// </summary>
        BoundingBox Bbx { get; set; }

        /// <summary>
        /// Initialize the Branch. Randomly pick the number of Blend objects and their direction to protrude
        /// </summary>
        void InitBranch(Random rand);

        /// <summary>
        /// Update the current branch upstream neighbor,
        /// update the other branch downstream neighbor
        /// </summary>
        void UpdateNeighbors(IBranch neighbor);

        /// <summary>
        /// Update the location of the Branch object in the Tree
        /// </summary>
        /// <param name="trans"></param>
        void UpdateLocation(Transform trans);

        /// <summary>
        /// Update the number of blends in this Branch
        /// </summary>
        void UpdateNumBlends(int num);


        /// <summary>
        /// Deep copy the Branch object
        /// </summary>
        /// <returns></returns>
        IBranch Clone();

        /// <summary>
        /// Equal compare between two branches
        /// </summary>
        /// <returns></returns>
        bool Equals(IBranch branch);
    }
}
