using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int stencilNumber;

    [SerializeField] int frontRenderQueue = 1850;
    [SerializeField] int backRenderQueue = 1950;

    private int layerMask;
    Transform[] roomObjects;

    private void Awake() {
        int layer = 1;
        layerMask = 1 << layer;
        layerMask = ~layerMask;

        roomObjects = GetComponentsInChildren<Transform>();
    }

    private void Update() {

        ObjectsInRoom();
        ObjectsBlocked();

    }

    private void ObjectsBlocked() {

        foreach (Transform child in roomObjects) {
            if (child.gameObject != this.gameObject && !child.tag.Equals("Portal")) {
                Ray objectRay = new Ray(child.position, Camera.main.transform.position - child.position);
                foreach (RaycastHit rayhit in Physics.RaycastAll(objectRay, 10, layerMask)) {
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

    private void ObjectsInRoom() {
        roomObjects = GetComponentsInChildren<Transform>();
    }
}
