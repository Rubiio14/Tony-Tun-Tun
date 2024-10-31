using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class playerMovement : MonoBehaviour
{
    [Header("Components")]

    private Rigidbody2D _rb;
    playerGround groundScript;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)] [Tooltip("Velocidad máxima de movimeinto")] public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] [Tooltip("Tiempo que tarda en alcanzar la velocidad máxima")] public float maxAcceleration = 52f;
    [SerializeField, Range(0f, 100f)] [Tooltip("Tiempo que tarda en decelerar")] public float maxDecceleration = 52f;
    [SerializeField, Range(0f, 100f)] [Tooltip("Velocidad a la que para al cambiar de dirección")] public float maxTurnSpeed = 80f;
    [SerializeField, Range(0f, 100f)] [Tooltip("Tiempo que tarda en alcanzar la velocidad máxima en el aire")] public float maxAirAcceleration;
    [SerializeField, Range(0f, 100f)] [Tooltip("Tiempo que tarda en frenarse si no se mueve en el aire")] public float maxAirDeceleration;
    [SerializeField, Range(0f, 100f)] [Tooltip("Tiempo que tarda al frenarse cuando cambia de dirección en el aire")] public float maxAirTurnSpeed = 80f;
    //[SerializeField] [Tooltip("Friction to apply against movement on stick")] private float friction;

    //Cálculos
    private float _directionX;
    private Vector2 _desiredVelocity;
    private Vector2 _velocity;
    private float _maxSpeedChange;
    private float _acceleration;
    private float _deceleration;
    private float _turnSpeed;

    [Header("Estado del jugador")]
    public bool onGround;
    public bool pressingKey;
    public bool useAcceleration;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        groundScript = GetComponent<playerGround>();
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        //This is called when you input a direction on a valid input type, such as arrow keys or analogue stick
        //The value will read -1 when pressing left, 0 when idle, and 1 when pressing right.

        if (playerMovementLimiter.instance.CharacterCanMove)
        {
            _directionX = context.ReadValue<float>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!playerMovementLimiter.instance.CharacterCanMove)
        {
            _directionX = 0;
        }



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
    }
    private void FixedUpdate()
    {
        //Fixed update runs in sync with Unity's physics engine

        //Get Kit's current ground status from her ground script
        onGround = playerGround.instance.GetOnGround();

        //Get the Rigidbody's current velocity
        _velocity = _rb.linearVelocity;

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
        //Set our acceleration, deceleration, and turn speed stats, based on whether we're on the ground on in the air

        _acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        _deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        _turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey)
        {
            //If the sign (i.e. positive or negative) of our input direction doesn't match our movement, it means we're turning around and so should use the turn speed stat.
            if (Mathf.Sign(_directionX) != Mathf.Sign(_velocity.x))
            {
                _maxSpeedChange = _turnSpeed * Time.deltaTime;
            }
            else
            {
                //If they match, it means we're simply running along and so should use the acceleration stat
                _maxSpeedChange = _acceleration * Time.deltaTime;
            }
        }
        else
        {
            //And if we're not pressing a direction at all, use the deceleration stat
            _maxSpeedChange = _deceleration * Time.deltaTime;
        }

        //Move our velocity towards the desired velocity, at the rate of the number calculated above
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        //Update the Rigidbody with this new velocity
        _rb.linearVelocity = _velocity;

    }

    private void runWithoutAcceleration()
    {
        //If we're not using acceleration and deceleration, just send our desired velocity (direction * max speed) to the Rigidbody
        _velocity.x = _desiredVelocity.x;

        _rb.linearVelocity = _velocity;
    }
}
    

