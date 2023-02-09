using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteObjectCreator : Editor
{
    [SerializeField] public static Sprite defaultParentSprite, defaultChildSprite;

    [MenuItem("GameObject/2D Object/Dynamic Sprite Object")]
    public static void createParent(MenuCommand menuCommand)
    {
        //Debug.Log("Create Parent...");
        GameObject GO = new GameObject("Dynamic Sprite");
        GameObjectUtility.SetParentAndAlign(GO, menuCommand.context as GameObject);

        SpriteRenderer SR = GO.AddComponent<SpriteRenderer>();
        //SR.sprite = defaultParentSprite;
        SR.drawMode = SpriteDrawMode.Tiled;

        //BoxCollider2D BC = GO.AddComponent<BoxCollider2D>();
        //BC.autoTiling = true;

        GO.AddComponent<SpriteSizeManager>();

        Undo.RegisterCreatedObjectUndo(GO, "Create " + GO.name);
        Selection.activeObject = GO;

        checkForAddOns(GO);

    }

    [MenuItem("GameObject/2D Object/Dynamic Sprite Sub-Object")]
    public static void createChild(MenuCommand menuCommand)
    {
        //Debug.Log("Create Child...");
        GameObject GO = new GameObject("Dynamic Sub-Sprite");
        GameObjectUtility.SetParentAndAlign(GO, menuCommand.context as GameObject);

        SpriteRenderer SR = GO.AddComponent<SpriteRenderer>();
        //SR.sprite = defaultChildSprite;
        SR.sortingOrder = 1;

        //BoxCollider2D BC = GO.AddComponent<BoxCollider2D>();
        //BC.autoTiling = true;

        GO.AddComponent<SubSpriteSizeManager>();

        Undo.RegisterCreatedObjectUndo(GO, "Create " + GO.name);
        Selection.activeObject = GO;

        checkForAddOns(GO);
    }

    private static void checkForAddOns(GameObject GO)
    {
        if(typeof(DynamicSpriteCreatorOverride) != null)
        {
            DynamicSpriteCreatorOverride.createCollider(GO);
        }
    }
}
