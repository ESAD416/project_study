using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Transition : MonoBehaviour
{
    public int sceneIndexToLoad;
    public string sceneNameToLoad;

    [Header("Player Position Controls")]
    public Vector2 playerPos ;
    public string jumpCollidersName;
    public PlayerStorage playerStorage;

    [Header("Transition Controls")]
    public TransitionStorage infoStorage;
    public Animator transitionAnimaCtrl;
    public float transitionDelay = 1f;

    protected virtual void LoadScene() {
        StartCoroutine(LoadSceneProcess());
    }

    protected virtual IEnumerator LoadSceneProcess() {
        transitionAnimaCtrl.SetTrigger("Start");

        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadScene(sceneNameToLoad);
    }
}
