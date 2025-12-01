using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapEditor : MonoBehaviour
{
    public LayerMask GridTileLayer => gridTileLayer;
    
    [Header("Grid Setting")]
    [SerializeField] private GameObject gridTile;
    [SerializeField] private Transform gridParent;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private LayerMask gridTileLayer;
    
    [SerializeField] private Transform levelParent;

    private List<GridTile> _gridTiles;
    private int _prevIndex;
    private int _curIndex = 0;

    public void CreateGridTile()
    {
        if (gridParent.childCount != 0)
        {
            return;
        }

        _gridTiles = new List<GridTile>();
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

                _gridTiles.Add(Instantiate(gridTile, pos, Quaternion.identity, gridParent)
                    .GetComponent<GridTile>());
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

    public void SetAlpha(int index)
    {
        _curIndex = index;
        
        if(_curIndex == _prevIndex) return;
        
        _gridTiles[_curIndex].SetAlpha(true);
        _gridTiles[_prevIndex].SetAlpha(false);

        _prevIndex = _curIndex;
    }

    public void PlaceLevelObject(int tileIndex, GameObject obj)
    {
        if (_gridTiles == null || _gridTiles.Count <= tileIndex) return;

        var tile = _gridTiles[tileIndex];
        var pos = tile.transform.position;
        
#if UNITY_EDITOR
        var spawnObj = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(obj);
        UnityEditor.Undo.RegisterCreatedObjectUndo(spawnObj, obj.name);
#endif

        spawnObj.transform.position = pos;
        spawnObj.transform.SetParent(levelParent);
    }
}
