using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public float hearingDistance = 15f;
    public float chaseDistance = 10f;
    public float searchDuration = 5f;
    public LayerMask playerLayer;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip alertClip;
    public AudioClip footstepClip;
    public AudioClip jumpscareClip;

    private Vector3 lastHeardPosition;
    private bool heardFootstep = false;
    private bool isChasing = false;
    private bool isSearching = false;
    private float searchTimer = 0f;
    float speed;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        SetRandomDestination();
        speed = agent.speed;
    }
    bool IsScreamHeard(float mesafe)
    {
        float sesSeviyesi = MicInput.MicLoudness;
        float gerekenEsik = HesaplaEsik(mesafe);
        if (sesSeviyesi >= gerekenEsik)
        {
            return true;
        }
        return false;
    }
    float HesaplaEsik(float mesafe)
    {
        if (mesafe <= 5f) return 0.2f;
        if (mesafe <= 10f) return 0.4f;
        if (mesafe <= 15f) return 0.6f;
        return 0.8f;
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isChasing && !isSearching &&
        ((distanceToPlayer < hearingDistance && PlayerIsRunning()) || IsScreamHeard(distanceToPlayer)))
        {
            heardFootstep = true;
            lastHeardPosition = player.position;
            GoToHeardPosition();
        }

        if (heardFootstep && !isChasing)
        {
            if (Vector3.Distance(transform.position, lastHeardPosition) < 1.5f)
            {
                isSearching = true;
                searchTimer = searchDuration;
                heardFootstep = false;
            }
        }

        if (isSearching)
        {
            searchTimer -= Time.deltaTime;

            if (searchTimer <= 0f)
            {
                isSearching = false;
                SetRandomDestination();
            }
        }
        if (CanSeePlayer())
        {
            StartChase();
        }
        else
        {
            agent.speed = speed;
        }
        if (isChasing)
        {
            agent.SetDestination(player.position);
            if (!CanSeePlayer())
            {
                isChasing = false;
                SetRandomDestination();
            }
            if (Vector3.Distance(transform.position, player.position) < 3f)
            {
                OnPlayerCaught();
            }
        }

        if (!isChasing && !isSearching && !agent.pathPending && agent.remainingDistance < 1f)
        {
            SetRandomDestination();
        }


    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 20f;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void GoToHeardPosition()
    {
        agent.speed = 2 * speed;
        agent.SetDestination(lastHeardPosition);
    }

    void StartChase()
    {
        isChasing = true;
        isSearching = false;
        agent.speed = 3 * speed;
        if (!audioSource.isPlaying && audioSource.clip != jumpscareClip)
        {
            audioSource.clip = alertClip;
            audioSource.Play();
        }
    }

    void OnPlayerCaught()
    {
        if (audioSource.clip != jumpscareClip)
        {
            audioSource.clip = jumpscareClip;
            audioSource.Play();
        }
    }

    bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out hit, chaseDistance))
        {
            if (hit.collider.transform == player)
            {
                Debug.DrawLine(transform.position + Vector3.up, hit.point, Color.red);
                return true;
            }
        }
        return false;
    }

    bool PlayerIsRunning()
    {
        return player.GetComponent<physicWalk_MouseLook>().isRunning;
    }
}

