using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCursor
{
    public class DoubleExponentialSmoothingFilter : IKinectPointFilter
    {
        private readonly float alpha;
        private readonly float gamma;
        private CameraSpacePoint? lastTrend;
        private CameraSpacePoint? lastOutput;

        public DoubleExponentialSmoothingFilter(float alpha, float gamma)
        {
            this.alpha = alpha;
            this.gamma = gamma;
        }

        public CameraSpacePoint Filter(CameraSpacePoint point)
        {
            if (this.lastTrend == null || this.lastOutput == null)
            {
                this.lastTrend = point;
                this.lastOutput = point;
                return point;
            }

            var newTrend = new CameraSpacePoint
            {
                X = gamma * (point.X - lastOutput.Value.X) + (1 - gamma) * lastTrend.Value.X,
                Y = gamma * (point.Y - lastOutput.Value.Y) + (1 - gamma) * lastTrend.Value.Y,
                Z = gamma * (point.Z - lastOutput.Value.Z) + (1 - gamma) * lastTrend.Value.Z
            };

            var newOutput = new CameraSpacePoint
            {
                X = alpha * point.X + (1 - alpha) * (lastOutput.Value.X + lastTrend.Value.X),
                Y = alpha * point.Y + (1 - alpha) * (lastOutput.Value.Y + lastTrend.Value.Y),
                Z = alpha * point.Z + (1 - alpha) * (lastOutput.Value.Z + lastTrend.Value.Z)
            };

            this.lastTrend = newTrend;
            this.lastOutput = newOutput;
            return newOutput;
        }

        public void Reset()
        {
            this.lastTrend = null;
            this.lastOutput = null;
        }
    }
}
