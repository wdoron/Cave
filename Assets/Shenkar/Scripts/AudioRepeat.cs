using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRepeat : MonoBehaviour
{
    [SerializeField] private AudioSource audioS;
    [SerializeField]private float defaultWaitTime = 2;
    [SerializeField] private Vector2 percentClamp = new Vector2(0f,1f);
    [SerializeField] private Vector2 pitchRange = new Vector2(0f, 0.3f);
    private float curWaitFactor = 1;
    public float minPitchValue = .75f;
    private bool wasAudioActive;

    
    // Start is called before the first frame update
    void OnEnable()
    {
        curWaitFactor = 1;
    }

    public void SetPercent(float per) {
        curWaitFactor = per;
        if (per.InRange(pitchRange.x, pitchRange.y)) {
            var pitchPerc = 1-((per - pitchRange.x) / (pitchRange.y - pitchRange.x));
            audioS.pitch = minPitchValue + (3- minPitchValue) * pitchPerc;
        }
    }

    void AudioDone() {
        var curFactor = Mathf.Clamp(curWaitFactor, percentClamp.x, percentClamp.y);
        var waitTime = defaultWaitTime * curFactor;
        if (waitTime < Time.deltaTime) audioS.Play();
        else 
            this.WaitForSeconds(waitTime, () => audioS.Play());

        //Debug.Log("Call audioDone " + waitTime + ": " + curFactor);
    }

    void Update()
    {
        if (wasAudioActive && audioS.isPlaying == false) {
            
            AudioDone();
        }
        wasAudioActive = audioS.isPlaying;
    }
}
