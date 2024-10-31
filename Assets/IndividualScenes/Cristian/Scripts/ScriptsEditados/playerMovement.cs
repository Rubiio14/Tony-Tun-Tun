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
    }
}
