using UnityEditor;
using UnityEngine;

public static class ObjectPreview
{
    public static Texture2D GetObjectPreview(GameObject obj, int width = 150, int height = 150)
    {
        var path = AssetDatabase.GetAssetPath(obj);

        var editor = Editor.CreateEditor(obj);
        var tex = editor.RenderStaticPreview(path, null, width, height);
        Object.DestroyImmediate(editor);

        return tex;
    }
}
