using UnityEngine;

public class SetBoolenBehaviour : StateMachineBehaviour
{
    #region Serialized Fields
    [SerializeField] private string _boolName;
    [SerializeField] private bool _updateOnState = true;
    [SerializeField] private bool _valueOnEnter;
    [SerializeField] private bool _valueOnExit;
    #endregion

    #region Unity Callbacks
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_updateOnState)
            animator.SetBool(_boolName, _valueOnEnter);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_updateOnState)
            animator.SetBool(_boolName, _valueOnExit);
    }
    #endregion
}
