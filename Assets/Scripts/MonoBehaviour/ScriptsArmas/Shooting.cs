using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// =====================================================================================================================================================================================//
//                                          PRESETS DO JOGO                                                                                                                             //
// =====================================================================================================================================================================================//

// Classe que gerencia as ações de tiro, recarregamento, troca de arma e ataque com arma branca do player

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
    public Transform firePoint;                                                                                                 // variável de posição
    public GameObject bulletPrefab;                                                                                             // prefab do projétil
    public GameObject pelletPrefab;                                                                                             // prefab de "chumbinho" da espingarda
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
    [HideInInspector] public List<Weapon> WeaponList = new List<Weapon>();                                                      // Lista de armas que o player possui
    public int ammountOfGuns = 2;                                                                                               // quantidade de armas que o player possui 
    Weapon currentGun;

    Animator animator;                                                                                                          // inicia um animator para carregar as animações dos projéteis

    // Corrotinas das armas
    Coroutine shootingCoroutine;
    Coroutine reloadingCoroutine;
    Coroutine MachinegunShootCoroutine;
    float time;

    //  Inicia o jogo e define as condições iniciais
    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.SetFloat("State", 0.5f);
        AddWeaponsToList();                                                                       // Adiciona as armas a weaponList
        currentGun = WeaponList[0];
        ammountOfGuns = WeaponList.Count;
        shotAudioSource = gameObject.AddComponent<AudioSource>();
        reloadAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Atualiza as condições de jogo
    void Update()
    {
        //currentGun = Weapons.GetComponent<Weapon>();
        GameObject shotPrefab;
        bool changeWeapon = UpdateGunSelection();
        if (changeWeapon)
            currentGun = WeaponList[GunType];
        shotPrefab = currentGun.projectilePrefab;
        animator.SetInteger("GunType", GunType);
        animator.SetFloat("GunTypeFloat", GunType / 10f);
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentGun.CanShoot())
            {
                if (shootingCoroutine == null && reloadingCoroutine == null)
                {
                    shootingCoroutine = StartCoroutine(ShootWeapon(currentGun, shotPrefab, currentGun.shootDelay, currentGun.shootAnimTime));
                    print(shootingCoroutine);
                }

            }
        }
        else if (Input.GetButton("Fire1"))
        {
            if(currentGun.name == "AK47")
            {
                if (currentGun.CanShoot())
                {
                    if (reloadingCoroutine == null)
                    {
                        if (shootingCoroutine == null)
                            shootingCoroutine = StartCoroutine(ShootMachinegun(currentGun, shotPrefab, currentGun.shootAnimTime));
                        if (MachinegunShootCoroutine == null && shootingCoroutine != null)
                            MachinegunShootCoroutine = StartCoroutine(PlayMachinegunSound(currentGun.shotSoundTime));
                        //print(shootingCoroutine);
                    }

                }
                else
                {
                    if (MachinegunShootCoroutine == null && shootingCoroutine != null)
                        MachinegunShootCoroutine = StartCoroutine(StopPlayingMachinegunSound(currentGun.shotSoundTime));
                }
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            if (currentGun.name == "AK47")
            {
                if (MachinegunShootCoroutine != null)
                {
                    StopCoroutine(MachinegunShootCoroutine);
                }
                MachinegunShootCoroutine = StartCoroutine(StopPlayingMachinegunSound(currentGun.shotSoundTime));
            }

        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentGun.CanReload())
            {
                if (reloadingCoroutine == null)
                {
                    reloadingCoroutine = StartCoroutine(ReloadWeapon(currentGun, currentGun.reloadTime, currentGun.reloadAnimTime));
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
        else if (WeaponList[ammountOfGuns - 1].isAvailable)
            return ammountOfGuns - 1;
        return ++GunNum;
    }

    // Gerencia todos os aspectos relacionados à execução de um tiro
    public IEnumerator ShootWeapon(Weapon currentGun, GameObject shotPrefab, float timetoShootAgain, float shootAnimTime)
    {
        while (true)
        {
            shotSound = currentGun.shotSound;                           // Atribui o clip de audio de tiro da arma atual ao clip de audio de tiro do player 
            shotAudioSource.PlayOneShot(shotSound);                     // Executa o som de tiro da arma
            currentGun.Shoot(firePoint, shotPrefab);                   // Chama a funçao da classe Weapon responsavel por instanciar o(s) projetil(eis)
            animator.SetTrigger("StartedShoot");                          // Dispara o trigger para iniciar a animaçao de tiro
            //animator.SetFloat("State",0f);
            if (shootAnimTime > float.Epsilon)
            {
                yield return new WaitForSeconds(shootAnimTime);
                //animator.SetFloat("State", .5f);
            }
            if (timetoShootAgain > float.Epsilon)
            {
                yield return new WaitForSeconds(timetoShootAgain - shootAnimTime);      // Executa um delay até poder atirar de novo
                break;
            }
        }
        StopCoroutine(shootingCoroutine);                               // Encerra a corrotina de tiro
        shootingCoroutine = null;                                       // Atribui null à corrotina de tiro
    }

    public IEnumerator ShootMachinegun(Weapon currentGun, GameObject shotPrefab, float shootAnimTime)
    {
        //float timeElapsed = 0f;
        while (true)
        {
            shotSound = currentGun.shotSound;                           // Atribui o clip de audio de tiro da arma atual ao clip de audio de tiro do player 
            currentGun.Shoot(firePoint, shotPrefab);                   // Chama a funçao da classe Weapon responsavel por instanciar o(s) projetil(eis)
            animator.SetTrigger("StartedShoot");                          // Dispara o trigger para iniciar a animaçao de tiro
            if (shootAnimTime > float.Epsilon)
            {
                yield return new WaitForSeconds(shootAnimTime);
                break;
                //timeElapsed += shootAnimTime;
            }
        }
        StopCoroutine(shootingCoroutine);                               // Encerra a corrotina de tiro
        shootingCoroutine = null;                                       // Atribui null à corrotina de tiro
    }

    public IEnumerator PlayMachinegunSound(float duration)
    {
        print(time - Time.time);
        time = Time.time;
        shotAudioSource.Stop();
        shotAudioSource.PlayOneShot(shotSound);                     // Executa o som de tiro da arma
        //shotAudioSource.Play();
        yield return new WaitForSeconds(duration);
        MachinegunShootCoroutine = null;
    }

    public IEnumerator StopPlayingMachinegunSound(float delay)
    {

        yield return new WaitForSeconds(delay);
        shotAudioSource.Stop();
        MachinegunShootCoroutine = null;
    }

    // Gerencia todos os aspectos relacionados ao recarregamento da arma
    public IEnumerator ReloadWeapon(Weapon currentGun, float reloadDuration, float reloadAnimTime)
    {
        while (true)
        {
            reloadSound = Instantiate(currentGun.reloadSound);
            reloadAudioSource.clip = reloadSound;
            reloadAudioSource.PlayDelayed(currentGun.reloadSoundTime);                  // Executa o som de recarga da arma com atraso dado por "reloadSoundTime", que varia com a arma 
            animator.SetTrigger("StartedReload");                                         // Dispara o trigger para iniciar a animaçao de tiro                                   
            //animator.SetFloat("State",1f);                                              // Estado alterado para RECARGA
            if (reloadAnimTime > float.Epsilon)
            {
                yield return new WaitForSeconds(reloadAnimTime);                        // Tempo até terminar a animação de recarga
                //animator.SetFloat("State", .5f);                                        // Estado alterado para NEUTRO
            }
            if (reloadDuration > float.Epsilon)
            {
                yield return new WaitForSeconds(reloadDuration - reloadAnimTime);          // Delay após a recarga
                currentGun.Reload();                                                    // Atualiza a muniçao após a recarga
                break;
            }
        }
        StopCoroutine(reloadingCoroutine);                                              // Encerra a corrotina de recarga
        reloadingCoroutine = null;                                                      // Atribui null à corrotina de recarga
    }

    // Retorna a arma atual carregada pelo player
    public Weapon GetCurrentGun() { return currentGun; }

    // Adicionar arma à lista de armas do player
    public void AddWeaponsToList()
    {
        foreach (Weapon weapon in Weapons.GetComponentsInChildren<Weapon>())
        {
            if (!WeaponList.Contains(weapon))
            {
                WeaponList.Add(weapon);
            }
        }
    }
}
