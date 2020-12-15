using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualAnimation : MonoBehaviour {

    private Animator animator;

    [Tooltip("Requires controller with the animation state")]
    public string animationName;

    public int fps;
    public float length;


    public bool loopAnimation = false;

    private float percentPerFrame = -1;
    private float PercentPerFrame
    {
        get
        {
            if (percentPerFrame < 0)
            {
                percentPerFrame = 1f / (length * fps);
            }
			#if UNITY_EDITOR
			percentPerFrame = 1f / (length * fps);
			#endif
            return percentPerFrame;
        }
    }

    private float currentPercent;

	private void Awake(){
		animator = GetComponent<Animator>();
	}
    private void Start()
    {
        animator.speed = 0.0f;
    }

    public void SkipFrames(int frames)
    {
        if (loopAnimation)
        {
            currentPercent = (currentPercent + frames * PercentPerFrame) % 2;//TODO check animation for -0.1f percent
            animator.Play(animationName, 0, currentPercent > 1 ? 2 - currentPercent : currentPercent);
        }
        else
        {
            currentPercent = (currentPercent + frames * PercentPerFrame);
            currentPercent = Mathf.Max(0, Mathf.Min(currentPercent, 1));
            animator.Play(animationName, 0, currentPercent);

        }
    }


   
}