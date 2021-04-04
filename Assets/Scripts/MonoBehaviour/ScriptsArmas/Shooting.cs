using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// =====================================================================================================================================================================================//
//                                          PRESETS DO JOGO                                                                                                                             //
// =====================================================================================================================================================================================//

//classe que gerencia as ações de tiro, recarregamento, troca de arma e ataque com arma branca do player
[RequireComponent(typeof(AudioSource))]
public class Shooting : MonoBehaviour
{
    // Audios
    AudioSource[] allMyAudioSources;                                                                                            // Abre os audios disponíveis
    AudioSource shotAudioSource;
    AudioSource reloadAudioSource;
    AudioClip shotSound;                                                                                              // Som de arma atirando é iniciado
    AudioClip reloadSound;                                                                                            // Som de arma recarregando é iniciado
    AudioClip MeleeSound;                                                                                             // Som da faca ao atacar é iniciado
    //private static Dictionary<int, int> dictShootAudioSources = new Dictionary<int, int>() { { 0, 0 }, { 1, 3 } };              // Associa o GunType ao respectivo Audio Source de tiro
    //private static Dictionary<int, int> dictReloadAudioSources = new Dictionary<int, int>() { { 0, 2 }, { 1, 4} };             // Associa o GunType ao respectivo Audio Source de recarga
                                                                                                                               //private static Dictionary<int, int> dictMeleeAudioSources = new Dictionary<int, int>() { {1, }, { 2, 3} };               // Associa o MeleeType ao respectivo Audio Source de Facas
    // Variaveis de tiro
    public Transform firePoint;                                                                                                 // variável de posição
    public GameObject bulletPrefab;                                                                                             // prefab do projétil
    public GameObject pelletPrefab;                                                                                             // prefab pallete
    public GameObject knifePrefab;                                                                                              // prefab da sprite de movimento da Faca
    public GameObject MuzzleflashPrefab;                                                                                        // prefab do flash do tiro
    //List<String> projectilePrefabs = new List<String>() {null, "bulletPrefab", "pelletPrefab" } ;                                                             // Lista de prefabs de projeteis

    //public float bulletForce = 20f;
    public int loaded_ammo;                                                                                                      // variavel que calcula o quanto de projeteis foram carregados para a arma
    public int remaining_ammo;                                                                                                   // variavel que calcula o quanto de projeteis ainda existem no paint da arma
  
    //List<BulletType> BulletTypes = new List<BulletType>() { new BulletType(1, "bulletPrefab") };

    // Variaveis das armas
    private int GunType = 0;                                                                                                    // Variavel para seleçao da arma
    public GameObject Weapons;                                                                                                    // Objeto que contem todas as armas
    [HideInInspector] public List<Weapon> WeaponList = new List<Weapon>();
    public int ammountOfGuns = 2;                                                                                               // quantidade de armas que o player possui 
    Weapon currentGun;

    Animator animator;                                                                                                          // inicia um animator para carregar as animações dos projéteis

    //  Inicia o jogo e define as condições iniciais
    void Start()
    {
        animator = GetComponent<Animator>();
        allMyAudioSources = GetComponents<AudioSource>();
        AddWeaponsToList();                                                                       // Adiciona as armas a weaponList
        currentGun = WeaponList[0];
        ammountOfGuns = WeaponList.Count;
    }

    // Atualiza as condições de jogo
    void Update()
    {
        //currentGun = Weapons.GetComponent<Weapon>();
        GameObject shotPrefab;
        bool changeWeapon = UpdateGunSelection();
        if(changeWeapon)
            currentGun = WeaponList[GunType];
        if (currentGun.isReloading)
            currentGun.CountdownToFinishReload();
        shotPrefab = currentGun.projectilePrefab;
        animator.SetInteger("GunType", GunType);
        currentGun.CountdownToShoot();
        //if(!canShootAgain) // Se pode nao pode executar animaçao de tiro **
            //animator.SetBool("IsShooting", false); // entao setar a flag IsShooting para falso **
        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.Log(currentGun.CanShoot());
            if (currentGun.CanShoot())
            {
                ShootWeapon(currentGun,shotPrefab);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentGun.CanReload())
            {
                ReloadWeapon(currentGun);
            }
        }
        (loaded_ammo, remaining_ammo) = (currentGun.AmmoLoaded(),currentGun.AmmoRemaining());
        //Debug.Log(loaded_ammo + "-" + remaining_ammo);
    }


    // define as teclas utilizadas para a troca das armas
    bool UpdateGunSelection()
    {
        AddWeaponsToList();
        if (Input.GetKeyDown(KeyCode.E))
        {
            GunType = SelectNextWeapon(GunType);
            print(WeaponList[GunType]);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GunType = SelectPreviousWeapon(GunType);
            print(WeaponList[GunType]);
            return true;
        }
        return false;
    }

    int SelectNextWeapon(int GunNum)
    {
        GunNum++;
        //GunType %= (ammountOfGuns+1);
        if (GunNum < ammountOfGuns) 
        {
            print(GunNum);
            print(WeaponList.Find(w => w.name=="Shotgun").isAvailable);
            if (WeaponList[GunNum] != null && WeaponList[GunNum].isAvailable)
            {
                return GunNum;
            }
            else
                return 0;
        }
        else return 0;
    }

    int SelectPreviousWeapon(int GunNum)
    {
        GunNum--;
        if (GunNum >= 0)
        {
            print(GunNum);
            if (WeaponList[GunNum] != null && WeaponList[GunNum].isAvailable)
                return GunNum;
            else
                return ammountOfGuns - 1;
        }
        else if(WeaponList[ammountOfGuns - 1].isAvailable)
            return ammountOfGuns - 1;
        return ++GunNum;
    }

    void ShootWeapon(Weapon currentGun,GameObject shotPrefab)
    {
        //print(currentGun);
        shotSound = currentGun.shotSound;
        //print(shotAudioSource);
        shotAudioSource = GetComponent<AudioSource>();
        //shotAudioSource.clip = shotSound;
        shotAudioSource.PlayOneShot(shotSound);
        //currentGun.Shoot(firePoint, GameObject.Find("bulletPrefab"));
        currentGun.Shoot(firePoint, shotPrefab);
        animator.SetTrigger("IsShooting");
        //animator.SetBool("IsShooting",true); //***
    }

    void ReloadWeapon(Weapon currentGun)
    {
        //reloadSound = allMyAudioSources[dictReloadAudioSources[GunType]];
        reloadSound = Instantiate(currentGun.reloadSound);
        reloadAudioSource = GetComponent<AudioSource>();
        reloadAudioSource.clip = reloadSound;
        reloadAudioSource.PlayDelayed(currentGun.reloadSoundTime);
        currentGun.Reload();
        animator.SetTrigger("IsReloading");
        //animator.SetBool("IsReloading",true); //**
        animator.SetInteger("GunType", GunType); // redundante?
    }

    public Weapon GetCurrentGun()
    {
        return currentGun;
    }

    public void AddWeaponsToList()
    {
        foreach (Weapon weapon in Weapons.GetComponentsInChildren<Weapon>())
        {
            if(!WeaponList.Contains(weapon)){
                WeaponList.Add(weapon);
            }   
        }
    }
}
