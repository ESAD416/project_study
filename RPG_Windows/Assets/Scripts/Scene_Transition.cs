using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Transition : MonoBehaviour
{

    public string sceneNameToLoad;


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && !other.isTrigger) {
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
