using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tri : MonoBehaviour
{
    public GameObject panel;
    private void OnTriggerEnter(Collider other)
    {
        panel.SetActive(true);

    }
}
