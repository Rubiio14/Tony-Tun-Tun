using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerJuice : MonoBehaviour
{
    public static playerJuice instance;
    [Header("Components")]
    playerMovement moveScript;
    playerJump jumpScript;
    [SerializeField]  public Animator myAnimator;
    [Header("Calculations")]
    public float runningSpeed;
    public bool playerGrounded;
    public bool endJump = false;
    public float randomCheckInterval = 5f;
    public float idleProbability = 0.3f;
    private float _timer = 0f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        moveScript = GetComponent<playerMovement>();
        jumpScript = GetComponent<playerJump>();
    }
    private void Update()
    {
        /*
        if (!playerGrounded && !jumpScript._currentlyJumping && !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|ChargedJump_TonyTunTun"))
        {
            myAnimator.SetBool("IsFalling", true);
            myAnimator.ResetTrigger("Landed");
        }
        if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|NormalJump_TonyTunTun") && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            
            //myAnimator.ResetTrigger("Landed");
        }
        */
        if (moveScript.velocity.y < -5)
        {
            myAnimator.SetBool("IsFalling", true);
        }
        /*
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Falling_TonyTunTun 1") && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            myAnimator.SetBool("IsFalling", false);
            //myAnimator.ResetTrigger("Landed");
        }*/
        if (_timer >= randomCheckInterval)
        {
            _timer = 0f;

            if (Random.value < idleProbability)
            {
                myAnimator.SetBool("IsIdleB", true);
            }
        }

        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|IDLE_B") && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {

            myAnimator.SetBool("IsIdleB", false);
        }

        //We need to change the character's running animation to suit their current speed
        runningSpeed = Mathf.Clamp(Mathf.Abs(moveScript.velocity.x), 0, moveScript.maxSpeed);
        myAnimator.SetFloat("runSpeed", runningSpeed);
        checkForLanding();
       
    }

    private void checkForLanding()
    {
        if (playerGround.instance.GetOnGround())
        {
            //By checking for this, and then immediately setting playerGrounded to true, we only run this code once when the player hits the ground 
            playerGrounded = true;
            //Play an animation, some particles, and a sound effect when the player lands
            
            myAnimator.SetTrigger("Landed");
            myAnimator.SetBool("IsFalling", false);
            
        }
        else
        {
            // Player has left the ground, so stop playing the running particles
            playerGrounded = false;
        }
    }


    public void jumpEffects()
    {
        //Play these effects when the player jumps, courtesy of jump script
        myAnimator.ResetTrigger("Landed");
        myAnimator.SetTrigger("Jump");
        //myAnimator.SetBool("IsFalling", true);
        
    }
    public void chargedjumpEffects()
    {
        //Play these effects when the player jumps, courtesy of jump script
        myAnimator.ResetTrigger("Landed");
        myAnimator.SetTrigger("ChargedJump");
        //myAnimator.SetBool("IsFalling", true);
        
    }



}
