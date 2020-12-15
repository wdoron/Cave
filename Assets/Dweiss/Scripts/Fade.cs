using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class Fade : MonoBehaviour
    {
        public float maxBlackPercent = .9f;


        public float fadeTime = 1, waitTime = 1;
        public Color color = Color.red;

        private Color emptyColor, fillColor, defaultColor;
        private Renderer rndr;
        private bool runRepeatedFade;

        void Awake()
        {
            rndr = GetComponent<Renderer>();
            rndr.material.color =  new Color(color.r, color.g, color.b, 0);
            defaultColor = rndr.material.color;
            emptyColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
            fillColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 1);
        }

        public void FullFade(int repeatCount)
        {
            if (runRepeatedFade == false)
            {
                runRepeatedFade = true;
                StartCoroutine(RepeatFadeEffect(repeatCount));
            }
        }
        IEnumerator RepeatFadeEffect(int repeatCount)
        {
            for (int i = 0; i < repeatCount && runRepeatedFade; ++i)
            {
                yield return FadeEffectColor(0, 1, maxBlackPercent, fadeTime);
                yield return new WaitForSeconds(waitTime);
                yield return FadeEffectColor(maxBlackPercent, 0, 1, fadeTime);
            }
            runRepeatedFade = false;
        }

        public void StopRepeatedFade()
        {
            runRepeatedFade = false;
        }

        public void StartFade(System.Action onCompleteBlack, System.Action onFinish)
        {
            StartCoroutine(FadeEffect(onCompleteBlack, onFinish));
        }

        private void OnDisable()
        {
            rndr.material.color = emptyColor;
            runRepeatedFade = false;
        }

        IEnumerator FadeEffect(System.Action onCompleteBlack, System.Action onFinish)
        {
            yield return FadeEffectColor(0, 1, maxBlackPercent, fadeTime);
            onCompleteBlack.Invoke();
            yield return new WaitForSeconds(waitTime);
            yield return FadeEffectColor(maxBlackPercent, 0, 1, fadeTime);
            onFinish.Invoke();
        }
        IEnumerator FadeEffectColor(float a, float b, float maxBlackPercent, System.Action onFinish)
        {
            yield return FadeEffectColor(a, b, maxBlackPercent, fadeTime);
            onFinish();
        }

        public void FadeIn(float length)
        {
            FadeEffectColor(0, 1, 1, length);
        }
        public void FadeOut(float length)
        {
            FadeEffectColor(1, 0, 1, length);
        }

        IEnumerator FadeEffectColor(float a, float b, float maxPercent, float fateTimeLength)
        {
            var startT = Time.time;
            var percent = 0f;
            float alpha;
            while (percent < maxPercent)
            {
                alpha = Mathf.Lerp(a, b, percent);
                SetColor(alpha);
                percent = (Time.time - startT) / fateTimeLength;
                yield return 0;
            }
            alpha = Mathf.Lerp(a, b, maxPercent);
            SetColor(alpha);
        }

        
        public void SetColor(float alpha)
        {
            rndr.material.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha);
        }

        public void FadeScreen(bool toBlack, float maxPercent, System.Action onFinish)
        {
            StartCoroutine(toBlack ? 
                FadeEffectColor(0, 1, maxPercent, onFinish) : 
                FadeEffectColor(1, 0, maxPercent, onFinish));
        }
        IEnumerator FullFadeSequence(float maxPercent, float waitBetweenFades, System.Action onFullFade, System.Action onReturnFade)
        {
            yield return FadeEffectColor(0, 1, maxPercent, fadeTime);
            try { onFullFade.Invoke(); } catch (System.Exception e) { Debug.LogError("Fade call had error on black (FadeIn) " + e); }
            yield return new WaitForSeconds(waitBetweenFades);
            yield return FadeEffectColor(1, 0, maxPercent, fadeTime);
            try { onReturnFade.Invoke(); } catch (System.Exception e) { Debug.LogError("Fade call had error on alpha (FadeOut) " + e); }
        }
        public void FadeInAndOutScreen(float maxPercent, float waitBetweenFades, System.Action onBlack, System.Action onAlpha)
        {
            StartCoroutine(FullFadeSequence(maxPercent, waitBetweenFades, onBlack, onAlpha));
                
        }
    }
}