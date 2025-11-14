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

    private void Start()
    {
        CreateGridTile();
    }

    private void CreateGridTile()
    {
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
}
