using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CharacterRaycast : MonoBehaviour
{
    [SerializeField] private Transform cameraOBJ;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask Interaction;
    [SerializeField] private GameObject InteractionCursor;

    private RaycastHit hit;
    private void Update()
    {
        if (Physics.Raycast(cameraOBJ.position, cameraOBJ.forward, out hit, distance, Interaction))
        {
            InteractionCursor.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.CompareTag("Door"))
                {
                    hit.transform.parent.GetComponent<Door>().DoorOpenClose();
                    Debug.Log("Kapı Acılıd");
                }
            }
        }
        else
        {
            InteractionCursor.SetActive(false);
        }
    }
}
