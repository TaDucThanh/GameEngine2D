using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    #region Fields
    private float _fadeDuration;
    private float _timeElapsed;
    private GameObject _objectToFade;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    #endregion

    #region Unity StateMachine Callbacks
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fadeDuration = stateInfo.length;
        _objectToFade = animator.gameObject;
        _spriteRenderer = _objectToFade.GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _timeElapsed = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timeElapsed += Time.deltaTime;
        float newAlpha = Mathf.Lerp(1f, 0f, _timeElapsed / _fadeDuration);
        _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, newAlpha);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Object.Destroy(_objectToFade);
    }
    #endregion
}
