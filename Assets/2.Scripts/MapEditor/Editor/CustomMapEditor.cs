using System;
using System.Collections.Generic;
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
    
    // uxml Library
    private VisualElement _objPreview;
    private ObjectField _selectedObj;
    private ListView _paletteListView;
    private List<GameObject> _paletteItemList = new List<GameObject>();
    
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        VisualTreeAsset.CloneTree(root);

        // Query
        var gridCreteBtn = root.Q<Button>("CreateGridBtn");
        var gridDestroyBtn = root.Q<Button>("DestroyGridBtn");
        _selectedObj = root.Q<ObjectField>("ObjectSelectField");
        _objPreview = root.Q<VisualElement>("PreviewTexture");
        var addPaletteBtn = root.Q<Button>("AddPaletteBtn");
        _paletteListView = root.Q<ListView>("PaletteList");

        // Add Click evt
        gridCreteBtn.clicked += _mapEditor.CreateGridTile;
        gridDestroyBtn.clicked += _mapEditor.DestroyGrid;
        addPaletteBtn.clicked += CreatePaletteItem;
        
        // Add Value Change evt
        _selectedObj.RegisterValueChangedCallback(RegisterObjectPreview);
        
        // ListView
        SetUpListView();
        
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

    private void RegisterObjectPreview(ChangeEvent<Object> evt)
    {
        var obj = evt.newValue;
        if (obj == null) return;

        var path = AssetDatabase.GetAssetPath(obj);
        
        // preview road
        var editor = CreateEditor(obj);
        var tex = editor.RenderStaticPreview(path, null, 150, 150);
        DestroyImmediate(editor);

        _objPreview.style.backgroundImage = new StyleBackground(tex);
    }

    private void SetUpListView()
    {
        // 데이터 목록 등록 -> 데이터를 볼 땐 해당 변수에 저장된 데이터 확인
        _paletteListView.itemsSource = _paletteItemList;
        
        // 아이템 생성 -> UI가 하나 필요할 때 해당 템플릿으로 생성
        _paletteListView.makeItem = () => _paletteListView.itemTemplate.CloneTree();
        
        // UI와 실제 데이터 연결
        _paletteListView.bindItem = (element, index) =>
        {
            var obj = _paletteItemList[index];

            var objPreview = element.Q<VisualElement>("ObjectPreview");
            var selectBtn = element.Q<Button>("SelectBtn");
            var removeBtn = element.Q<Button>("RemoveBtn");
            
            selectBtn.clicked -= selectBtn.userData as Action;
            removeBtn.clicked -= removeBtn.userData as Action;
            
            // TODO : 버튼 클릭 함수 등록
        };

        _paletteListView.fixedItemHeight = 100f;
    }

    private void CreatePaletteItem()
    {
        if (_selectedObj.value == null)
        {
            return;
        }
        
        _paletteItemList.Add(_selectedObj.value as GameObject);
        _paletteListView.Rebuild();
    }
}
