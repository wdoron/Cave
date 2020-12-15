using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class SoundOnParticle : MonoBehaviour
    {
        public ParticleSystem part;
        public AudioSource aSource;
        public bool debug;

        private void Reset()
        {
            aSource = GetComponent<AudioSource>();
            part = GetComponent<ParticleSystem>();

            aSource.playOnAwake = false;
        }

        List<ParticleCollisionEvent> collisionEvents;
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();


        void OnParticleTrigger()
        {
            int numEnter = part.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

            if(debug)Debug.LogFormat(name +" OnParticleTrigger {0} ", numEnter);
            if(numEnter > 0) aSource.Play();
        }

        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

            aSource.Play();
            if (debug) Debug.LogFormat(name + " OnParticleCollision {0}", numCollisionEvents);
        }
    }
}