using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] List<AudioClip> FootStepSound;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] List<AudioClip> ShootSoundS;
    AudioSource source;
    // Start is called before the first frame update
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    void Start()
    {
    }
    private void Update()
    {
        
    }
    // Update is called once per fram
    public void FootStep(float volume)
    {
        source.PlayOneShot(FootStepSound[Random.Range(0, FootStepSound.Count)], volume);
        
    }
    public void ReloadSound()
    {
        source.PlayOneShot(reloadSound);
    }
    public void ShootSound()
    {
        source.PlayOneShot(ShootSoundS[Random.Range(0, ShootSoundS.Count)]);
    }
}
