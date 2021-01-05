using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProectionFieldView : MonoBehaviour
{
    public Transform field;
    int xsize, ysize;
    FieldRotateType type;
    ProectionFieldType side;
    List<List<FieldView>> fields = new List<List<FieldView>>();

    private RaycastHit vision;

    public void Init(ProectionFieldType side, FieldRotateType rotate)
    {
        Vector3 pos = GetProectionFieldPosition(side);
        Build();
        SetRotationType(rotate);
        this.transform.position = pos;
        this.side = side;
        SavePosition();

    }

    void SavePosition()
    {
        for (int i = 0; i < xsize; i++)
        {
            for (int j = 0; j < ysize; j++)
            {
                if (side == ProectionFieldType.BOTTOM)
                    fields[i][j].SetBottom();
                fields[i][j].SavePosition();
                fields[i][j].UnEdge();
                fields[i][j].InitFieldPositionDAta(this.type, this.side);
            }
        }
    }

    public Vector3 GEtPosition(int x, int y)
    {
        //Debug.Log(y + " : "+ ysize + " : " + x + "  ->  " + (y * ysize + x) + " = " + fields.Count);
        return fields[x][y].transform.position;
    }
    /*
     * Build Proection field, size of roection field we takoke from Model;
     */
     void Build()
    {
        for(int i = 0;i <xsize; i++)
        {
            fields.Add(new List<FieldView>());
            for (int j = 0; j<ysize;j++)
            {
                Transform element = Instantiate(field, new Vector3(i, j, 0), Quaternion.identity);
                element.SetParent(this.transform);
                element.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                fields[i].Add(element.GetComponent<FieldView>());
                fields[i][j].idx = i;
                fields[i][j].idy = j;
                
            }
        }
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(CameraScript.GetInstance.CentralObject.transform.position);
        RaycastHit hit = new RaycastHit();
        //Debug.DrawRay(CameraScript.GetInstance.transform.position, -CameraScript.GetInstance.CentralObject.transform.TransformDirection(CameraScript.GetInstance.transform.position)  , Color.red);
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(CameraScript.GetInstance.transform.position, -CameraScript.GetInstance.CentralObject.transform.TransformDirection(CameraScript.GetInstance.transform.position) , out hit, Mathf.Infinity))
        {
            
            
            if (hit.collider.gameObject.name == this.gameObject.name)
            {
                Debug.Log(side);
                CameraScript.GetInstance.SetCurentType(side);
                OposictyOn();
            }
            else
            {
                OposictyOff();
            }
        }
    }
    /*
     * Opacity Regulal Methods
     * 
     * */
    public void OposictyOn()
    {
        for (int i = 0; i < fields.Count; i++)
        {
            for (int j = 0; j < fields[i].Count; j++)
            {
                fields[i][j].OpacityOn();
            }
        }
    }
    public void OposictyOff()
    {
        for (int i = 0; i < fields.Count; i++)
        {
            for (int j = 0; j < fields[i].Count; j++)
            {
                fields[i][j].OpacityOff();
            }
        }
    }
    /*How we need to rotate Our Field
     * */
    void SetRotationType(FieldRotateType type)
    {
        this.type = type;
        switch (type)
        {
            case FieldRotateType.HIGH:
                this.transform.Rotate(new Vector3(-90, 0, 0));
                break;
            case FieldRotateType.DEPTH:
                this.transform.Rotate(new Vector3(0, 0, 0));
                break;
            case FieldRotateType.SIDE:
                this.transform.Rotate(new Vector3(0, 90, 0));
                break;
        }

    }

    
    Vector3 GetProectionFieldPosition(ProectionFieldType side)
    {
        if (!GameFieldModel.IsInitialized) return Vector3.zero;
        switch (side)
        {
            case ProectionFieldType.BOTTOM:
                
                xsize = GameFieldModel.GetInstance.width;
                ysize = GameFieldModel.GetInstance.depth;
                return new Vector3(-(GameFieldModel.GetInstance.width / 2), -(GameFieldModel.GetInstance.height/2+1), (GameFieldModel.GetInstance.depth/2 + 2.5f));
                break;
            case ProectionFieldType.TOP:
                xsize = GameFieldModel.GetInstance.width;
                ysize = GameFieldModel.GetInstance.depth;
                return new Vector3(-(GameFieldModel.GetInstance.width / 2), (GameFieldModel.GetInstance.height / 2 ), (GameFieldModel.GetInstance.depth / 2 + 2.5f));
                break;
            case ProectionFieldType.BACK:
                xsize = GameFieldModel.GetInstance.width;
                ysize = GameFieldModel.GetInstance.height;
                return new Vector3(-(GameFieldModel.GetInstance.width / 2), -(GameFieldModel.GetInstance.height / 2 + 0.5f), (GameFieldModel.GetInstance.depth / 2 + 2.5f) + .5f);
                break;
            case ProectionFieldType.FORWARD:
                xsize = GameFieldModel.GetInstance.width;
                ysize = GameFieldModel.GetInstance.height;
                return new Vector3(-(GameFieldModel.GetInstance.width / 2), -(GameFieldModel.GetInstance.height / 2 + 0.5f), -(GameFieldModel.GetInstance.depth / 2 - 2f));
                break;
            case ProectionFieldType.LEFT:
                xsize = GameFieldModel.GetInstance.depth;
                ysize = GameFieldModel.GetInstance.height;
                return new Vector3(-(GameFieldModel.GetInstance.width / 2 + .5f), -(GameFieldModel.GetInstance.height / 2 + 0.5f), (GameFieldModel.GetInstance.depth / 2 + 2.5f));
                break;
            case ProectionFieldType.RIGHT:
                xsize = GameFieldModel.GetInstance.depth;
                ysize = GameFieldModel.GetInstance.height;
                return new Vector3(GameFieldModel.GetInstance.width / 2 + .5f, -(GameFieldModel.GetInstance.height / 2 + 0.5f), (GameFieldModel.GetInstance.depth / 2 + 2.5f));
                break;
        }
        return Vector3.zero;
    }


    public void SetShadowView(Vector3Int pos)
    {
        //Debug.Log("Shadow: "+pos);
        switch (type)
        {
            case FieldRotateType.HIGH:
                fields[pos.x][pos.z].Edge();
                
                break;
            case FieldRotateType.DEPTH:
                fields[pos.x][pos.y].Edge();
                break;
            case FieldRotateType.SIDE:
                fields[pos.z][pos.y].Edge();
                //Debug.Log(pos.z + " : " + pos.y + " = " + (pos.z * this.ysize + pos.y));
                break;
        }
    }

    public void UnShdowView(Vector3Int pos)
    {        

        switch (type)
        {
            case FieldRotateType.HIGH:
                fields[pos.x][pos.z].UnEdge();
                break;
            case FieldRotateType.DEPTH:
                fields[pos.x][pos.y].UnEdge();
                break;
            case FieldRotateType.SIDE:
                fields[pos.z][pos.y].UnEdge();
                //Debug.Log("Un Shdow: " + pos+" : "+ pos.y + " : " + pos.z + " = " + (pos.y * this.xsize + pos.z));
                break;
        }
    }
}
