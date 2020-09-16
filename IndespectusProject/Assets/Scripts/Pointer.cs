using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Pointer : MonoBehaviour {
   
    public float defaultLength = 5f;

    public EventSystem eventSystem;
    public StandaloneInputModule inputModule;

    private LineRenderer line;

    private void Awake() {
        line = GetComponent<LineRenderer>();
        GameObject es = GameObject.FindGameObjectWithTag("EventSystem");
        eventSystem = es.GetComponent<EventSystem>();
        inputModule = es.GetComponent<StandaloneInputModule>();
    }

    // Update is called once per frame
    private void Update() {
        UpdateLine();
    }

    private void UpdateLine() {
        // use default or distance
       line.SetPosition(0, transform.position);
       line.SetPosition(1, GetEnd());

    }

    private Vector3 GetEnd() {
        float distance = GetCanvasDistance();
        Vector3 endPosition = CalculateEnd(defaultLength);

        if(distance != 0) {
            endPosition = CalculateEnd(distance);
        }

        return endPosition;
    }

    private float GetCanvasDistance() {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = inputModule.inputOverride.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        RaycastResult closestResult = FindFirstRaycast(results);
        float distance = closestResult.distance;

        distance = Mathf.Clamp(distance, 0, defaultLength);

        return distance;
    }

    private RaycastResult FindFirstRaycast(List<RaycastResult> results) {
        foreach(RaycastResult result in results) {
            if(!result.gameObject) {
                continue;
            }
            return result;
        }
        return new RaycastResult();
    }

    private Vector3 CalculateEnd(float length) {
        return transform.position  + (transform.forward * length);
    }


    private RaycastHit CreateRaycast() {
        
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, defaultLength);

        return hit;
    }
}
