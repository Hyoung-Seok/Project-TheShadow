using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MapEditor : EditorWindow
{
    [SerializeField] private VisualTreeAsset visualTreeAsset;

    [MenuItem("Window/MapEditor")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<MapEditor>();
        wnd.titleContent = new GUIContent("MapEditor");
    }

    public void CreateGUI()
    {
        
    }
}
