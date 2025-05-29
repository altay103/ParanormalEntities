using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    float hearingDistance = 10f;
    float chaseDistance = 35f;
    float searchDuration = 1f;
    public LayerMask playerLayer;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip alertClip;
    public AudioClip footstepClip;
    public AudioClip jumpscareClip;

    private Vector3 lastHeardPosition;
    private bool isChasing = false;
    private bool isHearing = false;
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
            Debug.Log($"Scream heard! Distance: {mesafe}, Level: {sesSeviyesi}, Threshold: {gerekenEsik}");
            return true;
        }
        return false;
    }
    float HesaplaEsik(float mesafe)
    {
        //return 10;
        if (mesafe <= 5f) return 0.2f;
        if (mesafe <= 10f) return 0.4f;
        if (mesafe <= 15f) return 0.6f;
        if (mesafe <= 20) return 0.8f;
        return 1f;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (CanSeePlayer())
        {
            StartChase();
            searchTimer = searchDuration;
        }
        else if (CanHearPlayer() && !isChasing)
        {
            lastHeardPosition = player.position;
            GoToHeardPosition();
        }

        // if (heardFootstep && !isChasing)
        // {
        //     if (Vector3.Distance(transform.position, lastHeardPosition) < 1.5f)
        //     {
        //         isSearching = true;
        //         searchTimer = searchDuration;
        //         heardFootstep = false;
        //     }
        // }

        // if (isSearching)
        // {
        //     searchTimer -= Time.deltaTime;

        //     if (searchTimer <= 0f)
        //     {
        //         isSearching = false;
        //         SetRandomDestination();
        //     }
        // }


        if (isChasing)
        {
            isHearing = false;
            agent.SetDestination(player.position);
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0f)
            {
                isChasing = false;
            }
            if (distanceToPlayer < 4f)
            {
                OnPlayerCaught();
            }
            // if (!CanSeePlayer())
            // {
            //     isChasing = false;
            //     SetRandomDestination();
            // }

        }
        if (Vector3.Distance(transform.position, lastHeardPosition) < 1.5f && isHearing)
        {
            isHearing = false;
        }


        else if (!isChasing && !isHearing && !agent.pathPending)
        {
            agent.speed = speed;
            SetRandomDestination();
        }




    }
    bool CanHearPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log($"Heard : {distanceToPlayer} < {hearingDistance} = {distanceToPlayer < hearingDistance} and PlayerIsRunning: {PlayerIsRunning()}");
        if ((distanceToPlayer < hearingDistance && PlayerIsRunning()) || IsScreamHeard(distanceToPlayer))
        {
            Debug.Log($"Hearing player! Distance: {distanceToPlayer}");
            isHearing = true;
            return true;
        }
        return false;
    }
    void SetRandomDestination()
    {
        float moveRadius = 20f;
        float destinationCheckDistance = 0.5f;
        // Eğer zaten bir yere gidiyorsa ve henüz varmadıysa, yeni hedef verme
        if ((agent.velocity.sqrMagnitude > 0.1f) && (agent.pathPending || agent.remainingDistance > destinationCheckDistance))
        {
            return;
        }


        // 1. Rastgele bir yön (sadece yatay düzlemde)
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0f;
        randomDirection *= moveRadius;
        randomDirection += transform.position;

        // 2. NavMesh üzerinde uygun bir pozisyon bul
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, moveRadius, NavMesh.AllAreas))
        {
            // 3. O pozisyona gidebilecek yol var mı, kontrol et
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(hit.position);

                // DEBUG: Hedef çizgisi (sadece geliştirirken işe yarar)
                Debug.DrawLine(transform.position, hit.position, Color.green, 1.5f);
            }
        }
    }

    void GoToHeardPosition()
    {
        agent.speed = 1.5f * speed;
        agent.SetDestination(lastHeardPosition);
    }

    void StartChase()
    {
        isChasing = true;
        agent.speed = 2.5f * speed;
        if (!audioSource.isPlaying)
        {
            audioSource.clip = alertClip;
            audioSource.Play();
        }

    }

    void OnPlayerCaught()
    {
        Debug.Log("Player caught!");
        if (audioSource.clip != jumpscareClip || !audioSource.isPlaying)
        {
            audioSource.clip = jumpscareClip;
            audioSource.Play();
        }
    }

    bool CanSeePlayer()
    {
        Debug.Log("Checking if player is visible...");
        Vector3 direction = player.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out hit, chaseDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawLine(transform.position + Vector3.up, hit.point, Color.red);
                return true;
            }
            else
            {
                Debug.DrawLine(transform.position + Vector3.up, hit.point, Color.yellow);
            }
        }
        Debug.DrawLine(transform.position + Vector3.up, transform.position + direction.normalized * chaseDistance, Color.blue);
        return false;
    }

    bool PlayerIsRunning()
    {
        return player.GetComponent<physicWalk_MouseLook>().isRunning;
    }
}

