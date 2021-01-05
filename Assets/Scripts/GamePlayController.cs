using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
   
    // Update is called once per frame
    /*
     * MoveOour BlockInside Game field by arrows Button
     * Left - Right 
     * Front - Back
     * 
     * Rotation  - 
     * W S - X 
     * A D - Y
     * Q E - Z
     * 
     * SpaceBar - Fast falling of the Block-Figure
     * */
    void Update()
    {
        if (UiLevelInfo.GetInstance.IsActive && !UiLevelInfo.GetInstance.GetIsPause)
        {
            MovementVector move = MovementVector.FORVARD;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                switch(CameraScript.GetInstance.GetType)
                {
                    case ProectionFieldType.FORWARD:
                        move = MovementVector.FORVARD;
                        break;
                    case ProectionFieldType.BACK:
                        move = MovementVector.BACK;
                        break;
                    case ProectionFieldType.RIGHT:
                        move = MovementVector.RIGHT;
                        break;
                    case ProectionFieldType.LEFT:
                        move = MovementVector.LEFT;
                        break;
                }
                BlockController.GetInstance.MoveInMatrix(move);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                switch (CameraScript.GetInstance.GetType)
                {
                    case ProectionFieldType.FORWARD:
                        move = MovementVector.BACK;
                        break;
                    case ProectionFieldType.BACK:
                        move = MovementVector.FORVARD;
                        break;
                    case ProectionFieldType.RIGHT:
                        move = MovementVector.LEFT;
                        break;
                    case ProectionFieldType.LEFT:
                        move = MovementVector.RIGHT;
                        break;
                }
                BlockController.GetInstance.MoveInMatrix(move);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                switch (CameraScript.GetInstance.GetType)
                {
                    case ProectionFieldType.FORWARD:
                        move = MovementVector.LEFT;
                        break;
                    case ProectionFieldType.BACK:
                        move = MovementVector.RIGHT;
                        break;
                    case ProectionFieldType.RIGHT:
                        move = MovementVector.FORVARD;
                        break;
                    case ProectionFieldType.LEFT:
                        move = MovementVector.BACK;
                        break;
                }
                BlockController.GetInstance.MoveInMatrix(move);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                switch (CameraScript.GetInstance.GetType)
                {
                    case ProectionFieldType.FORWARD:
                        move = MovementVector.RIGHT;
                        break;
                    case ProectionFieldType.BACK:
                        move = MovementVector.LEFT;
                        break;
                    case ProectionFieldType.RIGHT:
                        move = MovementVector.BACK;
                        break;
                    case ProectionFieldType.LEFT:
                        move = MovementVector.FORVARD;
                        break;
                }
                BlockController.GetInstance.MoveInMatrix(move);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                BlockController.GetInstance.MoveInMatrix(MovementVector.XROTATE);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                BlockController.GetInstance.MoveInMatrix(MovementVector.XROTATEBACK);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                BlockController.GetInstance.MoveInMatrix(MovementVector.YROTATE);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                BlockController.GetInstance.MoveInMatrix(MovementVector.YROTATEBACK);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                BlockController.GetInstance.MoveInMatrix(MovementVector.ZROTATE);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                BlockController.GetInstance.MoveInMatrix(MovementVector.ZROTATEBACK);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BlockController.GetInstance.SpeedWaterFall(true);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                BlockController.GetInstance.SpeedWaterFall(false);
            }
        }
    }
}
