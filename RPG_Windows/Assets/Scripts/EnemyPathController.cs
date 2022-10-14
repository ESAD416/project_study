using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathController : MonoBehaviour
{
    [SerializeField] private Enemy_Abstract Enemy;
    [SerializeField] private AIChaser AIChaser;
    [SerializeField] private bool isHorizontal;

    private Vector3 defaultPos;
    private Dictionary<string, Vector3> pathPara = new Dictionary<string, Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        defaultPos = Enemy.transform.position;
        Debug.Log("defaultPos: "+defaultPos);

        if(isHorizontal) {
            foreach (Transform child in transform)
            {
                if(child.gameObject.tag == "Enemy_Turn") {
                    //Debug.Log("child pos: "+child.position);
                    Vector3 v = defaultPos - child.position;
                    //Debug.Log("dir: "+v);
                    pathPara.Add(child.gameObject.name, v);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(AIChaser.gameObject.activeSelf) {
            if(isHorizontal) {
                if((AIChaser.TargetModel != null && AIChaser.targetVisable)) {
                    UpdatePathParaForHorizontal();
                } else {
                    StartCoroutine(UpdatePathParaEndingDelay());
                }
            }
        }
    }

    private IEnumerator UpdatePathParaEndingDelay() {
        UpdatePathParaForHorizontal();
        yield return new WaitForSeconds(AIChaser.chaseModeDelay);  // hardcasted casted time for debugged
    }

    private void UpdatePathParaForHorizontal() {
        var enemyPos = Enemy.transform.position;

        foreach (Transform child in transform) {
            if(child.gameObject.tag == "Enemy_Turn") {
                var v = pathPara[child.gameObject.name];
                child.position = enemyPos - v;
            }
        }
    }
}
