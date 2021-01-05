using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BlockController : MonoBehaviour
{
    private static BlockController instance;

    GameObject figure;
    FigureModel model;

    

    private void Awake()
    {
        figure = new GameObject();
        figure.transform.position = new Vector3(0, 0, 0);
        figure.transform.SetParent(this.transform);
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    public static BlockController GetInstance
    {
        get
        {
            return instance;
        }
    }

    public FigureModel GetFigure
    {
        get
        {
            return model;
        }
    }
    
    

    public void BuildBlock()
    {
        model = GameMatrix.GetMatrix.PutIn((FiguraType)UnityEngine.Random.Range(1,BlockModel.GetInstance.Figurs.Length-1));
        StartCoroutine("WatterFall", false);
    }
    
    /*
     * Move Our Figure in GAme Field
     * move - Type of movment LEFT, RIGHT, UP, DOWN, FORWAR, BACK
     * */
    public void MoveInMatrix( MovementVector move)
    {
        //Debug.Log(move);
        //Debug.Log("Move Matrix"+ ":"+ GameMatrix.GetMatrix.CubeValidate(model.cubes));
        if (GameMatrix.GetMatrix.MoveValidate(model.cubes, move))
        {
            TakeFigure();
            for (int i = 0; i < model.cubes.Length; i++)
            {
                if (model.cubes[i] == null)
                {
                    //Debug.Log("NoModels");
                    break;
                }

                GameMatrix.GetMatrix.UnShadow(model.cubes[i].matrixPosition);
                Vector3Int pos = GameMatrix.GetMatrix.GetNewPosition(model.cubes[i], move);
                model.cubes[i].matrixPosition = pos;
                
            }
            RotationMove(move);
            PutFigure();
        }
        if (SoundController.IsInitialized)
            SoundController.GetSingleton.PlaySound(SoundType.Move);
    }

    void RotationMove(MovementVector move)
    {
        if (move == MovementVector.BACK || move == MovementVector.DOWN || move == MovementVector.FORVARD || move == MovementVector.UP || move == MovementVector.LEFT || move == MovementVector.RIGHT)
            return;
        
        StartCoroutine(Rotation(move));
    }

    /*
     * Take From Matrix all Cubes in figure And Set null for his matrix Positon 
     * */
    public void TakeFigure()
    {
        for (int i = 0; i < model.cubes.Length; i++)
        {
            GameMatrix.GetMatrix.UnShadow(model.cubes[i].matrixPosition);
        }
    }
    /*
     * Put Figure in Matrix 
     * */
    public void PutFigure()
    {
        for (int i = 0; i < model.cubes.Length; i++)
        {
            GameMatrix.GetMatrix.Put(model.cubes[i]);
            //kof += "-" + i + "-: " + model.cubes[i].matrixPosition + ":";
        }
        //Debug.Log("Put -> :" + kof);
    }

    /*
    * GEt Centr position of Figure
    * */
    public Vector3Int GetCentrOfFigure
    {
        get
        {
            return model.cubes[0].matrixPosition;
        }
    }

    

    

    IEnumerator Rotation(MovementVector move)
    {
        Vector3 rotate = BlockModel.GetInstance.MoveFloatVector[move];
        int i = 0;
        figure.transform.position = model.cubes[0].gObject.transform.position;
        for ( i = 0; i < model.cubes.Length; i++)
        {
            if (model.cubes[i] == null) break;
            model.cubes[i].gObject.transform.SetParent(figure.transform);
        }
        i = 0;
        while (!(Mathf.Abs(figure.transform.rotation.x) > 0.7f || Mathf.Abs(figure.transform.rotation.y) > 0.7f || Mathf.Abs(figure.transform.rotation.z) > 0.7f))
        {
            figure.transform.Rotate(rotate);
            yield return null;
        }
        for (i = 0; i < model.cubes.Length; i++)
        {
            if (model.cubes[i] == null) break;
            model.cubes[i].gObject.transform.SetParent(figure.transform.parent);

        }
        yield return null;
        AfterRotateCorrection();
    }
    

    public void AfterRotateCorrection()
    {
        figure.transform.rotation = Quaternion.identity;
        for (int i = 0; i < model.cubes.Length; i++)
        {
            
            if (model.cubes[i] == null) break;
            model.cubes[i].gObject.transform.position = GameFieldController.GetInstance.TransformToRealPosition(model.cubes[i].matrixPosition);
            model.cubes[i].gObject.transform.rotation = Quaternion.identity;
        }
    }

    bool IsFigureCanFall()
    {
        for (int i = 0; i < model.cubes.Length; i++)
        {
            if (model.cubes[i].matrixPosition.y == 0
                ||
                GameMatrix.GetMatrix.CubeValidate(model.cubes[i].matrixPosition + new Vector3Int(0, -1, 0)))
            { 
                return false;
            }
        }
        
        return true;
    }

    public void SpeedWaterFall(bool speed)
    {
        //Debug.Log("Spase Fall");
        StopCoroutine("WatterFall");
        StartCoroutine("WatterFall", speed);
    }

    IEnumerator WatterFall(bool speed)
    {
        while (UiLevelInfo.GetInstance.GetIsPause)
        {
            yield return null;
        }
        
        if (!speed)
            yield return new WaitForSeconds(LevelModel.GetInstance.FallDowntime);
        else
            yield return new WaitForSeconds(LevelModel.GetInstance.SpeedFallDowntime);

        //Debug.Log("Speed = " + speed);

        while (UiLevelInfo.GetInstance.GetIsPause)
        {
            yield return null;
        }
        MoveInMatrix(MovementVector.DOWN);
        if(!IsFigureCanFall())
        {
            
            EndFAll();
            //BuildBlock();
        }
        else
        {
           // Debug.Log("Continue");
            StartCoroutine("WatterFall", speed);
            
        }
    }

    void EndFAll()
    {
        //Debug.Log("End FAll");
        for(int i = 0; i<model.cubes.Length;i++)
        {
            GameFieldController.GetInstance.UnShadowOnBoards(model.cubes[i].matrixPosition);
            //model.cubes[i].IsMove = false;
            GameMatrix.GetMatrix.PutInMatrix(model.cubes[i]);
            
        }
        if (SoundController.IsInitialized)
            SoundController.GetSingleton.PlaySound(SoundType.Drop);
        AfterRotateCorrection();
        GameOverMethod();
        StartCoroutine("waitendAnimation");
    }

    void GameOverMethod()
    {
        for(int i = 0; i<model.cubes.Length;i++)
        {
            if(model.cubes[i].matrixPosition.y > 9)
            {
                UiLevelInfo.GetInstance.GameOverOpen();
            }
        }
    }

    IEnumerator waitendAnimation()
    {

        while (model.cubes[0].isAnimated)
            yield return null;
        GameMatrix.GetMatrix.CheckLines();
        BuildBlock();
    }


}
