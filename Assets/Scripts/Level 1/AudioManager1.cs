using System;
using UnityEngine;
/* Unmerged change from project 'Assembly-CSharp.Player'
Before:
using System;
After:
using UnityEngine.Audio;
*/


public class AudioManager1 : MonoBehaviour // this class was created by BRACKEYS, please check em out on ytube if you want to learn unity
{
    public Sound1[] sounds;

    public static AudioManager1 instance;
    private void Awake()
    {        transform.SetParent(null);
        if (instance == null)
        { instance = this; }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        foreach (Sound1 s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        } 
    }
    public void Play(string name)
    {
        Sound1 s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("trying to play sound: " + name + ", that isn't there/spelled wrong");
            return;
        }
        s.source.Play();

        //CODE TO PLAY SOUND ANYWHERE: FindObjectOfType<AudioManager1>().Play("name of the sound");
    }
}
