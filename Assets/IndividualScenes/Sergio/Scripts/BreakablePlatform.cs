using UnityEngine;
using System.Collections;

public class BreakablePlatform : MonoBehaviour
{
    //PREFAB completo
    [SerializeField]
    GameObject _prefabBreakablePlatform;
    //Collider para situarse sobre la plataforma
    BoxCollider2D _platformCollider;

    //Collider para usar el bloqueo del shortcut
    [SerializeField]
    GameObject _shortcutColliderEmpty;

    //Trozos de la plataforma para romperse
    [SerializeField]
    GameObject piece1, piece2, piece3, piece4, piece5, piece6, piece7;
    [SerializeField]
    GameObject _platformMesh;

    //Partículas de polvo y explosión al romperse
    [SerializeField]
    GameObject _vfxPlatformBreakDust;
    [SerializeField]
    GameObject _vfxPlatformShakingDust;

    [Header("Temblor Plataforma")]
    public bool _isShaking;
    [Tooltip("La cantidad de distancia por la que tiembla la plataforma")] public float shakeDistance;
    //Repetición del shake
    public float _shakeRepeat = 0.25f;

    [Header("Timer")]
    public float _timer = 0f;

    //Breaking y Despawn
    [Header("Tiempo hasta romperse")]
    public bool _needBreakingTimer = false;
    public float _platformBreakingTime = 3f;
    //Tiempo en el que tardan las piezas en desactivarse
     public float _despawnPieceTime = 2f;

    //Rotación de las piezas
    Vector3 _pieceRotation;
    private bool _needRotations;
    [Header("Rotación piezas")]
    [Tooltip("Cantidad mínima de rotación")] public float _minRotationValue = -1f;
    [Tooltip("Cantidad máxima de rotación")] public float _maxRotationValue = 1f;


    //Para declarar si es un atajo, para activar el collider que bloquea al jugador por el lado contrario al que rompe la plataforma.
    [Header("Es atajo")]
    public bool _isShortcut;

    Vector3 originalPos;


    void Start()
    {
        _platformCollider = GetComponent<BoxCollider2D>();
        _vfxPlatformBreakDust.SetActive(false);
        _vfxPlatformShakingDust.SetActive(false);

        originalPos = _platformMesh.transform.position; 
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerGround.instance._isOnGround)
        {
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

            //Temblor de la plataforma
            StartCoroutine(ShakePlatform());

            //Partículas de rocas y polvo mientras tiembla
            _vfxPlatformShakingDust.SetActive(true);

            if (_isShaking)
            {
                Vector3 newPos = originalPos + Random.insideUnitSphere * (Time.deltaTime * shakeDistance);
                _platformMesh.transform.position = newPos;
            }

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

        if (_isShortcut == true)
        {
            _shortcutColliderEmpty.SetActive(true);
        }
        else
        {
            _shortcutColliderEmpty.SetActive(false);
        }
    }
    public void Hit()
    {
        _needBreakingTimer = true;
    }

    public IEnumerator ShakePlatform()
    {
        Vector3 originalPos = _platformMesh.transform.position;

        if(_isShaking == false)
        {
            _isShaking = true;
        }

        yield return new WaitForSeconds(_shakeRepeat);

        _isShaking = false;
        _platformMesh.transform.position = originalPos;
    }
    public void Broke()
    {
        //Desactiva Collider de la plataforma, caen las piezas y rotan
        _platformCollider.enabled = false;
        _isShortcut = false;
        _needRotations = true;
        FallingPhysics();

        //audioRomperse
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.brokenPlatform, this.gameObject.transform.position);

        //Corrutina para que desaparezcan las piezas tras un tiempo
        StartCoroutine(Waiting());
        _vfxPlatformBreakDust.SetActive(true);
        _vfxPlatformShakingDust.SetActive(false);

    }

    //físicas de caída de las piezas
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
        _vfxPlatformBreakDust.SetActive(false);
        Destroy(_prefabBreakablePlatform);
    }   
}
