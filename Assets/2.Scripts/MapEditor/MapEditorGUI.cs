using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UXML의 Query, 버튼 이벤트 연결 등
/// </summary>
public class MapEditorGUI
{
    private readonly VisualElement _visualElement;
    private readonly MapEditor _mapEditor;
    private PaletteGUI _paletteGUI;
    
    // UXML Libray
    private VisualElement _selectedObjPreview;
    private ObjectField _selectedObj;
    
    public MapEditorGUI(VisualElement visualTree, MapEditor mapEditor)
    {
        _visualElement = visualTree;
        _mapEditor = mapEditor;
    }

    public void CreateMapEditorGUI()
    {
        CreatePalletUI(_visualElement);
        ObjectFieldSetUp(_visualElement);
        ButtonActionBinding(_visualElement);
    }
    
    private void CreatePalletUI(VisualElement root)
    {
        var listView = root.Q<ListView>("PaletteList");
        _paletteGUI = new PaletteGUI(listView);
        
        _paletteGUI.SetupListView();
    }

    private void ObjectFieldSetUp(VisualElement root)
    {
        _selectedObjPreview = root.Q<VisualElement>("PreviewTexture");
        _selectedObj = root.Q<ObjectField>("ObjectSelectField");
        
        _selectedObj.RegisterValueChangedCallback(RegisterSelectedObjectField);
    }

    private void RegisterSelectedObjectField(ChangeEvent<Object> evt)
    {
        var obj = evt.newValue as GameObject;
        if (obj == null) return;

        var tex = ObjectPreview.GetObjectPreview(obj);
        _selectedObjPreview.style.backgroundImage = new StyleBackground(tex);
    }

    private void ButtonActionBinding(VisualElement root)
    {
        var gridCreateBtn = root.Q<Button>("CreateGridBtn");
        var gridRemoveBtn = root.Q<Button>("DestroyGridBtn");
        var addPaletteBtn = root.Q<Button>("AddPaletteBtn");
        
        gridCreateBtn.clicked += _mapEditor.CreateGridTile;
        gridRemoveBtn.clicked += _mapEditor.DestroyGrid;
        addPaletteBtn.clicked += AddPaletteItem;
    }

    private void AddPaletteItem()
    {
        if (_selectedObj.value == null) return;
        
        _paletteGUI.CreatePaletteItem(_selectedObj.value as GameObject);
    }
}
