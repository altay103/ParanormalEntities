using System;
using UnityEngine;

public class JumpScareCollider : MonoBehaviour
{
    public bool playeryakalandi = false;
    private void OnTriggerEnter(Collider other)
    {
        playeryakalandi = true;
    }
}
