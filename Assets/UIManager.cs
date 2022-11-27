using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image HelthImage;
    public TextMeshProUGUI BulletText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void ChangHealthUI(float value)
    {
        HelthImage.DOFillAmount(value, 0.2f);
    }
    public void ChangeBullet(int f1,  int f2)
    {
        BulletText.text = f1.ToString() + "/" + f2.ToString();
    }
}
