using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Stairs_Gateway<T> : MonoBehaviour where T : Collider2D
{

    [SerializeField] protected Player<T> m_player;
    [SerializeField] protected StairsController m_stairsCtrl;
    public int SelfHeight;

    void Start() {
    }

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        // Debug.Log("Stairs_Gateway OnTriggerEnter2D name: "+otherCollider.gameObject.name);
        // Debug.Log("Stairs_Gateway OnTriggerEnter2D layer: "+otherCollider.gameObject.layer);
        // Debug.Log("Stairs_Gateway OnTriggerEnter2D Hittable layer: "+LayerMask.NameToLayer("Hittable"));
        if(otherCollider.CompareTag("Player") && otherCollider.gameObject.layer !=  LayerMask.NameToLayer("Hittable")) {
            if(m_player.OnStairs) {
                Debug.Log("Stairs_Gateway OnTriggerEnter2D name: "+otherCollider.gameObject.name);
                if(!m_stairsCtrl.OnStairsAtPlayerPosition(m_player.BodyCollider.bounds, true)) {
                    m_player.SetCurrentHeight(SelfHeight);
                    m_player.SetLastHeight(SelfHeight);
                    m_player.SetOnStairs(false);
                }
            }
 
        }
        
    }

    protected void OnTriggerExit2D(Collider2D otherCollider) {
        if(otherCollider.CompareTag("Player") && otherCollider.gameObject.layer !=  LayerMask.NameToLayer("Hittable") ) {
            if(!m_player.OnStairs) {
                Debug.Log("Stairs_Gateway OnTriggerEnter2D name: "+otherCollider.gameObject.name);
                if(m_stairsCtrl.OnStairsAtPlayerPosition(m_player.BodyCollider.bounds)) {
                    m_player.SetOnStairs(true);
                }
            } 
        }
    }

}
