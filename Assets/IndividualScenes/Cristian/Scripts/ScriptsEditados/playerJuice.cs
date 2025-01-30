using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
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

    //VFXs Tony
    //Movimiento
    [SerializeField]
    GameObject _vfxMove;
    //Salto
    [SerializeField]
    GameObject _vfxJump;

    //Caída
    [SerializeField]
    GameObject _vfxHitGround;

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
        if (moveScript.velocity.y < -5)
        {
            myAnimator.SetBool("IsFalling", true);
        }
        if (_timer >= randomCheckInterval)
        {
            _timer = 0f;

            if (Random.value < idleProbability)
            {
                myAnimator.SetBool("IsIdleB", true);
            }
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|ChargedJump_TonyTunTun") && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !jumpScript.pulsa)
        {
            myAnimator.SetBool("IsIdle", true);
        }

        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|IDLE_B") && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {

            myAnimator.SetBool("IsIdleB", false);
        }

        //We need to change the character's running animation to suit their current speed
        runningSpeed = Mathf.Clamp(Mathf.Abs(moveScript.velocity.x), 0, moveScript.maxSpeed);
        myAnimator.SetFloat("runSpeed", runningSpeed);
        checkForLanding();

        //VFX
        if (playerMovement.instance.pressingKey == true && playerGrounded)
        {
            _vfxMove.SetActive(true);
        }
        else
        {
            _vfxMove.SetActive(false);
        }

        if (playerJump.instance._currentlyJumping == true)
        {
            _vfxJump.SetActive(true);
        }
        else
        {
            _vfxJump.SetActive(false);
        }

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
            

            _vfxHitGround.SetActive(true);

        }
        else
        {
            // Player has left the ground, so stop playing the running particles
            playerGrounded = false;

            _vfxHitGround.SetActive(false);

        }
    }


    public void jumpEffects()
    {
        //Play these effects when the player jumps, courtesy of jump script
        myAnimator.ResetTrigger("Landed");
        myAnimator.SetTrigger("Jump");
        myAnimator.ResetTrigger("ChargedJump");


        //Audio
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.playerJump, this.gameObject.transform.position);

    }
    public void chargedjumpEffects()
    {
        //Play these effects when the player jumps, courtesy of jump script
        //myAnimator.ResetTrigger("Landed");
        myAnimator.SetBool("IsIdle", false);
        myAnimator.SetTrigger("ChargedJump");
        //myAnimator.SetBool("IsFalling", true);
    }
}