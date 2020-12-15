using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class AudioSourceGuard : MonoBehaviour
    {
        [SerializeField]private new AudioSource audio;

        public Transform newAudioParent;

        public bool duplicateOnPlay;

        private void Reset()
        {
            audio = this.GetComponentInChildrenOrParents<AudioSource>();
        }
        

        public void Play()
        {
            if (duplicateOnPlay)
            {
                var go = Instantiate(gameObject, newAudioParent? newAudioParent : transform.parent);
                Destroy(go.GetComponent<AudioSourceGuard>());
                var newAudio = go.GetComponent<AudioSource>();
                newAudio.Play();
                Destroy(go, audio.clip.length+.1f);
                
            }
            else if(audio.isPlaying == false)
            {
                audio.Play();
            }
        }
    }
}