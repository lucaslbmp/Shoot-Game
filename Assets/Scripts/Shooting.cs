using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Variaveis de tiro
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    // Temporizaçao do tiro
    private float timeToShoot; // tempo de espera para o proximo tiro
    private const float handgunShootDelay = 0.8f;

    // Variavesi de Muniçao
    public int handgunBullets = 5;

    private int GunType = 1; // Variavel para seleçao da arma
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CountdownToShoot();
        if (Input.GetButtonDown("Fire1"))
        {
            switch (GunType)
            {
                case 1: 
                    if(handgunBullets > 0 && timeToShoot <= 0)
                        Shoot();
                    break;
            }
        }
        else if (Input.GetButtonDown("Reload"))
        {
            switch (GunType)
            {
                case 1:
                    if (handgunBullets <= 0 && timeToShoot <= 0)
                        Reload();
                    break;
            }
        }
    }

    void Shoot()
    {
        animator.SetInteger("GunType", GunType);
        animator.SetTrigger("IsShooting");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        handgunBullets--;
        timeToShoot = handgunShootDelay;
    }

    void Reload()
    {
        animator.SetInteger("Reload",GunType);
    }

    void CountdownToShoot()
    {
        if(timeToShoot >=0)
            timeToShoot -= Time.deltaTime;
    }
}
