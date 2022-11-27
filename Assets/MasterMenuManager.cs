using LittleFoxLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterMenuManager : MonoBehaviour
{
    public static MasterMenuManager instance;
    [SerializeField] List<GameObject> TabContent;
    // Start is called before the first frame update
    private void Awake()
    {
         if (instance == null) instance = this;
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTabButtonPress(int index)
    {
        foreach(var k in TabContent)
        {
            k.SetActive(false);
        }
        DialogueManager.instance.PlayerAudio();

        TabContent[index].SetActive(true);
    }
    public void OnExitButtonPress()
    {
        DialogueManager.instance.PlayerAudio();

        OnESCPressed();
    }
    public void OnESCPressed()
    {
        
        DialogueManager.instance.PlayerAudio();

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            InteractiveHandel.instance.ChangeToNormalState();
            GameManager.instance.ResumeGame();

        }
        else
        {
            GameManager.instance.PauseGame();
            InteractiveHandel.instance.ChangeToEventState();
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
    public void OnChangeSoundSlider(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
    public void OnQuitButtonPress()
    {
        Application.Quit();
    }
}
