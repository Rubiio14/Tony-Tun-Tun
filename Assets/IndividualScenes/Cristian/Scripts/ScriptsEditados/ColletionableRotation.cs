using UnityEngine;

public class ColletionableRotation : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed;
    public float verticalSpeed;
    public float verticalAmplitude;
    private Vector3 startPosition;

   
    public bool canGoUp;

    void Start()
    {
        startPosition = transform.position;
    }

    public void Update()
    {
        //Eje y
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

        if (canGoUp)
        {
            //Movimiento oscilante
            float newY = startPosition.y + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        
    }
}
