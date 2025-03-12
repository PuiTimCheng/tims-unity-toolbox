using TimToolBox.Extensions;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GridDebugDrawer : MonoBehaviour
{
    private GridMap _gridMap;
    private Grid _grid;
    [SerializeField] private Vector3 _planeNormal = Vector3.up;
    
    private void OnEnable()
    {
        _gridMap = GetComponent<GridMap>();
        _grid = GetComponent<Grid>();
    }

    private void OnDrawGizmosSelected()
    {
        if (_grid == null) return;

        Vector3 cellSize = _grid.cellSize;
        Vector3 cellGap = _grid.cellGap;
        Vector3 origin = _grid.transform.position;

        Gizmos.color = Color.white;

        var startPoint = origin + cellSize / 2 -  _planeNormal.Multiply(cellSize / 2);
        for (int x = 0; x <= _gridMap.gridCapacity.x-1; x++)
        for (int y = 0; y <= _gridMap.gridCapacity.y-1; y++)
        for (int z = 0; z <= _gridMap.gridCapacity.z-1; z++)
        {
            Vector3 cellCenter = startPoint + new Vector3(
                x * (cellSize.x + cellGap.x),
                y * (cellSize.y + cellGap.y),
                z * (cellSize.z + cellGap.z)
            );
            DrawSquare(cellCenter, cellSize, _planeNormal);
        }
    }
    
    private void DrawSquare(Vector3 center, Vector3 size, Vector3 normal)
    {
        // Calculate the orientation of the square based on the plane normal
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        Vector3 halfSize = size * 0.5f;
        // Define the four corners of the square in local space
        Vector3[] corners = new Vector3[]
        {
            new(-halfSize.x, 0, -halfSize.z),
            new(halfSize.x, 0, -halfSize.z),
            new(halfSize.x, 0, halfSize.z),
            new(-halfSize.x, 0, halfSize.z)
        };
        // Transform the corners to world space and draw lines to form the square
        for (int i = 0; i < 4; i++)
        {
            Vector3 start = center + rotation * corners[i];
            Vector3 end = center + rotation * corners[(i + 1) % 4];
            Gizmos.DrawLine(start, end);
        }
    }
}