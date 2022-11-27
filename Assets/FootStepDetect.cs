using LittleFoxLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController.Instance.AudioFootSepSound();
    }
}
