using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitSystem : MonoBehaviour
{
    [Header("基本物件")]
    [SerializeField] protected HealthSystem m_HealthSystem;
    [SerializeField] protected KnockbackFeedback m_KnockbackFeedback;

    #region 受擊參數

    [Header("基本參數")]
    /// <summary>
    /// 正在受擊
    /// </summary>
    public bool isTakingHit = false;

    [SerializeField] protected float onHitDelay;
    public float OnHitDelay => onHitDelay;


    [Header("無敵參數")]
    /// <summary>
    /// 是否無敵狀態
    /// </summary>
    [SerializeField] protected bool isInvulnerable = false;
    /// <summary>
    /// 無敵持續時間
    /// </summary>
    [SerializeField] protected float invulnerableDuration;


    [Header("霸體參數")]
    [SerializeField] protected bool isHyperArmor = false;

    /// <summary>
    /// 受擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine takeHitRoutine = null;

    #endregion

    public virtual void TakeHiProcess(Attack attacker) {
        Debug.Log("TakeHit attacker: "+attacker.name);
        takeHitRoutine = StartCoroutine(TakeHit(attacker));
        
    }

    public virtual void TakeHiProcess(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) {
        Debug.Log("TakeHit damage: "+damageSystem.Damage+", attackedLocation: "+attackedLocation.position);
        takeHitRoutine = StartCoroutine(TakeHit(damageSystem, attackedLocation, damageCounter));
    }

    protected virtual IEnumerator TakeHit(Attack attacker) 
    {
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

            attacker.DamageSystem.OnDamage(m_HealthSystem);
            
            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        } else if(breakableTakenHit) {
            // TODO
        }
        
        FinishTakeHit();
    }

    protected virtual IEnumerator TakeHit(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) 
    {
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

            damageSystem.OnDamage(m_HealthSystem);
            
            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        } else if(breakableTakenHit) {
            // TODO
        }
        
        FinishTakeHit();
    }

    protected virtual void FinishTakeHit() {
        Debug.Log("HitSystem FinishTakeHit");
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        isTakingHit = false;
        isInvulnerable = false;
        
        //OnHitEventEnd?.Invoke();
        
        Debug.Log("HitSystem FinishTakeHit");
    }

    protected virtual void ViewHitEffect(Vector3 initialPosition = default(Vector3)) {
        GameObject projectile = HitEffectPool.instance.GetPooledGameObject(initialPosition);
        if(projectile != null) {

            // TODO


            projectile.SetActive(true);
            StartCoroutine(HitEffectPool.instance.DestroyHitEffectObjects(projectile));
        }
    }

}
