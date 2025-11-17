using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(MapEditor))]
public class CustomMapEditor : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private MapEditor _mapEditor;
    
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        VisualTreeAsset.CloneTree(root);

        var gridCreteBtn = root.Q<Button>("CreateGridBtn");
        var gridDestroyBtn = root.Q<Button>("DestroyGridBtn");

        gridCreteBtn.clicked += _mapEditor.CreateGridTile;
        gridDestroyBtn.clicked += _mapEditor.DestroyGrid;
        
        return root;
    }

    private void OnEnable()
    {
        _mapEditor = (MapEditor)target;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        var e = Event.current;
        var hit = new RaycastHit();

        if (e.type == EventType.MouseMove)
        {
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                var index = hit.transform.GetSiblingIndex();
                _mapEditor.SetAlpha(index);
            }
            
            SceneView.RepaintAll();
        }
    }
}
