using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Banshee : MonoBehaviour
{
    private AudioSource audioSource;
    private NavMeshAgent agent;
    private Transform hedef;
    private SesDinleyici sesDinleyici;
    
    [Header("Jump Scare")]
    
    [SerializeField] Transform MidPoint;
    [SerializeField] private int DistanceJumpScare;
    [SerializeField] private GameObject JumpScareCamera;
    [SerializeField] private AudioClip JumpScareSound1;
    private RaycastHit hitJumpScare;

    [Header("FieldOfView")]
    [SerializeField] Transform HeadPoint;
    [SerializeField] int DistanceFieldOfView;
    private RaycastHit hitFieldOfView;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            hedef = playerObj.transform;
        
        sesDinleyici = FindObjectOfType<SesDinleyici>();
    }

    void Update()
    {
        
        if (sesDinleyici != null && sesDinleyici.canavarDuydu && hedef != null)
        {
            agent.SetDestination(hedef.position);
        }
        
        if (GameObject.Find("JumpScareCollider").GetComponent<JumpScareCollider>().playeryakalandi)
        {
            if (hitJumpScare.transform.CompareTag("Player"))
            {
                Debug.Log("Player Yakalandı");
                
                StartCoroutine(JumpScare());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Canavar Playeri Gördü (Trigger)");

            agent.isStopped = false;
            agent.SetDestination(hedef.position);

            Vector3 direction = hedef.position - transform.position;
            direction.y = 0f; 
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

    IEnumerator JumpScare()
    {
        GameObject Character = hitJumpScare.transform.gameObject;
        audioSource.PlayOneShot(JumpScareSound1);
        Character.SetActive(false);
        JumpScareCamera.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
        JumpScareCamera.SetActive(false);
        Character.SetActive(true);
        GameObject.Find("JumpScareCollider").GetComponent<JumpScareCollider>().playeryakalandi = false;
    }
}
