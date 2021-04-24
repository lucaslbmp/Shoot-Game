using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Autor: Lucas Barboza
/// Data: 03/01/2021
/// Classe que gerencia as ações de tiro, recarregamento e troca de arma do player
/// </summary>

public class Shooting : MonoBehaviour
{
    // Audios                                                                                          
    AudioSource shotAudioSource;                               // Audio Source associado ao som de disparo da arma
    AudioSource reloadAudioSource;                             // Audio Source associado ao som de recarga da arma
    AudioClip shotSound;                                       // Clip de audio associado ao som de disparo da arma                                                                                    
    AudioClip reloadSound;                                     // Clip de audio associado ao som de recarga da arma                                             
    AudioClip MeleeSound;                                      // Clip de audio associado ao som de recarga da arma                                        

   // Variaveis de tiro
    public Transform firePoint;                                // Transform associado à posição de intanciação do tiro 
    public GameObject bulletPrefab;                            // Prefab de projetil da handgun e AK47                                                             
    public GameObject pelletPrefab;                            // Prefab de projetil da shotgun                                                             
    public GameObject knifePrefab;                             // Prefab de faca                                                
    public GameObject MuzzleflashPrefab;                       // prefab do flash do tiro

    //public float bulletForce = 20f;
    public int loaded_ammo;                                    // Muniçao carregada na arma                                                                             // variavel que calcula o quanto de projeteis foram carregados para a arma
    public int remaining_ammo;                                 // Muniçao nao carregada na arma                                                                        // variavel que calcula o quanto de projeteis ainda existem no paint da arma

    // Variaveis das armas
    private int GunType = 0;                                    // Index da arma                                                                                              // Variavel para seleçao da arma
    public Weapons WeaponsPrefab;                               // Prefab de Lista de Weapons                                                                                                 // Objeto que contem todas as armas
    [HideInInspector] public Weapons Weapons;                   // Lista de Weapons                                                 // Lista de armas que o player possui
    public int ammountOfGuns = 2;                               // Quanridade de armas no jogo                                                                                             // quantidade de armas que o player possui 
    Weapon currentGun;                                          // Armazena Weapon atual

    Animator animator;                                          // Armazena componente Animator do player                                                                                                // inicia um animator para carregar as animações dos projéteis

    // Corrotinas das armas
    Coroutine shootingCoroutine;                            // Armazena corrotina de tiro
    Coroutine reloadingCoroutine;                           // Armazena corrotina de recarga
    Coroutine MachinegunShootCoroutine;                     // Armazena corrotina de som de tiro para metralhadoras
    //float time;

    // Bool para a execução das ações relacionadas a arma
    private bool dialogueIsShowing = false;                 // Flag que indica de a caixa de dialogo esta sendo mostrada

    // Funçao que retorna true se a caixa de dialogo esta sendo mostrada
    public void showDialogue()
    {
        dialogueIsShowing = true;
    }

    // Funçao que para a caixa de dialogo
    public void stopDialogue()
    {
        dialogueIsShowing = false;
    }

    //  Funçao que inicia o jogo e define as condições iniciais
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();             // Obtem o Animator do player
        Weapons = Instantiate(WeaponsPrefab);                       // Instancia o objeto Weapons a partir do seu prefab
        Weapons.AddWeaponsToList();                                 // Adiciona as armas a weaponList
        currentGun = Weapons.GetWeapon(0);                          // Inicializa a arma atual como sendo a de index 0
        shotAudioSource = gameObject.AddComponent<AudioSource>();   // Adiciona AudioSource de tiro
        reloadAudioSource = gameObject.AddComponent<AudioSource>(); // Adiciona AudioSource de recarga
    }

    // Atualiza as condições de jogo
    void Update()
    {
        if (!dialogueIsShowing)                                     // Se a caixa de dialogo nao esta sendo mostrada...
        {
            GameObject shotPrefab;
            bool changeWeapon = UpdateGunSelection();               // Chama a funçao que atualiza a arma e retorna true se mudou
            if (changeWeapon)                                       // Se mudou a arma...      
                currentGun = Weapons.GetWeapon(GunType);            // Obtem a arma com o novo index (GunType)
            shotPrefab = currentGun.projectilePrefab;               // Obtem o prefab de projetil da arma escolhida
            animator.SetInteger("GunType", GunType);                // Passa o index de arma (GunType) para o Animator
            animator.SetFloat("GunTypeFloat", GunType / 10f);       // Passa o parametro floatGunType, associado à Blend Tree, para o Animator
            if (Input.GetButtonDown("Fire1"))                       // Se o botao de atirar for pressionado...
            {
                if (currentGun.CanShoot())                          // Se pode atirar com a arma atual...
                {
                    if (shootingCoroutine == null && reloadingCoroutine == null) // Se as corritinas de tiro e recarga nao estao sendo executadas...
                    {
                        shootingCoroutine = StartCoroutine(ShootWeapon(currentGun, shotPrefab, currentGun.shootDelay, currentGun.shootAnimTime)); // Inicia corrotina de tiro
                    }

                }
            }
            else if (Input.GetButton("Fire1"))            // Caso contrario, enquanto o botao de tiro dor segurado... 
            {
                if (currentGun.name == "AK47")           // Se a arma atual for a AK47...
                {
                    if (currentGun.CanShoot())          // Se pode atirar com a AK47...
                    {
                        if (reloadingCoroutine == null)     // Se a corrotina derecarga nao está sendo executada...
                        {
                            if (shootingCoroutine == null)    // Se a corrotina de tiro nao esta sendo excutada
                                shootingCoroutine = StartCoroutine(ShootMachinegun(currentGun, shotPrefab, currentGun.shootAnimTime)); // Inicia a corrotina de tiro com a metralhadora
                            if (MachinegunShootCoroutine == null && shootingCoroutine != null) // Se a corrotina de tiro foi iniciada mas a de som da metralhadora nao...
                                MachinegunShootCoroutine = StartCoroutine(PlayMachinegunSound(currentGun.shotSoundTime)); // Inicie a corrotina de som da metralhadora
                        }

                    }
                    else                                // Se nao pode atirar com a AK47... 
                    {
                        if (MachinegunShootCoroutine == null && shootingCoroutine != null)  // Se a corrotina de tiro foi iniciada mas a de som da metralhadora nao...
                            MachinegunShootCoroutine = StartCoroutine(StopPlayingMachinegunSound(currentGun.shotSoundTime)); // Inicie a corrotina de parar o som de metralhadora
                    }
                }
            }
            else if (Input.GetButtonUp("Fire1"))                // Caso contrario, se o usuario soltar botao do mouse... 
            {
                if (currentGun.name == "AK47")                  // Se a arma for a AK47
                {
                    if (MachinegunShootCoroutine != null)       // Se a corrotina de som da metralhadora nao for nula
                    {
                        StopCoroutine(MachinegunShootCoroutine); // Para a corrotina de som da metralhadora
                    }
                    MachinegunShootCoroutine = StartCoroutine(StopPlayingMachinegunSound(currentGun.shotSoundTime)); // Inicia a corrotina de parar o som da metralhadora
                }

            }
            else if (Input.GetKeyDown(KeyCode.R))               // Caso contrário, se a tecla R foi pressionada...
            {
                if (currentGun.CanReload())                     // Se a arma pode ser recarregada...
                {
                    if (reloadingCoroutine == null)             // Se a corrotina de recarga nao esta sendo executada...
                    {
                        reloadingCoroutine = StartCoroutine(ReloadWeapon(currentGun, currentGun.reloadTime, currentGun.reloadAnimTime)); // Inicia a corrotina de recarga...
                    }
                }
            }
            (loaded_ammo, remaining_ammo) = (currentGun.AmmoLoaded(), currentGun.AmmoRemaining()); // Recebe a muniçao carregada e nao carregada
        }
    }


    // funçao que define as teclas utilizadas para a troca das armas
    bool UpdateGunSelection()
    {
        //Weapons.AddWeaponsToList();
        if (Input.GetKeyDown(KeyCode.E))
        {
            GunType = Weapons.SelectNextWeapon(GunType);
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GunType = Weapons.SelectPreviousWeapon(GunType);
            return true;
        }
        return false;
    }

    // Função que gerencia todos os aspectos relacionados à execução de um tiro
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

    //Corrotina que gerencia a execução do tiro com umametralhadora
    public IEnumerator ShootMachinegun(Weapon currentGun, GameObject shotPrefab, float shootAnimTime)
    {
        while (true)
        {
            shotSound = currentGun.shotSound;                           // Atribui o clip de audio de tiro da arma atual ao clip de audio de tiro do player 
            currentGun.Shoot(firePoint, shotPrefab);                   // Chama a funçao da classe Weapon responsavel por instanciar o(s) projetil(eis)
            animator.SetTrigger("StartedShoot");                          // Dispara o trigger para iniciar a animaçao de tiro
            if (shootAnimTime > float.Epsilon)
            {
                yield return new WaitForSeconds(shootAnimTime);         // Aguarda o temp da animação de tiro (shootAnimTime)
                break;
            }
        }
        StopCoroutine(shootingCoroutine);                               // Encerra a corrotina de tiro
        shootingCoroutine = null;                                       // Atribui null à corrotina de tiro
    }

    public IEnumerator PlayMachinegunSound(float duration)
    {
        shotAudioSource.Stop();                                     // Para o AudioSourec de tiro
        shotAudioSource.PlayOneShot(shotSound);                     // Executa o som de tiro da arma
        yield return new WaitForSeconds(duration);                  // Aguarda a duraçao do som de tiro passada
        MachinegunShootCoroutine = null;                            // Atribui num a corrotina de som de tiro da metralhadora
    }

    // Corrotina que gerencia o som executado ao para de atirar com a metralhadora
    public IEnumerator StopPlayingMachinegunSound(float delay)
    {
        yield return new WaitForSeconds(delay);                     // Aguarda um tempo (delay)
        shotAudioSource.Stop();                                     // Interrompe o AudioSource
        MachinegunShootCoroutine = null;                            // Atribui null à corrotina de metralhadora
    }

    // Corrotina que gerencia todos os aspectos relacionados ao recarregamento da arma
    public IEnumerator ReloadWeapon(Weapon currentGun, float reloadDuration, float reloadAnimTime)
    {
        while (true)
        {
            reloadSound = Instantiate(currentGun.reloadSound);                          
            reloadAudioSource.clip = reloadSound;
            reloadAudioSource.PlayDelayed(currentGun.reloadSoundTime);                  // Executa o som de recarga da arma com atraso dado por "reloadSoundTime", que varia com a arma 
            animator.SetTrigger("StartedReload");                                       // Dispara o trigger para iniciar a animaçao de tiro                                                                                // Estado alterado para RECARGA
            if (reloadAnimTime > float.Epsilon)
            {
                yield return new WaitForSeconds(reloadAnimTime);                        // Tempo até terminar a animação de recarga
            }
            if (reloadDuration > float.Epsilon)
            {
                yield return new WaitForSeconds(reloadDuration - reloadAnimTime);       // Delay após a recarga
                currentGun.Reload();                                                    // Atualiza a muniçao após a recarga
                break;
            }
        }
        StopCoroutine(reloadingCoroutine);                                              // Encerra a corrotina de recarga
        reloadingCoroutine = null;                                                      // Atribui null à corrotina de recarga
    }

    // Retorna a arma atual carregada pelo player
    public Weapon GetCurrentGun() { return currentGun; }

}
