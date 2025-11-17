using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MapEditor))]
public class CustomMapEditor : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private MapEditor _mapEditor;
    
    private VisualElement _objPreview;
    
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        VisualTreeAsset.CloneTree(root);

        var gridCreteBtn = root.Q<Button>("CreateGridBtn");
        var gridDestroyBtn = root.Q<Button>("DestroyGridBtn");
        var objSelect = root.Q<ObjectField>("ObjectSelectField");
        _objPreview = root.Q<VisualElement>("PreviewTexture");

        gridCreteBtn.clicked += _mapEditor.CreateGridTile;
        gridDestroyBtn.clicked += _mapEditor.DestroyGrid;

        objSelect.RegisterValueChangedCallback(RegisterObjectPreview);
        
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

    private void RegisterObjectPreview(ChangeEvent<Object> evt)
    {
        var obj = evt.newValue;
        if (obj == null) return;

        var path = AssetDatabase.GetAssetPath(obj);
        
        // preview road
        var editor = CreateEditor(obj);
        var tex = editor.RenderStaticPreview(path, null, 200, 200);
        DestroyImmediate(editor);

        _objPreview.style.backgroundImage = new StyleBackground(tex);
    }
}
