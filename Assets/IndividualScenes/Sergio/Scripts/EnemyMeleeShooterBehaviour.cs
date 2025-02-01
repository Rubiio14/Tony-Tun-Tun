using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyMeleeShooterBehaviour : MonoBehaviour
{
    //Para ajustar la distancia del raycast desde el inspector
    //[Header("Detection")]
    //[SerializeField, Range(1f, 50f)] [Tooltip("Longitud maxima de deteccion")] public float raycastLength = 5f;

    //Para que el raycast empiece desde el enemigo pero no impacte con su propio collider. No es modificable en el inspectos para ahorrarnos errores.
    //private float _startRaycast = 0.9f;

    //Variables para instanciar la bala
    public Transform bulletPosition;
    public float speed;
    public bool isShooting, inRange;

    //Animaciones del shooter
    private Animator _meleeShooterAnimator;
    //Para detectar solo el collider del jugador
    [SerializeField] private LayerMask _playerLayer;

    //VFX dìsparo
    //[SerializeField]
    //GameObject _vfxShot;
    [SerializeField]
    GameObject _vfxShotWaypoint;
    public float firstShotParticle = 1;


    private void Start()
    {
        _meleeShooterAnimator = GetComponent<Animator>();
        //_vfxShot.SetActive(false);
    }
    void FixedUpdate()
    { /*
        //Rayo Debug en la escena
        Vector2 forward = transform.TransformDirection(Vector2.right) * raycastLength;
        Debug.DrawRay(new Vector2(transform.position.x * _startRaycast, transform.position.y), forward, Color.green);

        //Crear raycast e impactar con el layer del jugador 
        if (Physics2D.Raycast(new Vector2(transform.position.x * _startRaycast, transform.position.y), forward, raycastLength, _playerLayer))
        {
            if(isShooting == false)
            {
                //activar animacion de disparo y partículas
                _meleeShooterAnimator.SetBool("playerDetected", true);
                StartCoroutine(FirstShotDelay(firstShotParticle));
            }
        }
        else
        {
            //Desactivar animacion de disparo y partículas
            _meleeShooterAnimator.SetBool("playerDetected", false);
            _vfxShot.SetActive(false);
            isShooting = false;
            StopAllCoroutines();
        } */
    } 

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
            if (isShooting == false)
            {
                isShooting = true;

                //activar animacion de disparo y partículas
                _meleeShooterAnimator.SetBool("playerDetected", true);
                StartCoroutine(FirstShotDelay(firstShotParticle));
            }
        }

    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            isShooting = false;

            //Desactivar animacion de disparo y partículas
            _meleeShooterAnimator.SetBool("playerDetected", false);
            //_vfxShot.SetActive(false);
        }
    }

    private void BulletInstance()
    {
        GameObject Bullet = ObjectPool.SharedInstance.GetBulletPooledObject();
        Bullet InstancedBullet = null;
            if (Bullet && Bullet.TryGetComponent<Bullet>(out InstancedBullet))
            {
                InstancedBullet.transform.position = bulletPosition.transform.position;
                InstancedBullet.transform.rotation = bulletPosition.transform.rotation;
                InstancedBullet.Speed = speed;
                InstancedBullet.Direction = bulletPosition.right;
                InstancedBullet.gameObject.SetActive(true);
                //_vfxShot.transform.position = _vfxShotWaypoint.transform.position;
                //_vfxShot.SetActive(true);
            }
        StartCoroutine(ActivateShotVFX(firstShotParticle));
    }

    public IEnumerator ActivateShotVFX(float timer)
    {
        yield return new WaitForSeconds(timer);

        Debug.Log("Disparo");
        //vfx.SetActive(false);
        BulletInstance();
    }

    public IEnumerator FirstShotDelay(float timer)
    {
        yield return new WaitForSeconds(timer);
        BulletInstance();
    }
}