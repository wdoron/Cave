using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundOnImpact : MonoBehaviour
    {

        public bool debug;

        public AudioSource aSource;
        public LayerMask layerToCollideWith;
        public string tagToCollideWith;
        public float minRelativeVelocity;

        public float volumeFactorOnCollision, collisionPowToCalculateVolume = 1;
        public bool enableOnTrigger;

        public float minTimeBetweenCollision = .1f;
        private float _lastCollisionTime;
        private void Reset()
        {
            aSource = GetComponent<AudioSource>();
            aSource.playOnAwake = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (debug) Debug.Log(name + " " + collision.collider.name + " " + collision.relativeVelocity.magnitude);

            if (Time.time - _lastCollisionTime > minTimeBetweenCollision)
            {
                if (collision.relativeVelocity.magnitude >= minRelativeVelocity)
                {
                    if (string.IsNullOrEmpty(tagToCollideWith) || collision.collider.CompareTag(tagToCollideWith)
                        || layerToCollideWith.HasLayer(collision.gameObject.layer))
                    {
                        _lastCollisionTime = Time.time;

                        if (volumeFactorOnCollision > 0)
                        {
                            aSource.volume =  (volumeFactorOnCollision * Mathf.Pow(collision.relativeVelocity.magnitude, collisionPowToCalculateVolume));
                        }
                        else if (volumeFactorOnCollision < 0)
                        {
                            aSource.volume = .3f + (Mathf.Abs(volumeFactorOnCollision) * Mathf.Pow(collision.relativeVelocity.magnitude, collisionPowToCalculateVolume));
                        }
                        aSource.Play();
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider cldr)
        {
            if (enableOnTrigger == false) return;
            if (debug) Debug.Log(name + " " + cldr.name);

            if (Time.time - _lastCollisionTime > minTimeBetweenCollision)
            {
                if (string.IsNullOrEmpty(tagToCollideWith) || cldr.CompareTag(tagToCollideWith))
                {
                    _lastCollisionTime = Time.time;
                    aSource.volume = 1;
                    aSource.Play();
                }
            }
        }
    }
}