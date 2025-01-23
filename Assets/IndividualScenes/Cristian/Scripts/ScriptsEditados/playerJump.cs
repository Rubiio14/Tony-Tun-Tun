using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using FMODUnity;
public class playerJump : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector] public Rigidbody2D rb;
    private playerGround _ground;
    [HideInInspector] public Vector2 velocity;
    private playerJuice juice;

    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 10f)] [Tooltip("Altura máxima de salto")] public float jumpHeight = 7.3f;
    [SerializeField, Range(0.2f, 1.25f)] [Tooltip("Tiempo que tarda en llegar a la altura máxima de salto")] public float timeToJumpApex;
    [SerializeField, Range(0f, 5f)] [Tooltip("Multiplicador de gravedad cuando está subiendo")] public float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)] [Tooltip("Multiplicador de gravedad cuando está bajando")] public float downwardMovementMultiplier = 6.17f;
    

    [Header("Options")]
    [SerializeField, Range(1f, 10f)] [Tooltip("Cuanto mantener pulsado para alcanzar la altura máxima")] public float jumpCutOff;
    [SerializeField] [Tooltip("Velocidad máxima de caida del personaje")] public float speedLimit;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("Duración del coyote time")] public float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("Distancia del suelo a la que guarda el Jump Buffer")] public float jumpBuffer = 0.15f;

    [Header("Calculations")]
    public float jumpSpeed;
    private float _defaultGravityScale;
    public float gravMultiplier;

    [Header("Current State")]
    public bool desiredJump;
    private float jumpBufferCounter;
    private float _coyoteTimeCounter = 0;
    public bool pressingJump;
    public bool onGround;
    public bool _currentlyJumping;

    [Header("Charged Jump Current State")]
    public bool _desiredChargedJump;
    private float _jumpMultiplier;
    public bool pulsa;


    void Awake()
    {
        //Find the character's Rigidbody and ground detection
        rb = GetComponent<Rigidbody2D>();
        _ground = GetComponent<playerGround>();
        _defaultGravityScale = 1f;
        juice = GetComponentInChildren<playerJuice>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (playerMovementLimiter.instance.CharacterCanMove)
        {           
            if(context.performed)
            {
                pulsa = true;
            }
            if (context.canceled)
            {
                pulsa = false;
                //If bar is being filled
                if (hudManager.instance.jumpImage.fillAmount >= 0.1f)
                {                    
                    //Reset Stamina
                    hudManager.instance.staminaImage.fillAmount = 0f;
                    this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.680205f, 2.148567f);
                    if (onGround)
                    {
                        
                        juice.jumpEffects();
                        _desiredChargedJump = true;
                        //Take the amount of jump charged
                        float fillAmount = hudManager.instance.jumpImage.fillAmount;
                        splitedChargedJump(fillAmount);//Assign _jumpMultiplier value              
                    }
                }
                //If is not being filled execute a normal jump
                else
                {
                    if (onGround)
                    {
                        juice.jumpEffects();
                        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.680205f, 2.148567f);
                        desiredJump = true;
                        pressingJump = true;
                    }
                }
            }    
        }
        
    }
    void Update()
        {
            setPhysics();
            if (hudManager.instance.jumpImage.fillAmount >= 0.1f && !juice.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|ChargedJump_TonyTunTun") && rb.linearVelocity == Vector2.zero && pulsa)
            {
                juice.chargedjumpEffects();
            }
           
            //Check if we're on ground, using player Ground script
            onGround = _ground.GetOnGround();

            //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
            if (jumpBuffer > 0)
            {
            //Instead of immediately turning off "desireJump" && "_desiredChargedJump", start counting up "jumpBufferCounter"
            //All the while, the DoAJump function will repeatedly be fired off
            if (desiredJump || _desiredChargedJump)
                {
                    jumpBufferCounter += Time.deltaTime;

                    if (jumpBufferCounter > jumpBuffer)
                    {
                        //If time exceeds the jump buffer, turn off "desireJump"
                        desiredJump = false;
                        _desiredChargedJump = false;
                        jumpBufferCounter = 0;
                    }
                }
            }
            
            //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
            //So, start the coyote time counter
            if (!_currentlyJumping && !onGround)
            {
                _coyoteTimeCounter += Time.deltaTime;
            }
            else
            {
                //Reset it when we touch the ground, or jump
                _coyoteTimeCounter = 0;
            }
        }

        private void setPhysics()
        {           
            if (_desiredChargedJump)
            {
                //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier
                //Use _jumpMultiplier to jump higher with charged jump
                Vector2 newGravity = new Vector2(0, (-2 * jumpHeight * _jumpMultiplier) / (timeToJumpApex * timeToJumpApex));
                rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
            }
            else
            {
                //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier
                Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
                rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
            }
        }

        private void FixedUpdate()
        {
            //Get velocity from player Rigidbody 
            velocity = rb.linearVelocity;

        //Keep trying to do a jump, for as long as "desiredJump" or "_desiredChargedJump" is true
        if (desiredJump || _desiredChargedJump)
            {
                DoAJump();
                rb.linearVelocity = velocity;
                return;
            }

            calculateGravity();
        }

        private void calculateGravity()
        {
            //We change the character's gravity based on her Y direction

            //If player is going up
            if (rb.linearVelocity.y > 0.01f)
            {
                if (onGround)
                {
                    //Don't change it if player is stood on something (such as a moving platform)
                    gravMultiplier = _defaultGravityScale;
                }
                else
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
            }
            //Else if going down
            else if (rb.linearVelocity.y < -0.01f)
            {
                this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.680205f, 2.967794f);
                if (onGround)
                //Don't change it if player is stood on something (such as a moving platform)
                {
                    gravMultiplier = _defaultGravityScale;
                }
                else
                {
                    //Apply the downward gravity multiplier as player comes back to Earth
                    gravMultiplier = downwardMovementMultiplier;
                }

            }
            //Else not moving vertically at all
            else
            {
                if (onGround)
                {
                    _currentlyJumping = false;                
                }

                gravMultiplier = _defaultGravityScale;
            }

            //Set the character's Rigidbody's velocity
            //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
            rb.linearVelocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
        }

        private void DoAJump()
        {
            //Create the jump, provided we are on the ground, in coyote time
            if (onGround || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < coyoteTime))
            {
                jumpBufferCounter = 0;
                _coyoteTimeCounter = 0;

                //Determine the power of the jump, based on our gravity and stats
                if (desiredJump)
                {
                    jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight);
                }
                else
                {
                    jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight * _jumpMultiplier);
                }


                //If player is moving up or down when she jumps, change the jumpSpeed
                //This will ensure the jump is the exact same strength, no matter your velocity.
                if (velocity.y > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
                }
                else if (velocity.y < 0f)
                {
                    jumpSpeed += Mathf.Abs(rb.linearVelocity.y);
                }

                //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate
                velocity.y += jumpSpeed;
                _currentlyJumping = true;


            }

            if (jumpBuffer == 0)
            {
                //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
                desiredJump = false;
                _desiredChargedJump = false;
            }
        }

        IEnumerator DelayJump()
        {
            //Jump once squash animation ends  
            yield return new WaitForSeconds(0.1f);
            desiredJump = true;
            pressingJump = true;
        }
        IEnumerator DelayChargedJump()
        {   
            //Jump once charged squash animation ends
            yield return new WaitForSeconds(0.3f);
            rb.linearVelocityX = 4f;
            yield return new WaitForSeconds(0.05f);
            _desiredChargedJump = true;
        }
    void splitedChargedJump(float acumulatedJumpForce)
    {
        //If you have all the shoes
        if (acumulatedJumpForce >= 1)
        {
            _jumpMultiplier = hudManager.instance.maxThirdBar;
            Debug.Log("MaxThirdBar");
            return; //Stops the flow
        }
        else if (acumulatedJumpForce >= 0.875)
        {
            _jumpMultiplier = hudManager.instance.halfThirdBar;
            Debug.Log("HalfThirdBar");
            return; //Stops the flow
        }
        SecondShoe(acumulatedJumpForce); //if there is no return jumps to the next function
    }

    void SecondShoe(float fillAmount)
    {
        //If you have 2 Shoes
        if (fillAmount >= 0.75)
        {
            _jumpMultiplier = hudManager.instance.maxSecondBar;
            Debug.Log("MaxSecondBar");
            return; //Stops the flow
        }
        else if (fillAmount >= 0.625)
        {
            _jumpMultiplier = hudManager.instance.halfSecondBar;
            Debug.Log("HalfSecondBar");
            return; //Stops the flow
        }
        FirstShoe(fillAmount); //if there is no return jumps to the next function
    }

    void FirstShoe(float fillAmount)
    {
        //If you have 1 shoe
        if (fillAmount >= 0.5)
        {
            _jumpMultiplier = hudManager.instance.maxFirstBar;
            Debug.Log("MaxFirstBar");
            return; //Stops the flow
        }
        else if (fillAmount >= 0.375)
        {
            _jumpMultiplier = hudManager.instance.halfFirstBar;
            Debug.Log("HalfFirstBar");
            return; //Stops the flow
        }
        BaseShoe(fillAmount); //if there is no return jumps to the next function
    }

    void BaseShoe(float fillAmount)
    {
        //If you don't have shoes
        if (fillAmount >= 0.25)
        {
            _jumpMultiplier = hudManager.instance.maxBaseBar;
            Debug.Log("MaxBaseBar");
            return; //Stops the flow
        }
        else if (fillAmount >= 0.125)
        {
            _jumpMultiplier = hudManager.instance.halfBaseBar;
            Debug.Log("HalfBaseBar");
            return; //Stops the flow
        }
    }

}

