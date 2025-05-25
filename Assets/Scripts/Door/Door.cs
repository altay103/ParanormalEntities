using System;
using UnityEngine;
using UnityEngine.AI;
public class Door : MonoBehaviour
{
    [SerializeField] Animator doorAnim;
    [SerializeField] AudioSource doorSound;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    [SerializeField] NavMeshObstacle navObstacle;
    private bool isAnimating = false;
    public void DoorOpenClose()
    {
        if (isAnimating) return;

        isAnimating = true;

        if (doorAnim.GetBool("Open"))
        {
            CloseDoor();

        }
        else if (doorAnim.GetBool("Close"))
        {
            OpenDoor();


        }
        isAnimating = true;
        Invoke(nameof(AnimasyonBitti), doorAnim.GetBool("Close") ? 1.20f : 1f);
    }



    private void AnimasyonBitti()
    {
        isAnimating = false;
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {

            OpenDoor();



        }
    }
    void OpenDoor()
    {

        doorSound.PlayOneShot(doorOpen);
        doorAnim.SetBool("Open", true);
        doorAnim.SetBool("Close", false);
        transform.GetChild(0).GetComponent<NavMeshObstacle>().carving = true;



    }
    void CloseDoor()
    {

        doorSound.PlayOneShot(doorClose);
        doorAnim.SetBool("Close", true);
        doorAnim.SetBool("Open", false);
        transform.GetChild(0).GetComponent<NavMeshObstacle>().carving = false;

    }
}