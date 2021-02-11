using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shenkar
{
    public class AudioFader : MonoBehaviour
    {
        public AudioSource aSource;
        public float fadeTime = .3f;

        private float targetVolume;

        // Start is called before the first frame update
        void Reset() {
            aSource = GetComponent<AudioSource>();
        }

        public void FadeIn() {
            targetVolume = 1;
            StartFade();
        }
        public void FadeOut() {
            targetVolume = 0;
            StartFade();
        }

        private void StartFade() {
            StopAllCoroutines();
            StartCoroutine(FadeNow());
        }
        // Update is called once per frame
        IEnumerator FadeNow() {
            var startVolume = aSource.volume;
            var startT = Time.time;
            float percent = 0;
            while(percent < 1) {
                percent = Mathf.Min(1, (Time.time - startT) / fadeTime);
                aSource.volume = Mathf.Lerp(startVolume, targetVolume, percent);
                yield return 0;
            }
            aSource.volume = targetVolume;
        }
    }
}