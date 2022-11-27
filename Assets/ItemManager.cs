using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;
    public List<ItemInfor> items;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnItemClick(int index)
    {
        DialogueManager.instance.PlayerAudio();

        image.sprite = items[index].Image;
        text.text = "Descripton: " + items[index].Description;
    }
    private void OnEnable()
    {
        OnItemClick(0);
    }
}
