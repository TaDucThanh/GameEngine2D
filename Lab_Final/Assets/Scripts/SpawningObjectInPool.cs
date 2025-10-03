using System.Collections.Generic;
using UnityEngine;

public class SpawningObjectInPool : MonoBehaviour
{
    #region Singleton
    public static SpawningObjectInPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Fields
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private Transform _spawningPosition;

    private readonly Queue<GameObject> _pool = new();
    #endregion

    #region Public Methods
    public GameObject GetObject()
    {
        GameObject obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNewObject();
        ResetObject(obj);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        ResetTransform(obj);
        _pool.Enqueue(obj);
    }
    #endregion

    #region Private Methods
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(_objectToSpawn, _spawningPosition.position, Quaternion.identity, transform);
        obj.SetActive(false);
        return obj;
    }

    private void ResetObject(GameObject obj)
    {
        // Đặt hướng dựa trên player
        int direction = PlayerController.Instance.IsFacingRight ? 1 : -1;
        Vector2 originalScale = obj.transform.localScale;
        obj.transform.localScale = new Vector2(Mathf.Abs(originalScale.x) * direction, originalScale.y);

        // Reset vị trí và active
        obj.transform.position = _spawningPosition.position;
        obj.SetActive(true);
    }

    private void ResetTransform(GameObject obj)
    {
        obj.transform.position = _spawningPosition.position;
        obj.transform.rotation = Quaternion.identity;
    }
    #endregion
}
