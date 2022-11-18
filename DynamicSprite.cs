using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
[ExecuteInEditMode]
public class DynamicSprite : MonoBehaviour
{
    [Range(0.5f, 25f)] //change this for larger objects
    public float objectHeight, objectWidth;

    private SpriteRenderer SR;

    public List<DynamicSubSprite> subManagers = new List<DynamicSubSprite>();

    private bool validating;


    //private void Awake()
    //{
    //    Debug.Log("Awake has been called");
    //    SR = this.gameObject.GetComponent<SpriteRenderer>();
    //    objectHeight = SR.size.y;
    //    objectWidth = SR.size.x;
    //    storeSize();
    //    //_subManagers = gameObject.GetComponentsInChildren<DynamicSubSprite>();
    //    //foreach(DynamicSubSprite manager in _subManagers)
    //    //{
    //    //    subManagers.Add(manager);
    //    //}
    //}
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
        foreach(DynamicSubSprite subManager in subManagers)
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
        foreach(DynamicSubSprite manager in subManagers)
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

    public void addMe(DynamicSubSprite subManager)
    {
        if (!subManagers.Contains(subManager))
        {
            subManagers.Add(subManager);
        }
    }
    public void removeMe(DynamicSubSprite subManager)
    {
        if (subManagers.Contains(subManager))
        {
            subManagers.Remove(subManager);
        }
    }

    //public void prepareForDelete()
    //{
    //    foreach(DynamicSubSprite subManager in subManagers)
    //    {
    //        subManager.waitingForDelete = true;
    //    }
    //}
}
#endif //if unity_editor