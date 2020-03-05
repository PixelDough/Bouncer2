using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string doorName = "";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("Game Manager Spawned");
    }
}
