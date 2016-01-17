using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCursor
{
    public class HandClosedClickDetector : IClickDetector
    {
        private HandState? lastHandState;

        event EventHandler<CameraSpacePoint> IClickDetector.LeftMouseDown
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void OnNewFrame(Body activeBody)
        {
            if (this.lastHandState == HandState.Open && activeBody.HandLeftState == HandState.Closed)
            {
                this.LeftMouseDown.Raise(this, new EventArgs());
            }
                
            this.lastHandState = activeBody.HandLeftState;
        }

        public void OnNewFrame(BodyFrame frame)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> LeftMouseDown;
    }
}
