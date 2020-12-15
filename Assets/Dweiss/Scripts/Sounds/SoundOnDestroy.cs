using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundOnDestroy : MonoBehaviour

{
    public float volume = 1f;
    public AudioClip[] clips;
    private void Start()
    {
        
    }
    private void OnDestroy()
    {
        if(enabled) AudioSource.PlayClipAtPoint(clips[UnityEngine.Random.Range(0, clips.Length)], transform.position, volume);
    }

}
