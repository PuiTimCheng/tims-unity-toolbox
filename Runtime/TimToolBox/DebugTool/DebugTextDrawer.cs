using System;
using Sirenix.OdinInspector;
using TimToolBox.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class DebugTextDrawer : MonoBehaviour
{
    [TextArea] public string _text = "Debug Text";
    [SerializeField] private bool showOnlyWhenSelected = false;
    [SerializeField] private int fontSize = 12;
    [SerializeField] private Vector3 _offset = Vector3.up;
    [SerializeField] private Color _backgroundColor = Color.black.SetAlpha(0.5f);
    
    private Texture2D _backgroundTexture;

    private void OnDrawGizmos()
    {
        if(!showOnlyWhenSelected) Draw();
    }

    private void OnDrawGizmosSelected()
    {
        if(showOnlyWhenSelected) Draw();
    }

    private void Draw()
    {
        if (_backgroundTexture == null || _backgroundTexture.GetPixel(0, 0) != _backgroundColor)
        {
            _backgroundTexture = CreateTexture(_backgroundColor);
        }

        var style = new GUIStyle
        {
            normal = { textColor = Color.white, background = _backgroundTexture },
            hover = { textColor = Color.yellow, background = _backgroundTexture },
            fontSize = fontSize,
        };
        Handles.Label(transform.position + _offset, _text, style);
    }
    
    private Texture2D CreateTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

}