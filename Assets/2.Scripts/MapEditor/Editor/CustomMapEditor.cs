using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(MapEditor))]
public class CustomMapEditor : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    private MapEditor _mapEditor;
    private MapEditorGUI _mapEditorGUI;

    private GameObject _placeObj;
    
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        VisualTreeAsset.CloneTree(root);

        _mapEditorGUI = new MapEditorGUI(root);
        _mapEditorGUI.CreateMapEditorGUI();
        
        RegisterMapEditorGUIEvent(root);
        
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
                if (TryGetGridTileIndexFormRay(e, out var hoverIndex))
                {
                    _mapEditor.SetAlpha(hoverIndex);
                    SceneView.RepaintAll();
                }
                break;
            
            case EventType.MouseDown:
                if (TryGetGridTileIndexFormRay(e, out var clickedIndex) && _placeObj != null)
                {
                    _mapEditor.PlaceLevelObject(clickedIndex, _placeObj);
                }
                break;
            
            default:
                return;
        }
    }

    private void RegisterMapEditorGUIEvent(VisualElement root)
    {
        _mapEditorGUI.CreateGridTileEvent += _mapEditor.CreateGridTile;
        _mapEditorGUI.DestroyGridTileEvent += _mapEditor.DestroyGrid;
        
        _mapEditorGUI.RegisterOnPaletteSelect(obj =>
        {
            _placeObj = obj;
        });
        
        _mapEditorGUI.ButtonActionBinding(root);
    }

    private bool TryGetGridTileIndexFormRay(Event e, out int index)
    {
        index = -1;
        var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                
        if(Physics.Raycast(ray, out var hit, Mathf.Infinity, _mapEditor.GridTileLayer))
        {
            index = hit.transform.GetSiblingIndex();
            return true;
        }

        return false;
    }
}
