using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCursor
{
    public static class EventExtensions
    {
        public static void Raise(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender, T e)
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
