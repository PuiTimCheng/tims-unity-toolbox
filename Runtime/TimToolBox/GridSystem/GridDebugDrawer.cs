using TimToolBox.Extensions;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GridDebugDrawer : MonoBehaviour
{
    public Vector3Int gridCapacity;
    private Grid _grid;
    [SerializeField] private bool showOnlyWhenSelected = false;
    [SerializeField] private Vector3 _planeNormal = Vector3.up;

    private void OnEnable()
    {
        _grid = GetComponent<Grid>();
    }
    private void OnDrawGizmos()
    {
        if (!showOnlyWhenSelected) DrawGrid();
    }
    private void OnDrawGizmosSelected()
    {
        if (showOnlyWhenSelected) DrawGrid();
    }

    private void DrawGrid()
    {
        if (_grid == null) return;

        Vector3 cellSize = _grid.cellSize;
        Vector3 cellGap = _grid.cellGap;
        Vector3 origin = _grid.transform.position;

        Gizmos.color = Color.white;

        var startPoint = origin + cellGap /2 + cellSize / 2 - _planeNormal.Multiply(cellSize / 2);
        for (int x = 0; x <= gridCapacity.x - 1; x++)
        for (int y = 0; y <= gridCapacity.y - 1; y++)
        for (int z = 0; z <= gridCapacity.z - 1; z++)
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