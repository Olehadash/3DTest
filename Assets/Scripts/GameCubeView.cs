using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCubeView : MonoBehaviour
{
    public CubeModel model;
    Color color;
    private void Start()
    {
        //Debug.Log(" Cube Inialized ");
        gameObject.GetComponent<MeshRenderer>().material = MatrixModel.GetInstance.CubeMaterial;
        if (this.color != null)
            SetColor(this.color);
    }
    private void Update()
    {
        if (model != null)
        {
            /*if(model.matrixPosition.y>0 && !model.IsMove)
                Debug.Log("CubeUpdate : " + model.matrixPosition + " : " + transform.position + " : " + GameFieldController.GetInstance.TransformToRealPosition(model.matrixPosition));*/
             
            transform.position = Vector3.MoveTowards(transform.position,
                GameFieldController.GetInstance.TransformToRealPosition(model.matrixPosition),
                LevelModel.GetInstance.speed * Time.deltaTime);

        }
    }

    public void SetColor(Color color)
    {
        this.gameObject.GetComponent<MeshRenderer>().material.color = color;
        this.color = color;
    }

    public void SetModel(CubeModel m)
    {
        model = m;
        
    }

    public CubeModel GetModel
    {
        get
        {
            return model;
        }
    }

    public void SetMatrixPosition(Vector3Int matrixPosition)
    {
        model.matrixPosition = matrixPosition;
    }

    /*
     * Hide and DestroyCube
     * */
    public void HideCube()
    {
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        model.isAnimated = true;
        while (this.transform.localScale != Vector3.zero)
        {
            this.transform.localScale = this.transform.localScale + new Vector3(-0.025f, -0.025f, -0.025f);
            yield return null;
        }
        GameMatrix.GetMatrix.DropFromMAtrix(this.model.matrixPosition);
        Destroy(this.gameObject);
        LevelModel.GetInstance.Score += 10;
    }









}
