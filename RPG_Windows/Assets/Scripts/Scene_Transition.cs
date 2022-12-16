using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Transition : MonoBehaviour
{

    public string sceneNameToLoad;

    public Vector2 playerPos ;

    public string jumpCollidersName;

    public PlayerStorage playerStorage;

    public TransitionStorage infoStorage;
    
    public Animator transitionAnimaCtrl;

    public float transitionDelay = 1f;


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && !other.isTrigger) {
            playerStorage.initialPos = playerPos;
            playerStorage.jumpCollidersName = jumpCollidersName;
            infoStorage.fadeTransitionActive = true;
            LoadAlterScene();
        }
    }

    private void Start() {
        // Debug.Log("Scene_Transition Start");
        if(infoStorage.fadeTransitionActive) {
            // Debug.Log("Scene_Transition SetTrigger End");
            transitionAnimaCtrl.SetTrigger("End");
            infoStorage.fadeTransitionActive = false;
        }
    }

    public void LoadAlterScene() {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess() {
        transitionAnimaCtrl.SetTrigger("Start");

        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadScene(sceneNameToLoad);
    }
}
