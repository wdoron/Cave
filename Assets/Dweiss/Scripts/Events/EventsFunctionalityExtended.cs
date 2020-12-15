using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Dweiss.Common
{
    public class EventsFunctionalityExtended : Singleton<EventsFunctionalityExtended>
    {
        public void Quit()
        {
            Application.Quit();
        }

        public void PlayOneShotSound(AudioSource reference)
        {
            AudioSource.PlayClipAtPoint(reference.clip, reference.transform.position, reference.volume);
        }

        public void PlayOneShotSound(AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }

        public void ContinueToNextScene()
        {
            var nextBuild = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount;
            
            SceneManager.LoadScene(nextBuild% UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings);
        }
        public void ContinueToNextSceneAsync()
        {
            SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount);
        }

        public void ContinueToScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        public void ContinueToSceneAsync(int number)
        {
            SceneManager.LoadSceneAsync(number);
        }

        public void ContinueToScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void ContinueToSceneAsync(string name)
        {
            SceneManager.LoadSceneAsync(name);
        }
        
    }
}