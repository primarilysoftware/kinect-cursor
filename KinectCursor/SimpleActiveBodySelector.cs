using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCursor
{
    public class SimpleActiveBodySelector : IActiveBodySelector
    {
        private ulong trackingId;

        public Body Select(BodyFrame frame)
        {
            if (frame == null || frame.BodyCount == 0)
            {
                this.trackingId = 0;
                return null;
            }

            var bodies = new Body[frame.BodyCount];
            frame.GetAndRefreshBodyData(bodies);

            var trackedBody = bodies.FirstOrDefault(b =>
                b.TrackingId == this.trackingId &&
                b.Joints[JointType.HandLeft].Position.Y > b.Joints[JointType.HipLeft].Position.Y + 0.05);

            if (trackedBody != null)
            {
                return trackedBody;
            }

            var activeBody = bodies.FirstOrDefault(b =>
                b.IsTracked &&
                b.HandLeftState == HandState.Open &&
                b.Joints[JointType.HandLeft].Position.Y > b.Joints[JointType.ShoulderLeft].Position.Y);

            if (activeBody != null)
            {
                this.trackingId = activeBody.TrackingId;
                return activeBody;
            }

            this.trackingId = 0;
            return null;
        }
    }
}
