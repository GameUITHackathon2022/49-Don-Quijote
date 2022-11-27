using LittleFoxLite;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject diaogueObject;
    public static DialogueManager instance;
    public DialogueActor dialogue;
    public TextMeshProUGUI gText;
    private void Awake()
    {
        if (instance == null) instance = this;
        source = GetComponent<AudioSource>();
        source.ignoreListenerPause = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextDialogue()
    {
        if (dialogue == null) return;
        dialogue.NextDialogue();
        PlayerAudio();
    }
    public void StartDialog()
    {
        diaogueObject.SetActive(true);
        PlayerAudio();

    }
    public void StopDialog()
    {
        if (diaogueObject.activeSelf) diaogueObject.SetActive(false);
        PlayerAudio();

    }
    public void ExitPress()
    {
        if (dialogue == null) return;
        dialogue.EndDialogueAction();
        PlayerAudio();

    }
    AudioSource source;
    [SerializeField] List<AudioClip> FootStepSound;
    public void PlayerAudio()
    {

            source.PlayOneShot(FootStepSound[Random.Range(0, FootStepSound.Count)]);

    }
}
