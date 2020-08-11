/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule {
    
    public Camera mCamera;
    public SteamVR_Input_Sources targetSource;
    public SteamVR_Action_Boolean clickAction;

    private GameObject currentObject;
    private PointerEventData mData;
    

    protected override void Awake() {
        base.Awake();
        mData = new PointerEventData(eventSystem);
    }

    public override void Process() {
        // reset data, set camera
        mData.Reset();
        mData.position = new Vector2(mCamera.pixelWidth / 2, mCamera.pixelHeight / 2);

        // raycast
        eventSystem.RaycastAll(mData, m_RaycastResultCache);
        mData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = mData.pointerCurrentRaycast.gameObject;
        
        // clear raycast
        m_RaycastResultCache.Clear();

        // hover
        HandlePointerExitAndEnter(mData, currentObject);

        // press
        if(clickAction.GetStateDown(targetSource))
            ProcessPress(mData);

        // release
        if(clickAction.GetStateUp(targetSource))
            ProcessRelease(mData);

    }

    public PointerEventData GetData() {
        return mData;
    }

    private void ProcessPress(PointerEventData data) {
        // raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        // check hit, get handler, call
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, data, ExecuteEvents.pointerDownHandler);

        // if no down, try to get click handler
        if(newPointerPress == null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        // set data
        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = currentObject;
    }

    private void ProcessRelease(PointerEventData data) {
        // execute up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        // check click
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        // check if actual
        if(data.pointerPress == pointerUpHandler) {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        // clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        // reset data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;

    }

}*/
