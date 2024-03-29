using System;
using Unity.VisualScripting;
using UnityEngine;
/* Unmerged change from project 'Assembly-CSharp.Player'
Before:
using System;
After:
using UnityEngine.Audio;
*/


public class AudioManager : MonoBehaviour // this class was created by BRACKEYS, please check em out on ytube if you want to learn unity
{
    public Sound[] sounds;
    public Sound[] typingSounds;
    public static AudioManager instance;
    private void Awake()
    {
        transform.SetParent(null);
        if (instance == null)
        { instance = this; }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        loadSounds(sounds);
        loadSounds(typingSounds);
    }
    private void loadSounds(Sound[] list)
    {
        foreach (Sound s in list)
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
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("trying to play sound: " + name + ", that isn't there/spelled wrong");
            return;
        }
        s.source.Play();
    }//CODE TO PLAY SOUND ANYWHERE: FindObjectOfType<AudioManager>().Play("name of the sound");
    public void PlayTypingSound(string name)
    {
        Sound z = Array.Find(typingSounds, sound => sound.name == name);
        if (z == null)
        {
            Debug.LogWarning("trying to play typing sound: " + name + ", that isn't there/spelled wrong");
            return;
        }
        z.source.Play();
    }//CODE TO PLAY SOUND ANYWHERE: FindObjectOfType<AudioManager>().PlayTypingSound("name of the sound");
}
