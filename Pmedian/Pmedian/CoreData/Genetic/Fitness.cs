using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmedian.CoreData.Genetic
{
    public static class Fitness
    {
        public static int Function(int[][] cost, int[] chromosome)
        {
            int min = 0;
            int iCh = 0;
            for (int i = 0; i < cost.Length; i++)
            {
                for (int j = 0, c = iCh; j < cost[0].Length; j++, c++)
                {
                    min += cost[i][j] * chromosome[c];
                    iCh = c + 1;
                }
            }

            return min;
        }
    }
}
