using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem : MonoBehaviour
{
    [SerializeField] private HealthSystem m_HealthSystem;

    #region 受擊參數
    /// <summary>
    /// 正在受擊
    /// </summary>
    public bool isTakingHit = false;

    [SerializeField] private float onHitDelay;

    /// <summary>
    /// 是否無敵狀態
    /// </summary>
    [SerializeField] private bool isInvulnerable = false;
    /// <summary>
    /// 無敵持續時間
    /// </summary>
    [SerializeField] private float invulnerableDuration;


    [SerializeField] private bool isHyperArmor = false;
    [SerializeField] private int knockOffThrust ;


    /// <summary>
    /// 受擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine takeHitRoutine = null;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeHiProcess(Attack attacker) {
        Debug.Log("TakeHit attacker: "+attacker.name);
        takeHitRoutine = StartCoroutine(TakeHit(attacker));
        
    }

    protected IEnumerator TakeHit(Attack attacker) {
        
        isTakingHit = true;

        var charactorTakenHit = GetComponent<Charactor>();
        var breakableTakenHit = GetComponent<BreakableObjects>();
        if(charactorTakenHit != null) {
            if(isInvulnerable)
                yield break;
            else if(!isInvulnerable) isInvulnerable = true;

            if(!isHyperArmor) {
                KnockbackFeedback feedback = GetComponent<KnockbackFeedback>();
                feedback.ActiveFeedback(attacker.transform.position);
            }

            m_HealthSystem.OnDamage(attacker.Damage);
            
            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        } else if(breakableTakenHit) {
            // TODO
        }
        
        FinishTakeHit();
    }

    protected void FinishTakeHit() {
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        isTakingHit = false;
        isInvulnerable = false;
        
        Debug.Log("FinishTakeHit");
    }

    


}
