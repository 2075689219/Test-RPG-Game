using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCame : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        transform.LookAt(transform.position + target.forward);
    }
}
