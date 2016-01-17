using MathNet.Spatial;
using MathNet.Spatial.Units;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCursor
{
    public static class CameraSpacePointExtensions
    {
        public static CameraSpacePoint RotateY(this CameraSpacePoint target, double theta)
        {
            var vector = new Vector3D(target.X, target.Y, target.Z);
            var rotated = vector.Rotate(UnitVector3D.YAxis, Angle.FromRadians(theta));
            return new CameraSpacePoint
            {
                X = (float)rotated.X,
                Y = (float)rotated.Y,
                Z = (float)rotated.Z
            };
        }

        public static CameraSpacePoint Translate(this CameraSpacePoint point, float dx, float dy, float dz)
        {
            return new CameraSpacePoint
            {
                X = point.X + dx,
                Y = point.Y + dy,
                Z = point.Z + dz
            };
        }

        public static CameraSpacePoint Scale(this CameraSpacePoint target, float scaleX, float scaleY, float scaleZ)
        {
            return new CameraSpacePoint
            { 
                X = target.X * scaleX,
                Y = target.Y * scaleY,
                Z = target.Z * scaleZ
            };
        }

        public static CameraSpacePoint BoundX(this CameraSpacePoint target, float min, float max)
        {
            return new CameraSpacePoint { X = Math.Min(max, Math.Max(min, target.X)), Y = target.Y, Z = target.Z };
        }

        public static CameraSpacePoint BoundY(this CameraSpacePoint target, float min, float max)
        {
            return new CameraSpacePoint { X = target.X, Y = Math.Min(max, Math.Max(min, target.Y)), Z = target.Z };
        }

        public static CameraSpacePoint FilterWith(this CameraSpacePoint point, IKinectPointFilter filter)
        {
            return filter.Filter(point);
        }
    }
}
