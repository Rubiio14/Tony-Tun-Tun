using UnityEngine;

public class Telaraña : MonoBehaviour
{
    public float speedWhileInSpiderWeb = 1f;
    float normalMaxSpeed = 20f;
    float normalAccele_Decele = 25f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement tmpPlayerMvm = other.GetComponent<playerMovement>();
            playerJump tmpJump = other.GetComponent<playerJump>();

            print("Estoy dentro de la tela de araña");
            tmpJump.enabled = false;
            tmpPlayerMvm.maxSpeed = speedWhileInSpiderWeb;
            tmpPlayerMvm.maxAcceleration = speedWhileInSpiderWeb;
            tmpPlayerMvm.maxDecceleration = speedWhileInSpiderWeb;
            tmpPlayerMvm.useAcceleration = false;

            //Debug.LogFormat($"Max Speed: {tmpPlayerMvm.maxSpeed} , Max Acce: {tmpPlayerMvm.maxAcceleration} , Max Dec: {tmpPlayerMvm.maxDecceleration}");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement tmpPlayerMvm = other.GetComponent<playerMovement>();
            playerJump tmpJump = other.GetComponent<playerJump>();

            print("Sali de la telaraña");
            tmpJump.enabled = true;
            tmpPlayerMvm.maxSpeed = normalMaxSpeed;
            tmpPlayerMvm.maxAcceleration = normalAccele_Decele;
            tmpPlayerMvm.maxDecceleration = normalAccele_Decele;
            tmpPlayerMvm.useAcceleration = true;
        }
    }
}