using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour
{

    [SerializeField] private float timeUntilDestroy;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeUntilDestroy);
    }
}
