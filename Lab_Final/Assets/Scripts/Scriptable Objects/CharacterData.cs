using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/Character")]
public class CharacterData : ScriptableObject
{
    #region Movement
    public float _maxHealth;
    public float _speed;
    public float _jumpSpeed;
    public float _jumpForce;
    #endregion

    #region Attack 1
    public float _attackDamage1;
    public Vector2 _knockBack1;
    #endregion

    #region Attack 2
    public float _attackDamage2;
    public Vector2 _knockBack2;
    #endregion

    #region Attack 3
    public float _attackDamage3;
    public Vector2 _knockBack3;
    #endregion

    #region Misc
    public float _invincibleTime;
    #endregion
}
