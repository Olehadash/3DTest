using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundViewer : MonoBehaviour
{
    public RawImage Sound;
    public RawImage Music;

    public Texture2D musicon, musicOff;
    public Texture2D soundon, soundoff;

    
    // Start is called before the first frame update
    void Start()
    {
        Music.texture = SoundController.GetSingleton.Music_Mute ? musicOff : musicon;
        Sound.texture = SoundController.GetSingleton.Sound_Mute ? soundoff : soundon;

    }

    public void MusickMute()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (SoundController.IsInitialized)
            SoundController.GetSingleton.PlaySound(SoundType.Move);
        
        SoundController.GetSingleton.Music_Mute = !SoundController.GetSingleton.Music_Mute;
        Music.texture = SoundController.GetSingleton.Music_Mute ? musicOff : musicon;
    }

    public void SoundMute()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (SoundController.IsInitialized)
            SoundController.GetSingleton.PlaySound(SoundType.Move);
        SoundController.GetSingleton.Sound_Mute = !SoundController.GetSingleton.Sound_Mute;
        Sound.texture = SoundController.GetSingleton.Sound_Mute ? soundoff : soundon;
    }

}
