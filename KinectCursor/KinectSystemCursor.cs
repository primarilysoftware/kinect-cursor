using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace KinectCursor
{
    public class KinectSystemCursor
    {
        private readonly KinectSensor sensor;
        private readonly IActiveBodySelector activeBodySelector;
        private readonly IKinectPointFilter anchorFilter;
        private readonly IKinectPointFilter handFilter;
        private readonly float highPrecisionScaleFactor;
        private HandState? lastHandState;
        private CameraSpacePoint relativeCenter;
        private float currentPrecisionScaleFactor;

        public KinectSystemCursor(KinectSensor sensor, IActiveBodySelector activeBodySelector, IKinectPointFilter anchorFilter, IKinectPointFilter handFilter, float highPrecisionScaleFactor)
        {
            this.sensor = sensor;
            this.activeBodySelector = activeBodySelector;
            this.anchorFilter = anchorFilter;
            this.handFilter = handFilter;
            this.highPrecisionScaleFactor = highPrecisionScaleFactor;
            this.sensor.BodyFrameSource.OpenReader().FrameArrived += FrameArrived;
            this.currentPrecisionScaleFactor = 1;
        }

        public event EventHandler<CursorPosition> CursorMoved;
        public event EventHandler<CursorPosition> LeftMouseDown;
        public event EventHandler<CursorPosition> LeftMouseUp;
        
        private void FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                // try and select the active body from the frame
                var activeBody = this.activeBodySelector.Select(bodyFrame);
                if (activeBody == null)
                {
                    return;
                }
                
                // establish the anchor point
                var anchor = activeBody.Joints[JointType.ShoulderLeft].Position
                    .FilterWith(anchorFilter);

                // rotate anchor so that it lies on YZ plane
                var theta = -Math.Atan(anchor.X / anchor.Z);
                anchor = anchor.RotateY(theta);

                // establish virtual screen in front of user
                const float D = 0.4f;
                const float L = 0.15f;
                var pCenter = anchor.Translate(0, 0, -D);
                var topLeft = pCenter.Translate(-L, L, 0);
                var topRight = pCenter.Translate(L, L, 0);
                var bottomLeft = pCenter.Translate(-L, -L, 0);
                var bottomRight = pCenter.Translate(L, -L, 0);

                var virtualScreenPoint = activeBody.Joints[JointType.HandLeft].Position
                    .FilterWith(this.handFilter)
                    .RotateY(theta)
                    .BoundX(topLeft.X, topRight.X)
                    .BoundY(bottomLeft.Y, topLeft.Y);

                // check for high precision mode
                if (activeBody.HandLeftState == HandState.Lasso && this.relativeCenter == new CameraSpacePoint())
                {
                    this.relativeCenter = virtualScreenPoint;
                    this.currentPrecisionScaleFactor = this.highPrecisionScaleFactor;
                }

                if (activeBody.HandLeftState == HandState.Open)
                {
                    this.relativeCenter = new CameraSpacePoint();
                    this.currentPrecisionScaleFactor = 1;
                }

                // translate hand position to screen coordinates
                var relativeScreenPoint = virtualScreenPoint
                    .Translate(-this.relativeCenter.X, -this.relativeCenter.Y, 0);

                var cursorPosition = this.relativeCenter
                    .Translate(relativeScreenPoint.X * this.currentPrecisionScaleFactor, relativeScreenPoint.Y * this.currentPrecisionScaleFactor, 0)
                    .Translate(-topLeft.X, -topLeft.Y, 0)
                    .Scale(65535 / (2 * L), -65535 / (2 * L), 1)
                    .As(point => new CursorPosition(point.X, point.Y));

                this.CursorMoved.Raise(this, cursorPosition);

                // check for left mouse down
                if ((this.lastHandState == HandState.Open || this.lastHandState == HandState.Lasso)
                    && activeBody.HandLeftState == HandState.Closed)
                {
                    this.LeftMouseDown.Raise(this, cursorPosition);
                }
                else if (this.lastHandState == HandState.Closed &&
                    (activeBody.HandLeftState == HandState.Open || activeBody.HandLeftState == HandState.Lasso))
                {
                    this.LeftMouseUp.Raise(this, cursorPosition);
                }

                if (activeBody.HandLeftState != HandState.Unknown)
                {
                    this.lastHandState = activeBody.HandLeftState;
                }
            }
        }
    }
}
