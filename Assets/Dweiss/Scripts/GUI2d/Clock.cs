using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dweiss;

public class Clock : MonoBehaviour {

    public AudioSource audioS;
    public int slowSound = 120, makeSoundFromTime = 30, makeQuickSoundFromTime = 15, doubleTimeSound = 15;
    [System.Serializable]
    public class ColorSequence
    {
        public Color color;
        public float percent;
    }
    public List<ColorSequence> colorSequence;

    public enum ClockShape
    {
        Seconds,
        MinutesAndSeconds,
        HoursAndMinutesAndSeconds
    }

    public int countTime = 360;
    [SerializeField] private ClockShape shape;

    [SerializeField] private UnityEngine.UI.Text _uiText;
    [SerializeField] private UnityEngine.TextMesh _txtMesh;

    [SerializeField] private bool startClock;

    public UnityEngine.Events.UnityEvent onReset;

    private int secondsLeft;
    public int SecondsLeft { get { return secondsLeft; } set { secondsLeft = value; } }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public void Reset(int countSec)
    {
        secondsLeft = countTime = countSec;
        StopAllCoroutines();
        StartCoroutines();
        onReset.Invoke();
    }

	private void Start() {
        colorSequence.Sort((a,b) => -a.percent.CompareTo(b.percent));
        if (startClock) StartCoroutines();
    }

    private void StartCoroutines()
    {
        SetColor(countTime);
        StartCoroutine(CountTime());
        StartCoroutine(TickSound());
    }



    private void SetColor(float secondsLeft)
    {
        var currentPercent = (secondsLeft / (float)countTime)*100f;
        for(int i=0; i < colorSequence.Count; ++i)
        {
            if(currentPercent > colorSequence[i].percent)
            {
                if(i  > 0)
                {
                    var i2 = i - 1;
                    var pSmall = colorSequence[i].percent;
                    var pLarge = colorSequence[i2].percent;
                    var t = (currentPercent - pSmall) /(pLarge - pSmall);
                    //Debug.LogFormat("T: {0}, i {1},{2}\n ps {3} pl {4}", t, i, i2, pSmall, pLarge);
                    _txtMesh.color = Color.Lerp(colorSequence[i].color, colorSequence[i2].color, t);
                }
                else _txtMesh.color = colorSequence[i].color;
                break;
            }
        }
        
    }
    private IEnumerator TickSound()
    {
        while (secondsLeft > 0)
        {
            if(doubleTimeSound > secondsLeft)
            {
                yield return new WaitForSeconds(.25f);
                if(audioS) audioS.Play();
            }
            else if (makeQuickSoundFromTime > secondsLeft)
            {
                yield return new WaitForSeconds(.5f);
                if (audioS) audioS.Play();
            }
            else if (makeSoundFromTime > secondsLeft)
            {
                yield return new WaitForSeconds(1f);
                if (audioS) audioS.Play();
            }
            else if (slowSound > secondsLeft)
            {
                if (audioS) audioS.Play();
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return 0;
            }
            
        }
    }
    private IEnumerator CountTime() {
        string str = "";
        while (secondsLeft > 0 )
        {
            //secondsLeft = countTime - i;
            str = ToClock(secondsLeft);
            if(_uiText != null) _uiText.text = str;
            if (_txtMesh != null) _txtMesh.text = str;
            SetColor(secondsLeft);
            yield return new WaitForSeconds(1f);
            secondsLeft--;
        }
        str = ToClock(0);
        if (_uiText != null) _uiText.text = str;
        if (_txtMesh != null) _txtMesh.text = str;
    }

    private string ToClock(int timeLeft)
    {
        switch (shape)
        {
            case ClockShape.Seconds:
                return string.Format("{0}", (timeLeft % 60).ToString("00"));

            case ClockShape.MinutesAndSeconds:
                return string.Format("{0}:{1}", (timeLeft / 60 % 60).ToString("00"), (timeLeft % 60).ToString("00"));

            case ClockShape.HoursAndMinutesAndSeconds:
                return string.Format("{0}:{1}:{2}", (timeLeft / 60 / 60).ToString("00"), (timeLeft / 60 % 60).ToString("00"), (timeLeft % 60).ToString("00"));

            default:
                throw new System.ArgumentOutOfRangeException("clock shapre not supported " + shape);
        }
    }
}
