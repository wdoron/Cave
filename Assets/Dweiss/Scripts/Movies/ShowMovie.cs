using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Dweiss
{
    public class ShowMovie : MonoBehaviour
    {
        public bool showDebug;
        // public Texture texture;
        private string prefix;
        public VideoPlayer videoPlayer;
        public AudioSource audioS;
        public string defaultSuffix = ".mp4";
        private Action<ShowMovie> onPrepare;

        [System.Serializable]
        public class VideoEvent : UnityEngine.Events.UnityEvent<ShowMovie> { }

        public VideoEvent onPreperationComplete, onLoopReached;

        public float videoDuration;

        private void OnGUI()
        {
            if (showDebug && videoPlayer.isPlaying)

                GUI.Label(new Rect(Screen.width*1 / 5 - 10,  10, Screen.width/2 + 10, 90), 
                    string.Format("{1} - {0}/{2}"
                        , videoPlayer.time.ToString("0.0") 
                        , videoPlayer.url.Substring(videoPlayer.url.LastIndexOf("/")+1)
                        , videoDuration.ToString("0.0")
                        )
                    );
        }

        private void Awake()
        {
            prefix = Application.dataPath + "/StreamingAssets";
        }

        public string videoStart;
        private void Start()
        {
           
            videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
            videoPlayer.prepareCompleted += OnMoviePreperd;
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

           
            ////Set Audio Output to AudioSource
            //videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

            ////Assign the Audio from Video to AudioSource to be played
            //videoPlayer.EnableAudioTrack(0, true);
            //videoPlayer.SetTargetAudioSource(0, audioSource);
            if(string.IsNullOrEmpty(videoStart) == false)
            {
                PrepereMovie(videoStart, true);
            }
        }

        private void VideoPlayer_loopPointReached(VideoPlayer source)
        {
            onLoopReached.Invoke(this);
            if (videoPlayer.isLooping)
            {

            }
        }
        public override string ToString()
        {
            return string.Format("{0} - stop {1}", videoPlayer.url, stopping);
        }
        private void OnMoviePreperd(VideoPlayer vp)
        {
            if (stopping) return;
           // Debug.Log("Ready: " + this);
            videoDuration = videoPlayer.frameCount / videoPlayer.frameRate;
            onPreperationComplete.Invoke(this);

            SetSound();
        }

        private string GetFullMovieName(string name)
        {
            if (name[0] != '/' && name[0] != '\\') name = "/" + name;
            if (name.IndexOf(".") < 0)
            {
                name += defaultSuffix;
            }
            if (name.StartsWith(prefix) == false)
            {
                name = prefix + name;
            }
            return name;//.Replace("/", "\\"); ;
        }
        private bool stopping = true;
        public void Stop()
        {
            stopping = true;
            videoPlayer.Pause();
        }
      
        public void PrepereMovie(string name, bool loop )
        {
            stopping = false;
            videoPlayer.isLooping = loop;
            var fullMovieName = GetFullMovieName(name);
            
            //Debug.Log("PrepereMovie " + fullMovieName);
            videoPlayer.url = fullMovieName;// "/Users/graham/movie.mov";
            videoPlayer.Stop();
            videoPlayer.Prepare();


            SetSound();
        }
        private void SetSound()
        {
            videoPlayer.controlledAudioTrackCount = 1;
            videoPlayer.EnableAudioTrack(0, true);
            videoPlayer.SetTargetAudioSource(0, audioS);
            videoPlayer.SetDirectAudioVolume(0, 1);
        }
        public void Play()
        {
            videoPlayer.Play();
            SetSound();
        }
    }
}