using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneChanger : MonoBehaviour
{

    public LoadingScreen loadingScreen;
    public Transform playerSpawnPoint;
    public string doorName = null;
    public SceneIndexes sceneIndex = SceneIndexes.MANAGER;
    public LoadingScreen.TransitionTypes transitionType;

    private bool changingScenes = false;

    private void Start()
    {
        if (GameManager.Instance.doorName == doorName)
        {
            FindObjectOfType<BallController>().transform.position = playerSpawnPoint.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (changingScenes) { Debug.LogError("Already changing scenes."); return; }
        if (doorName == null) { Debug.LogError("doorName is null."); return; }
        if (collision.tag != "Player") { Debug.LogError("Collider does not have the tag \"Player\""); return; }
        if ((int)sceneIndex > SceneManager.sceneCountInBuildSettings) { Debug.LogError("Scene Index outside of SceneManager range."); return; }

        if (GameManager.Instance) GameManager.Instance.doorName = doorName;

        Debug.Log("Changing Scenes");
        changingScenes = true;

        LoadingScreen ls = null;
        if (loadingScreen)
        {
            ls = Instantiate(loadingScreen);
            ls.transitionType = transitionType;
            ls.sceneIndex = sceneIndex;
        }


    }

    private IEnumerator ChangeScene()
    {

        Debug.Log("Changing Scenes");

        changingScenes = true;

        LoadingScreen ls = null;
        if (loadingScreen)
        {
            ls = Instantiate(loadingScreen);
            ls.transitionType = transitionType;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Single);

        if (ls != null) ls.asyncLoad = asyncLoad;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return null;
    }

    private void OnDrawGizmos()
    {
        if (playerSpawnPoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(playerSpawnPoint.position, 0.5f);
        }
    }

}
