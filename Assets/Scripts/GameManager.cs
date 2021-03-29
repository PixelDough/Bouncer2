using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool playerCanMove = true;
    public string doorName = "";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("Game Manager Spawned");

        Application.targetFrameRate = 60;

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneIndexes.MANAGER) SceneManager.LoadScene((int)SceneIndexes.LEVEL_TREE_HOUSE);
    }
}
