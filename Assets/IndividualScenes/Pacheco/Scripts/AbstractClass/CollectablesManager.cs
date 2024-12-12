using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public static CollectablesManager instance;

    [Header("Rayo")]
    
    public float stamina;

    [Header("Carrot")]

    public int currentCarrot;

    [Header("Shoes")]

    public float currentShoes;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void IncrementRayo(float staminaAmount)
    {
        stamina += staminaAmount;
        hudManager.instance.updateRayo(stamina);
    }


    public void IncrementCarrot()
    {
        hudManager.instance.updateCarrots();
    }

    public void IncrementShoe()
    {
        hudManager.instance.updateShoes();
    }

}
