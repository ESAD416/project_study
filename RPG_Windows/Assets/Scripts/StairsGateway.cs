using System.Collections;
using UnityEngine;

public class StairsGateway : MonoBehaviour
{

    protected Player player;

    void Start() {
        player = GameObject.FindObjectOfType(typeof(Player)) as Player;
    }    

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Player") {
            if(string.IsNullOrEmpty(player.onStairs)) {
                switch(gameObject.tag) 
                {  
                    case "Stair_Top":
                        player.stair_start = "Stair_Top";
                        break;
                    case "Stair_Down":
                        player.stair_start = "Stair_Down";
                        break;
                    default:
                        player.stair_start = "Untagged"; 
                        break;

                }
            }
            else {
                switch(gameObject.tag) 
                {  
                    case "Stair_Top":
                        player.stair_end = "Stair_Top";
                        break;
                    case "Stair_Down":
                        player.stair_end = "Stair_Down";
                        break;
                    default:
                        player.stair_end = "Untagged"; 
                        break;

                }
            }

        }
        // Debug.Log("stair_start: "+player.stair_start);
        // Debug.Log("stair_end: "+player.stair_end);
        // base.OnTriggerEnter2D(otherCollider);
    }

    protected void OnTriggerExit2D(Collider2D otherCollider) {

    }

}
