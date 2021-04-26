using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Autor: Gilmar Correia
/// Data: 20/04/2021
/// Classe que gerencia as caixas de dialogo
/// </summary>

public class DialogManager : MonoBehaviour
{
    public Text nameText;                                   // Titulo da caixa de dialogo
    public Text dialogText;                                 // Texto da caixa de dialogo

    [HideInInspector]
    public static Player player;                            // player

    public Animator animator;                               // Animator 

    public static bool dialogueHasEnded = true;             // Flag que indica se o dialogo terminou

    private AudioSource dialogueSound;                      // AudioSorce que executa o som  durante a exibiçao do dialogo
    public AudioClip beepSound;                             // Som executado na exibiçao do dialogo

    //FIFO
    private Queue<string> sentences;                        // Fila de sentenças a ser exibida

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();                    // Iniciliza a fila de sentanças como vazia

        AudioSource[] allMyAudioSources = GetComponents<AudioSource>(); // Obtem todos os AudioSources do gameobject
        dialogueSound = allMyAudioSources[0];           // Obtem o AudioSource da caixa de dialogo
    }

    // Metodo que recebe um dialogo e exibe-o na tela
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueHasEnded = false;                                   // Dialogo nao terminou
       
        animator.SetBool("IsOpen", true);                           // Altera a flag IsOpen no Animator para true
        
        player.GetComponent<PlayerMovement>().showDialogue();       // Chama uma função para interromper o mov. do plyaer durante o dialogo
        player.GetComponent<Shooting>().showDialogue();             // Chama uma função para interromper o tiro do plyaer durante o dialogo

        nameText.text = dialogue.name;                              // Obtem o nome (titulo) do dialogo

        sentences.Clear();                                          // Limpa as sentenças

        foreach(string sentence in dialogue.sentences)              // Para cada sentença do dialogo...
        {
            sentences.Enqueue(sentence);                            // Enfileire a sentença para exibiçao
        }

        DisplayNextSentence();                                      // Chama funçao que mostra a proxima sentença
    }

    // Funçao que gerencia a exibiçao de sentenças de um dialogo
    public void DisplayNextSentence()
    {
        if (!dialogueSound.isPlaying)                           // Se o som de dialogo nao esta sendo executado...
            dialogueSound.PlayOneShot(beepSound);               // Execute-o

        if (sentences.Count == 0)                               // Se nao há nenhuma sentença
        {
            EndDialogue();                                     // Finaliza o dialogo 
            return;
        }

        string sentence = sentences.Dequeue();                 // Desinfileira uma sentaça e a armazena em sentence 

        StopAllCoroutines();                                   // Para todas as corrotinas
        StartCoroutine(TypeSentence(sentence));                // Inicia a corrotina de digitar a sentença desenfileirada
 
    }

    // Corrotina que recebe uma sentença e gerencia sua exibição na caixa de dialogo
    IEnumerator TypeSentence (string sentence)
    {

        dialogText.text = "";                                   // Limpa o texto de dialogText

        foreach (char letter in sentence.ToCharArray())         // Para cada letra na sentença... 
        {
            dialogText.text += letter;                          // Acrescenta a letra ao texto de dialogText
            yield return null;                                  // Retorna null
        }
    }

    // Funçao que finaliza a exibiçao da caixa de dialogo
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);                      // Muda a flag IsOpen do Animator para false 
        dialogueSound.Stop();

        player.GetComponent<PlayerMovement>().stopDialogue();   // Atualiza para o script Player do player que o dialogo terminou
        player.GetComponent<Shooting>().stopDialogue();         // Atualiza para o script Shooting do player que o dialogo terminou

        dialogueHasEnded = true;                            // Flag indica que o dialogo terminou
    }
}
