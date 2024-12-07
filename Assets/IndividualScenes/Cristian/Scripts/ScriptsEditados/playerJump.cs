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
    //[Tooltip("Mantener pulsado para saltar más alto")] public bool variablejumpHeight;
    [SerializeField, Range(1f, 10f)] [Tooltip("Cuanto mantener pulsado para alcanzar la altura máxima")] public float jumpCutOff;
    [SerializeField] [Tooltip("Velocidad máxima de caida del personaje")] public float speedLimit;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("Duración del coyote time")] public float coyoteTime = 0.15f;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("Distancia del suelo a la que guarda el Jump Buffer")] public float jumpBuffer = 0.15f;

    [Header("Calculations")]
    public float jumpSpeed;
    private float _defaultGravityScale;
    public float gravMultiplier;

    [Header("Current State")]
    //public bool canJumpAgain = false;
    public bool desiredJump;
    private float jumpBufferCounter;
    private float _coyoteTimeCounter = 0;
    public bool pressingJump;
    public bool onGround;
    private bool _currentlyJumping;

    [Header("Charged Jump Current State")]
    public bool _desiredChargedJump;
    //private bool _pressingChargedJump;
    private float _jumpMultiplier;
  
   

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
            if (context.canceled)
            {
                if (hudManager.instance.jumpImage.fillAmount >= 0.1)
                {
                    hudManager.instance.staminaImage.fillAmount = 0f;
                    if (onGround)
                    {
                        juice.chargedjumpEffects();
                        StartCoroutine(DelayChargedSalto());
                    }
                    float fillAmount = hudManager.instance.jumpImage.fillAmount;
                    splitedChargedJump(fillAmount);                   
                }
                else
                {
                    if (onGround)
                    {
                        juice.jumpEffects();
                        StartCoroutine(DelaySalto());
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
                rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
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

                    gravMultiplier = upwardMovementMultiplier;
                    //If we're using variable jump height...
                    /*
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
                        Debug.Log("Else");
                        gravMultiplier = upwardMovementMultiplier;
                    }
                    */
                }
            }

            //Else if going down...
            else if (rb.linearVelocity.y < -0.01f)
            {

                if (onGround)
                //Don't change it if player is stood on something (such as a moving platform)
                {
                    gravMultiplier = _defaultGravityScale;
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

            //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
            if (onGround || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < coyoteTime))
            {
                //desiredJump = false;
                //_desiredChargedJump = false;
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
        IEnumerator DelayChargedSalto()
        {
            yield return new WaitForSeconds(0.2f);
            rb.linearVelocityX = 0f;
            yield return new WaitForSeconds(0.1f);
            _desiredChargedJump = true;
            //_pressingChargedJump = true;
        }
    void splitedChargedJump(float fillAmount)
    {
        if (fillAmount >= 1)
        {
            _jumpMultiplier = hudManager.instance.maxThirdBar;
            Debug.Log("MaxThirdBar");
            return; // Detiene el flujo
        }
        else if (fillAmount >= 0.875)
        {
            _jumpMultiplier = hudManager.instance.halfThirdBar;
            Debug.Log("HalfThirdBar");
            return; // Detiene el flujo
        }
        SecondShoe(fillAmount); // Llama a la siguiente función solo si no se cumple ninguna condición
    }

    void SecondShoe(float fillAmount)
    {
        if (fillAmount >= 0.75)
        {
            _jumpMultiplier = hudManager.instance.maxSecondBar;
            Debug.Log("MaxSecondBar");
            return; // Detiene el flujo
        }
        else if (fillAmount >= 0.625)
        {
            _jumpMultiplier = hudManager.instance.halfSecondBar;
            Debug.Log("HalfSecondBar");
            return; // Detiene el flujo
        }
        FirstShoe(fillAmount); // Llama a la siguiente función solo si no se cumple ninguna condición
    }

    void FirstShoe(float fillAmount)
    {
        if (fillAmount >= 0.5)
        {
            _jumpMultiplier = hudManager.instance.maxFirstBar;
            Debug.Log("MaxFirstBar");
            return; // Detiene el flujo
        }
        else if (fillAmount >= 0.375)
        {
            _jumpMultiplier = hudManager.instance.halfFirstBar;
            Debug.Log("HalfFirstBar");
            return; // Detiene el flujo
        }
        BaseShoe(fillAmount); // Llama a la siguiente función solo si no se cumple ninguna condición
    }

    void BaseShoe(float fillAmount)
    {
        if (fillAmount >= 0.25)
        {
            _jumpMultiplier = hudManager.instance.maxBaseBar;
            Debug.Log("MaxBaseBar");
            return; // Detiene el flujo
        }
        else if (fillAmount >= 0.125)
        {
            _jumpMultiplier = hudManager.instance.halfBaseBar;
            Debug.Log("HalfBaseBar");
            return; // Detiene el flujo
        }
    }

}

