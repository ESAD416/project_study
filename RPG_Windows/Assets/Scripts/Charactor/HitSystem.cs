using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitSystem : MonoBehaviour
{
    [Header("基本物件")]
    [SerializeField] private HealthSystem m_HealthSystem;
    [SerializeField] private KnockbackFeedback m_KnockbackFeedback;


    #region 受擊參數

    [Header("基本參數")]
    /// <summary>
    /// 正在受擊
    /// </summary>
    public bool isTakingHit = false;

    [SerializeField] private float onHitDelay;


    [Header("無敵參數")]
    /// <summary>
    /// 是否無敵狀態
    /// </summary>
    [SerializeField] private bool isInvulnerable = false;
    /// <summary>
    /// 無敵持續時間
    /// </summary>
    [SerializeField] private float invulnerableDuration;


    [Header("霸體參數")]
    [SerializeField] private bool isHyperArmor = false;

    [HideInInspector] public int TargetSelectedIndex = 0;

    [Header("Avatar受擊觸發事件")]
    [HideInInspector] public UnityEvent<BaseStateMachine_Avatar> AvatarOnHitEventBegin;
    [HideInInspector] public UnityEvent<BaseStateMachine_Avatar> AvatarOnHitEventEnd;

    [Header("Enemy受擊觸發事件")]
    [HideInInspector] public UnityEvent<BaseStateMachine_Enemy> EnemyOnHitEventBegin;
    [HideInInspector] public UnityEvent<BaseStateMachine_Enemy> EnemyOnHitEventEnd;


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
    private void Update()
    {
        
    }

    public void TakeHiProcess(Attack attacker) {
        Debug.Log("TakeHit attacker: "+attacker.name);
        takeHitRoutine = StartCoroutine(TakeHit(attacker));
        
    }

    public void TakeHiProcess(float damage, Transform attackedLocation) {
        Debug.Log("TakeHit damage: "+damage+", attackedLocation: "+attackedLocation.position);
        takeHitRoutine = StartCoroutine(TakeHit(damage, attackedLocation));
    }

    protected IEnumerator TakeHit(Attack attacker) {
        
        isTakingHit = true;

        var charactorTakenHit = GetComponent<Charactor>();
        var breakableTakenHit = GetComponent<BreakableObjects>();
        if(charactorTakenHit != null) {
            if(isInvulnerable)
                yield break;
            else if(!isInvulnerable) isInvulnerable = true;

            
            //OnHitEventBegin?.Invoke();

            if(!isHyperArmor && m_KnockbackFeedback != null) {
                m_KnockbackFeedback.ActiveFeedback(attacker.transform.position);
            }

            m_HealthSystem.OnDamage(attacker.Damage);
            
            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        } else if(breakableTakenHit) {
            // TODO
        }
        
        FinishTakeHit();
    }

    protected IEnumerator TakeHit(float damage, Transform attackedLocation) {
        
        isTakingHit = true;

        var charactorTakenHit = GetComponent<Charactor>();
        var breakableTakenHit = GetComponent<BreakableObjects>();
        if(charactorTakenHit != null) {
            if(isInvulnerable)
                yield break;
            else if(!isInvulnerable) isInvulnerable = true;

            //OnHitEventBegin?.Invoke();

            if(!isHyperArmor && m_KnockbackFeedback != null) {
                m_KnockbackFeedback.ActiveFeedback(attackedLocation.position);
            }

            m_HealthSystem.OnDamage(damage);
            
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
        
        //OnHitEventEnd?.Invoke();
        
        Debug.Log("FinishTakeHit");
    }

    


}
