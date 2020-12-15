using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class SoundOnIntervals : MonoBehaviour
    {
        private new AudioSource audio;
        public Vector2 randomTimeBetweenTrigger;

        private void Awake()
        {
            audio = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            StartCoroutine(CoroutineRandomTrigger());
        }

        private IEnumerator CoroutineRandomTrigger()
        {
            while (true)
            {
                var nextTime = Random.Range(randomTimeBetweenTrigger.x, randomTimeBetweenTrigger.y);
                yield return new WaitForSeconds(nextTime);

                audio.Play();
                yield return new WaitForSeconds(audio.clip.length);
            }
        }


    }
}