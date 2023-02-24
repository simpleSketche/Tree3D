using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree3D
{
    public class Blend : IBlend
    {
        public List<GeometryBase> Geometries { get; set; }
        public Curve BlendCrv { get; set; }
        public Vector3d direction { get; set; }

        public Plane orientation { get; set; }

        public BoundingBox Bbx { get; set; }

        public Blend(List<GeometryBase> geometries)
        {
            Geometries = geometries;
            InitializeBlend();
        }

        /// <summary>
        /// Construct the actual blend doobjectbject 
        /// </summary>
        private void InitializeBlend()
        {
            BoundingBox bbx = GetBranchBoundingBox(Geometries);
            Bbx = bbx;
            BlendCrv = InitLocationCrv(bbx);
        }

        /// <summary>
        /// Get the corners of the boundingbox of each geometry, and calculate the parent boundingbox based on the corners
        /// </summary>
        /// <param name="geos"></param>
        /// <returns></returns>
        private BoundingBox GetBranchBoundingBox(List<GeometryBase> geos)
        {
            List<BoundingBox> childBbx = geos.Select(geo => geo.GetBoundingBox(true)).ToList();
            List<Point3d> corners = childBbx.SelectMany(bbx => bbx.GetCorners()).ToList();
            BoundingBox branchBbx = new BoundingBox(corners);

            return branchBbx;
        }

        /// <summary>
        /// Use the boundingbox of the Blend object to find the location curve, which starts
        /// from the center of the bottom face of the boundingbox in z axis.
        /// </summary>
        /// <param name="bbx"></param>
        /// <returns></returns>
        private Curve InitLocationCrv(BoundingBox bbx)
        {
            double lowZ = bbx.Min.Z;

            Point3d basePt = new Point3d(bbx.Center.X, bbx.Center.Y, lowZ);

            Line locationRay = new Line(basePt, Vector3d.ZAxis);

            // initialize direction along z axis, meanwhile initialize base point
            direction = Vector3d.ZAxis;
            orientation = new Plane(basePt, direction);

            Curve locationCrv = locationRay.ToNurbsCurve();

            return locationCrv;
        }

        public void Move(Transform trans)
        {
            Plane tempOrientation = orientation.Clone();
            Vector3d tempDirection = new Vector3d(direction.X, direction.Y, direction.Z);

            tempOrientation.Transform(trans);
            tempDirection.Transform(trans);


            orientation = tempOrientation;
            BlendCrv.Transform(trans);
            direction = tempDirection;

            List<GeometryBase> tempGeometries = Geometries;

            foreach(GeometryBase geo in tempGeometries)
            {
                geo.Transform(trans);
            }

            Geometries = tempGeometries;
        }

        public IBlend Clone()
        {
            Blend cloneBlend = new Blend(Geometries.Select(g => g.Duplicate()).ToList())
            {
                BlendCrv = BlendCrv.Duplicate() as Curve,
                direction = new Vector3d(direction.X, direction.Y, direction.Z),
                orientation = orientation.Clone()
            };

            return cloneBlend;
        }

        public bool Equals(IBlend otherBlend)
        {
            bool sameOrientation = orientation == otherBlend.orientation;

            return sameOrientation;
        }
    }
}
