using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixModel : MonoBehaviour
{
    private static MatrixModel instance;

    CubeModel[,,] CubeMatrix;
    Vector3Int Max;

    public CubeModel standartCubeModel;

    public GameObject cubePrefab;
    public Vector3Int startPosition = new Vector3Int(3, 5, 3);
    public Material CubeMaterial;

    // Singltone initialization
    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        CubeMatrix = new CubeModel[GameFieldModel.GetInstance.width, GameFieldModel.GetInstance.height, GameFieldModel.GetInstance.depth];
    }
    /*
     * Singltone
     */
    public static MatrixModel GetInstance
    {
        get
        {
            return instance;
        }
    }

    public CubeModel GetCubeModel(Vector3Int matrixPostion)
    {
        return CubeMatrix[matrixPostion.x, matrixPostion.y, matrixPostion.z];
    }

    public void SetMatrixPArametr(Vector3Int matrixPostion, CubeModel model)
    {
        CubeMatrix[matrixPostion.x, matrixPostion.y, matrixPostion.z] = model;
    }

    public CubeModel[,,] GetMatrix
    {
        get
        {
            return CubeMatrix;
        }
    }
}
