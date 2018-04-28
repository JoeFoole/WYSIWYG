using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYSIWYG
{
    class Withholding
    {
        static void Single()
        {
            int[,] table = new int[,] { { 3400, 8500, 50000 }, {6800,17000,50000 } };   // single fewer than 3 allaowances - Single 3 or more and married.
            double[,] baseWH = new double[,] { { 197, 367, 724 }, { 197, 537, 1251 } };
            double[] rate = new double[] { 0.05, 0.07, 0.09 };
        }
    }
}
