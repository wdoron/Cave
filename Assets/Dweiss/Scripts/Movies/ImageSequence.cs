using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class ImageSequence : MonoBehaviour
    {
        public bool sort;
        public bool loop;
        public bool calc;

        [SerializeField] private Renderer rndr;
        private void Reset() { rndr = GetComponentInChildren<Renderer>(); }
        void OnValidate() {
            if (sort) { SortImages(); sort = false; }
            if (calc)
            {
                calc = false;
                for(int i= movieTxters.Count-1; i >= 0; --i)
                {
                    if (i % 2 != 0) {
                        movieTxters.RemoveAt(i);
                        }
                }
            }
        }
        public List<Texture2D> movieTxters;
        public int movieFps;
        public bool runOnStart;

        public UnityEngine.Events.UnityEvent onMovieFinished;

        private void OnEnable() { if (runOnStart) StartMovie();}

        private void SortImages()
        {
            movieTxters.Sort((a,b) => a.name.CompareTo(b.name));
        }

        public void StartMovie()
        {
            StopAllCoroutines();
            StartCoroutine("RunMovie");
        }

        private IEnumerator RunMovie()
        {
            var startTime = Time.time;
            int movieIndex = 0;
            while (loop || movieIndex < movieTxters.Count)
            {
                rndr.material.mainTexture = movieTxters[movieIndex % movieTxters.Count];
                if(loop == false && movieIndex > 0)
                {
                    movieTxters[movieIndex - 1] = null;
                }
                var timeDiff = Time.time - startTime;
                movieIndex = (int) (timeDiff * movieFps);
                yield return 0;
            }
            //Application.GarbageCollectUnusedAssets();
            onMovieFinished.Invoke();
        }
    }

}
