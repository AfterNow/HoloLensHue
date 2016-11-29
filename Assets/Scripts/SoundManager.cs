using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource NotificationPopupSFX;

    public static SoundManager instance = null;

    private AudioSource audioSource;
    public AudioClip[] PopupClips = new AudioClip[3];

    void Awake()
    {
        //Check if an instance of SoundManager already exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //This enforces our singleton pattern so there can only be one instance of SoundManager
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Debug.Log("SoundMgr Awake");
    }

    //Main function to play single, non spatialized notification sound clips
    public void PlayNotificationPopup(string clipName)
    {
        foreach (AudioClip clip in PopupClips)
        {
            if (clip != null && clipName == clip.name)
            {
                NotificationPopupSFX.clip = clip;
                NotificationPopupSFX.Play();
            }
        }
    }
}
