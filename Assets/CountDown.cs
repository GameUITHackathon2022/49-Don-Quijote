using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CountDown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float timeSurival = 180;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDownTimer());
    }
    IEnumerator CountDownTimer()
    {
        while(timeSurival >0)
        {
            yield return new WaitForSeconds(1);
            timeSurival -= 1;
            text.text = timeSurival.ToString();
        }
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
