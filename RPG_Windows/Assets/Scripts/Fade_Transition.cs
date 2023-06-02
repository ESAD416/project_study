using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade_Transition : Scene_Transition
{
    private void Start() {
        // Debug.Log("Scene_Transition Start");
        if(transInfoStorage.fadeTransitionActive) {
            // Debug.Log("Scene_Transition SetTrigger End");
            transitionAnimaCtrl.SetTrigger("End");
            transInfoStorage.fadeTransitionActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && !other.isTrigger) {
            destinationPlayerStorage.initialPos = destinationPlayerPos;
            destinationPlayerStorage.jumpCollidersName = destinationJumpCollidersName;
            destinationPlayerStorage.initialHeight = destinationPlayerHeight;
            transInfoStorage.fadeTransitionActive = true;
            LoadScene();
        }
    }

    protected override IEnumerator LoadSceneProcess() {
        transitionAnimaCtrl.SetTrigger("Start");

        yield return base.LoadSceneProcess();
    }
}
