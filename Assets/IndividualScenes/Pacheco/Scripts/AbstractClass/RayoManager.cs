using UnityEngine;

public class RayoManager : MonoBehaviour
{
    public static RayoManager instance;

    public float stamina;

    public int currentRayo;
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
        currentRayo++;
        stamina += staminaAmount;
        hudManager.instance.updateRayo(currentRayo);
    }

}
