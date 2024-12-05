using UnityEngine;


public class CollectableCarrot : MonoBehaviour, ICollectable
{

    //ID para identificarla
    public int totalCarrot = 0;

   public void Collect()
   {
        //Hace un sonido especifico, un efecto de particulas o una animación
        //Manage.Instance y sumar 1 zanahoria, que le dice cual cogio basado en la ID 
        totalCarrot++;
        Destroy(gameObject);
    }

}
