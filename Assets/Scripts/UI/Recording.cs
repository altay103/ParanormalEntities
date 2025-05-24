using System;
using System.Collections;
using UnityEngine;

public class Recording : MonoBehaviour
{
    [SerializeField] GameObject recui;
    private Coroutine recRoutine;

    private void Update()
    {
        if (recRoutine == null)
        {
            recRoutine = StartCoroutine(Rec());
        }
    }

    IEnumerator Rec()
    {
        while (true)
        {
            recui.SetActive(true);
            yield return new WaitForSeconds(2f);
            recui.SetActive(false);
            yield return new WaitForSeconds(2f);
        }
    }
}