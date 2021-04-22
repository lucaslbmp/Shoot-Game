using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        Sprite charSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

    }
}
