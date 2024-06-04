using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Stairs_Gateway : MonoBehaviour
{

    [SerializeField] protected Avatar m_avatar;
    [SerializeField] protected StairsController m_stairsCtrl;
    public int SelfHeight;

    void Start() {
    }

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        // Debug.Log("Stairs_Gateway OnTriggerEnter2D name: "+otherCollider.gameObject.name);
        // Debug.Log("Stairs_Gateway OnTriggerEnter2D layer: "+otherCollider.gameObject.layer);
        // Debug.Log("Stairs_Gateway OnTriggerEnter2D Hittable layer: "+LayerMask.NameToLayer("Hittable"));
        if(otherCollider.CompareTag("Player") && otherCollider.gameObject.layer !=  LayerMask.NameToLayer("Hittable")) {
            if(m_avatar.OnStairs) {
                Debug.Log("Stairs_Gateway OnTriggerEnter2D name: "+otherCollider.gameObject.name);
                if(!m_stairsCtrl.OnStairsAtPlayerPosition(m_avatar.BodyCollider.bounds, true)) {
                    m_avatar.SetCurrentHeight(SelfHeight);
                    m_avatar.SetLastHeight(SelfHeight);
                    m_avatar.OnStairs = false;
                }
            }
 
        }
        
    }

    protected void OnTriggerExit2D(Collider2D otherCollider) {
        if(otherCollider.CompareTag("Player") && otherCollider.gameObject.layer !=  LayerMask.NameToLayer("Hittable") ) {
            if(!m_avatar.OnStairs) {
                Debug.Log("Stairs_Gateway OnTriggerEnter2D name: "+otherCollider.gameObject.name);
                if(m_stairsCtrl.OnStairsAtPlayerPosition(m_avatar.BodyCollider.bounds)) {
                    m_avatar.OnStairs = true;
                }
            } 
        }
    }

}
