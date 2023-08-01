using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolder : MonoBehaviour
{
    [SerializeField] private Transform CamPos;

    private void Update() 
    {
        transform.position = CamPos.position;
    }
}
