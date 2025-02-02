using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyMeleeShooterBehaviour : MonoBehaviour
{
    //Variables para instanciar la bala
    public Transform bulletPosition;
    public float speed;

    //Animaciones del shooter
    private Animator _meleeShooterAnimator;
    //Para detectar solo el collider del jugador
    [SerializeField] private LayerMask _playerLayer;

    //VFX dìsparo
    [SerializeField]
    GameObject _vfxShot;
    [SerializeField]
    GameObject _vfxShotWaypoint;

    [Header("La cantidad de veces que ejecuta la animación de disparo por segundo")]
    public float _shootAnimMulti = 2;
    [Header("Cuántos segundos espera hasta el próximo disparo NOTA: Si se anima a 2/s, el delay deberá ser de 0.5, si se anima a 0.5/s el delay deberá de ser de 2")]
    public float _shootDelay = 1;

    //El tiempo que tarda la animación de detección en hacerse, dura 1 segundo, no tocar este valor.
    private float _waitForDetection = 1;

    public bool _playerDetected = false;

    private void Start()
    {
        _meleeShooterAnimator = GetComponent<Animator>();
        //Ejecuta X animaciones por segundo, es decir, 2 = 2 animaciones por segundo
        _meleeShooterAnimator.SetFloat("_shootAnimSpeed", _shootAnimMulti);
        _vfxShot.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //activar animacion de disparo
            _meleeShooterAnimator.SetBool("playerDetected", true);
            _playerDetected = true;

            //Espera X segundos para el siguiente disparo, es decir, 2 = 2 segundos hasta el siguiente disparo
            StartCoroutine(FirstShotDelay(_waitForDetection));
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playerDetected = false;
            _meleeShooterAnimator.SetBool("playerDetected", false);
        }
    }
    private void BulletInstance()
    {
        if (_playerDetected == true)
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
                _vfxShot.SetActive(true);
                _vfxShot.transform.position = _vfxShotWaypoint.transform.position;
            }

            StartCoroutine(ActivateShot(_shootDelay));
        }
    }
    
    public IEnumerator ActivateShot(float timer)
    {
        yield return new WaitForSeconds(timer);
        _vfxShot.SetActive(false);

        Debug.Log("Disparo");
        BulletInstance();
    }

    public IEnumerator FirstShotDelay(float timer)
    {
        yield return new WaitForSeconds(timer);
        BulletInstance();
    }
}