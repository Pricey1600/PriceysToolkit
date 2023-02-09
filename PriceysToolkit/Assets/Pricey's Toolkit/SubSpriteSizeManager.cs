using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
[ExecuteInEditMode]
public class SubSpriteSizeManager : MonoBehaviour
{
    public enum AnchorPoint{TopLeft, TopCenter, TopRight, RightCenter, BottomLeft, BottomCenter, BottomRight, LeftCenter};
    [SerializeField]private AnchorPoint Anchor;

    public enum ElementType { Stretch, Static };
    [SerializeField] private ElementType Type;

    public float horizontalOffset, verticalOffset;

    private SpriteRenderer SR;
    private GameObject GO;

    private float parentWidth, parentHeight;
    private float leftDis, topDis, rightDis, bottomDis;
    private Vector3 prevPos;
    private float prevRelativePosY, prevRelativePosX;

    [HideInInspector] public bool inPrefabStage = false;
    private void Awake()
    {
        //Debug.Log("Setting GO");
        GO = this.gameObject;
        SR = GO.GetComponent<SpriteRenderer>();

    }

    private void OnEnable()
    {
        GO.GetComponentInParent<SpriteSizeManager>().addMe(this);
        prevPos = GO.transform.localPosition;
        askForParentSize();
        checkDistances();
    }
    void OnDisable()
    {
        //if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null && GO.transform.parent != null)
        //{
            
        //}
        try
        {
            GO.GetComponentInParent<SpriteSizeManager>().removeMe(this);
        }
        catch
        {
            return;
        }
        

    }

    public void adapt(float _parentWidth, float _parentHeight)
    {
        //Debug.Log("Adapting...");
        if(GO == null)
        {
            return;
        }
        if (GO.transform.localPosition != prevPos)
        {
            
            prevPos = GO.transform.localPosition;
            prevRelativePosY = prevPos.y - (parentHeight/2);
            prevRelativePosX = prevPos.x;
            checkDistances();
        }

        parentWidth = _parentWidth;
        parentHeight = _parentHeight;
        
        if (Type == ElementType.Static) //don't want to edit scale if static
        {

            if (SR.drawMode != SpriteDrawMode.Simple)
            {
                SR.drawMode = SpriteDrawMode.Simple;
            }

            GO.transform.localPosition = findNewPos();
            checkDistances();


        }
        if (Type == ElementType.Stretch) //want to edit width dynamically
        {

            if(SR.drawMode != SpriteDrawMode.Tiled)
            {
                SR.drawMode = SpriteDrawMode.Tiled;
            }
            if(Anchor == AnchorPoint.TopCenter || Anchor == AnchorPoint.TopLeft || Anchor == AnchorPoint.TopRight)
            {
                GO.transform.localPosition = new Vector2(0 ,findNewPos().y + verticalOffset);
                SR.size = new Vector2((parentWidth / GO.transform.localScale.x) + horizontalOffset, SR.size.y);
            }
            else if(Anchor == AnchorPoint.LeftCenter)
            {
                GO.transform.localPosition = new Vector2(findNewPos().x + horizontalOffset, 0);
                SR.size = new Vector2(SR.size.x,(parentHeight / GO.transform.localScale.y) + verticalOffset);
            }
            else if (Anchor == AnchorPoint.RightCenter)
            {
                GO.transform.localPosition = new Vector2(findNewPos().x + horizontalOffset, 0);
                SR.size = new Vector2(SR.size.x, (parentHeight / GO.transform.localScale.y) + verticalOffset);
            }
            else if (Anchor == AnchorPoint.BottomCenter || Anchor == AnchorPoint.BottomLeft || Anchor == AnchorPoint.BottomRight)
            {
                GO.transform.localPosition = new Vector2(0, findNewPos().y + verticalOffset);
                SR.size = new Vector2((parentWidth / GO.transform.localScale.x) + horizontalOffset, SR.size.y);
            }

        }
        
        return;

    }
    
    private Vector2 findNewPos()
    {
        if(Anchor == AnchorPoint.TopLeft)
        {
            float posX = (-parentWidth / 2) - (leftDis);
            float posY = (parentHeight) - (topDis);

            return new Vector2(posX, posY);
        }
        else if (Anchor == AnchorPoint.TopRight)
        {
            float posX = (parentWidth / 2) - (rightDis);
            float posY = (parentHeight) - (topDis);

            return new Vector2(posX, posY);
        }
        else if (Anchor == AnchorPoint.BottomRight)
        {
            float posX = (parentWidth / 2) - (rightDis);
            //float posY = (-parentHeight) - (bottomDis);
            float posY = bottomDis;

            return new Vector2(posX, posY);
        }
        else if (Anchor == AnchorPoint.BottomLeft)
        {
            float posX = (-parentWidth / 2) - (leftDis);
            //float posY = (-parentHeight) - (bottomDis);
            float posY = bottomDis;

            return new Vector2(posX, posY);
        }
        else if (Anchor == AnchorPoint.BottomCenter)
        {

            float posX = prevRelativePosX;
            //float posY = (-parentHeight) - (bottomDis);
            float posY = bottomDis;

            return new Vector2(posX, posY);
        }
        else if (Anchor == AnchorPoint.TopCenter)
        {
            float posX = prevRelativePosX;
            float posY = (parentHeight) - (topDis);

            return new Vector2(posX, posY);
        }
        else if (Anchor == AnchorPoint.LeftCenter)
        {
            float posX = (-parentWidth / 2) - (leftDis);
            float posY = (parentHeight / 2) + prevRelativePosY;

            return new Vector2(posX, posY);
        }
        else if (Anchor == AnchorPoint.RightCenter)
        {
            float posX = (parentWidth / 2) - (rightDis);
            float posY = (parentHeight/2) + prevRelativePosY;

            return new Vector2(posX, posY);
        }
        return new Vector2(0,0);
        //float posX = (parentWidth / 2) - findDifference().x;
        //float posY = (parentHeight / 2) - findDifference().y;


    } //find new position after parent has been edited

    private void checkDistances() //update distances
    {
        leftDis = (-parentWidth / 2) - (GO.transform.localPosition.x);
        rightDis = (parentWidth / 2) - (GO.transform.localPosition.x);
        topDis = (parentHeight) - (GO.transform.localPosition.y);
        bottomDis = 0 + (GO.transform.localPosition.y);
    }

    private void askForParentSize()
    {
        parentWidth = GO.GetComponentInParent<SpriteSizeManager>().objectWidth;
        parentHeight = GO.GetComponentInParent<SpriteSizeManager>().objectHeight;
    }

    public void flip(string axis)
    {
        if(axis == "X")
        {
            //Debug.Log("Flipped X");
            SR.flipX = !SR.flipX;
        }
        else if (axis == "Y")
        {
            //Debug.Log("Flipped Y");
            SR.flipY = !SR.flipY;
        }

    }

    private void changeStage()
    {
        inPrefabStage = false;
    }
}
#endif //if unity_editor