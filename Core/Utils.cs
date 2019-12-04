using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace neuralnetwork.Core
{
    public class Utils
    {
        public static double[] ToDoubleArray(string data)
        {
            char[] arr = data.ToCharArray();
            double[] ret = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                ret[i] = Convert.ToDouble(arr[i] - 48);
            }
            return ret;
        }

        public static string DoubleArrayToString(double[] data)
        {
            string res = "";
            for (int i = 0; i < data.Length; i++)
            {
                res += Math.Round(data[i]).ToString();
            }
            return res;
        }

    }
}