using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Components")]

    private Rigidbody2D body;
    characterGround.instance ground;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)] [Tooltip("Maximum movement speed")] public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)] [Tooltip("How fast to reach max speed")] public float maxAcceleration = 52f;
    [SerializeField, Range(0f, 100f)] [Tooltip("How fast to stop after letting go")] public float maxDecceleration = 52f;
    [SerializeField, Range(0f, 100f)] [Tooltip("How fast to stop when changing direction")] public float maxTurnSpeed = 80f;
    [SerializeField, Range(0f, 100f)] [Tooltip("How fast to reach max speed when in mid-air")] public float maxAirAcceleration;
    [SerializeField, Range(0f, 100f)] [Tooltip("How fast to stop in mid-air when no direction is used")] public float maxAirDeceleration;
    [SerializeField, Range(0f, 100f)] [Tooltip("How fast to stop when changing direction when in mid-air")] public float maxAirTurnSpeed = 80f;
    [SerializeField] [Tooltip("Friction to apply against movement on stick")] private float friction;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
