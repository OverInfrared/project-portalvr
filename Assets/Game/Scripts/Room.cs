using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int stencilNumber;

    int frontRenderQueue = 1906;
    int backRenderQueue = 1908;

    private int layerMask;

    private void Awake() {
        int layer = 6;
        layerMask = 1 << layer;
        layerMask = ~layerMask;
    }

    private void Update() {

        if (gameObject.layer == 6) {
            ObjectsBlocked();
        }
        //RecursivePortals();

    }

    //private void RecursivePortals() {
    //    foreach (Transform portals in this.transform) {
    //        if (portals.tag.Equals("Portal")) {
    //            var iworld = transform.root.GetComponent<IWorld>();
    //            if (iworld == null) return;
    //            var conectedRoom = iworld.getRoomFromStencil(stencilNumber);
    //        }
    //    }
    //}

    private void ObjectsBlocked() {
        foreach (Transform child in transform) {
            if (!child.tag.Equals("Portal")) {
                Ray objectRay = new Ray(child.position, Camera.main.transform.position - child.position);
                foreach (RaycastHit rayhit in Physics.RaycastAll(objectRay, 100, layerMask)) {
                    if (rayhit.transform.gameObject.tag.Equals("Portal")) {
                        child.GetComponent<Renderer>().material.renderQueue = backRenderQueue;
                        break;
                    } else {
                        child.GetComponent<Renderer>().material.renderQueue = frontRenderQueue;
                    }
                }
            }
        }
    }
}
