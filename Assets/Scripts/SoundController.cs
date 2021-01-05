using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Drop,
    Move,
    Rotate,
    Success,
    Crash,
}

public class SoundController : MonoBehaviour
{
    static SoundController instance;
    public  bool Music_Mute = false;
    public  bool Sound_Mute = false;

    public static bool AutoStart = false;

    public AudioSource musicSource;
    

    [Header("Musik For BG Play :")]
    public AudioClip[] music = new AudioClip[2];

    [Header("Musik Move :")]
    public AudioClip[] MoveSound = new AudioClip[2];

    [Header("Musik Drop:")]
    public AudioClip[] DropSounds = new AudioClip[3];

    [Header("ROtate :")]
    public AudioClip[] TrampolineSounds = new AudioClip[3];

    [Header("Success Sounds :")]
    public AudioClip successSounds;

    [Header("Crush Sounds :")]
    public AudioClip[] CrashSounds = new AudioClip[3];

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
        Replay();
    }

    public void Replay()
    {
        musicSource.clip = music[Random.Range(0, music.Length)];
        musicSource.Play();
    }


    public static bool IsInitialized
    {
        get
        {
            return instance != null;
        }
    }

    public static SoundController GetSingleton
    {
        get
        {
            return instance;
        }
    }

    public void PlaySound(SoundType type)
    {
        StartCoroutine(PlaySoundCorutine(type));
        
    }


    IEnumerator PlaySoundCorutine(SoundType type)
    {
        
            AudioSource soundSource = this.gameObject.AddComponent<AudioSource>();
            soundSource.mute = Sound_Mute;
            soundSource.volume = .4f;
            switch (type)
            {
                case SoundType.Drop:
                    soundSource.clip = DropSounds[Random.Range(0, DropSounds.Length)];
                    break;
                case SoundType.Move:
                    soundSource.clip = MoveSound[Random.Range(0, MoveSound.Length)];
                    break;
                case SoundType.Rotate:
                    soundSource.clip = TrampolineSounds[Random.Range(0, TrampolineSounds.Length)];
                    break;
                case SoundType.Success:
                    soundSource.clip = successSounds;
                    break;
                case SoundType.Crash:
                    soundSource.clip = CrashSounds[Random.Range(0, CrashSounds.Length)];
                    break;
            }
            soundSource.Play();
            yield return new WaitForSeconds(2);
            DestroyImmediate(soundSource);
    }

    // Update is called once per frame
    void Update()
    {
        musicSource.mute = SoundController.GetSingleton.Music_Mute;
        
    }
}
