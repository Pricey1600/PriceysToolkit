using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicSubSprite))]
public class SpriteControlInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DynamicSubSprite manager = (DynamicSubSprite)target;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Sprite Flip");
            if (GUILayout.Button("X"))
            {
                manager.flip("X");
            }
            if (GUILayout.Button("Y"))
            {
                manager.flip("Y");
            }
        }
        GUILayout.EndHorizontal();
        
    }
}
