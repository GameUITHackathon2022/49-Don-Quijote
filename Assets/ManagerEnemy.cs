using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerEnemy : MonoBehaviour
{
    int maxEnemy = 3;
    [SerializeField] Transform[] spawnpos;
    [SerializeField] GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
       for(int i = 0; i< maxEnemy; i++)
        {
            SpawnEnemy();
        }
    }
    void SpawnEnemy()
    {
        Instantiate(enemy, spawnpos[Random.Range(0,spawnpos.Length)]);
    }
    // Update is called once per frame
    void Update()
    {
        if (EnemyBase.enemycount < maxEnemy) SpawnEnemy();
    }
}
