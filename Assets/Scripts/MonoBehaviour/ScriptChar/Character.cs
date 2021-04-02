using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public HealthBar HealthPoints;                          //Saúde do player
    public float maxHitpoints;                              // valor maximo de saude permitido
    public float initialHealthPoints;                       //Valor inicial da vida
    
    /*
    public abstract void ResetCharacter();
    public abstract IEnumerator InflictDamage(int ammount, float interval);

      
    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
    */
}