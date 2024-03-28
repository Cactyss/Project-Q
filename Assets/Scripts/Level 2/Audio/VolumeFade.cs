using UnityEngine;

public class VolumeFade : MonoBehaviour  //https://forum.unity.com/threads/anyone-know-how-to-make-2d-audio-fade-audio-source-by-distance-to-cam-audio-listener.993974/
{
    public Transform listenerTransform;
    public AudioSource audioSource;
    public float minDist = 1;
    public float maxDist = 400;

    void Update()
    {
        listenerTransform = FindObjectOfType<Camera>().transform;
        float dist = Vector3.Distance(transform.position, listenerTransform.position);

        if (dist < minDist)
        {
            audioSource.volume = 1;
        }
        else if (dist > maxDist)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1 - ((dist - minDist) / (maxDist - minDist));
        }
    }
}