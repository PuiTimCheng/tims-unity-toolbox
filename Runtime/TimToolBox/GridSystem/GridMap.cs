using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Grid))]
public class GridMap : MonoBehaviour
{
    private Grid _grid;
    private Transform[,,] _items;
    
    public Vector3Int gridCapacity;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _items = new Transform[gridCapacity.x, gridCapacity.y, gridCapacity.z];
    }

    [Button]
    public bool PutItemOnGrid(Transform item, Vector3Int position)
    {
        if (!IsPositionValid(position) || _items[position.x, position.y, position.z] != null)
            return false;
        
        _items[position.x, position.y, position.z] = item;
        item.position = _grid.GetCellCenterWorld(position);
        item.SetParent(transform);
        return true;
    }

    [Button]
    public bool RemoveItemFromGrid(Vector3Int position)
    {
        if (!IsPositionValid(position) || _items[position.x, position.y, position.z] == null)
            return false;
        
        _items[position.x, position.y, position.z].SetParent(transform.parent);
        _items[position.x, position.y, position.z] = null;
        return true;
    }

    private bool IsPositionValid(Vector3Int position)
    {
        return position.x >= 0 && position.x < gridCapacity.x &&
               position.y >= 0 && position.y < gridCapacity.y &&
               position.z >= 0 && position.z < gridCapacity.z;
    }
}