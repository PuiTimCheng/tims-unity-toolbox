using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimToolBox.Extensions
{
    public static class VectorExtensions
    {
        #region Vector3

        //Vector3
        public static Vector3 SetX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 SetY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static Vector3 Offset(this Vector3 vector, float x = 0, float y = 0, float z = 0)
        {
            return new Vector3(vector.x + x, vector.y + y, vector.z + z);
        }

        public static Vector3 MultiplyXYZ(this Vector3 vector, Vector3 multVec)
        {
            return new Vector3(vector.x * multVec.x, vector.y * multVec.y, vector.z * multVec.z);
        }
        public static Vector2 XY(this Vector3 v) {
            return new Vector2(v.x, v.y);
        }
        #endregion


        #region Vector2

        //Vector2
        public static Vector2 SetX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        public static Vector2 SetY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }
        
        public static Vector2 Offset(this Vector2 vector, float x = 0, float y = 0)
        {
            return new Vector2(vector.x + x, vector.y + y);
        }

        public static Vector2 MultiplyXY(this Vector2 vector, Vector2 multVec)
        {
            return new Vector2(vector.x * multVec.x, vector.y * multVec.y);
        }
        public static Vector3 ToVec3XY(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0.0f);
        }
        public static Vector3 ToVec3XZ(this Vector2 vector)
        {
            return new Vector3(vector.x, 0.0f, vector.y);
        }
        public static Vector3 ToVec3YZ(this Vector2 vector)
        {
            return new Vector3(0.0f,vector.x, vector.y);
        }
        #endregion


        #region Color

        public static Color SetAlpha(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }

        #endregion"
    }
}