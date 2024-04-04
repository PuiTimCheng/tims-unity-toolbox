using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimToolBox.Extensions
{
    public static class VectorExtensions
    {
        #region Vector3

        //Vector3
        public static Vector3 Set(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        public static Vector3 Offset(this Vector3 vector, float x = 0, float y = 0, float z = 0)
        {
            return new Vector3(vector.x + x, vector.y + y, vector.z + z);
        }
        public static Vector3 Multiply(this Vector3 vector, Vector3 vector2)
        {
            return new Vector3(vector.x * vector2.x, vector.y * vector2.y, vector.z * vector2.z);
        }
        public static Vector3 Multiply(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector.x * (x??1), vector.y * (y??1), vector.z * (z??1));
        }
        public static Vector2 XY(this Vector3 v) {
            return new Vector2(v.x, v.y);
        }
        #endregion
        
        #region Vector3Int
        //Vector3Int
        public static Vector3Int Set(this Vector3Int vector, int? x = null, int? y = null, int? z = null)
        {
            return new Vector3Int(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        public static Vector3Int Offset(this Vector3Int vector, int x = 0, int y = 0, int z = 0)
        {
            return new Vector3Int(vector.x + x, vector.y + y, vector.z + z);
        }
        public static Vector3Int Multiply(this Vector3Int vector, Vector3Int vector2)
        {
            return vector.Multiply(vector2.x, vector2.y, vector2.z);
        }
        public static Vector3Int Multiply(this Vector3Int vector, int? x = null, int? y = null, int? z = null)
        {
            return new Vector3Int(vector.x * (x??1), vector.y * (y??1), vector.z * (z??1));
        }
        public static Vector2Int XY(this Vector3Int v) {
            return new Vector2Int(v.x, v.y);
        }
        #endregion

        #region Vector2
        //Vector2
        public static Vector2 Set(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector.x, y ?? vector.y);
        }
        public static Vector2 Offset(this Vector2 vector, float x = 0, float y = 0)
        {
            return new Vector2(vector.x + x, vector.y + y);
        }
        public static Vector2 Multiply(this Vector2 vector, Vector2 vector2)
        {
            return vector.Multiply(vector2.x, vector2.y);
        }
        public static Vector2 Multiply(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(vector.x * (x??1), vector.y * (y??1));
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


        #region Vector2Int
        //Vector2
        public static Vector2Int Set(this Vector2Int vector, int? x = null, int? y = null)
        {
            return new Vector2Int(x ?? vector.x, y ?? vector.y);
        }
        public static Vector2Int Offset(this Vector2Int vector, int x = 0, int y = 0)
        {
            return new Vector2Int(vector.x + x, vector.y + y);
        }
        public static Vector2Int Multiply(this Vector2Int vector, Vector2Int vector2)
        {
            return vector.Multiply(vector2.x, vector2.y);
        }
        public static Vector2Int Multiply(this Vector2Int vector, int? x = null, int? y = null)
        {
            return new Vector2Int(vector.x * (x??1), vector.y * (y??1));
        }
        public static Vector3Int ToVec3XY(this Vector2Int vector)
        {
            return new Vector3Int(vector.x, vector.y, 0);
        }
        public static Vector3Int ToVec3XZ(this Vector2Int vector)
        {
            return new Vector3Int(vector.x, 0, vector.y);
        }
        public static Vector3Int ToVec3YZ(this Vector2Int vector)
        {
            return new Vector3Int(0,vector.x, vector.y);
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