using System;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    
    private MaterialPropertyBlock _mpb;
    private Color _originColor;
    
    private static readonly int BASE_COLOR_ID = Shader.PropertyToID("_BaseColor");
    
    public void SetAlpha(bool isSelected)
    {
        if (_mpb == null)
        {
            _mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(_mpb);

            _originColor = renderer.sharedMaterial.GetColor(BASE_COLOR_ID);
        }

        var color = _originColor;
        color.a = (isSelected) ? 1 : 0.2f;

        _mpb.SetColor(BASE_COLOR_ID, color);
        renderer.SetPropertyBlock(_mpb);
    }
}
