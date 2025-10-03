using UnityEngine;
using UnityEngine.Events;

public class GameEvents
{
    #region Events
    /// <summary>
    /// Event khi nhân vật bị gây sát thương
    /// </summary>
    public static UnityAction<GameObject, float> characterDamaged;

    /// <summary>
    /// Event khi nhân vật được hồi máu
    /// </summary>
    public static UnityAction<GameObject, float> characterHealed;
    #endregion
}
