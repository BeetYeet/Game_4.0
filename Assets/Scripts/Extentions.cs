using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Fighting;

namespace Tools
{
    public static class Extentions
    {
        public static void Split(this long l, out int left, out int right)
        {
            left = (int)(l >> 32);
            right = (int)((l << 32) >> 32);
        }

        public static Vector3 Mult(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static List<Drop> DiscontainsWeapon(this List<Drop> a, List<WeaponInfo> b)
        {
            return a.Where(ea => !b.Any(eb => ea.weapon.Equals(eb))).ToList();
        }

        public static List<Drop> ContainsWeapon(this List<Drop> a, List<WeaponInfo> b)
        {
            return a.Where(ea => b.Any(eb => ea.weapon.Equals(eb))).ToList();
        }

        public static void GetSnapToPositionToBringChildIntoView(this ScrollRect instance, RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = instance.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            Vector2 result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );
            instance.content.localPosition = result;
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