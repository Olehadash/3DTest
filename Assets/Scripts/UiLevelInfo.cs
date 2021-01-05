using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiLevelInfo : MonoBehaviour
{

    private static UiLevelInfo instance;

    public Text ScoreLabel;
    public Text LevelLabel;

    public GameObject GameOver;
    public GameObject Pause;
    public GameObject MainPage;
    public GameObject HowToPlay;
    public Image BackImage;
    public Transform Sounds;
    

    public bool IsActive = false;
    
    int curentpoinst = 0;
    
    bool isPause = false;
    bool isGame = false;
    bool isLevelSwitch = false;
    

    public static UiLevelInfo GetInstance
    {
        get
        {
            return instance;
        }
    }
    public bool GetIsPause
    {
        get
        {
            return isPause;
        }
    }

    private void Awake()
    {
        instance = this;
        if(!SoundController.IsInitialized)
        {
            Instantiate(Sounds, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ScoreLabel.text = curentpoinst.ToString();
        if (SoundController.AutoStart)
        {
            StartGame();
        }
        else
        {
            MainPage.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(curentpoinst < LevelModel.GetInstance.Score)
        {
            curentpoinst++;
            ScoreLabel.text = curentpoinst.ToString();
        }

        if(!isGame)
        {
            CameraScript.GetInstance.transform.RotateAround(CameraScript.GetInstance.CentralObject.transform.position, -Vector3.up, 0.01f); 

        }
        PuseUpdate();
        LEvelChange();
    }

    /*
     * Switch on - off Pause
     * */
    void PuseUpdate()
    {
        Pause.SetActive(isPause);
        if (IsActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
        }
    }

     void LEvelChange()
    {

        LevelLabel.text = LevelModel.GetInstance.Level.ToString();
        if (curentpoinst !=0 && curentpoinst % 1000 == 0 && !isLevelSwitch)
        {
            LevelModel.GetInstance.Level++;
            //SkyboxScript.GetInstanse.ChangeSkybox();
            LevelModel.GetInstance.speed += 0.5f;
            LevelModel.GetInstance.rotationSpeed += 0.5f;
            BlockModel.GetInstance.ResetParametrs();
            if (SoundController.IsInitialized)
                SoundController.GetSingleton.Replay();
            isLevelSwitch = true;
        }
        if(curentpoinst-1 % 1000 == 0)
        {
            isLevelSwitch = false;
        }
    }
    /*
     * New Game Buttons press
     */
    public void StartGame()
    {
        if(SoundController.IsInitialized)
            SoundController.GetSingleton.PlaySound(SoundType.Move);
        BackImage.gameObject.SetActive(true);
        
        StartCoroutine(StartGameCorutine());
    }

    IEnumerator StartGameCorutine()
    {
        float alpha = 0;
        while (alpha<1)
        {
            Color color = BackImage.color;
            BackImage.color = new Color(color.r, color.g, color.b, alpha);
            alpha += 0.1f;
            yield return null;
        }

        MainPage.SetActive(false);
        isGame = true;
        IsActive = true;
        while (alpha > 0)
        {
            Color color = BackImage.color;
            BackImage.color = new Color(color.r, color.g, color.b, alpha);
            alpha -= 0.1f;
            yield return null;
        }

        CameraScript.GetInstance.transform.position = new Vector3(0, 0, -20);
        int i = 0;
        while(i < 20)
        {
            CameraScript.GetInstance.transform.position += new Vector3(0,0,0.5f);
            yield return null;
            i++;
        }
        BlockController.GetInstance.BuildBlock();
    }

    public void HelpOpen()
    {
        if(SoundController.IsInitialized)
            SoundController.GetSingleton.PlaySound(SoundType.Move);
        SoundController.GetSingleton.PlaySound(SoundType.Move);
        MainPage.SetActive(false);
        HowToPlay.SetActive(true);
    }

    public void GameOverOpen()
    {
        GameOver.SetActive(true);

    }

    public void Restart()
    {
        SoundController.AutoStart = true;
        SceneManager.LoadScene(0);
    }

    public void BAck()
    {
        SoundController.AutoStart = false;
        SceneManager.LoadScene(0);
    }
}
