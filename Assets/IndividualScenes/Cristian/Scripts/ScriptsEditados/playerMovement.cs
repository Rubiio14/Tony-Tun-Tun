using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class playerMovement : MonoBehaviour
{
    public static playerMovement instance;

    [Header("Components")]
    private Rigidbody2D _rb;
    playerGround groundScript;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 40f)] public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] public float maxAcceleration = 52f;
    [SerializeField, Range(0f, 100f)] public float maxDecceleration = 52f;
    [SerializeField, Range(0f, 100f)] public float maxTurnSpeed = 80f;
    [SerializeField, Range(0f, 100f)] public float maxAirAcceleration;
    [SerializeField, Range(0f, 100f)] public float maxAirDeceleration;
    [SerializeField, Range(0f, 100f)] public float maxAirTurnSpeed = 80f;

    private float _directionX;
    private Vector2 _desiredVelocity;
    public Vector2 velocity;
    private float _maxSpeedChange;
    private float _acceleration;
    private float _deceleration;
    private float _turnSpeed;

    [Header("Player State")]
    public bool onGround;
    public bool pressingKey;
    public bool useAcceleration;
    public bool isFacingRight;
    private bool isSliding = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        groundScript = GetComponent<playerGround>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (playerMovementLimiter.instance.CharacterCanMove)
        {
            _directionX = context.ReadValue<float>();
        }
    }

    void Update()
    {
        if (!playerMovementLimiter.instance.CharacterCanMove)
        {
            _directionX = 0;
        }
        slideTony();
    }

    private void FixedUpdate()
    {
        onGround = groundScript.GetOnGround();
        velocity = _rb.linearVelocity;

        if (useAcceleration)
        {
            runWithAcceleration();
        }
        else
        {
            if (onGround)
            {
                runWithoutAcceleration();
            }
            else
            {
                runWithAcceleration();
            }
        }
    }

    private void runWithAcceleration()
    {
        _acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        _deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        _turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey)
        {
            if (Mathf.Sign(_directionX) != Mathf.Sign(velocity.x) && _directionX != 0)
            {
                _maxSpeedChange = _turnSpeed * Time.deltaTime;
                playerJuice.instance.myAnimator.SetBool("IsTurn", true);
            }
            else
            {
                _maxSpeedChange = _acceleration * Time.deltaTime;
                playerJuice.instance.myAnimator.SetBool("IsTurn", false);
            }
        }
        else
        {
            _maxSpeedChange = _deceleration * Time.deltaTime;
        }

        velocity.x = Mathf.MoveTowards(velocity.x, _desiredVelocity.x, _maxSpeedChange);
        _rb.linearVelocity = velocity;
    }

    private void runWithoutAcceleration()
    {
        velocity.x = _desiredVelocity.x;
        _rb.linearVelocity = velocity;
    }

    private void slideTony()
    {
        if (_directionX != 0)
        {
            transform.localScale = new Vector3(_directionX > 0 ? 1 : -1, 1, 1);
            pressingKey = true;
        }
        else
        {
            pressingKey = false;
        }

        _desiredVelocity = new Vector2(_directionX, 0f) * Mathf.Max(maxSpeed, 0f);

        if (_directionX > 0)
        {
            isFacingRight = true;
        }
        else if (_directionX < 0)
        {
            isFacingRight = false;
        }

        if (playerJuice.instance.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SLIDETURN"))
        {
            if (!isSliding)
            {
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.playerSlide, this.gameObject.transform.position);
                isSliding = true;
            }
        }
        else
        {
            isSliding = false;
        }

        resetSlideAnimation();
    }

    private void resetSlideAnimation()
    {
        if (playerJuice.instance.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("SLIDETURN"))
        {
            playerJuice.instance.myAnimator.SetBool("IsTurn", false);
        }

        if (velocity.x == 0)
        {
            playerJuice.instance.myAnimator.SetBool("IsTurn", false);
        }
    }
}
