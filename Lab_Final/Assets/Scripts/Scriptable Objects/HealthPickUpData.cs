using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthPickUpData", menuName = "Scriptable Objects/HealthPickUp")]
public class HealthPickUpData : ScriptableObject
{
    #region Properties
    public float _healthRestore;
    public Sprite _healthPickUpSprite;
    #endregion
}
