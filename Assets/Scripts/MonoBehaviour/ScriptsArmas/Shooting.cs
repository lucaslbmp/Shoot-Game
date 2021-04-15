using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// =====================================================================================================================================================================================//
//                                          PRESETS DO JOGO                                                                                                                             //
// =====================================================================================================================================================================================//

// Classe que gerencia as a��es de tiro, recarregamento, troca de arma e ataque com arma branca do player

[RequireComponent(typeof(AudioSource))]
public class Shooting : MonoBehaviour
{
    // Audios                                                                                          
    AudioSource shotAudioSource;                                            // Audio Source associado ao som de disparo da arma
    AudioSource reloadAudioSource;                                          // Audio Source associado ao som de recarga da arma
    AudioClip shotSound;                                                    // Clip de audio associado ao som de disparo da arma                                                                                    
    AudioClip reloadSound;                                                  // Clip de audio associado ao som de recarga da arma                                             
    AudioClip MeleeSound;                                                   // Clip de audio associado ao som de recarga da arma                                        
    //private static Dictionary<int, int> dictShootAudioSources = new Dictionary<int, int>() { { 0, 0 }, { 1, 3 } };              // Associa o GunType ao respectivo Audio Source de tiro
    //private static Dictionary<int, int> dictReloadAudioSources = new Dictionary<int, int>() { { 0, 2 }, { 1, 4} };             // Associa o GunType ao respectivo Audio Source de recarga
                                                                                                                               //private static Dictionary<int, int> dictMeleeAudioSources = new Dictionary<int, int>() { {1, }, { 2, 3} };               // Associa o MeleeType ao respectivo Audio Source de Facas
    // Variaveis de tiro
    public Transform firePoint;                                                                                                 // vari�vel de posi��o
    public GameObject bulletPrefab;                                                                                             // prefab do proj�til
    public GameObject pelletPrefab;                                                                                             // prefab de "chumbinho" da espingarda
    public GameObject knifePrefab;                                                                                              // prefab da sprite de movimento da Faca
    public GameObject MuzzleflashPrefab;                                                                                        // prefab do flash do tiro
    //List<String> projectilePrefabs = new List<String>() {null, "bulletPrefab", "pelletPrefab" } ;                                                             // Lista de prefabs de projeteis

    //public float bulletForce = 20f;
    public int loaded_ammo;                                                                                                      // variavel que calcula o quanto de projeteis foram carregados para a arma
    public int remaining_ammo;                                                                                                   // variavel que calcula o quanto de projeteis ainda existem no paint da arma
  
    //List<BulletType> BulletTypes = new List<BulletType>() { new BulletType(1, "bulletPrefab") };

    // Variaveis das armas
    private int GunType = 0;                                                                                                    // Variavel para sele�ao da arma
    public GameObject Weapons;                                                                                                    // Objeto que contem todas as armas
    [HideInInspector] public List<Weapon> WeaponList = new List<Weapon>();                                                      // Lista de armas que o player possui
    public int ammountOfGuns = 2;                                                                                               // quantidade de armas que o player possui 
    Weapon currentGun;

    Animator animator;                                                                                                          // inicia um animator para carregar as anima��es dos proj�teis

    // Corrotinas das armas
    Coroutine shootingCoroutine;
    Coroutine reloadingCoroutine;

    //  Inicia o jogo e define as condi��es iniciais
    void Start()
    {
        animator = GetComponent<Animator>();
        AddWeaponsToList();                                                                       // Adiciona as armas a weaponList
        currentGun = WeaponList[0];
        ammountOfGuns = WeaponList.Count;
        shotAudioSource = gameObject.AddComponent<AudioSource>();
        reloadAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Atualiza as condi��es de jogo
    void Update()
    {
        //currentGun = Weapons.GetComponent<Weapon>();
        GameObject shotPrefab;
        bool changeWeapon = UpdateGunSelection();
        if (changeWeapon)
            currentGun = WeaponList[GunType];
        shotPrefab = currentGun.projectilePrefab;
        animator.SetInteger("GunType", GunType);
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentGun.CanShoot())
            {
                if (shootingCoroutine == null && reloadingCoroutine == null)
                {
                    shootingCoroutine = StartCoroutine(ShootWeapon(currentGun, shotPrefab, currentGun.shootDelay));
                    print(shootingCoroutine);
                }

            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentGun.CanReload())
            {
                if (reloadingCoroutine == null)
                {
                    reloadingCoroutine = StartCoroutine(ReloadWeapon(currentGun, currentGun.reloadTime));
                }
            }
        }
        (loaded_ammo, remaining_ammo) = (currentGun.AmmoLoaded(), currentGun.AmmoRemaining());
        //Debug.Log(loaded_ammo + "-" + remaining_ammo);
    }


    // define as teclas utilizadas para a troca das armas
    bool UpdateGunSelection()
    {
        AddWeaponsToList();
        if (Input.GetKeyDown(KeyCode.E))
        {
            GunType = SelectNextWeapon(GunType);
            //print(WeaponList[GunType]);
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
            //print(GunNum);
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

    // Gerencia todos os aspectos relacionados � execu��o de um tiro
    public IEnumerator ShootWeapon(Weapon currentGun, GameObject shotPrefab, float timetoShootAgain)
    {
        while (true)
        {
            shotSound = currentGun.shotSound;                           // Atribui o clip de audio de tiro da arma atual ao clip de audio de tiro do player 
            shotAudioSource.PlayOneShot(shotSound);                     // Executa o som de tiro da arma
            currentGun.Shoot(firePoint, shotPrefab);                   // Chama a fun�ao da classe Weapon responsavel por instanciar o(s) projetil(eis)
            animator.SetTrigger("IsShooting");                          // Dispara o trigger para iniciar a anima�ao de tiro
            if (timetoShootAgain > float.Epsilon)
            {
                yield return new WaitForSeconds(timetoShootAgain);      // Executa um delay at� poder atirar de novo
                break;
            }
        }
        StopCoroutine(shootingCoroutine);                               // Encerra a corrotina de tiro
        shootingCoroutine = null;                                       // Atribui null � corrotina de tiro
    }

    // Gerencia todos os aspectos relacionados ao recarregamento da arma
    public IEnumerator ReloadWeapon(Weapon currentGun, float reloadDuration)
    {
        while (true)
        {
            reloadSound = Instantiate(currentGun.reloadSound);
            reloadAudioSource.clip = reloadSound;
            reloadAudioSource.PlayDelayed(currentGun.reloadSoundTime);                  // Executa o som de recarga da arma com atraso dado por "reloadSoundTime", que varia com a arma 
            animator.SetTrigger("IsReloading");                                         // Dispara o trigger para iniciar a anima�ao de tiro                                   
            if (reloadDuration > float.Epsilon)
            {
                yield return new WaitForSeconds(reloadDuration);                        // Delay correspondente � anima��o de recarga
                currentGun.Reload();                                                    // Atualiza a muni�ao ap�s a recarga
                break;
            }
        }
        StopCoroutine(reloadingCoroutine);                                              // Encerra a corrotina de recarga
        reloadingCoroutine = null;                                                      // Atribui null � corrotina de recarga
    }

    // Retorna a arma atual carregada pelo player
    public Weapon GetCurrentGun()   {return currentGun;}

    // Adicionar arma � lista de armas do player
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
