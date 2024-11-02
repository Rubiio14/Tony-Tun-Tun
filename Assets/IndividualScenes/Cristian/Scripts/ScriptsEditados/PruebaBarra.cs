using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PruebaBarra : MonoBehaviour
{
    public static PruebaBarra instance;
    public Image barImage;
    private bool isFilling = false; // Bandera para controlar el llenado
    private float holdTime = 0f; // Temporizador para contar el tiempo que se mantiene presionado
    private float requiredHoldTime = 0.3f; // Tiempo requerido antes de comenzar el llenado

    private void Awake()
    {
        barImage = transform.Find("JB").GetComponent<Image>();
        barImage.fillAmount = 0f;
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
        // Si el botón se está manteniendo presionado, incrementa el temporizador
        if (isFilling)
        {
            holdTime += Time.deltaTime;

            // Solo comienza a llenar la barra si ha pasado el tiempo requerido
            if (holdTime >= requiredHoldTime && barImage.fillAmount < 1f)
            {
                barImage.fillAmount += 0.3f * Time.deltaTime; // Rellenado gradual
            }
        }
    }

    public void refillBar(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isFilling = true; // Activa el temporizador de relleno
            holdTime = 0f; // Reinicia el temporizador al presionar
        }
        if (context.canceled)
        {
            isFilling = false; // Detiene el relleno y reinicia
            barImage.fillAmount = 0f;
            holdTime = 0f; // Reinicia el temporizador al soltar
        }
    }
}
