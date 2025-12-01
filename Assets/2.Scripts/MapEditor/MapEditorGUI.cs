using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

/// <summary>
/// UXML의 Query, 버튼 이벤트 연결 등
/// </summary>
public class MapEditorGUI
{
    #region Event
    public event Action CreateGridTileEvent;
    public event Action DestroyGridTileEvent;
    #endregion
    
    private readonly VisualElement _visualElement;
    private PaletteGUI _paletteGUI;
    
    // UXML Library
    private VisualElement _selectedObjPreview;
    private ObjectField _selectedObj;
    
    public MapEditorGUI(VisualElement visualElement)
    {
        _visualElement = visualElement;
    }

    public void CreateMapEditorGUI()
    {
        CreatePalletUI(_visualElement);
        ObjectFieldSetUp(_visualElement);
    }

    public void RegisterOnPaletteSelect(Action<GameObject> handler)
    {
        _paletteGUI.PaletteItemClickEvent += handler;
    }
    
    public void ButtonActionBinding(VisualElement root)
    {
        var gridCreateBtn = root.Q<Button>("CreateGridBtn");
        var gridRemoveBtn = root.Q<Button>("DestroyGridBtn");
        var addPaletteBtn = root.Q<Button>("AddPaletteBtn");

        gridCreateBtn.clicked += () => CreateGridTileEvent?.Invoke();
        gridRemoveBtn.clicked += () => DestroyGridTileEvent?.Invoke();
        addPaletteBtn.clicked += AddPaletteItem;
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

    private void AddPaletteItem()
    {
        if (_selectedObj.value == null) return;
        
        _paletteGUI.CreatePaletteItem(_selectedObj.value as GameObject);
    }
}
