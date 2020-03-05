using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public AsyncOperation asyncLoad;
    public RectTransform canvasGroup;
    public SceneIndexes sceneIndex = SceneIndexes.MANAGER;

    [FMODUnity.ParamRef]
    public string sceneChangeParameter;

    public enum TransitionTypes
    {
        Instant,
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom
    }
    public TransitionTypes transitionType = TransitionTypes.Instant;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        switch (transitionType)
        {
            case TransitionTypes.LeftToRight:
                canvasGroup.transform.localPosition = new Vector2(-1920, 0);
                break;
            case TransitionTypes.RightToLeft:
                canvasGroup.transform.localPosition = new Vector2(1920, 0);
                break;
            case TransitionTypes.BottomToTop:
                canvasGroup.transform.localPosition = new Vector2(0, -1920);
                break;
            case TransitionTypes.TopToBottom:
                canvasGroup.transform.localPosition = new Vector2(0, 1920);
                break;
        }

        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {

        // Mute music
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(sceneChangeParameter, 1);

        Vector2 startPos = canvasGroup.transform.localPosition;

        LTDescr leanStartX = LeanTween.moveX(canvasGroup, 0, 1f).setEase(LeanTweenType.easeInOutCubic);
        LTDescr leanStartY = LeanTween.moveY(canvasGroup, 0, 1f).setEase(LeanTweenType.easeInOutCubic);

        while (LeanTween.isTweening(leanStartX.id) && LeanTween.isTweening(leanStartY.id))
        {
            yield return null;
        }

        // Wait for new level to load
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Play music
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(sceneChangeParameter, 0);


        LeanTween.moveX(canvasGroup, -startPos.x, 1f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.moveY(canvasGroup, -startPos.y, 1f).setEase(LeanTweenType.easeInOutCubic);

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);


        yield return null;
    }

}
