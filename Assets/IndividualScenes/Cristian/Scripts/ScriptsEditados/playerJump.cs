using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
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
    //[SerializeField, Range(0, 1)] [Tooltip("How many times can you jump in the air?")] public int maxAirJumps = 0;

    [Header("Options")]
    [Tooltip("Mantener pulsado para saltar más alto")] public bool variablejumpHeight;
    [SerializeField, Range(1f, 10f)] [Tooltip("Cuanto mantener pulsado para alcanzar la altura máxima")] public float jumpCutOff;
    [SerializeField] [Tooltip("Velocidad máxima de caida del personaje")] public float speedLimit;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("Duración del coyote time")] public float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("Distancia del suelo a la que guarda el Jump Buffer")] public float jumpBuffer = 0.15f;

    [Header("Calculations")]
    public float jumpSpeed;
    private float _defaultGravityScale;
    public float gravMultiplier;

    [Header("Current State")]
    public bool canJumpAgain = false;
    public bool desiredJump;
    private float jumpBufferCounter;
    private float _coyoteTimeCounter = 0;
    public bool pressingJump;
    public bool onGround;
    private bool _currentlyJumping;

    [Header("Charged Jump Current State")]
    private bool _desiredChargedJump;
    private bool _pressingChargedJump;
    private float _jumpMultiplier;
    
    public bool canChargedjump = true;
   

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
        //This function is called when one of the jump buttons (like space or the A button) is pressed.

        if (playerMovementLimiter.instance.CharacterCanMove)
        {
            //When we press the jump button, tell the script that we desire a jump.
            //Also, use the started and canceled contexts to know if we're currently holding the button
            if (context.started)
            {
                pressingJump = false;
                _pressingChargedJump = false;
            }

            if (context.canceled)
            {
                
                    if (canChargedjump)
                    {
                        juice.chargedjumpEffects();
                        //_desiredChargedJump = true;
                        //_pressingChargedJump = true;
                        StartCoroutine(DelayChargedSalto());
                    }
                    else
                    {
                        StartCoroutine(DelayChargedSaltoAgain());
                    }

                    if (jumpBarBehaviour.instance.baseBarImage.fillAmount >= 0.1)
                    {
                        //Base Bar                  
                        if (jumpBarBehaviour.instance.baseBarImage.fillAmount <= 0.5)
                        {
                            _jumpMultiplier = jumpBarBehaviour.instance.halfBaseBar;
                        }
                        if (jumpBarBehaviour.instance.baseBarImage.fillAmount >= 0.5)
                        {
                            //First bar
                            if (jumpBarBehaviour.instance.firstBarImage.fillAmount <= 0.5)
                            {
                                _jumpMultiplier = jumpBarBehaviour.instance.halfFirstBar;
                            }
                            if (jumpBarBehaviour.instance.firstBarImage.fillAmount >= 0.5)
                            {
                                //Second bar
                                if (jumpBarBehaviour.instance.secondBarImage.fillAmount <= 0.5)
                                {
                                    _jumpMultiplier = jumpBarBehaviour.instance.halfSecondBar;
                                }
                                if (jumpBarBehaviour.instance.secondBarImage.fillAmount >= 0.5)
                                {
                                    //Third bar
                                    if (jumpBarBehaviour.instance.thirdBarImage.fillAmount <= 0.5)
                                    {
                                        _jumpMultiplier = jumpBarBehaviour.instance.halfThirdBar;
                                    }
                                    if (jumpBarBehaviour.instance.thirdBarImage.fillAmount >= 0.5)
                                    {
                                        _jumpMultiplier = jumpBarBehaviour.instance.maxThirdBar;
                                    }
                                    else
                                    {
                                        _jumpMultiplier = jumpBarBehaviour.instance.maxSecondBar;
                                    }
                                }
                                else
                                {
                                    _jumpMultiplier = jumpBarBehaviour.instance.maxFirstBar;
                                }
                            }
                            else
                            {
                                _jumpMultiplier = jumpBarBehaviour.instance.maxBaseBar;
                            }
                            canChargedjump = false;
                        }

                    }
                    
                else
                {
                    if (juice != null)
                    {
                        //Apply the jumping effects on the juice script
                        if(!_currentlyJumping)
                        {
                            juice.jumpEffects();
                            StartCoroutine(DelaySalto());
                        }
                        
                                            
                    }
                    
                }
            }
        }
    }

    void Update()
    {
        setPhysics();

        //Check if we're on ground, using player Ground script
        onGround = _ground.GetOnGround();

        //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
        if (jumpBuffer > 0)
        {
            //Instead of immediately turning off "desireJump", start counting up...
            //All the while, the DoAJump function will repeatedly be fired off
            if (desiredJump || _desiredChargedJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    //If time exceeds the jump buffer, turn off "desireJump"
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
            else if (_desiredChargedJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    //If time exceeds the jump buffer, turn off "desireJump"
                    _desiredChargedJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }

        //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
        //So, start the coyote time counter...
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
            //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
            Vector2 newGravity = new Vector2(0, (-2 * jumpHeight * _jumpMultiplier) / (timeToJumpApex * timeToJumpApex));
            rb.gravityScale = (newGravity.y/ Physics2D.gravity.y) * gravMultiplier;
            Debug.Log("Cargado");
        }
        else
        {
            //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
            Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
            rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
        }
        
    }

    private void FixedUpdate()
    {
        //Get velocity from Kit's Rigidbody 
        velocity = rb.linearVelocity;

        //Keep trying to do a jump, for as long as desiredJump is true
        if (desiredJump || _desiredChargedJump)
        {
            DoAJump();
            rb.linearVelocity = velocity;


            //Skip gravity calculations this frame, so currentlyJumping doesn't turn off
            //This makes sure you can't do the coyote time double jump bug
            return;
        }       

        calculateGravity();
    }

    private void calculateGravity()
    {
        //We change the character's gravity based on her Y direction

        //If player is going up...
        if (rb.linearVelocity.y > 0.01f)
        {
            if (onGround)
            {
                //Don't change it if player is stood on something (such as a moving platform)
                gravMultiplier = _defaultGravityScale;
            }
            else
            {
                //If we're using variable jump height...
                if (variablejumpHeight)
                {
                    //Apply upward multiplier if player is rising and holding jump
                    if (pressingJump && _currentlyJumping || _pressingChargedJump && _currentlyJumping)
                    {
                        gravMultiplier = upwardMovementMultiplier;
                    }
                    //But apply a special downward multiplier if the player lets go of jump
                    else
                    {
                        gravMultiplier = jumpCutOff;
                    }
                }
                else
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
            }
        }

        //Else if going down...
        else if (rb.linearVelocity.y < -0.01f)
        {

            if (onGround)
            //Don't change it if player is stood on something (such as a moving platform)
            {
                gravMultiplier = _defaultGravityScale;
                juice.resetFalling();
            }
            else
            {
                //Otherwise, apply the downward gravity multiplier as player comes back to Earth
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
        //FallingAnimationStart
        juice.isFalling();

        //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
        if (onGround || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < coyoteTime) || canJumpAgain)
        {
            desiredJump = false;
            _desiredChargedJump = false;
            jumpBufferCounter = 0;
            _coyoteTimeCounter = 0;

            //Determine the power of the jump, based on our gravity and stats
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight);

            //If player is moving up or down when she jumps (such as when doing a double jump), change the jumpSpeed;
            //This will ensure the jump is the exact same strength, no matter your velocity.
            if (velocity.y > 0f)
            {
                
                
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.linearVelocity.y);
            }

            //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
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

    public void bounceUp(float bounceAmount)
    {
        //Used by the spring y pad
        rb.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
    }
    IEnumerator DelaySalto()
    {
        yield return new WaitForSeconds(0.1f);
        desiredJump = true;
        pressingJump = true;
    }
    IEnumerator DelayChargedSaltoAgain()
    {
        yield return new WaitForSeconds(0.3f);
        canChargedjump = true;
    }
    IEnumerator DelayChargedSalto()
    {
        yield return new WaitForSeconds(0.3f);
        _desiredChargedJump = true;
        _pressingChargedJump = true;
    }
}
