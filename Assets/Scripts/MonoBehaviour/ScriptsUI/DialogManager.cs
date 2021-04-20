using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogText;

    [HideInInspector]
    public Player player;

    public Animator animator;

    public bool dialogueHasEnded = true;

    private AudioSource dialogueSound;
    public AudioClip beepSound;

    //FIFO
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

        AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        dialogueSound = allMyAudioSources[0];
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        dialogueHasEnded = false;

        player.GetComponent<PlayerMovement>().showDialogue();
        player.GetComponent<Shooting>().showDialogue();

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
 
    }

    IEnumerator TypeSentence (string sentence)
    {
        if (!dialogueSound.isPlaying)
            dialogueSound.PlayOneShot(beepSound);

        dialogText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);

        player.GetComponent<PlayerMovement>().stopDialogue();
        player.GetComponent<Shooting>().stopDialogue();

        dialogueHasEnded = true;
    }
}
