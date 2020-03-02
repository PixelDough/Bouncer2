using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{

    Transform initialParent;

    private void Start()
    {
        initialParent = transform.parent;
        transform.parent = null;
    }

    private void LateUpdate()
    {
        transform.position = initialParent.position;
    }

}
