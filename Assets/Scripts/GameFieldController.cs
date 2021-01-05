using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldController : MonoBehaviour
{
    private static GameFieldController instance;

    public Transform proectionFieldPrefab;

    ProectionFieldView[] p_fields = new ProectionFieldView[6];

    private void Awake()
    {
        instance = this;
    }

    public static GameFieldController GetInstance
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

    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    void Build()
    {
        for (int i = 0; i < GameFieldModel.GetInstance.sidesCount; i++)
        {
            Transform element = Instantiate(proectionFieldPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            BoxCollider collider = element.gameObject.AddComponent<BoxCollider>();

            element.gameObject.name = "Proection" + i;
            collider.size = new Vector3();
            element.SetParent(this.transform);
            p_fields[i] = element.gameObject.GetComponent<ProectionFieldView>();
            p_fields[i].Init(GameFieldModel.GetInstance.proectionFieldModel[i].side, GameFieldModel.GetInstance.proectionFieldModel[i].rotateType);
            
            switch (GameFieldModel.GetInstance.proectionFieldModel[i].rotateType)
            {
                case FieldRotateType.HIGH:
                    collider.center = new Vector3(3, 3, 0);
                    collider.size = new Vector3(GameFieldModel.GetInstance.width, GameFieldModel.GetInstance.depth, .5f);
                    break;
                case FieldRotateType.DEPTH:
                    collider.center = new Vector3(3, 7, 0);
                    collider.size = new Vector3(GameFieldModel.GetInstance.width, GameFieldModel.GetInstance.height, .5f);
                    break;
                case FieldRotateType.SIDE:
                    collider.center = new Vector3(3, 7, 0);
                    collider.size = new Vector3(GameFieldModel.GetInstance.depth, GameFieldModel.GetInstance.height, 0.5f);
                    break;
            }

        }
    }

    public Vector3 TransformToRealPosition(Vector3Int fake)
    {
        float one = p_fields[0].GEtPosition(fake.x, fake.z).x;
        float two = p_fields[1].GEtPosition(fake.x, fake.y).y;
        float three = p_fields[0].GEtPosition(fake.x, fake.z).z;
        //Debug.Log(fake.x +":"+ fake.y + ":" + fake.z + ":------------- " + one + ":" + two + ":" + three);
        return new Vector3(one,two,three);
    }

    public void SetShadowOnBoards(Vector3Int matrixPosition)
    {
        //Debug.Log(matrixPosition);
        for (int i = 0; i< GameFieldModel.GetInstance.sidesCount; i++)
        {
            if (p_fields[i] != null)
            {
                p_fields[i].SetShadowView(matrixPosition);
            }
            else
            {
                Debug.Log(i);
            }
        }
    }

    public void UnShadowOnBoards(Vector3Int fake)
    {

        for (int i = 0; i < GameFieldModel.GetInstance.sidesCount; i++)
        {
            if (p_fields[i] != null)
            {
                p_fields[i].UnShdowView(fake);
            }
            else
            {
                Debug.Log(i);
            }
        }
    }

}
