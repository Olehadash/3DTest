using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //public Camera camera;
    public GameObject CentralObject;
    [Range(0,10)]
    public float sensitivity = 0.3f;

    private static CameraScript instance;

    ProectionFieldType type = ProectionFieldType.FORWARD;

    public void SetCurentType(ProectionFieldType type)
    {
        this.type = type;
    }
    public ProectionFieldType GetType
    {
        get
        {
            return this.type;
        }
    }

    public static CameraScript GetInstance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(CentralObject.transform);

        if (Input.GetMouseButtonDown(0))
        {

            isButClicke = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isButClicke = false;
        }
        //Debug.Log(isButClicke);


    }

    bool isButClicke = false;

    void FixedUpdate()
    {
        

        if (isButClicke)
        {
            float rotateHorizontal = -Input.GetAxis("Mouse X");
            float rotateVertical = -Input.GetAxis("Mouse Y");
            //Debug.Log("cameraaaa");
            transform.RotateAround(CentralObject.transform.position, -Vector3.up, rotateHorizontal * sensitivity); //use transform.Rotate(-transform.up * rotateHorizontal * sensitivity) instead if you dont want the camera to rotate around the player
            transform.RotateAround(Vector3.zero, transform.right, rotateVertical * sensitivity); // again, use transform.Rotate(transform.right * rotateVertical * sensitivity) if you don't want the camera to rotate around the player

        }

    }
}
