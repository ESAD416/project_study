using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamniatController : MonoBehaviour
{
    [Header("Control Scripts")]
    [SerializeField] protected Movement_Lamniat m_lamniatMovement;
    public Movement_Lamniat LamniatMovement =>this.m_lamniatMovement;
    [SerializeField] protected DynamicJump_Lamniat m_LamiatJump;
    public DynamicJump_Lamniat LamniatJump =>this.m_LamiatJump;
    [SerializeField] protected Combat_Lamniat m_lamniatCombat;
    public Combat_Lamniat LamniatCombat =>this.m_lamniatCombat;
    [SerializeField] protected Dodge_Lamniat m_lamniatDodge;
    public Dodge_Lamniat LamniatDodge =>this.m_lamniatDodge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
