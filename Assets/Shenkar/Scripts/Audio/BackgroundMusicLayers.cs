using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shenkar
{
    public class BackgroundMusicLayers : MonoBehaviour {

        public AudioFader[] musicTracks;

        public float delayTime = 1;
        public float slotTime = 0.1f;

        private int targetZoneCount = 0;

        private int activeZoneCount = 0;

        private float timeOfZoneChange;

        private void Awake() {
            for (int i = 0; i < musicTracks.Length; i++) {
                musicTracks[i].aSource.loop = true;
                musicTracks[i].aSource.volume = 0;
            }
           // musicTracks[0].aSource.volume = 1;
        }
        public void EnterZone(int zoneId) {
            if (zoneId > targetZoneCount) targetZoneCount = zoneId;
            ZoneChanged();
        }
        public void EnterZone() {
            Debug.Log("EnterZone");
            targetZoneCount++;
            ZoneChanged();
        }
        public void ExitZone(int zoneId) {
            if (zoneId < targetZoneCount) targetZoneCount = zoneId;
            ZoneChanged();
        }
        public void ExitZone() {
            Debug.Log("ExitZone");
            targetZoneCount--;
            ZoneChanged();
        }

        private void ZoneChanged() {
            if (timeOfZoneChange < Time.time)
                timeOfZoneChange = GetNextTime();

            TryUpdateAudio();
        }

        private float GetNextTime() {
            return delayTime + (
                slotTime == 0 ? Time.time : 
                Mathf.Floor((Time.time + slotTime) / slotTime) * slotTime);
        }

        private void MoveToZone(bool incZone) {
            if (incZone) {
                musicTracks[activeZoneCount].FadeIn();
            } else {
                musicTracks[activeZoneCount-1].FadeOut();
            }
            activeZoneCount += (incZone?1:-1);
        }

        // Update is called once per frame
        private void Update() {
            TryUpdateAudio();
        }

        private void TryUpdateAudio() {
            var target = System.Math.Max(0, System.Math.Min(targetZoneCount, musicTracks.Length));
            if (target != activeZoneCount) {
                if (timeOfZoneChange <= Time.time) {
                    MoveToZone(activeZoneCount < target);

                    target = System.Math.Max(0, System.Math.Min(targetZoneCount, musicTracks.Length));
                    if (target != activeZoneCount)
                        timeOfZoneChange = GetNextTime();
                }
            }
        }
    }
}