using Microsoft.Kinect;
using System;
namespace KinectCursor
{
    public interface IClickDetector
    {
        void OnNewFrame(BodyFrame frame);
        event EventHandler<CameraSpacePoint> LeftMouseDown;
    }
}
