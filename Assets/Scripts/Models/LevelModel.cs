using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel : MonoBehaviour
{
    private static LevelModel instance;

    [Range(0, 20)]
    public float speed;
    public int frames;
    [Range(0, 20)]
    public float rotationSpeed;
    public float FallDowntime = 5;
    public float SpeedFallDowntime = 1;
    // Start is called before the first frame update

    public int Score = 0;
    public int Level = 1;
    void Awake()
    {
        instance = this;
    }
    public static LevelModel GetInstance
    {
        get
        {
            return instance;
        }
    }
}
