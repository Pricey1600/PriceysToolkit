using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
[ExecuteInEditMode]
public class SpriteSizeManager : MonoBehaviour
{
    //[Range(0.5f, 25f)] //change this for larger objects
    [HideInInspector] public float objectHeight, objectWidth;
    public Vector3 objectSize = Vector3.one;

    [HideInInspector] public SpriteRenderer SR;

    public List<SubSpriteSizeManager> subManagers = new List<SubSpriteSizeManager>();

    private bool validating;
    private void OnEnable()
    {
        //Debug.Log("OnEnable has been called");
        SR = this.gameObject.GetComponent<SpriteRenderer>();
        objectHeight = SR.size.y;
        objectWidth = SR.size.x;

        if(SR.drawMode != SpriteDrawMode.Tiled)
        {
            SR.drawMode = SpriteDrawMode.Tiled;
        }
    }
    private void OnDestroy()
    {
        foreach(SubSpriteSizeManager subManager in subManagers)
        {
            DestroyImmediate(subManager);
        }
    }


    private void OnValidate()
    {
        //Debug.Log("OnValidate Called");
        if (Application.isEditor)
        {
            EditorApplication.update += _OnValidate;
            return;
        }
        
        
    }
    private void _OnValidate()
    {
        EditorApplication.update -= _OnValidate;
        setSize();
        changeSubManagerValues();
        

    }

    private void changeSubManagerValues()
    {
        if(subManagers.Count == 0)
        {
            return;
        }
        foreach(SubSpriteSizeManager manager in subManagers)
        {
            manager.adapt(objectWidth, objectHeight);
        }

    }

    private void setSize()
    {
        //Debug.Log("Setting Size");
        if (SR == null)
        {
            return;
        }
        SR.size = new Vector2(objectWidth, objectHeight);
    }

    public void addMe(SubSpriteSizeManager subManager)
    {
        if (!subManagers.Contains(subManager))
        {
            subManagers.Add(subManager);
        }
    }
    public void removeMe(SubSpriteSizeManager subManager)
    {
        if (subManagers.Contains(subManager))
        {
            subManagers.Remove(subManager);
        }
    }
    public void Update()
    {
        if(Selection.activeObject != this.gameObject)
        {
            return;
        }

        objectWidth = objectSize.x;
        objectHeight = objectSize.y;
        setSize();
        changeSubManagerValues();

    }

}
#endif //if unity_editor

