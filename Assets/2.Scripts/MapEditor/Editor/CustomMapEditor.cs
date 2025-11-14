using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(MapEditor))]
public class CustomMapEditor : Editor
{
    public VisualTreeAsset VisualTreeAsset;
    
    public override VisualElement CreateInspectorGUI()
    {
        var mapEditor = (MapEditor)target;
        var root = new VisualElement();
        VisualTreeAsset.CloneTree(root);

        var gridCreteBtn = root.Q<Button>("CreateGridBtn");
        var gridDestroyBtn = root.Q<Button>("DestroyGridBtn");

        gridCreteBtn.clicked += mapEditor.CreateGridTile;
        gridDestroyBtn.clicked += mapEditor.DestroyGrid;
        
        return root;
    }
}
