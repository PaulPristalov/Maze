using UnityEngine;

namespace FirerusUtilities
{
    public static class VectorsExtensions
    {
        public static Vector3 AngleToVector(float angle)
        {
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)).normalized;
        }

        public static float RotationToMouse(Vector3 transformPosition)
        {
            return RotationTo(transformPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        public static float RotationTo(Vector3 start, Vector3 target)
        {
            Vector3 difference = target - start;
            float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            return rotation;
        }

        public static Vector3 Round(this Vector3 vector)
        { 
            return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        }
    }
}
