using UnityEngine;

public class skullbunnycesta : MonoBehaviour
{
    public GameObject Skullbunny;
    public GameObject Waypoint;
    public float speed;

    private void Start()
    {

    }
    void Update()
    {
        Skullbunny.transform.position = Vector3.MoveTowards (Skullbunny.transform.position,Waypoint.transform.position,speed);
        if (Skullbunny.transform.position.x <= Waypoint.transform.position.x) 
        {
            Destroy(gameObject);
        }
    }
}


