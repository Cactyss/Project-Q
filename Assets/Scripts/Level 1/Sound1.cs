using UnityEngine;

[System.Serializable]
public class Sound1
{
    // this class was created by BRACKEYS, please check em out on ytube if you want to learn unity
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    public bool loop;

    [Range(-3f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;



}
