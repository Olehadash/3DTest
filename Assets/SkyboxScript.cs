using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxScript : MonoBehaviour
{

    private static SkyboxScript instance;
    public Material[] materials;
    public void ChangeSkybox()
    {
        RenderSettings.skybox = materials[Random.Range(0, materials.Length)];
    }

    public static SkyboxScript GetInstanse
    {
        get
        {
            return instance;
        }
    }
}
