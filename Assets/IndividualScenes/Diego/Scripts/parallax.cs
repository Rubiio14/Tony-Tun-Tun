using UnityEngine;

public class parallax : MonoBehaviour 

{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<MeshRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos +dist, transform.position.y, transform.position.z);
    }
}
