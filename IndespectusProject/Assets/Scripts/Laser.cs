using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position));
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider && hit.transform.tag != "Bullet")
            {
                lr.SetPosition(1, transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point));
            }
        }
        else
        {
            lr.SetPosition(1, transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position + transform.forward * 5000));
        }
    }
}
