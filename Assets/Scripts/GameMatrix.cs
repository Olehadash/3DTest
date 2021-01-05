using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMatrix : MonoBehaviour
{
    private static GameMatrix instance;


    // Singltone initialization
    private void Awake()
    {
        instance = this;
    }
    /*
     * Singltone
     */
    public static GameMatrix GetMatrix
    {
        get
        {
            return instance;
        }
    }

    public static bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }

    // Cube Matrix initialization
    void Start()
    {
        
        
    }
    /*
     * Create Figure and All Cubes inside
     * And 
     */
    public FigureModel PutIn(FiguraType type)
    {
        FigureModel model = BlockModel.GetInstance.Figurs[(int)type];
        model.cubes = new CubeModel[model.BuildMatrix.Length + 1];
        for (int i = 0; i < model.BuildMatrix.Length + 1; i++)
        {
            Vector3Int buildVector = Vector3Int.zero;
            if (i > 0) buildVector = model.BuildMatrix[i - 1];
            model.cubes[i] = new CubeModel();
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube";
            model.cubes[i].matrixPosition = MatrixModel.GetInstance.startPosition + buildVector;
            cube.transform.position = GameFieldController.GetInstance.TransformToRealPosition(model.cubes[i].matrixPosition);
            model.cubes[i].gObject = cube.AddComponent<GameCubeView>();
            cube.transform.SetParent(transform);
            model.cubes[i].gObject.SetModel(model.cubes[i]);
            model.cubes[i].gObject.SetColor(model.color);
            Put(model.cubes[i]);
            //Debug.Log("Set cube " + model.cubes[i].matrixPosition);
        }

        return model;
    }

    /*
     * Put Elemet inside of Game Matrix (of CudeModels)
     * model - Element of our Game Matrix;
     */
    public void Put(CubeModel model)
    {
        if (!OutOfBondMatrixValidate(model.matrixPosition)) return;
        GameFieldController.GetInstance.SetShadowOnBoards(model.matrixPosition);
    }
    /*
     * Put our Cube Medel in our Matrix with;
     * 
     * */
    public void PutInMatrix(CubeModel model)
    {
        if (OutOfBondMatrixValidate(model.matrixPosition))
        {
            //Debug.Log(model.matrixPosition +":" + OutOfBondMatrixValidate(model.matrixPosition));
            MatrixModel.GetInstance.SetMatrixPArametr(model.matrixPosition, model);
        }
    }

    /*
     * Check Validation and Disable Shadow of Proection position, 
     * matrixPosition - Position that we need to check
     */
    public void UnShadow (Vector3Int matrixPosition)
    {
        //Debug.Log("Take");
        if (!OutOfBondMatrixValidate(matrixPosition)) return;
        
        GameFieldController.GetInstance.UnShadowOnBoards(matrixPosition);
    }
    /*
     * Get Element from cube Matrix
     * */
    public CubeModel GetCubeModel(Vector3Int matrixPostion)
    {
        return MatrixModel.GetInstance.GetCubeModel(matrixPostion);
    }
    /*
     * Delete Element from cube Matrix
     * */
    public CubeModel DropFromMAtrix(Vector3Int matrixPosition)
    {
        if (!CubeValidate(matrixPosition)) return null;
        CubeModel model = MatrixModel.GetInstance.GetCubeModel(matrixPosition);
        MatrixModel.GetInstance.SetMatrixPArametr(matrixPosition, null);
        return model;
    }

    /* 
     * 
     * All Cube Expectable position Validation Methods
     * OutOfBondMatrixValidate - Check if our cube inside or not inside Our matrix 
     * if inside return true
     * else false
     * 
     */
    public bool OutOfBondMatrixValidate(Vector3Int pos, bool blink = false)
    {
        
        bool b =  ((pos.x >= 0) && (pos.x < GameFieldModel.GetInstance.width) && (pos.y >= 0) 
            && (pos.y < GameFieldModel.GetInstance.height) 
            && (pos.z >= 0) && (pos.z < GameFieldModel.GetInstance.depth));

        if (blink && !b) Debug.Log("Out of Bound : " + pos);

        return b;
    }

    /* Validation have Any Cube in this position
     * matrixPosition - Check Position
     * blink - Ned to show Log?
     * */
    public bool CubeValidate(Vector3Int matrixPosition, bool blink = false)
    {
        if (OutOfBondMatrixValidate(matrixPosition, blink))
        {
            if(blink)Debug.Log("Cube In position:" + matrixPosition+ " : "+ (GetCubeModel(matrixPosition)!= null));
            return GetCubeModel(matrixPosition) != null;
        }
        if (blink)
            Debug.Log("No Cube In position" + matrixPosition);
        return false;
    }
    public bool CubeValidate(CubeModel model)
    {
        return MatrixModel.GetInstance.GetCubeModel(model.matrixPosition) != null;
    }
    public bool CubeValidate(CubeModel[] models)
    {
        for (int i = 0; i < models.Length; i++)
        {
            if (models[i] == null) return false;
            if (!OutOfBondMatrixValidate(models[i].matrixPosition)) return false;
                return MatrixModel.GetInstance.GetCubeModel(models[i].matrixPosition) != null;
        }

        return false;
    }


    /*
     * Check If Move is avaleble
     * */

    public bool MoveValidate(CubeModel[] models, MovementVector move, bool blink = false)
    {
        for (int i = 0; i < models.Length; i++)
        {
            if (!MoveValidate(models[i], move, blink)) return false;
        }
        return true;
    }
    public bool MoveValidate(CubeModel model, MovementVector move, bool blink = false)
    {
        if (model == null) return false;
        if (blink) Debug.Log(!OutOfBondMatrixValidate(GetNewPosition(model, move)) + " : " + CubeValidate(GetNewPosition(model, move)));
        if (!OutOfBondMatrixValidate(GetNewPosition(model, move))
            || CubeValidate(GetNewPosition(model, move)))
        {
            return false;
        }
        return true;
    }
    

    /*
     * Get New Position For CubeModel in matrix
     * */
    public Vector3Int GetNewPosition(CubeModel model, MovementVector move)
    {
        if(BlockModel.GetInstance.MoveInMatrixVectrors[move] != Vector3Int.zero)
            return model.matrixPosition + BlockModel.GetInstance.MoveInMatrixVectrors[move];
        /*Return position if we wont to rotate fegure*/
        return CalculateRotationVector(model, move);
    }

    /*
     * Calculate After Position in Martrix When Rotation animation Will be finish. 
     * */

    public Vector3Int CalculateRotationVector(CubeModel cube, MovementVector move)
    {
        Vector3Int diference = BlockController.GetInstance.GetCentrOfFigure - cube.matrixPosition;
        Vector2Int v;
        Vector3Int pos = Vector3Int.zero;
        
        switch (move)
        {
            case MovementVector.XROTATE:
                v = new Vector2Int(diference.y, diference.z);
                if ((diference.y == -1 && diference.z == 1) || (diference.y == 1 && diference.z == -1))
                {
                    pos = new Vector3Int(0, -BlockModel.GetInstance.RotationPosChange[v].x, BlockModel.GetInstance.RotationPosChange[v].y);
                }
                else
                {
                    pos = new Vector3Int(0, BlockModel.GetInstance.RotationPosChange[v].y, BlockModel.GetInstance.RotationPosChange[v].x);
                }
                break;
            case MovementVector.XROTATEBACK:
                v = new Vector2Int(diference.z, diference.y);
                if ((diference.y == -1 && diference.z == 1) || (diference.y == 1 && diference.z == -1))
                {
                    pos = new Vector3Int(0, -BlockModel.GetInstance.RotationPosChange[v].y, BlockModel.GetInstance.RotationPosChange[v].x);
                }
                else
                {
                    pos = new Vector3Int(0, BlockModel.GetInstance.RotationPosChange[v].x, BlockModel.GetInstance.RotationPosChange[v].y);
                }
                break;
            case MovementVector.YROTATE:
                v = new Vector2Int(diference.x, diference.z);
               
                if ((diference.x == -1 && diference.z == 1) || (diference.x == 1 && diference.z == -1))
                {
                    pos = new Vector3Int(-BlockModel.GetInstance.RotationPosChange[v].x, 0, BlockModel.GetInstance.RotationPosChange[v].y);
                }
                else
                {
                    pos = new Vector3Int(BlockModel.GetInstance.RotationPosChange[v].y, 0, BlockModel.GetInstance.RotationPosChange[v].x);
                }
                break;
            case MovementVector.YROTATEBACK:
                v = new Vector2Int(diference.x, diference.z);
                if ((diference.x == -1 && diference.z == 1) || (diference.x == 1 && diference.z == -1))
                {
                    pos = new Vector3Int(-BlockModel.GetInstance.RotationPosChange[v].y, 0, BlockModel.GetInstance.RotationPosChange[v].x);
                }
                else
                {
                    pos = new Vector3Int(BlockModel.GetInstance.RotationPosChange[v].x, 0, -BlockModel.GetInstance.RotationPosChange[v].y);
                }
                break;
            case MovementVector.ZROTATE:
                v = new Vector2Int(diference.x, diference.y);
                if ((diference.x == -1 && diference.y == 1) || (diference.x == 1 && diference.y == -1))
                {
                    
                    pos = new Vector3Int(BlockModel.GetInstance.RotationPosChange[v].y, BlockModel.GetInstance.RotationPosChange[v].x, 0);
                }
                else
                {
                    pos = new Vector3Int(BlockModel.GetInstance.RotationPosChange[v].x, -BlockModel.GetInstance.RotationPosChange[v].y, 0);
                }
                break;
            case MovementVector.ZROTATEBACK:
                v = new Vector2Int(diference.x, diference.y);

                if (diference.x == diference.y)
                {
                    //Debug.Log("Z UP");
                    pos = new Vector3Int(-BlockModel.GetInstance.RotationPosChange[v].y, BlockModel.GetInstance.RotationPosChange[v].x, 0);
                }
                else if((diference.x == -1 && diference.y == 1) || (diference.x == 1 && diference.y == -1))
                {

                    pos = new Vector3Int(-BlockModel.GetInstance.RotationPosChange[v].x, -BlockModel.GetInstance.RotationPosChange[v].y, 0);
                }
                else
                {
                    pos = new Vector3Int(BlockModel.GetInstance.RotationPosChange[v].y, BlockModel.GetInstance.RotationPosChange[v].x, 0);
                }
                break;
        }
        //Debug.Log(cube.matrixPosition + pos);
        return cube.matrixPosition+pos;
    }


    /*
     * After Figure FellDown We chwck Is All line of CubeMatrix Is Full
     * if yes Start CubeHideAnimation
     * */

    public void CheckLines()
    {
        for(int y = 0; y< GameFieldModel.GetInstance.height;y++)
        {
            //Debug.Log(checkline(y));
            if(checkline(y))
            {
                //Debug.Log("checkline(" +y+") - True");
                if (SoundController.IsInitialized)
                    SoundController.GetSingleton.PlaySound(SoundType.Success);
                OnHideAnimation(y);

            }
        }
    }

    void OnHideAnimation(int y)
    {
        for (int x = 0; x < GameFieldModel.GetInstance.width; x++)
        {
            for (int z = 0; z < GameFieldModel.GetInstance.depth; z++)
            {
                MatrixModel.GetInstance.GetCubeModel(new Vector3Int( x, y, z)).gObject.HideCube();
                StartCoroutine(AfterHideFallDown(x,y+1,z));
            }
        }

    }

    IEnumerator AfterHideFallDown(int x, int y, int z, bool isfirst = true)

    {
        if(isfirst)
        {
            while (CubeValidate(new Vector3Int(x,y-1,z)))
                yield return null;
        }
        CubeModel model = GetCubeModel(new Vector3Int(x, y, z));
        //Debug.Log(CubeValidate(new Vector3Int(x, y, z)) && MoveValidate(model, MovementVector.DOWN));
        if (CubeValidate(new Vector3Int(x,y,z)) && MoveValidate(model, MovementVector.DOWN ))
        {

            //Debug.Log(model.matrixPosition + " : " + GetNewPosition(model, MovementVector.DOWN));
            model.matrixPosition = GetNewPosition(model, MovementVector.DOWN);
            PutInMatrix(DropFromMAtrix(new Vector3Int(x, y, z)));
            BlockController.GetInstance.AfterRotateCorrection();
        }
       if(y<10)
        {
            StartCoroutine(AfterHideFallDown(x, y + 1, z, false));
        }
        
    }

    bool checkline(int y)
    {
        for (int x =0; x<GameFieldModel.GetInstance.width; x++)
        {
            for(int z = 0; z<GameFieldModel.GetInstance.depth;z++)
            {
                if(!CubeValidate(new Vector3Int(x, y, z)))
                    return false;
            }
        }
        return true;
    }
}
