using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullletPooling : MonoBehaviour
{
    [Header("BulletPoolSetup")]
    [SerializeField] int PoolCapacity;
    [SerializeField] Bullet gameObject;
    [SerializeField] Transform gunPos;
    Queue<Bullet> pool;
    private void Awake()
    {
        pool = new Queue<Bullet>();
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < PoolCapacity; i++)
        {
            GameObject go = Instantiate(gameObject.gameObject);
            Bullet bullet = go.GetComponent<Bullet>();
            bullet.mypool = this;
            bullet.gameObject.SetActive(false);
            pool.Enqueue(bullet);
        }
    }

    // Update is called once per frame
    public void ShootAction()
    {
       Bullet bullet = pool.Dequeue();
        bullet.gameObject.SetActive(true);
        bullet.transform.position = gunPos.position;
        bullet.transform.rotation = transform.rotation;
    }
    public void DeQueue(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        pool.Enqueue(bullet);
    }
}
