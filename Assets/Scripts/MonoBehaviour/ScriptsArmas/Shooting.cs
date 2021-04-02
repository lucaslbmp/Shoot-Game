using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =====================================================================================================================================================================================//
//                                          PRESETS DO JOGO                                                                                                                             //
// =====================================================================================================================================================================================//

//classe que gerencia as ações de tiro, recarregamento, troca de arma e ataque com arma branca do player
public class Shooting : MonoBehaviour
{
    // Audios
    AudioSource[] allMyAudioSources;                                                                                            // Abre os audios disponíveis
    private AudioSource shotSound;                                                                                              // Som de arma atirando é iniciado
    private AudioSource reloadSound;                                                                                            // Som de arma recarregando é iniciado
    private AudioSource MeleeSound;                                                                                             // Som da faca ao atacar é iniciado
    private static Dictionary<int, int> dictShootAudioSources = new Dictionary<int, int>() { { 1, 0 }, { 2, 3 } };              // Associa o GunType ao respectivo Audio Source de tiro
    private static Dictionary<int, int> dictReloadAudioSources = new Dictionary<int, int>() { { 1, 2 }, { 2, 4} };             // Associa o GunType ao respectivo Audio Source de recarga
    //private static Dictionary<int, int> dictMeleeAudioSources = new Dictionary<int, int>() { {1, }, { 2, 3} };               // Associa o MeleeType ao respectivo Audio Source de Faca
                                                                                                 
    // Variaveis de tiro
    public Transform firePoint;                                                                                                 // variável de posição
    public GameObject bulletPrefab;                                                                                             // prefab do projétil
    public GameObject pelletPrefab;                                                                                             // prefab pallete
    public GameObject knifePrefab;                                                                                              // prefab da sprite de movimento da Faca
    public GameObject MuzzleflashPrefab;                                                                                        //prefab do tiro

    //public float bulletForce = 20f;
    public int loaded_ammo;                                                                                                      // variavel que calcula o quanto de projeteis foram carregados para a arma
    public int remaining_ammo;                                                                                                   // variavel que calcula o quanto de projeteis ainda existem no paint da arma
  
    //public IDictionary<int, dynamic> dictBulletType = new Dictionary<int, dynamic>() { { 1, bulletPrefab }, { 2, shellPrefab } };
    //enum BulletTypes {bulletPrefab,shellPrefab}
    List<BulletType> BulletTypes = new List<BulletType>() { new BulletType(1, "bulletPrefab") };

    // Variaveis das armas
    private int GunType = 1;                                                                                                    // Variavel para seleçao da arma
    Handgun hg = new Handgun(30, 5, 15, true);                                                                                  // Objeto associado a pistola
    Shotgun sg = new Shotgun(10,3,5,true);                                                                                      // Objeto associado a espingarda
    AK47 rifle = new AK47(30, 15, 30, true);                                                                                    // Objeto associado a AK47
    Knife knf = new Knife(10, 10, 10, true);                                                                                    // Objeto associado a faca
    public int ammountOfGuns = 2;                                                                                               // quantidade de armas que o player possui 

    Animator animator;                                                                                                          // inicia um animator para carregar as animações dos projéteis


    //classe que seleciona um tipo específico de projétil
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

    // classe que declara as condições de uma arma no jogo
    public class Weapon
    {
        protected int totalAmmo {get; set;}                                                                                     // Total de muniçao que o player possui para a arma
        protected int loadedAmmo {get; set;}                                                                                    // Muniçao que está carregada na arma
        protected int ammoCapacity { get; set; }                                                                                // Capacidade de munição da arma
        protected bool isAvailable { get; set; }                                                                                // Informa se a arma está disponivel para o player ou nao
        public float shootDelay { get; protected set; }                                                                         // tempo de espera para o proximo tiro
        public float timeToShoot { get; protected set; }                                                                        // timer que contabiliza o tempo de espera para o proximo tiro
        public float reloadSoundTime { get; protected set; }                                                                    // delay correspondente ao tempo de recarga


        public Weapon(int total_ammo, int loaded_ammo, int ammo_capacity, bool is_available)
        {
            totalAmmo = total_ammo;
            loadedAmmo = loaded_ammo;
            ammoCapacity = ammo_capacity;
            isAvailable = is_available;
        }
        //determina a consequencia da ação de atirar
        public virtual void Shoot()
        {
            totalAmmo--;
            loadedAmmo--;
        }
        
        public virtual void Shoot(Transform firePoint, GameObject bulletPrefab)
        {
        }

        //determina a consequencia da ação de recarregar
        public void Reload()
        {
            int ammoToReload = Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo);
            loadedAmmo += ammoToReload;
        }

        /*
        public virtual void Reload(Animator animator)
         {
         }
        */

        //verifica se o player pode atirar
        public bool CanShoot()
        {
            return totalAmmo > 0 && loadedAmmo > 0 && timeToShoot <= 0;
            
        }

        //verifica se o player pode recarregar a arma
        public bool CanReload()
        {
            return totalAmmo > 0 && Mathf.Min(ammoCapacity - loadedAmmo, totalAmmo - loadedAmmo) > 0;
        }

        // delay para o player começar a atirar
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

        // carrega a quantidade de munição que o player possui
        public (int,int) AmmoStats()
        {
            return (loadedAmmo, totalAmmo - loadedAmmo);
        }
       
    }

    // define as condições para a pistola
    public class Handgun:Weapon
    {
        private float bulletForce = 30f;
        public Handgun(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available):base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
        {
            shootDelay = 0.6f; // tempo até poder atirar de novo com a pistola
            reloadSoundTime = 0.2f;
        }

        /*
        public override void Reload(Animator animator)
        {
            base.Reload();
            animator.SetBool("IsReloading", true);
        }
        */

        public override void Shoot(Transform firePoint,GameObject bulletPrefab)
        {
            base.Shoot();
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
            timeToShoot = shootDelay;
        }
    }

    // define as condições para a espingarda
    public class Shotgun : Weapon
    {
        private float bulletForce = 30f;
        public Shotgun(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
        {
            shootDelay = 1.2f; // tempo até poder atirar de novo com a espingarda
            reloadSoundTime = 0.5f;
        }

        public override void Shoot(Transform firePoint, GameObject pelletPrefab)
        {
            Vector3 posIncrement = firePoint.right*0.5f + firePoint.up*0.6f;
            Vector3 direction = firePoint.right;
            float angleIncrement=-45f;
            base.Shoot();
            for(int i=0; i < 7; i++) {
                // Posicinando os pellets a uma dada distancia na direção do vetor perpendicilar à direção do personagem e 
                // com uma diferença de 20° entre si
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

    // define as condições para o rifle
    public class AK47 : Weapon
    {
        private float bulletForce = 40f;                                                                                         //define o dano do rifle
        public AK47(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
        {
            shootDelay = 0.1f;                                                                                                    // tempo até poder atirar de novo com a AK47
            reloadSoundTime = 0.3f;
        }

        public override void Shoot(Transform firePoint, GameObject bulletPrefab)
        {
            base.Shoot(); 
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
            timeToShoot = shootDelay;
        }
    }

    // define as condições para a faca
    public class Knife : Weapon
    {
        private float bulletForce = 15f;                                                                                          //define o dano da faca
        public Knife(int total_ammo, int loaded_ammo, int max_ammo_loaded, bool is_available) : base(total_ammo, loaded_ammo, max_ammo_loaded, is_available)
        {
            shootDelay = 0.6f;                                                                                                    // tempo até poder acertar um hit com a faca
        }

        //define o ataque da faca
        public override void Shoot(Transform firePoint, GameObject knifePrefab)
        {
            base.Shoot();
            GameObject KnifeHit = Instantiate(knifePrefab, firePoint.position + firePoint.right * 0.5f, firePoint.rotation);
            Rigidbody2D rb = knifePrefab.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.right * bulletForce, 0);
            timeToShoot = shootDelay;
        }
    }


    // =====================================================================================================================================================================================//
    //                                          INICIO DA ROTINA DO JOGO                                                                                                                    //
    // =====================================================================================================================================================================================//

    //  Inicia o jogo e define as condições iniciais
    void Start()
    {
        animator = GetComponent<Animator>();
        allMyAudioSources = GetComponents<AudioSource>();

        //shotSound = allMyAudioSources[0];
        //reloadSound = allMyAudioSources[2];
    }

    // Atualiza as condições de jogo
    void Update()
    {
        Weapon currentGun;
        GameObject shotPrefab;
        UpdateGunSelection();
        switch (GunType)
        {
            case 1: currentGun = hg; shotPrefab = bulletPrefab;    break;
            case 2: currentGun = sg; shotPrefab = pelletPrefab;    break;
            case 3: currentGun = rifle; shotPrefab = bulletPrefab; break;                                                         // nova add de arma 1
            case 4: currentGun = knf; shotPrefab = knifePrefab;    break;                                                         // nova add de arma 2
            default: currentGun = hg; shotPrefab = bulletPrefab;   break;
        }

        animator.SetInteger("GunType", GunType);
        bool canShootAgain = currentGun.CountdownTimers();
        //if(!canShootAgain) // Se pode nao pode executar animaçao de tiro **
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


    // define as teclas utilizadas para a troca das armas
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
