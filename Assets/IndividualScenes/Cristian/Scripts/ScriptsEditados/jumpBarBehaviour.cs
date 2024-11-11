using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class jumpBarBehaviour : MonoBehaviour
{
    public static jumpBarBehaviour instance;
    public Image baseBarImage;
    public Image firstBarImage;
    public Image secondBarImage;
    public Image thirdBarImage;
    private bool _isBaseFilling = false; // Bandera para controlar el llenado
    private float holdTime = 0f; // Temporizador para contar el tiempo que se mantiene presionado
    private float requiredHoldTime = 0.3f; // Tiempo requerido antes de comenzar el llenado

    [Header("Trameo de salto base")]
    public float halfBaseBar = 0f;
    public float maxBaseBar = 0f;
    [Header("Trameo de primera fase")]
    public float halfFirstBar = 0f;
    public float maxFirstBar = 0f;
    [Header("Trameo de segunda fase")]
    public float halfSecondBar = 0f;
    public float maxSecondBar = 0f;
    [Header("Trameo de tercera fase")]
    public float halfThirdBar = 0f;
    public float maxThirdBar = 0f;


    private void Awake()
    {
        //baseBarImage = transform.Find("JB").GetComponent<Image>();
        //firstBarImage = transform.Find("first_JB").GetComponent<Image>();
        baseBarImage.fillAmount = 0f;
        firstBarImage.fillAmount = 0f;
        secondBarImage.fillAmount = 0f;
        thirdBarImage.fillAmount = 0f;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        // Si el botón se está manteniendo presionado y el personaje está en el suelo
        if (_isBaseFilling && playerGround.instance.GetOnGround())
        {
            holdTime += Time.deltaTime;
            if (_isBaseFilling && holdTime >= requiredHoldTime && baseBarImage.fillAmount < 1f)
            {
                baseBarImage.fillAmount += 3f * Time.deltaTime; // Rellenado gradual
            }
            // Solo comienza a llenar la barra base si ha pasado el tiempo requerido
            if (baseBarImage.fillAmount >= 1f && firstBarImage.fillAmount < 1f)
            {
                firstBarImage.fillAmount += 3f * Time.deltaTime;
            }
            // Solo comienza a llenar la primera barra si las segunda está llena
            if (firstBarImage.fillAmount >= 1f && secondBarImage.fillAmount < 1f)
            {
                secondBarImage.fillAmount += 3f * Time.deltaTime;
            }
            // Solo comienza a llenar la primera barra si las segunda está llena
            if (secondBarImage.fillAmount >= 1f && thirdBarImage.fillAmount < 1f)
            {
                thirdBarImage.fillAmount += 3f * Time.deltaTime;
            }
            
        }
    }

    public void refillBar(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isBaseFilling = true; // Activa el temporizador de relleno
            holdTime = 0f; // Reinicia el temporizador al presionar
            
        }
        if (context.canceled)
        {
            _isBaseFilling = false; // Detiene el relleno y reinicia
            baseBarImage.fillAmount = 0f;
            firstBarImage.fillAmount = 0f;
            secondBarImage.fillAmount = 0f;
            thirdBarImage.fillAmount = 0f;
            holdTime = 0f; // Reinicia el temporizador al soltar
            
        }
    }
}
