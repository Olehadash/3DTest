using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CubeModel
    {
    public Vector3Int matrixPosition;
    public Vector3 moveVec;
    public bool isAnimated;
    
    //public bool IsMove = true;

    public GameCubeView gObject;

    public bool rotattionSpeed;

    public CubeModel()
    {

    }

}
