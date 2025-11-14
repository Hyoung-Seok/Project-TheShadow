using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MapEditor : MonoBehaviour
{
    [Header("Grid Setting")]
    [SerializeField] private GameObject gridTile;
    [SerializeField] private Transform gridParent;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize = 1f;

    [Header("Level")] 
    [SerializeField] private Transform levelParent;

    private void Start()
    {
        CreateGridTile();
    }

    public void CreateGridTile()
    {
        if (gridParent.childCount != 0)
        {
            return;
        }
        
        var center = gridParent.position;

        var startX = center.x - (gridSize.x * cellSize) / 2 + cellSize * 0.5f;
        var startZ = center.z + (gridSize.y * cellSize) / 2 + cellSize * 0.5f;
        var pos = new Vector3();
        
        for (var i = 0; i < gridSize.x; ++i)
        {
            for (var j = 0; j < gridSize.y; ++j)
            {
                pos.x = startX + j * cellSize;
                pos.y = center.y;
                pos.z = startZ - i * cellSize;

                Instantiate(gridTile, pos, Quaternion.identity, gridParent);
            }
        }
    }

    public void DestroyGrid()
    {
        if (gridParent.childCount <= 0)
        {
            return;
        }

        var cnt = gridParent.childCount;
        for (var i = cnt - 1; i >= 0; --i)
        {
            DestroyImmediate(gridParent.GetChild(i).gameObject);
        }
    }
}
