using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitSystem : MonoBehaviour
{
    [Header("基本物件")]
    [SerializeField] protected HealthSystem m_targetHealthSystem;
    [SerializeField] protected KnockbackFeedback m_targetKnockbackFeedback;

    #region 受擊參數

    [Header("基本參數")]
    /// <summary>
    /// 正在受擊
    /// </summary>
    public bool IsTakingHit = false;
    public bool IsIgnoreHit = false;

    [SerializeField] protected float onHitDelay;
    public float OnHitDelay => onHitDelay;


    [Header("無敵參數")]
    /// <summary>
    /// 是否無敵狀態
    /// </summary>
   public bool IsInvulnerable = false;
    /// <summary>
    /// 無敵持續時間
    /// </summary>
    [SerializeField] protected float invulnerableDuration;
    protected float invulnerableElapsed;


    [Header("霸體參數")]
    [SerializeField] protected bool isHyperArmor = false;



    protected DamageFlash damageFlash;
    protected Coroutine takeHitRoutine = null;                  // 受擊動作為即時觸發，故宣告一協程進行處理獨立的動作

    #endregion

    protected virtual void Start() {
        damageFlash = GetComponent<DamageFlash>();
    }


    protected virtual void Update() {
        if(IsInvulnerable) {
            invulnerableElapsed += Time.deltaTime;
            if(invulnerableElapsed >= invulnerableDuration) {
                invulnerableElapsed = invulnerableDuration;
                IsInvulnerable = false;
            }
        }
    }
    

    public virtual void TakeHiProcess(Attack_System attacker) {
        Debug.Log("TakeHit attacker: "+attacker.name);
        takeHitRoutine = StartCoroutine(TakeHit(attacker));
        
    }

    public virtual void TakeHiProcess(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) {
        Debug.Log("TakeHit damage: "+damageSystem.Damage+", attackedLocation: "+attackedLocation.position);
        takeHitRoutine = StartCoroutine(TakeHit(damageSystem, attackedLocation, damageCounter));
    }

    protected virtual IEnumerator TakeHit(Attack_System attacker) 
    {
        IsTakingHit = true;
        
        // TODO
            
        yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        // TODO
        
        FinishTakeHit();
    }

    protected virtual IEnumerator TakeHit(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) 
    {
        IsTakingHit = true;

        // TODO
            
        yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        // TODO
        
        FinishTakeHit();
    }

    protected virtual void FinishTakeHit() {
        Debug.Log("HitSystem FinishTakeHit");
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        IsTakingHit = false;
        IsInvulnerable = false;
        
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
