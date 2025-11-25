using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(MapEditor))]
public class CustomMapEditor : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private MapEditor _mapEditor;
    private MapEditorGUI _mapEditorGUI;
    
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        VisualTreeAsset.CloneTree(root);

        _mapEditorGUI = new MapEditorGUI(root, _mapEditor);
        _mapEditorGUI.CreateMapEditorGUI();
        
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
        
        // 마우스 선택 막기
        if (e.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }

        if (e.type == EventType.MouseMove)
        {
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if(Physics.Raycast(ray, out hit,_mapEditor.GridTileLayer))
            {
                var index = hit.transform.GetSiblingIndex();
                _mapEditor.SetAlpha(index);
            }
            
            SceneView.RepaintAll();
        }
    }
}
