using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Audios
    private AudioSource shotSound;
    private AudioSource reloadSound;

    // Variaveis de tiro
    public Transform firePoint;
    public GameObject bulletPrefab;
    //public float bulletForce = 20f;
    public int loaded_ammo;
    public int remaining_ammo;

    private int GunType = 1; // Variavel para seleçao da arma
    Handgun hg = new Handgun(30, 5, 15, true);

    Animator animator;

    public class Weapon
    {
        protected int totalAmmo {get; set;}
        protected int loadedAmmo {get; set;}
        protected int ammoCapacity { get; set; }
        protected bool isAvailable { get; set; }
        public float shootDelay { get; protected set; }
        public float timeToShoot { get; protected set; } // tempo de espera para o proximo tiro

        public Weapon(int total_ammo, int loaded_ammo, int ammo_capacity, bool is_available)
        {
            totalAmmo = total_ammo;
            loadedAmmo = loaded_ammo;
            ammoCapacity = ammo_capacity;
            isAvailable = is_available;
        }
        public virtual void Shoot()
        {
            totalAmmo--;
            loadedAmmo--;
        }

        public virtual void Shoot(Transform firePoint, GameObject bulletPrefab)
        {
        }

        public void Reload()
        {
            int ammoToReload = Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo);
            //totalAmmo -= ammoToReload;
            loadedAmmo += ammoToReload;
        }

        //public virtual void Reload(Animator animator)
        //{
        //}

        public bool CanShoot()
        {
            return totalAmmo > 0 && loadedAmmo > 0 && timeToShoot <= 0;
        }

        public bool CanReload()
        {
            return totalAmmo > 0 && Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo) > 0;
        }

        public void CountdownToShoot()
        {
            if (timeToShoot >= 0)
                timeToShoot -= Time.deltaTime;
        }

        public (int,int) AmmoStats()
        {
            return (loadedAmmo, totalAmmo - loadedAmmo);
        }
    }

    public class Handgun:Weapon
    {
        private float bulletForce = 20f;
        public Handgun(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available):base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
        {
            shootDelay = 0.6f; // tempo até poder atirar de novo com a pistola
        }

        //public override void Reload(Animator animator)
        //{
        //    base.Reload();
        //    animator.SetBool("IsReloading", true);
        //}

        public override void Shoot(Transform firePoint,GameObject bulletPrefab)
        {
            base.Shoot();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
            timeToShoot = shootDelay;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        shotSound = allMyAudioSources[0];
        reloadSound = allMyAudioSources[2];
        //shotSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //CountdownToShoot();
        Weapon currentGun = hg;
        switch (GunType)
        {
            case 1:     currentGun = hg;       break;
        }
        currentGun.CountdownToShoot();
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentGun.CanShoot())
            {
                shotSound.Play();
                currentGun.Shoot(firePoint,bulletPrefab);
                animator.SetTrigger("IsShooting");
                animator.SetInteger("GunType", GunType);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentGun.CanReload())
            {
                reloadSound.Play();
                currentGun.Reload();
                animator.SetTrigger("IsReloading");
                animator.SetInteger("GunType", GunType);
            }
        }
        (loaded_ammo,remaining_ammo) = currentGun.AmmoStats();
        Debug.Log(loaded_ammo + "-" + remaining_ammo);
    }
}
