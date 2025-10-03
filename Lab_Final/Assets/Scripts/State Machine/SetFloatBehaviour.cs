using UnityEngine;

public class SetFloatBehaviour : StateMachineBehaviour
{
    #region Inspector Fields
    [SerializeField] private string _floatName;
    [SerializeField] private bool _updateOnEnter;
    [SerializeField] private bool _updateOnExit;
    [SerializeField] private float _valueOnEnter;
    [SerializeField] private float _valueOnExit;
    #endregion

    #region StateMachine Callbacks
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_updateOnEnter)
            animator.SetFloat(_floatName, _valueOnEnter);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_updateOnExit)
            animator.SetFloat(_floatName, _valueOnExit);
    }
    #endregion
}
