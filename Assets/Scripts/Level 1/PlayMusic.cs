using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource intro1;
    public AudioSource intro2;
    public AudioSource song1;
    public AudioSource song2;
    public AudioSource song3;
    public AudioSource song4;
    public AudioSource song5;
    public AudioSource boss;
    public AudioSource bridge;
    public AudioSource endscene1;

    bool intro1played;
    bool song1played;
    bool song2played;
    bool song3played;
    bool song4played;
    //bool song5played;
    bool allsongsplayed;
    bool shouldPlayMusic;
    bool end;
    AudioManager1 manager;

    private void Start()
    {
        intro1.Play();
        manager = FindObjectOfType<AudioManager1>();
        end = false;
        intro1played = false;
        song1played = true;
        song2played = true;
        song3played = true;
        song4played = true;
        //song5played = true;
        allsongsplayed = true;
        shouldPlayMusic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldPlayMusic)
        {
            WaitToPlayIntro2();
            WaitToPlaySong2();
            WaitToPlaySong3();
            WaitToPlaySong4();
            WaitToPlaySong5();
            WaitToRepeatSongs();
        }
        else
        {
            if (!bridge.isPlaying && !boss.isPlaying && !end)
            {
                shouldPlayMusic = true;
            }
        }
    }

    void WaitToPlayIntro2()
    {
        if (!intro1played && !intro1.isPlaying)
        {
            intro1played = true;
            song1.Play();
            song1played = false;
        }
    }
    void WaitToPlaySong2()
    {
        if (!song1played && !song1.isPlaying)
        {
            song1played = true;
            song2.Play();
            song2played = false;
        }
    }
    void WaitToPlaySong3()
    {
        if (!song2played && !song2.isPlaying)
        {
            song2played = true;
            song3.Play();
            song3played = false;
        }
    }
    void WaitToPlaySong4()
    {
        if (!song3played && !song3.isPlaying)
        {
            song3played = true;
            song4.Play();
            song4played = false;
        }
    }
    void WaitToPlaySong5()
    {
        if (!song4played && !song4.isPlaying)
        {
            song4played = true;
            song5.Play();
            //song5played = false;
        }
    }
    void WaitToRepeatSongs()
    {
        if (!allsongsplayed && !song5.isPlaying)
        {
            allsongsplayed = true;
            song1.Play();
            song1played = false;
        }
    }
    public void PlayBridge()
    {
        if (!end)
        {
            StopAllSounds();
            if (boss.isPlaying)
            {
                boss.Stop();
            }
            shouldPlayMusic = false;
            bridge.Play();
        }
    }
    public void PlayBoss()
    {
        if (!end)
        {
            StopAllSounds();
            shouldPlayMusic = false;
            if (!boss.isPlaying)
            {
                boss.Play();
            }
        }
    }
    public void PlayEnd()
    {
        StopAllSounds();
        if (boss.isPlaying)
        {
            boss.Stop();
        }
        if (!endscene1.isPlaying)
        {
            shouldPlayMusic = false;
            endscene1.Play();
            end = true;
        }
    }
    public void StopAllSounds()
    {
        intro1.Stop();
        intro2.Stop();
        song1.Stop();
        song2.Stop();
        song3.Stop();
        song4.Stop();
        song5.Stop();
        bridge.Stop();
        /*foreach (Sound audioS in manager.sounds)
        {
            audioS.source.Stop();
        } */
    }
}
