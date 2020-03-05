using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    private void Start()
    {
        BallController player = FindObjectOfType<BallController>();
        Cinemachine.CinemachineVirtualCamera vcam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (player && vcam)
        {
            vcam.Follow = player.transform;
        }


    }

}
