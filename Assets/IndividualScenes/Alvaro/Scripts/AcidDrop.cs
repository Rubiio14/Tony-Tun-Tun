using System;
using System.Collections;
using UnityEngine;

public class AcidDrop : MonoBehaviour
{
    Rigidbody2D dropRigidbody;
    public GameObject particles;
    public GameObject mesh;

    public Vector3 Direction {  get; set; }
    public float Speed { get; set; }

    void OnEnable()
    {
        dropRigidbody = GetComponent<Rigidbody2D>();
        dropRigidbody.linearVelocity = Direction * Speed * Time.deltaTime;
    }

    private void OnDisable()
    {
        mesh.SetActive(true);
        particles.SetActive(false);
        dropRigidbody.linearVelocity = Direction * 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(DisableAfterDelay());
        mesh.SetActive(false);
        particles.SetActive(true);
        FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.AcidLeak, this.gameObject.transform.position);
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}
