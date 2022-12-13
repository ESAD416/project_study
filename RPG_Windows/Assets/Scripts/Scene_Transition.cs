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


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && !other.isTrigger) {
            playerStorage.initialPos = playerPos;
            playerStorage.jumpCollidersName = jumpCollidersName;
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
