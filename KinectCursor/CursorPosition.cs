using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCursor
{
    public class CursorPosition
    {
        private readonly float x;
        private readonly float y;

        public CursorPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float X
        {
            get { return this.x; }
        }

        public float Y
        {
            get { return this.y; }
        }
    }
}
