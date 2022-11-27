using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnemyManager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    public static AudioEnemyManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] List<AudioClip> ShootSoundS;
    public void PlayHit()
    {
        source.PlayOneShot(ShootSoundS[Random.Range(0, ShootSoundS.Count)]);
    }
    [SerializeField] AudioClip death;
    public void PlayDeath()
    {
        source.PlayOneShot(death);
    }
}
