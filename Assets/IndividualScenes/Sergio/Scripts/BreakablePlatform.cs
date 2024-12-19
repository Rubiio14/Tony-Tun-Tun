using UnityEngine;
using System.Collections;

public class BreakablePlatform : MonoBehaviour
{
    BoxCollider2D _platformCollider;
    [SerializeField]
    GameObject piece1, piece2, piece3, piece4, piece5, piece6, piece7;

    [Header("Timer")]
    public float _timer = 0f;
    //Breaking y Despawn
    [Header("Rotaci�n piezas")]
    public bool _needBreakingTimer = false;
    public float _platformBreakingTime = 3f;

    //Rotaci�n de las piezas
    private bool _needRotations;

    //Tiempo en el que tardan las piezas en desactivarse
    [Tooltip("Lo que tardan en desaparecer las piezas, debe ser INFERIOR al Platform Respawn Time")]  public float _despawnPieceTime = 2f;

    [Header("Respawn")]
    [Tooltip("Si esta plataforma puede reaparecer o no")] public bool _canRespawn;
    public bool _needRespawningTimer = false;
    [Tooltip("Lo que tarda en volver a aparecer la plataforma")] public float _platformRespawnTime = 5f;

    [Header("Rotaci�n piezas")]
    Vector3 _pieceRotation;
    [Tooltip("Cantidad m�nima de rotaci�n")] public float _minRotationValue = -1f;
    [Tooltip("Cantidad m�xima de rotaci�n")] public float _maxRotationValue = 1f;


    //Posiciones de las piezas para volver a colocarlas
    private Vector3 _piece1Position;
    private Vector3 _piece2Position;
    private Vector3 _piece3Position;
    private Vector3 _piece4Position;
    private Vector3 _piece5Position;
    private Vector3 _piece6Position;
    private Vector3 _piece7Position;

    //Rotacion de las piezas para volver a colocarlas
    private Quaternion _piece1Rotation;
    private Quaternion _piece2Rotation;
    private Quaternion _piece3Rotation;
    private Quaternion _piece4Rotation;
    private Quaternion _piece5Rotation;
    private Quaternion _piece6Rotation;
    private Quaternion _piece7Rotation;


    void Start()
    {
        _platformCollider = GetComponent<BoxCollider2D>();

        //Posiciones de las piezas para volver a colocarlas 
        _piece1Position = piece1.transform.position;
        _piece2Position = piece2.transform.position;
        _piece3Position = piece3.transform.position;
        _piece4Position = piece4.transform.position;
        _piece5Position = piece5.transform.position;
        _piece6Position = piece6.transform.position;
        _piece7Position = piece7.transform.position;

        //Rotacion de las piezas para volver a colocarlas
        _piece1Rotation = piece1.transform.rotation;
        _piece2Rotation = piece2.transform.rotation;
        _piece3Rotation = piece3.transform.rotation;
        _piece4Rotation = piece4.transform.rotation;
        _piece5Rotation = piece5.transform.rotation;
        _piece6Rotation = piece6.transform.rotation;
        _piece7Rotation = piece7.transform.rotation;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerGround.instance._isOnGround)
        {
            Debug.Log("ColliderPlataform");
            Hit();
        }
    }
    private void OnEnable()
    {
        _pieceRotation = new Vector3(Random.Range(_minRotationValue, _maxRotationValue), Random.Range(_minRotationValue, _maxRotationValue), Random.Range(_minRotationValue, _maxRotationValue));
    }
    void Update()
    {
        //Se ejecuta Hit() y empieza a temblar la plataforma
        if (_needBreakingTimer == true)
        {
            _timer += Time.deltaTime;
            if (_timer >= _platformBreakingTime)
            {
                _timer = 0f;
                _needBreakingTimer = false;
                //Se rompe la plataforma
                Broke();
            }
        }
        if (_needRotations == true)
        {
            piece1.transform.Rotate(_pieceRotation);
            piece2.transform.Rotate(_pieceRotation);
            piece3.transform.Rotate(_pieceRotation);
            piece4.transform.Rotate(_pieceRotation);
            piece5.transform.Rotate(_pieceRotation);
            piece6.transform.Rotate(_pieceRotation);
            piece7.transform.Rotate(_pieceRotation);
        }

        if(_canRespawn == true)
        {
            if (_needRespawningTimer == true)
            {
                _timer += Time.deltaTime;
                if (_timer >= _platformRespawnTime)
                {
                    _timer = 0f;
                    _needRespawningTimer = false;
                    Respawn();
                }
            }
        }
    }
    public void Hit()
    {
        _needBreakingTimer = true;
        //Temblor de la plataforma
        //particulas de polvo
    }
    public void Broke()
    {
        //Desactiva Collider de la plataforma, caen las piezas y rotan
        _platformCollider.enabled = false;
        _needRotations = true;
        FallingPhysics();
        //Se necesita el timer para respawnear la plataforma
        if (_canRespawn == true)
        {
            _needRespawningTimer = true;
        }

        //Corrutina para que desaparezcan las piezas tras un tiempo
        StartCoroutine(Waiting());
    }

    //f�sicas de ca�da de las piezas
    public void FallingPhysics()    
    {
        piece1.GetComponent<Rigidbody>().isKinematic = false;
        piece1.GetComponent<Rigidbody>().AddForce(new Vector3(3, 1, 3), ForceMode.Impulse);

        piece2.GetComponent<Rigidbody>().isKinematic = false;
        piece2.GetComponent<Rigidbody>().AddForce(new Vector3(3, 1, -3), ForceMode.Impulse);

        piece3.GetComponent<Rigidbody>().isKinematic = false;
        piece3.GetComponent<Rigidbody>().AddForce(new Vector3(-3, 1, -3), ForceMode.Impulse);

        piece4.GetComponent<Rigidbody>().isKinematic = false;
        piece4.GetComponent<Rigidbody>().AddForce(new Vector3(-3, 1, 3), ForceMode.Impulse);

        piece5.GetComponent<Rigidbody>().isKinematic = false;
        piece5.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 3), ForceMode.Impulse);

        piece6.GetComponent<Rigidbody>().isKinematic = false;
        piece6.GetComponent<Rigidbody>().AddForce(new Vector3(3, 1, 0), ForceMode.Impulse);

        piece7.GetComponent<Rigidbody>().isKinematic = false;
        piece7.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
    }
    public IEnumerator Waiting()
    {
         yield return new WaitForSeconds(_despawnPieceTime);
        piece1.SetActive(false);
        piece2.SetActive(false);
        piece3.SetActive(false);
        piece4.SetActive(false);
        piece5.SetActive(false);
        piece6.SetActive(false);
        piece7.SetActive(false);
    }

    //Vuelve a aparecer y a colocarse la pieza
    private void Respawn()
    {
        _needRotations = false;

        piece1.GetComponent<Rigidbody>().isKinematic = true;
        piece1.SetActive(true);
        piece1.transform.position = _piece1Position;
        piece1.transform.rotation = _piece1Rotation;

        piece2.GetComponent<Rigidbody>().isKinematic = true;
        piece2.SetActive(true);
        piece2.transform.position = _piece2Position;
        piece2.transform.rotation = _piece2Rotation;

        piece3.GetComponent<Rigidbody>().isKinematic = true;
        piece3.SetActive(true);
        piece3.transform.position = _piece3Position;
        piece3.transform.rotation = _piece3Rotation;

        piece4.GetComponent<Rigidbody>().isKinematic = true;
        piece4.SetActive(true);
        piece4.transform.position = _piece4Position;
        piece4.transform.rotation = _piece4Rotation;


        piece5.GetComponent<Rigidbody>().isKinematic = true;
        piece5.SetActive(true);
        piece5.transform.position = _piece5Position;
        piece5.transform.rotation = _piece5Rotation;

        piece6.GetComponent<Rigidbody>().isKinematic = true;
        piece6.SetActive(true);
        piece6.transform.position = _piece6Position;
        piece6.transform.rotation = _piece6Rotation;

        piece7.GetComponent<Rigidbody>().isKinematic = true;
        piece7.SetActive(true);
        piece7.transform.position = _piece7Position;
        piece7.transform.rotation = _piece7Rotation;

        _platformCollider.enabled = true;
    }
}
