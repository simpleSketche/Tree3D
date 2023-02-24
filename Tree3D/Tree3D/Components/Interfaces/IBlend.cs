using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree3D
{
    public interface IBlend
    {

        /// <summary>
        /// The geometry representations of the blend
        /// </summary>
        List<GeometryBase> Geometries { get; set; }

        /// <summary>
        /// The curve representation of the blend
        /// </summary>
        Curve BlendCrv { get; set; }

        /// <summary>
        /// The direction of the blend
        /// </summary>
        Vector3d direction { get; set; }

        /// <summary>
        /// The base location of the Blend
        /// </summary>
        Plane orientation { get; set; }

        /// <summary>
        /// Move and update the Blend geometires
        /// </summary>
        void Move(Transform trans);

        /// <summary>
        /// The boundingbox of the blend object
        /// </summary>
        BoundingBox Bbx { get; set; }

        /// <summary>
        /// Deep copy the blend object
        /// </summary>
        /// <returns></returns>
        IBlend Clone();

        /// <summary>
        /// Compare if same as the other blend object
        /// </summary>
        /// <param name="otherBlend"></param>
        /// <returns></returns>
        bool Equals(IBlend otherBlend);
    }
}
