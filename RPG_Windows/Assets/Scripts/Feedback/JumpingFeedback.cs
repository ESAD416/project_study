using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JumpMechanismUtils;

public class JumpingFeedback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;

    protected Vector3 takeOffCoord = Vector3.zero;
    protected Vector2 takeOffDir = Vector2.zero;
    protected float lastHeight = 0f;
    protected float minjumpOffSet = -0.3f;
    protected float jumpOffset = 0.3f;
    protected float maxJumpHeight = 1.5f;
    protected float jumpIncrement = 0f;
    protected float g = -0.0565f;
    protected bool isJumping;
    protected float jumpingMovementVariable = 0.5f;
    protected JumpState jumpState;
    protected bool jumpHitColli;
    protected Coroutine jumpDelayRoutine;
    protected float jumpDelay = 0.1f;
    protected bool jumpDelaying = false;
    protected Coroutine groundDelayRoutine;
    protected float groundDelay = 0.2f;
    protected bool groundDelaying = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
