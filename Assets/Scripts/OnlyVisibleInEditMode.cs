using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyVisibleInEditMode : MonoBehaviour
{

    private void Awake()
    {
        foreach(Renderer r in GetComponents<Renderer>())
        {
            r.enabled = false;
        }
    }

}
