using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerJuice : MonoBehaviour
{
    [Header("Components")]
    playerMovement moveScript;
    playerJump jumpScript;
    [SerializeField]  public Animator myAnimator;
    [Header("Calculations")]
    public float runningSpeed;
    public bool playerGrounded;
    public bool endJump = false;

    void Start()
    {
        moveScript = GetComponent<playerMovement>();
        jumpScript = GetComponent<playerJump>();
    }
    private void Update()
    {
        //We need to change the character's running animation to suit their current speed
        runningSpeed = Mathf.Clamp(Mathf.Abs(moveScript.velocity.x), 0, moveScript.maxSpeed);
        myAnimator.SetFloat("runSpeed", runningSpeed);
        checkForLanding();
       
    }

    private void checkForLanding()
    {
        if (!playerGrounded && jumpScript.onGround)
        {
            //By checking for this, and then immediately setting playerGrounded to true, we only run this code once when the player hits the ground 
            playerGrounded = true;
            //Play an animation, some particles, and a sound effect when the player lands
            myAnimator.SetTrigger("Landed");

        }
        else if (playerGrounded && !jumpScript.onGround)
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
        StartCoroutine(RestartIdle(0.01f, 0.5f));
    }
    public void chargedjumpEffects()
    {
        //Play these effects when the player jumps, courtesy of jump script
        myAnimator.ResetTrigger("Landed");
        myAnimator.SetTrigger("ChargedJump");
        StartCoroutine(RestartIdle(0.01f, 1.5f));
    }

    IEnumerator RestartIdle(float time, float timelanded)
    {
        yield return new WaitForSeconds(time);
        myAnimator.SetBool("IsFalling", true);
        yield return new WaitForSeconds(timelanded);
        myAnimator.SetBool("IsFalling", false);
    }
}
