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
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        var e = Event.current;

        switch (e.type)
        {
            case EventType.Layout:
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                break;
            
            case EventType.MouseMove:
                var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                
                if(Physics.Raycast(ray, out var hit,_mapEditor.GridTileLayer))
                {
                    var index = hit.transform.GetSiblingIndex();
                    _mapEditor.SetAlpha(index);
                }
            
                SceneView.RepaintAll();
                break;
            
            default:
                return;
        }
    }
}
