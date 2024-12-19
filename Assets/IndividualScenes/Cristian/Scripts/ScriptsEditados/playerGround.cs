using UnityEngine;

public class playerGround : MonoBehaviour
{
    //This script is used by both movement and jump to detect when the character is touching the ground
    public bool _isOnGround;
    public static playerGround instance;
    [Header("Collider Settings")]
    [SerializeField] [Tooltip("Alineas con los pies del personaje")] private float _groundLength = 0.95f;
    [SerializeField] [Tooltip("Distance between the ground-checking colliders")] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField] [Tooltip("Which layers are read as the ground")] private LayerMask _groundLayer;

    public void Awake()
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
    private void Update()
    {
        //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
        _isOnGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, _groundLength, _groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, _groundLength, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (_isOnGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * _groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * _groundLength);
    }

    //Send ground detection to other scripts
    public bool GetOnGround() { return _isOnGround; }
}

