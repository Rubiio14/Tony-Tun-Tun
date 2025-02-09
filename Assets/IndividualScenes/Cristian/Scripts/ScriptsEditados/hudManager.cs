using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;
using FMODUnity;
public class hudManager : MonoBehaviour
{
    public static hudManager instance;
    [Header("Base Image")]
    public Image baseImage;
    [Header("Stamina Image")]
    public Image staminaImage;
    [Header("Jump Image")]
    public Image jumpImage;
    [Header("Trameo de salto base")]
    public float halfBaseBar = 0f;
    public float maxBaseBar = 0f;
    [Header("Trameo de primer tramo")]
    public float halfFirstBar = 0f;
    public float maxFirstBar = 0f;
    [Header("Trameo de segundo tramo")]
    public float halfSecondBar = 0f;
    public float maxSecondBar = 0f;
    [Header("Trameo de tercer tramo")]
    public float halfThirdBar = 0f;
    public float maxThirdBar = 0f;
    [Header("Charged Jump Bars Variants")]
    public Sprite firstBar;
    public Sprite secondBar;
    public Sprite thirdBar;
    [Header("Empty Carrots")]
    public Image firstCarrot;
    public Image secondCarrot;
    public Image thirdCarrot;
    [Header("Colored Carrot")]
    public Sprite coloredCarrot;


    public int carrotsCounter;
    public int shoesCounter;
    private float _requiredHoldTime = 0.03f;
    public int rayoCounter;
    public TextMeshProUGUI rayoText;

    private bool _isBaseFilling = false; //Bandera para controlar el llenado
    private float holdTime = 0f;
    private float actualLimit = 0.25f;
    public float staminaRecharge = 0.1f;
    private float _resetRefillSpeed = 0.8f;
    public float refillSpeedBar = 0.5f;
    //Trameo barra
    public float firstVisualTram = 0.25f;
    public float secondVisualTram = 0.25f;
    public float thirdVisualTram = 0.25f;
    public float fullVisualTram = 0.25f;


    //VFX Salto Cargado
    [SerializeField]
    GameObject _vfxChargedJump;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;          
        }
        else
        {
            Destroy(this);
        }
    }
    // Update is called once per frame
    void Update()
    {      
        //Refill Stamina Bar
        if (staminaImage.fillAmount <= actualLimit)
        {            
            staminaImage.fillAmount += staminaRecharge * Time.deltaTime;
        }
        if (_isBaseFilling && playerGround.instance.GetOnGround() && Math.Round(staminaImage.fillAmount, 2) >= firstVisualTram)
        {
            holdTime += Time.deltaTime;
            //ActivarVFX y Audio
            _vfxChargedJump.SetActive(true);
            FMODAudioManager.instance.PlayChargedJump();
            

            if (_isBaseFilling && holdTime >= _requiredHoldTime && jumpImage.fillAmount <= firstVisualTram && shoesCounter == 0)
            {
                
                if (jumpImage.fillAmount < staminaImage.fillAmount)
                {
                    jumpImage.fillAmount += refillSpeedBar * Time.deltaTime;
                }
            }
            else if (_isBaseFilling && holdTime >= _requiredHoldTime && jumpImage.fillAmount <= secondVisualTram && shoesCounter == 1)
            {
                if (jumpImage.fillAmount < staminaImage.fillAmount)
                {
                    jumpImage.fillAmount += refillSpeedBar * Time.deltaTime;
                }
            }
            else if (_isBaseFilling && holdTime >= _requiredHoldTime && jumpImage.fillAmount <= thirdVisualTram && shoesCounter == 2)
            {
                if (jumpImage.fillAmount < staminaImage.fillAmount)
                {
                    jumpImage.fillAmount += refillSpeedBar * Time.deltaTime;
                }
            }
            else if (_isBaseFilling && holdTime >= _requiredHoldTime && jumpImage.fillAmount <= fullVisualTram && shoesCounter == 3)
            {
                if (jumpImage.fillAmount < staminaImage.fillAmount)
                {
                    jumpImage.fillAmount += refillSpeedBar * Time.deltaTime;
                }
            }
        }
        //reset jumpBar
        if (jumpImage.fillAmount >= 0 && _isBaseFilling == false)
        {
            jumpImage.fillAmount -= _resetRefillSpeed * Time.deltaTime;
        }
    }

    public void refillBar(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isBaseFilling = true; // Activa el temporizador de relleno
            holdTime = 0f;
            jumpImage.fillAmount = 0f;
        }
        if (context.canceled)
        {
            _isBaseFilling = false; // Detiene el relleno y reinicia
            holdTime = 0f;
            FMODAudioManager.instance.StopChargedJump();
            //Desactivar VFX
            _vfxChargedJump.SetActive(false);
        }
    }

    public void updateCarrots()
    {
        //Update Carrots Counter
        carrotsCounter++;

        if (carrotsCounter == 1)
        {
            firstCarrot.sprite = coloredCarrot;
        }
        if (carrotsCounter == 2)
        {
            secondCarrot.sprite = coloredCarrot;
        }
        if (carrotsCounter == 3)
        {
            thirdCarrot.sprite = coloredCarrot;
        }
    }

    public void updateShoes()
    {
        //Update Stamina Cap
        shoesCounter++;
        

        if (shoesCounter == 1)
        {
            actualLimit = 0.5f;
            baseImage.sprite = firstBar;
            staminaImage.sprite = firstBar;
            jumpImage.sprite = firstBar;
        }
        else if (shoesCounter == 2)
        {
            actualLimit = 0.75f;
            baseImage.sprite = secondBar;
            staminaImage.sprite = secondBar;
            jumpImage.sprite = secondBar;
        }
        else if (shoesCounter == 3)
        {
            actualLimit = 1f;
            baseImage.sprite = thirdBar;
            staminaImage.sprite = thirdBar;
            jumpImage.sprite = thirdBar;
        }
    }

    public void updateRayo(float stamina)
    {
        staminaImage.fillAmount += stamina * Time.deltaTime;
        rayoCounter++;
        rayoText.SetText("x" + rayoCounter.ToString());
    }
}
