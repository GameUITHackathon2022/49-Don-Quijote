using LittleFoxLite;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour, iDamagetable
{
    public static int enemycount;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float EnemySenRange = 3f;
    [SerializeField] float EnemyAttackRange = 3f;
    [SerializeField] Transform attackpoint;
    public bool isAttack;
    bool isDeath = false;
    Animator animator;
    int aniRun;
    int aniAttack;
    int aniDeath;
    int maxHealt = 20;
    int currentHealth;
    void AssignAniId()
    {
        aniRun = Animator.StringToHash("IsRun");
        aniAttack = Animator.StringToHash("IsAttack");
        aniDeath = Animator.StringToHash("IsDeath");

    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        AssignAniId();
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable()
    {
        animator.SetBool(aniDeath, false);
        currentHealth = maxHealt;
        isDeath = false;
        isAttack = false;
        animator.SetBool(aniAttack, false);
        animator.SetBool(aniRun, true);
        enemycount++;
    }
    private void OnDisable()
    {
        enemycount--;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.canAttack) return;
        HandleCalculateDistane();
        HandleLogicAction();
    }
    void HandleCalculateDistane()
    {
        if (isDeath) return;
        Vector3 deltapos = PlayerController.Instance.transform.position - transform.position;
        float length = deltapos.magnitude;
        if (EnemySenRange >= length)
        {
            if (isAttack) return;
            isAttack = true;
            
            animator.SetBool(aniAttack, true);
            animator.SetBool(aniRun, false);
        } else
        {
            isAttack = false;
            animator.SetBool(aniAttack, false);
            animator.SetBool(aniRun, true);
        }
    }
    void HandleLogicAction()
    {
        if (isDeath) return;
        if (isAttack)
        {
            agent.isStopped = true ;
        }
        else
        {
            agent.isStopped = false;
            agent.destination= PlayerController.Instance.transform.position;
       
        } 
    }
    public void TakeDamage(int value)
    {
        if (isDeath) return;
        PraticlePlayOnShoot();
        AudioEnemyManager.instance.PlayHit();
        currentHealth -= value;
        if (currentHealth<=0)
        {
            DieAction();
        }
    }
    public void DieAction()
    {
        AudioEnemyManager.instance.PlayDeath();
        isDeath = true;
        animator.SetBool(aniDeath, true);
        Destroy(this.gameObject, 2);
    }
    [SerializeField] LayerMask PlayerLayer; 
    public void OnAttackPerfome()
    {
        Collider[] coli = Physics.OverlapSphere(attackpoint.position, EnemyAttackRange);
    }
    [SerializeField] ParticleSystem particle;   
    
    public void PraticlePlayOnShoot()
    {
        particle.Play();
    }
}
