using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject hitEffect;
    Vector3 lastPosition;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.Euler(0,0,90));
        Destroy(effect,0.4f);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
        if (speed < 20f)
        {
            Destroy(gameObject);
        }
    }
}
