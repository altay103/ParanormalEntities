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
            doorSound.PlayOneShot(doorClose);
            doorAnim.SetBool("Close", true);
            doorAnim.SetBool("Open", false);
            
            Invoke(nameof(EnableObstacle), 1.20f);
        }
        else if (doorAnim.GetBool("Close"))
        {
            doorSound.PlayOneShot(doorOpen);
            doorAnim.SetBool("Open", true);
            doorAnim.SetBool("Close", false);
            
            navObstacle.carving = false;
        }
        isAnimating = true;
        Invoke(nameof(AnimasyonBitti), doorAnim.GetBool("Close") ? 1.20f : 1f);
    }
    
    private void EnableObstacle()
    {
        navObstacle.carving = true;
    }
    
    private void AnimasyonBitti()
    {
        isAnimating = false;
    }
}