using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FieldView : MonoBehaviour
{
    public Color BG_color, Edge_color;
    bool isActive = false;
    public int idx, idy;
    public Vector3 defaultPosition;
    private bool isBottom = false;
    FieldRotateType type;
    ProectionFieldType side;
    MeshRenderer render;

    public void SavePosition()
    {
        defaultPosition = this.transform.position;
    }

    public void InitFieldPositionDAta(FieldRotateType t, ProectionFieldType s)
    {
        type = t;
        side = s;

        if (side == ProectionFieldType.FORWARD)
        {
            Color old = render.materials[0].color;
            Color color = new Color(old.r, old.g, old.b, 0.08f);
            render.materials[0].color = color;
            render.materials[1].color = color;
        }
    }

    public void SetBottom()
    {
        isBottom = true;
    }
    /*
     * Set Color of Field View
     */
    void SetColor() {
        render = this.gameObject.GetComponent<MeshRenderer>();
        if (isActive)
        {
            render.materials[0].color = Edge_color;
            render.materials[1].color = Edge_color;
        }
        else
        {
            render.materials[0].color = BG_color;
            render.materials[1].color = BG_color;
        }
    }


    /*
     * Set Default Baackground color 
     * */
    public void UnEdge()
    {
        isActive = false;
        SetColor();
        this.transform.position = defaultPosition;
        
    }

    /*
     * Set Color for Choousen Field
     * And if it fild is bottom Set Position Under Puted on this coordinate 
     * */
    public void Edge()
    {
        isActive = true;
        SetColor();
         if(isBottom)
         {
           // Debug.Log("SetBottom : "+ GameFieldModel.GetInstance.height);
            for (int i = GameFieldModel.GetInstance.height-1; i>=0;i--)
            {
                if(GameMatrix.GetMatrix.CubeValidate(new Vector3Int (idx,i,idy)))
                {
                  //Debug.Log("SetAnder");
                    CubeModel model = GameMatrix.GetMatrix.GetCubeModel(new Vector3Int(idx, i, idy));
                    StartCoroutine(WaytForAnimarionFinished(model));
                    //Debug.Log(model.gObject.transform.position + ":" + this.transform.position);
                    break;
                }
            }
         }
    }

    IEnumerator WaytForAnimarionFinished(CubeModel model)
    {
        while (model.isAnimated)
            yield return null;
        this.transform.position = defaultPosition;
        this.transform.position = new Vector3(this.transform.position.x, model.gObject.transform.position.y + 0.6f, this.transform.position.z);
    }

    public void OpacityOn()
    {
        this.gameObject.SetActive(false);
        /*StopCoroutine(OpacityCorutineOFF());
        StartCoroutine(OpacityCorutine());*/
    }

    public void OpacityOff()
    {
        this.gameObject.SetActive(true);
        //SetColor();
        
    }

    IEnumerator OpacityCorutine()
    {
        MeshRenderer render = this.gameObject.GetComponent<MeshRenderer>();
        float alpha = 1;
        while(alpha >.05f)
        {
            Color old = render.materials[0].color;
            Color color = new Color(old.r, old.g, old.b, alpha);
            alpha -= 0.05f;
            render.materials[0].color = color;
            render.materials[1].color = color;
            yield return null;
        }
        
    }

    IEnumerator OpacityCorutineOFF()
    {
        MeshRenderer render = this.gameObject.GetComponent<MeshRenderer>();
        float alpha = render.materials[0].color.a;
        while (alpha < 0.25f)
        {
            Color old = render.materials[0].color;
            Color color = new Color(old.r, old.g, old.b, alpha);
            render.materials[0].color = color;
            render.materials[1].color = color;
            alpha += 0.05f;
            yield return null;
        }
        SetColor();

    }



}
