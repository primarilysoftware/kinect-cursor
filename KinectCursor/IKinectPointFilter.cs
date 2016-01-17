using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCursor
{
    public interface IKinectPointFilter
    {
        CameraSpacePoint Filter(CameraSpacePoint point);
        void Reset();
    }
}
