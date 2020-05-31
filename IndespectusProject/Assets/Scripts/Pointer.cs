using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Pointer : MonoBehaviour {
   
    public float defaultLength = 5f;
    public GameObject dot;
    public VRInputModule vRInput;

    private LineRenderer line;

   private void Awake() {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update() {
        UpdateLine();
    }

    private void UpdateLine() {
        // use default or distance
        PointerEventData data = vRInput.GetData();

        float tarLength = data.pointerCurrentRaycast.distance == 0 ? defaultLength : data.pointerCurrentRaycast.distance;

        // raycast
        RaycastHit hit = CreateRaycast(tarLength);

        // default
        Vector3 endPos = transform.position + (transform.forward * tarLength);

        // or based on hit
        if(hit.collider != null) {
            endPos = hit.point;
        }

        // set position of dot
        dot.transform.position = endPos;

        // set linerenderer
        line.SetPosition(0, transform.position);
        line.SetPosition(1, endPos);


    }

    private RaycastHit CreateRaycast(float length) {
        
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, defaultLength);

        return hit;
    }
}
