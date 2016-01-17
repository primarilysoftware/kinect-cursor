using Microsoft.Kinect;

namespace KinectCursor
{
    public class CummulativeMovingAverageFilter : IKinectPointFilter
    {
        private CameraSpacePoint? average;
        private int count;

        public CummulativeMovingAverageFilter()
        {
            this.Reset();
        }

        public CameraSpacePoint Filter(CameraSpacePoint point)
        {
            if (this.average == null)
            {
                this.average = point;
                this.count = 1;
                return this.average.Value;
            }

            count++;
            this.average = new CameraSpacePoint
            {
                X = this.average.Value.X + (point.X - this.average.Value.X) / count,
                Y = this.average.Value.Y + (point.Y - this.average.Value.Y) / count,
                Z = this.average.Value.Z + (point.Z - this.average.Value.Z) / count
            };

            return this.average.Value;
        }

        public void Reset()
        {
            this.average = null;
            this.count = 0;
        }
    }
}
