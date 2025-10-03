using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    #region Inspector Fields
    [Header("SFX Settings")]
    [SerializeField] private string _sfx;
    [SerializeField] private bool _playOnEnter = true;
    [SerializeField] private bool _playOnExit = false;
    [SerializeField] private bool _playAfterDelay = false;
    [SerializeField] private float _delay = 0.25f;

    [Header("Loop Settings")]
    [SerializeField] private bool _loop = false;
    [SerializeField] private bool _stopLoopOnExit = true;
    #endregion

    #region Private Fields
    private float _timeSinceEntered;
    private bool _hasDelayedSoundPlayed;
    #endregion

    #region StateMachine Callbacks
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timeSinceEntered = 0f;
        _hasDelayedSoundPlayed = false;

        if (_playOnEnter)
        {
            if (_loop)
                AudioManager.Instance.PlayLoopingSFX(_sfx);
            else
                AudioManager.Instance.PlaySFX(_sfx);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playAfterDelay && !_hasDelayedSoundPlayed)
        {
            _timeSinceEntered += Time.deltaTime;
            if (_timeSinceEntered >= _delay)
            {
                if (_loop)
                    AudioManager.Instance.PlayLoopingSFX(_sfx);
                else
                    AudioManager.Instance.PlaySFX(_sfx);

                _hasDelayedSoundPlayed = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playOnExit)
        {
            if (_loop)
                AudioManager.Instance.PlayLoopingSFX(_sfx);
            else
                AudioManager.Instance.PlaySFX(_sfx);
        }

        if (_loop && _stopLoopOnExit)
        {
            AudioManager.Instance.StopLoopingSFX();
        }
    }
    #endregion
}
