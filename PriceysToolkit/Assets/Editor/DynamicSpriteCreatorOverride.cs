using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DynamicSpriteCreatorOverride : Editor
{
    public static void createCollider(GameObject obj)
    {
        BoxCollider2D BC = obj.AddComponent<BoxCollider2D>();
        BC.size = new Vector2(1, 1);
        BC.autoTiling = true;
    }
}
