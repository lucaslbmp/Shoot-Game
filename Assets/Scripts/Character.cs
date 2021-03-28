using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float initialHitpoints; // valor minimo inicial de "saude" 
    public float maxHitpoints;  // valor maximo de saude permitido

    public abstract void ResetCharacter();

    //public abstract IEnumerator InflictDamage(int ammount, float interval);

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
}