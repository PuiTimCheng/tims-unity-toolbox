using System;
using TimToolBox.Math;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace TimToolBox.DataStructure{
    /// <summary>
    /// A generic 2D grid implementation that can be used in both 2D and 3D.
    /// The grid stores values of a specified type and provides methods for accessing and modifying
    /// these values based on grid coordinates or world coordinates. The grid can be displayed
    /// either vertically (X-Y plane) or horizontally (X-Z plane) using a CoordinateConverter.
    /// </summary>
    public class GridSystem2D<T> {
        int width;
        int height;
        float cellSize;
        Vector3 origin;
        Quaternion rotation; 
        T[,] gridArray;

        readonly CoordinateConverter coordinateConverter;

        public event Action<int, int, T> OnValueChangeEvent;
        
        public static GridSystem2D<T> VerticalGrid(int width, int height, float cellSize, Vector3 origin, Quaternion rotation, bool debug = false) {
            return new GridSystem2D<T>(width, height, cellSize, origin, rotation, new VerticalConverter(), debug);
        }

        public static GridSystem2D<T> HorizontalGrid(int width, int height, float cellSize, Vector3 origin, Quaternion rotation, bool debug = false) {
            return new GridSystem2D<T>(width, height, cellSize, origin, rotation, new HorizontalConverter(), debug);
        }
        public GridSystem2D(int width, int height, float cellSize, Vector3 origin, Quaternion rotation, CoordinateConverter coordinateConverter = null, bool debug = false) {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.origin = origin;
            this.rotation = rotation;
            this.coordinateConverter = coordinateConverter ?? new VerticalConverter();

            gridArray = new T[width, height];
        
            if (debug) {
                DrawDebugLines();
            }
        }
        
        // Set a value from a grid position
        public void SetValue(Vector3 worldPosition, T value) {
            Vector2Int pos = coordinateConverter.WorldToGrid(worldPosition, cellSize, origin, rotation);
            SetValue(pos.x, pos.y, value);
        }

        public Vector3 Origin {
            get => origin;
            set => origin = value;
        }
        public Quaternion Rotation {
            get => rotation;
            set => rotation = value;
        }
        public void SetValue(int x, int y, T value) {
            if (IsValid(x, y)) {
                gridArray[x, y] = value;
                OnValueChangeEvent?.Invoke(x, y, value);
            }
        }
        
        // Get a value from a grid position
        public T GetValue(Vector3 worldPosition) {
            Vector2Int pos = GetXY(worldPosition);
            return GetValue(pos.x, pos.y);
        }

        public T GetValue(int x, int y) {
            return IsValid(x, y) ? gridArray[x, y] : default;
        }

        bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;
        
        public Vector2Int GetXY(Vector3 worldPosition) => coordinateConverter.WorldToGrid(worldPosition, cellSize, origin, rotation);
        
        public Vector3 GetWorldPositionCenter(int x, int y) => coordinateConverter.GridToWorldCenter(x, y, cellSize, origin, rotation);

        Vector3 GetWorldPosition(int x, int y) => coordinateConverter.GridToWorld(x, y, cellSize, origin, rotation);

        public bool RayIntersectionWithInGrid(Ray ray, out Vector3 intersection) {
            if (IntersectionMath.RayIntersectsPlane(ray, origin, coordinateConverter.Normal(rotation), out intersection)) {
                intersection = coordinateConverter.GetClosestPointWithInGrid(intersection, origin,rotation,cellSize * width);
                return true;
            }
            return false;
        }
        void DrawDebugLines() {
            const float duration = 100f;
            var parent = new GameObject("Debugging");

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    CreateWorldText(parent, x + "," + y, GetWorldPositionCenter(x, y), coordinateConverter.Normal(rotation));
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, duration);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, duration);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, duration);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, duration);
        }
        
        public void DrawDebugByHandle(Color lineColor, Color textColor) {
            
            Handles.color = lineColor;
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    
                    Handles.Label(GetWorldPositionCenter(x, y), x + "," + y, new GUIStyle()
                    {
                        fontSize = 12,
                        normal = new GUIStyleState() { textColor =  textColor}
                    });
                    
                    Handles.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
                    Handles.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y));
                }
            }
            Handles.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));
            Handles.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
        }
        
        TextMeshPro CreateWorldText(GameObject parent, string text, Vector3 position, Vector3 dir, 
            int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center, int sortingOrder = 0) 
        {
            GameObject gameObject = new GameObject("DebugText_" + text, typeof(TextMeshPro));
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.position = position;
            gameObject.transform.forward = dir;

            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color == default ? Color.white : color;
            textMeshPro.alignment = textAnchor;
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMeshPro;
        }

        public abstract class CoordinateConverter {
            public abstract Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin, Quaternion rotation);
            
            public abstract Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin, Quaternion rotation);

            public abstract Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin, Quaternion rotation);

            public abstract Vector3 GetClosestPointWithInGrid(Vector3 worldPosition, Vector3 origin, Quaternion rotation, float maxSize);
            
            public abstract Vector3 Normal(Quaternion rotation);
        }
        
        /// <summary>
        /// A coordinate converter for vertical grids, where the grid lies on the X-Y plane.
        /// </summary>
        public class VerticalConverter : CoordinateConverter {
            public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin, Quaternion rotation) {
                Vector3 localPosition = new Vector3(x, y, 0) * cellSize;
                return origin + (rotation * localPosition); // Apply rotation
            }
            
            public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin, Quaternion rotation) {
                Vector3 localPosition = new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0);
                return origin + (rotation * localPosition); // Apply rotation
            }

            public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin, Quaternion rotation) {
                // Apply inverse rotation to convert world position to grid space
                Vector3 localPosition = Quaternion.Inverse(rotation) * (worldPosition - origin);
                int x = Mathf.FloorToInt(localPosition.x / cellSize);
                int y = Mathf.FloorToInt(localPosition.y / cellSize);
                return new Vector2Int(x, y);
            }

            public override Vector3 GetClosestPointWithInGrid(Vector3 worldPosition, Vector3 origin, Quaternion rotation, float maxSize) {
                
                Vector3 localPosition = Quaternion.Inverse(rotation) * (worldPosition - origin);
                float clampedX = Mathf.Clamp(localPosition.x, 0, maxSize);
                float clampedY = Mathf.Clamp(localPosition.y, 0, maxSize);
                return origin + (rotation * new Vector3(clampedX, clampedY, 0));
            }

            public override Vector3 Normal(Quaternion rotation) => rotation * -Vector3.forward;
        }
        
        /// <summary>
        /// A coordinate converter for horizontal grids, where the grid lies on the X-Z plane.
        /// </summary>
        public class HorizontalConverter : CoordinateConverter {
            public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin, Quaternion rotation) {
                Vector3 localPosition = new Vector3(x, 0, y) * cellSize;
                return origin + (rotation * localPosition); // Apply rotation
            }
    
            public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin, Quaternion rotation) {
                Vector3 localPosition = new Vector3(x * cellSize + cellSize * 0.5f, 0, y * cellSize + cellSize * 0.5f);
                return origin + (rotation * localPosition); // Apply rotation
            }

            public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin, Quaternion rotation) {
                // Apply inverse rotation to convert world position to grid space
                Vector3 localPosition = Quaternion.Inverse(rotation) * (worldPosition - origin);
                int x = Mathf.FloorToInt(localPosition.x / cellSize);
                int y = Mathf.FloorToInt(localPosition.z / cellSize);
                return new Vector2Int(x, y);
            }
            public override Vector3 GetClosestPointWithInGrid(Vector3 worldPosition, Vector3 origin, Quaternion rotation, float maxSize) {
                
                Vector3 localPosition = Quaternion.Inverse(rotation) * (worldPosition - origin);
                float clampedX = Mathf.Clamp(localPosition.x, 0, maxSize);
                float clampedZ = Mathf.Clamp(localPosition.z, 0, maxSize);
                return origin + (rotation * new Vector3(clampedX, 0, clampedZ));
            }
            public override Vector3 Normal(Quaternion rotation) => rotation * Vector3.up;
        }
    }
}