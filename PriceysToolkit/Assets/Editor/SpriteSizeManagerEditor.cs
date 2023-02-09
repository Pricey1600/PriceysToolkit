using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SpriteSizeManager))]
[CanEditMultipleObjects]
public class SpriteSizeManagerEditor : Editor
{
    public void OnSceneGUI()
    {
        if (Tools.current == Tool.Scale)
        {
            Tools.hidden = true;
        }
        else
        {
            Tools.hidden = false;
        }


        SpriteSizeManager linkedObject = (target as SpriteSizeManager);
        //var linkedObject = ((SpriteSizeManager)target);
        Handles.color = Color.magenta;

        EditorGUI.BeginChangeCheck();
        Vector3 scale = Handles.ScaleHandle(linkedObject.objectSize, linkedObject.transform.position, Quaternion.identity, HandleUtility.GetHandleSize(Vector3.zero) + 0.1f);
        scale.x = Mathf.Clamp(scale.x, 0.5f, 25f);
        scale.y = Mathf.Clamp(scale.y, 0.5f, 25f);
        ///Vector3 scale = Handles.Slider2D(linkedObject.transform.position, Vector3.right, Vector3.right, Vector3.right, HandleUtility.GetHandleSize(Vector3.zero) + 0.1f, Handles.CubeHandleCap, 0.1f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Scaled Dynamic Object");
            linkedObject.objectSize = scale;
            linkedObject.Update();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SpriteSizeManager manager = Selection.activeGameObject.GetComponent<SpriteSizeManager>();
        if (manager != null && manager.SR.sprite != null)
        {
            if (manager.SR.sprite.pivot.y != 0 || manager.SR.sprite.pivot.x != manager.SR.sprite.texture.width / 2)
            {
                EditorGUILayout.HelpBox("Sprite being used does not have it's pivot set to 'Bottom'", MessageType.Warning, true);
                if(EditorGUILayout.LinkButton("Fix This"))
                {
                    Selection.activeObject = manager.SR.sprite.texture;
                    
                }
            }
        }
    }
}

#endif // UNITY_EDITOR
