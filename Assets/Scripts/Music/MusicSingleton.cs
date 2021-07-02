using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSingleton : MonoBehaviour 
{
    private AudioSource _audioSource;

    private static MusicSingleton instance = null;

    public static MusicSingleton Instance
    {
        get { return instance; }
    }

    void Awake() 
    {
        if (instance != null && instance != this) 
        {
            Destroy(this.gameObject);
            return;
        } 
        else 
        {
            instance = this;
            _audioSource = GetComponent<AudioSource>();
        }

        DontDestroyOnLoad(this.gameObject);
    }
 
    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }


}
