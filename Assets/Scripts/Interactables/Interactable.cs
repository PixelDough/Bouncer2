using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;

[RequireComponent(typeof(Rigidbody2D))]
public class Interactable : MonoBehaviour
{

    public GameObject popupText;

    [Header("Interaction Event")]
    public UnityEvent interactEvent;

    private bool active = false;

    Player playerInput;

    protected virtual void Start()
    {
        popupText.transform.localScale = Vector3.zero;

        playerInput = ReInput.players.GetPlayer(0);
    }


    protected virtual void Update()
    {
        if (active && GameManager.Instance.playerCanMove && playerInput != null)
        {
            if (playerInput.GetButtonDown(RewiredConsts.Action.Interact))
            {
                _Interact();
            }
        }
    }


    protected virtual void Interact() { }
    private void _Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
        interactEvent.Invoke();

        Interact();
    }

    protected virtual void PlayerEnter() { }
    private void _PlayerEnter()
    {
        TweenEvents.TweenPopupSpiral(popupText.transform);

        active = true;

        PlayerEnter();
    }

    protected virtual void PlayerExit() { }
    private void _PlayerExit()
    {
        TweenEvents.TweenPopupSpiral(popupText.transform, false);

        active = false;

        PlayerExit();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        if (!GameManager.Instance.playerCanMove) { return; }

        _PlayerEnter();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        if (!GameManager.Instance.playerCanMove) { return; }

        _PlayerExit();
    }

}
