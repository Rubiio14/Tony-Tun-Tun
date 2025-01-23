using UnityEngine;
using System.Collections;

public class EnemyMeleeShooterBehaviour : MonoBehaviour
{
    //Para ajustar la distancia del raycast desde el inspector
    [Header("Detection")]
    [SerializeField, Range(1f, 50f)] [Tooltip("Longitud maxima de deteccion")] public float raycastLength = 5f;

    //Para que el raycast empiece desde el enemigo pero no impacte con su propio collider. No es modificable en el inspectos para ahorrarnos errores.
    private float _startRaycast = 0.9f;

    //Animaciones del shooter
    private Animator _meleeShooterAnimator;
    //Para detectar solo el collider del jugador
    [SerializeField] private LayerMask _playerLayer;

    //VFX dìsparo
    [SerializeField]
    GameObject _vfxShot;
    [SerializeField]
    GameObject _vfxShotWaypoint;
    public float firstShotParticle = 1;


    private void Start()
    {
        _meleeShooterAnimator = GetComponent<Animator>();
        _vfxShot.SetActive(false);
    }
    void FixedUpdate()
    {
        //Rayo Debug en la escena
        Vector2 forward = transform.TransformDirection(Vector2.right) * raycastLength;
        Debug.DrawRay(new Vector2(transform.position.x * _startRaycast, transform.position.y), forward, Color.green);

        //Crear raycast e impactar con el layer del jugador 
        if (Physics2D.Raycast(new Vector2(transform.position.x * _startRaycast, transform.position.y), forward, raycastLength, _playerLayer))
        {
            //activar animacion de disparo y partículas
            _meleeShooterAnimator.SetBool("playerDetected", true);
            StartCoroutine(ActivateShotVFX());
        }
        else
        {
            //Desactivar animacion de disparo y partículas
            _meleeShooterAnimator.SetBool("playerDetected", false);
            _vfxShot.SetActive(false);
            StopAllCoroutines();
        }
    }

    public IEnumerator ActivateShotVFX()
    {
        _vfxShot.transform.position = _vfxShotWaypoint.transform.position;

        yield return new WaitForSeconds(firstShotParticle);

        _vfxShot.SetActive(true);
    }
}