using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCursor
{
    public static class Extensions
    {
        public static TOut As<TIn, TOut>(this TIn input, Func<TIn, TOut> converter = null)
        {
            if (converter != null)
            {
                return converter(input);
            }

            return (TOut)Convert.ChangeType(input, typeof(TOut));
        }
    }
}
