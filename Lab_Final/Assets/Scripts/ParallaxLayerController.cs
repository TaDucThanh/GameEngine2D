using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SmoothParallax : MonoBehaviour
{
    #region Fields
    [SerializeField] private Camera mainCamera;
    [Range(-1f, 1f)]
    [SerializeField] private float parallaxEffect = 0.5f;

    private Material _material;
    private Vector3 _cameraStartPos;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        _material = GetComponent<Renderer>().material;
        _cameraStartPos = mainCamera.transform.position;
    }

    private void LateUpdate()
    {
        UpdateParallax();
        FollowCamera();
    }
    #endregion

    #region Methods
    private void UpdateParallax()
    {
        Vector3 cameraDelta = mainCamera.transform.position - _cameraStartPos;
        Vector2 offset = new Vector2(cameraDelta.x, cameraDelta.y) * parallaxEffect;
        _material.SetTextureOffset("_MainTex", offset);
    }

    private void FollowCamera()
    {
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);
    }
    #endregion
}
