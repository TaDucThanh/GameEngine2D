using Unity.Cinemachine;
using UnityEngine;
[RequireComponent(typeof(CinemachineCamera))]

public class AutoFollowPlayer : MonoBehaviour
{
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        var cinecam= GetComponent<CinemachineCamera>();
        cinecam.Follow = player.transform;
    }
}
