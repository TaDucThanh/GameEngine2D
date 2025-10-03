using UnityEngine;

public class Attack : MonoBehaviour
{
    #region Fields
    private ICharacter _character;
    private CharacterData _characterData;
    private Transform _rootParent;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _rootParent = GetRootParent();

        _character = _rootParent.GetComponent<ICharacter>();
        if (_character == null) return;

        _characterData = _character.GetCharacterData();
    }
    #endregion

    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damagable = collision.GetComponent<DamagableAndHealable>();
        if (damagable == null) return;
        if (_characterData == null || _rootParent == null)
            return;

        if (!TryGetAttackData(out float attackDamage, out Vector2 knockBack))
            return;

        Vector2 knockBackDir = new Vector2(knockBack.x * Mathf.Sign(_rootParent.localScale.x), knockBack.y);

        if (!damagable.Hit(attackDamage, knockBackDir))
            Debug.Log("Target is either dead or invincible.");
    }
    #endregion

    #region Helpers
    private Transform GetRootParent()
    {
        Transform root = transform;
        while (root.parent != null)
            root = root.parent;
        return root;
    }

    private bool TryGetAttackData(out float damage, out Vector2 knockBack)
    {
        damage = 0f;
        knockBack = Vector2.zero;

        string parentName = transform.parent.name;
        switch (parentName)
        {
            case "Attack 1":
                damage = _characterData._attackDamage1;
                knockBack = _characterData._knockBack1;
                return true;

            case "Attack 2":
                damage = _characterData._attackDamage2;
                knockBack = _characterData._knockBack2;
                return true;

            case "Attack 3":
                damage = _characterData._attackDamage3;
                knockBack = _characterData._knockBack3;
                return true;

            default:
                Debug.LogError("Attack object not named correctly. Use 'Attack 1', 'Attack 2', or 'Attack 3'.");
                return false;
        }
    }
    #endregion
}
