using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, Health.IHealthListener
{
    enum State
    {
        Idle,
        Follow,
        Attack,
        Die
    }

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    AudioSource audio;

    State state;
    float currentStateTime;
    public float timeForNextState = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();

        state = State.Idle;
        currentStateTime = timeForNextState;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                currentStateTime -= Time.deltaTime;
                if (currentStateTime < 0)
                {
                    float distance = (player.transform.position - transform.position).magnitude;
                    if (distance < 2f)
                    {
                        StartAttack();
                    }
                    else
                    {
                        StartFollow();
                    }
                }
                break;
            case State.Follow:
                if (agent.remainingDistance < 1.9f || !agent.hasPath)
                {
                    StartIdle();
                }
                else
                {
                    agent.destination = player.transform.position;

                }
                break;
            case State.Attack:
                currentStateTime -= Time.deltaTime;
                if (currentStateTime < 0)
                {
                    StartIdle();
                }
                break;
        }
    }

    void StartIdle()
    {
        audio.Stop();
        state = State.Idle;
        currentStateTime = timeForNextState;
        agent.isStopped = true;
        animator.SetTrigger("Idle");

    }

    void StartFollow()
    {
        audio.Play();
        state = State.Follow;
        agent.destination = player.transform.position;
        agent.isStopped = false;
        animator.SetTrigger("Walk");

    }

    void StartAttack()
    {
        audio.Stop();
        state = State.Attack;
        currentStateTime = timeForNextState;
        animator.SetTrigger("Talk");
    }

    public void OnDie()
    {
        audio.Stop();
        state = State.Die;
        agent.isStopped = true;
        animator.SetTrigger("Death");
        Invoke("DestroyThis", 2);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            other.GetComponent<Health>().Damage(1);
        }
    }
}
