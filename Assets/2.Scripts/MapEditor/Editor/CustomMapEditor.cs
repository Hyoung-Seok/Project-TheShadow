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
        var root = new VisualElement();
        VisualTreeAsset.CloneTree(root);

        return root;
    }
}
