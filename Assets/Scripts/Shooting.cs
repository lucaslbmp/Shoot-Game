using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Audios
    AudioSource[] allMyAudioSources;
    private AudioSource shotSound;
    private AudioSource reloadSound;
    private static Dictionary<int, int> dictShootAudioSources = new Dictionary<int, int>() { { 1, 0 }, { 2, 3 } }; // Associa o GunType ao respectivo Audio Source de tiro
    private static Dictionary<int, int> dictReloadAudioSources = new Dictionary<int, int>() { { 1, 2 }, { 2, 4 } }; // Associa o GunType ao respectivo Audio Source de recarga

    // Variaveis de tiro
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject pelletPrefab;
    //public float bulletForce = 20f;
    public int loaded_ammo;
    public int remaining_ammo;
    //public IDictionary<int, dynamic> dictBulletType = new Dictionary<int, dynamic>() { { 1, bulletPrefab }, { 2, shellPrefab } };
    //enum BulletTypes {bulletPrefab,shellPrefab}
    List<BulletType> BulletTypes = new List<BulletType>() { new BulletType(1, "bulletPrefab") };

    // Variaveis das armas
    private int GunType = 1; // Variavel para sele�ao da arma
    Handgun hg = new Handgun(30, 5, 15, true); // Objeto associado a pistola
    Shotgun sg = new Shotgun(10,3,5,true); // Objeto associado a espingarda
    public int ammountOfGuns = 2; // quantidade de armas que o player possui 

    Animator animator;

    public class BulletType
    {
        int TypeOfGun;
        string ShotPrefabName;
        public BulletType(int typeOfGun,string shotPrefabName)
        {
            TypeOfGun = typeOfGun;
            ShotPrefabName = shotPrefabName;
        }
    }

    public class Weapon
    {
        protected int totalAmmo {get; set;} // Total de muni�ao que o player possui para a arma
        protected int loadedAmmo {get; set;} // Muni�ao que est� carregada na arma
        protected int ammoCapacity { get; set; } // Capacidade de muni��o da arma
        protected bool isAvailable { get; set; } // Informa se a arma est� disponivel para o player ou nao
        public float shootDelay { get; protected set; } // tempo de espera para o proximo tiro
        public float timeToShoot { get; protected set; } // timer que contabiliza o tempo de espera para o proximo tiro
        public float reloadSoundTime { get; protected set; } // delay correspondente ao tempo de recarga

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

        public bool CountdownTimers()
        {
            bool canShootAgain, playShootAnim;
            if (timeToShoot >= 0)
            {
                timeToShoot -= Time.deltaTime;
                canShootAgain = false;
            }
            else
                canShootAgain = true;
            return canShootAgain;
        }

        public (int,int) AmmoStats()
        {
            return (loadedAmmo, totalAmmo - loadedAmmo);
        }
    }

    public class Handgun:Weapon
    {
        private float bulletForce = 30f;
        public Handgun(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available):base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
        {
            shootDelay = 0.6f; // tempo at� poder atirar de novo com a pistola
            reloadSoundTime = 0.2f;
        }

        //public override void Reload(Animator animator)
        //{
        //    base.Reload();
        //    animator.SetBool("IsReloading", true);
        //}

        public override void Shoot(Transform firePoint,GameObject bulletPrefab)
        {
            base.Shoot();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
            timeToShoot = shootDelay;
        }
    }

    public class Shotgun : Weapon
    {
        private float bulletForce = 30f;
        public Shotgun(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
        {
            shootDelay = 1.2f; // tempo at� poder atirar de novo com a espingarda
            reloadSoundTime = 0.5f;
        }

        public override void Shoot(Transform firePoint, GameObject pelletPrefab)
        {
            Vector3 posIncrement = firePoint.right*0.5f + firePoint.up*0.6f;
            Vector3 direction = firePoint.right;
            float angleIncrement=-45f;
            base.Shoot();
            for(int i=0; i < 7; i++) {
                // Posicinando os pellets a uma dada distancia na dire��o do vetor perpendicilar � dire��o do personagem e 
                // com uma diferen�a de 20� entre si
                GameObject shell = Instantiate(pelletPrefab, firePoint.position+posIncrement, firePoint.rotation*Quaternion.Euler(0,0,angleIncrement));
                Rigidbody2D rb = shell.GetComponent<Rigidbody2D>();
                //direction = Quaternion.AngleAxis(angleIncrement, Vector3.right)*direction;
                rb.AddForce(rb.transform.up * bulletForce, ForceMode2D.Impulse);
                posIncrement -= firePoint.up*0.2f;
                angleIncrement -= 15f;
            }
            timeToShoot = shootDelay;
        }
    }

    // ===============================================================================================================

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        allMyAudioSources = GetComponents<AudioSource>();
        //shotSound = allMyAudioSources[0];
        //reloadSound = allMyAudioSources[2];
    }

    // Update is called once per frame
    void Update()
    {
        Weapon currentGun;
        GameObject shotPrefab;
        UpdateGunSelection();
        switch (GunType)
        {
            case 1:     currentGun = hg;       shotPrefab = bulletPrefab;       break;
            case 2:     currentGun = sg;       shotPrefab = pelletPrefab;        break;
            default:    currentGun = hg;       shotPrefab = bulletPrefab;       break;
        }
        animator.SetInteger("GunType", GunType);
        bool canShootAgain = currentGun.CountdownTimers();
        //if(!canShootAgain) // Se pode nao pode executar anima�ao de tiro **
            //animator.SetBool("IsShooting", false); // entao setar a flag IsShooting para falso **
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log(currentGun.CanShoot());
            if (currentGun.CanShoot())
            {
                shotSound = allMyAudioSources[dictShootAudioSources[GunType]];
                shotSound.Play();
                //currentGun.Shoot(firePoint, GameObject.Find("bulletPrefab"));
                currentGun.Shoot(firePoint, shotPrefab);
                animator.SetTrigger("IsShooting");
                //animator.SetBool("IsShooting",true); //***
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentGun.CanReload())
            {
                reloadSound = allMyAudioSources[dictReloadAudioSources[GunType]];
                reloadSound.PlayDelayed(currentGun.reloadSoundTime);
                currentGun.Reload();
                animator.SetTrigger("IsReloading");
                //animator.SetBool("IsReloading",true); //**
                animator.SetInteger("GunType", GunType); // redundante?
            }
        }
        (loaded_ammo,remaining_ammo) = currentGun.AmmoStats();
        //Debug.Log(loaded_ammo + "-" + remaining_ammo);
    }

    void UpdateGunSelection()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GunType++;
            //GunType %= (ammountOfGuns+1);
            if (GunType == ammountOfGuns+1)
                GunType = 1;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GunType--;
            if (GunType == 0)
                GunType = ammountOfGuns;
        }
    }
}
