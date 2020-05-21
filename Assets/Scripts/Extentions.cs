using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools
{
    public static class Extentions
    {
        public static void Split(this long l, out int left, out int right)
        {
            left = (int)(l >> 32);
            right = (int)((l << 32) >> 32);
        }
    }

    public static class Tools
    {
        public static long MakeLong(int left, int right)
        {
            return (long)left << 32 | (long)(uint)right;
        }

        public static bool Coinflip()
        {
            if (Random.value - 0.5f > 0)
                return true;
            return false;
        }
    }
}