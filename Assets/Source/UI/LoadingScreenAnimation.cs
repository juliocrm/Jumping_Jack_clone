using UnityEngine;

public class LoadingScreenAnimation : MonoBehaviour {
    
    private Animator _titleAnimator;
    public bool AnimFinished { get; private set; }

    // Use this for initialization
    void Start () {
        _titleAnimator = GetComponent<Animator>();
        AnimFinished = false;
	}
	
    public void IntroAnimation()
    {
        _titleAnimator.SetInteger("State", 1);
    }
    
    public void OutAnimation()
    {
        _titleAnimator.SetInteger("State", 0);
    }

    public void AnimationFinished()
    {
        AnimFinished = true;
    }
}
