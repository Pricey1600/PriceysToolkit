using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PriceysTools : EditorWindow
{
    Texture2D headerSectionTexture, meshRendererSectionTexture;
    Color headerSectionColour = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f), meshRendererSectionColour = new Color(255f / 255f, 0 / 255f, 0 / 255f, 1f);
    Rect headerSection, meshRendererSection;

    public Material newMat;

    [MenuItem("Window/Priceys Toolkit")]
    static void OpenWindow()
    {
        PriceysTools window = (PriceysTools)GetWindow(typeof(PriceysTools));
        window.minSize = new Vector2(600, 300);
        window.Show();

    }

    private void OnEnable()
    {
        InitTextures();
    }
    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawMeshRendererSelection();

        


    }

    void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        meshRendererSection.x = 0;
        meshRendererSection.y = 50;
        meshRendererSection.width = Screen.width / 3f;
        meshRendererSection.height = Screen.height - 50f;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(meshRendererSection, meshRendererSectionTexture);
    }

    void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColour);
        headerSectionTexture.Apply();

        meshRendererSectionTexture = new Texture2D(1, 1);
        meshRendererSectionTexture.SetPixel(0, 0, meshRendererSectionColour);
        meshRendererSectionTexture.Apply();
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 40, fontStyle = FontStyle.Bold };
        GUILayout.Label("Pricey's Toolkit", style, GUILayout.ExpandWidth(true));

        GUILayout.EndArea();
    }

    void DrawMeshRendererSelection()
    {
        GUILayout.BeginArea(meshRendererSection);

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fontStyle = FontStyle.Bold };
        GUILayout.Label("MeshRenderer Selection", style, GUILayout.ExpandWidth(true));

        if (GUILayout.Button("Select MeshRenderers", GUILayout.Height(40)))
        {
            SelectRenderers(false);
        }

        newMat = (Material)EditorGUILayout.ObjectField("Material", newMat, typeof(Material));
        if (GUILayout.Button("Select MeshRenderers & Replace", GUILayout.Height(40)))
        {
            SelectRenderers(true);
        }

        GUILayout.EndArea();
    }

    private void SelectRenderers(bool replace)
    {
        Transform[] objs = Selection.transforms;

        List<MeshRenderer> childMRs = new List<MeshRenderer>();
        List<GameObject> childObjs = new List<GameObject>();

        foreach (Transform parent in objs)
        {
            MeshRenderer[] MRs = parent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer child in MRs)
            {
                childMRs.Add(child);
                childObjs.Add(child.gameObject);
            }

        }
        if (replace)
        {
            foreach (MeshRenderer renderer in childMRs)
            {
                renderer.material = newMat;
            }
        }
        Selection.objects = childObjs.ToArray();


    }
}


