using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public enum CryptoState
{
    IDLE,
    RUN,
    JUMP,
    KICK
}


public class CryptoBehaviour : MonoBehaviour
{
    [Header("Line of Sight")] 
    public bool HasLOS = false;

    public GameObject player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Attack")] 
    public float attackDistance;
    public PlayerBehaviour playerBehaviour;
    public float damageDelay = 1.0f;
    public bool isAttacking = false;
    public float kickForce = 0.01f;
    public float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerBehaviour = FindObjectOfType<PlayerBehaviour>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HasLOS)
        {
            agent.SetDestination(player.transform.position);
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        }


        if(HasLOS && distanceToPlayer < attackDistance && !isAttacking)
        {
                // could be an attack
            animator.SetInteger("AnimState", (int)CryptoState.KICK);
            transform.LookAt(transform.position - player.transform.forward);

            DoKickDamage();
            isAttacking = true;

            if (agent.isOnOffMeshLink)
            {
                animator.SetInteger("AnimState", (int)CryptoState.JUMP);
            }
        }
        else if (HasLOS && distanceToPlayer > attackDistance)
        {
            animator.SetInteger("AnimState", (int)CryptoState.RUN);
            isAttacking = false;
        }
        else
        {
            animator.SetInteger("AnimState", (int)CryptoState.IDLE);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HasLOS = true;
            player = other.transform.gameObject;
        }
    }

    private void DoKickDamage()
    {
        playerBehaviour.TakeDamage(20);
        StartCoroutine(kickBack());
    }

    private IEnumerator kickBack()
    {
        yield return new WaitForSeconds(damageDelay);
        
        var direction = Vector3.Normalize(player.transform.position - transform.position);
        playerBehaviour.controller.SimpleMove(direction * kickForce);
        StopCoroutine(kickBack());
    }

}

