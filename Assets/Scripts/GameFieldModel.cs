using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldRotateType
{
    DEPTH,
    HIGH,
    SIDE
}

public enum ProectionFieldType
{
    FORWARD,
    BACK,
    TOP,
    BOTTOM,
    LEFT,
    RIGHT
}

/*
 * Here I describet ifformation for building Our Game Fiels
 * ProectionFieldType side - Where Our 2d GameField (Proection Field) stay FORWARD, TOP, Left ect...
 * FieldRotateType rotateType - How need to rotate our (Proection Field) 
 * */

[System.Serializable]
public struct ProectionFieldModel
{
    public ProectionFieldType side;
    public FieldRotateType rotateType; 
    public bool Is_Bottom;
}


public class GameFieldModel : MonoBehaviour 
{
    private static GameFieldModel instance;

    public int width, height, depth;

    [Range(1, 6)]
    public int sidesCount = 4;  //Count Of Proection Field of The Game Field

    public ProectionFieldModel[] proectionFieldModel = new ProectionFieldModel[6]; //All Information Proection Field

    private void Awake()
    {

        instance = this;
    }

    public static GameFieldModel GetInstance
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

    
}
