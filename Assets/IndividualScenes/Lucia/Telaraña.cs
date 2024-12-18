using UnityEngine;

public class Telaraña : MonoBehaviour
{
    public float speedWhileInSpiderWeb = 2.5f;
    public float jumpMultiplyerWhileInWeb = 5f;
    float normalMaxSpeed = 20f;
    float normalAccele_Decele = 25f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("entra");
            other.GetComponent<playerJump>().jumpHeight = other.GetComponent<playerJump>().jumpHeight / jumpMultiplyerWhileInWeb;
            other.GetComponent<playerMovement>().maxSpeed = speedWhileInSpiderWeb;
            other.GetComponent<playerMovement>().maxAcceleration = speedWhileInSpiderWeb;
            other.GetComponent<playerMovement>().maxDecceleration = speedWhileInSpiderWeb;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("estoy dentro de la tela de araña");
            other.GetComponent<playerJump>().jumpHeight = other.GetComponent<playerJump>().jumpHeight / jumpMultiplyerWhileInWeb;
            other.GetComponent<playerMovement>().maxSpeed = speedWhileInSpiderWeb;
            other.GetComponent<playerMovement>().maxAcceleration = speedWhileInSpiderWeb;
            other.GetComponent<playerMovement>().maxDecceleration = speedWhileInSpiderWeb;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<playerJump>().jumpHeight = other.GetComponent<playerJump>().jumpHeight * jumpMultiplyerWhileInWeb;
            other.GetComponent<playerMovement>().maxSpeed = normalMaxSpeed;
            other.GetComponent<playerMovement>().maxAcceleration = normalAccele_Decele;
            other.GetComponent<playerMovement>().maxDecceleration = normalAccele_Decele;
        }
    }
}