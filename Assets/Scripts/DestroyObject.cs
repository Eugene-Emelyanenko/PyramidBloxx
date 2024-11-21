using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float timeToDestroy = 0.5f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
