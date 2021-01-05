using System.Collections.Generic;
using UnityEngine;
public enum MovementVector
{
    UP = 0,
    DOWN = 1,
    LEFT,
    RIGHT,
    FORVARD,
    BACK,
    XROTATE,
    XROTATEBACK,
    YROTATE,
    YROTATEBACK,
    ZROTATE,
    ZROTATEBACK,
}

public enum FiguraType
{
    POINT = 0,
    QUBE,
    STICK,
    HORSE,
    TFIGUR,
    PLUS,
    PONY,
    POP1,
    POP2,
    TEST,
}


[System.Serializable]
public struct FigureModel
{
    public FiguraType type;
    public Vector3Int[] BuildMatrix;
    public CubeModel[] cubes;
    public Color color;
}

public class BlockModel : MonoBehaviour
{
    private static BlockModel instance; 
    public FigureModel[] Figurs;

    public Dictionary<MovementVector, Vector3> MoveFloatVector = new Dictionary<MovementVector, Vector3>();
    public Dictionary<MovementVector, Vector3Int> MoveInMatrixVectrors = new Dictionary<MovementVector, Vector3Int>();
    public Dictionary<Vector2Int, Vector2Int> RotationPosChange = new Dictionary<Vector2Int, Vector2Int>();


    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        RotationPosChange.Add(new Vector2Int(1, 1), new Vector2Int(2, 0));
        RotationPosChange.Add(new Vector2Int(1, 0), new Vector2Int(1, 1));
        RotationPosChange.Add(new Vector2Int(1, -1), new Vector2Int(-2, 0));
        RotationPosChange.Add(new Vector2Int(0, 1), new Vector2Int(1, -1));
        RotationPosChange.Add(new Vector2Int(0, 0), new Vector2Int(0, 0));
        RotationPosChange.Add(new Vector2Int(0, -1), new Vector2Int(-1, 1));
        RotationPosChange.Add(new Vector2Int(-1, 1), new Vector2Int(2, 0));
        RotationPosChange.Add(new Vector2Int(-1, 0), new Vector2Int(-1, -1));
        RotationPosChange.Add(new Vector2Int(-1, -1), new Vector2Int(-2, 0));

        ResetParametrs();

        MoveInMatrixVectrors.Add(MovementVector.UP, new Vector3Int(0, 1, 0));
        MoveInMatrixVectrors.Add(MovementVector.DOWN, new Vector3Int(0, -1, 0));
        MoveInMatrixVectrors.Add(MovementVector.LEFT, new Vector3Int(-1, 0, 0));
        MoveInMatrixVectrors.Add(MovementVector.RIGHT, new Vector3Int(1, 0, 0));
        MoveInMatrixVectrors.Add(MovementVector.FORVARD, new Vector3Int(0, 0, 1));
        MoveInMatrixVectrors.Add(MovementVector.BACK, new Vector3Int(0, 0, -1));
        MoveInMatrixVectrors.Add(MovementVector.XROTATE, Vector3Int.zero);
        MoveInMatrixVectrors.Add(MovementVector.XROTATEBACK, Vector3Int.zero);
        MoveInMatrixVectrors.Add(MovementVector.YROTATE, Vector3Int.zero);
        MoveInMatrixVectrors.Add(MovementVector.YROTATEBACK, Vector3Int.zero);
        MoveInMatrixVectrors.Add(MovementVector.ZROTATE, Vector3Int.zero);
        MoveInMatrixVectrors.Add(MovementVector.ZROTATEBACK, Vector3Int.zero);
    }

    public void ResetParametrs()
    {
        MoveFloatVector.Add(MovementVector.BACK, new Vector3(0, 0, LevelModel.GetInstance.speed));
        MoveFloatVector.Add(MovementVector.FORVARD, new Vector3(0, 0, -LevelModel.GetInstance.speed));
        MoveFloatVector.Add(MovementVector.LEFT, new Vector3(-LevelModel.GetInstance.speed, 0, 0));
        MoveFloatVector.Add(MovementVector.RIGHT, new Vector3(LevelModel.GetInstance.speed, 0, 0));
        MoveFloatVector.Add(MovementVector.UP, new Vector3(0, LevelModel.GetInstance.speed, 0));
        MoveFloatVector.Add(MovementVector.DOWN, new Vector3(0, -LevelModel.GetInstance.speed, 0));
        MoveFloatVector.Add(MovementVector.XROTATE, new Vector3(LevelModel.GetInstance.rotationSpeed, 0, 0));
        MoveFloatVector.Add(MovementVector.XROTATEBACK, new Vector3(-LevelModel.GetInstance.rotationSpeed, 0, 0));
        MoveFloatVector.Add(MovementVector.YROTATE, new Vector3(0, -LevelModel.GetInstance.rotationSpeed, 0));
        MoveFloatVector.Add(MovementVector.YROTATEBACK, new Vector3(0, LevelModel.GetInstance.rotationSpeed, 0));
        MoveFloatVector.Add(MovementVector.ZROTATE, new Vector3(0, 0, LevelModel.GetInstance.rotationSpeed));
        MoveFloatVector.Add(MovementVector.ZROTATEBACK, new Vector3(0, 0, -LevelModel.GetInstance.rotationSpeed));
    }

    public static BlockModel GetInstance
    {
        get
        {
            return instance;
        }
    }

    public object MovementPositionCange { get; private set; }
}
