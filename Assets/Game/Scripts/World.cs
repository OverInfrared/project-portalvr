using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class World : MonoBehaviour, IWorld
{

    [SerializeField] GameObject playerRenderer;

    //TEMP
    [SerializeField] Transform floor0;
    [SerializeField] Transform floor1;
    [SerializeField] Transform floor2;
    [SerializeField] Transform floor3;
    [SerializeField] Transform floor4;
    [SerializeField] Transform floor5;
    [SerializeField] Transform floor01;


    public void setActiveRoom(int stencil) {
        playerRenderer.GetComponent<Renderer>().material.SetInt("_StencilMask", stencil);

        RemoveRoomLayers();
        UpdateRooms(stencil, 6);
    }

    private void RemoveRoomLayers() {
        foreach (Transform room in transform) {
            ChangeRoomLayer(room, 10);
        }
    }

    private void UpdateRooms(int stencil, int layer) {
        if (layer == 10) return;
        var room = getRoomFromStencil(stencil);
        if (room.gameObject.layer != 10) return;
        ChangeRoomLayer(room.transform, layer);
        layer++;
        foreach (Transform roomObject in room.GetComponent<Transform>()) {
            if (roomObject.tag.Equals("Portal")) {
                TogglePortals(room, roomObject);
                UpdatePortalMaterials(roomObject, layer - 1);
                UpdateRooms(roomObject.GetComponent<Portal>().mask, layer);
            } else {
                UpdateMaterials(roomObject, layer - 1);
            }
        }
    }

    private void UpdatePortalMaterials(Transform roomObject, int layer) {
        if (layer != 6) {
            var renderer = roomObject.GetChild(0).GetComponent<Renderer>();
            if (renderer == null) return;
            //renderer.material.renderQueue = (1920 - layer) - (layer - 6) + 1;
        }
    }

    private void UpdateMaterials(Transform roomObject, int layer) {
        if (layer != 6) {
            var renderer = roomObject.GetComponent<Renderer>();
            if (renderer == null) return;
            //renderer.material.renderQueue = (1920 - layer) - (layer - 6);
            //Debug.Log((layer - 6) + (1901 + layer));
        }
    }

    private static void TogglePortals(Room room, Transform roomObject) {
        var portal = roomObject.GetComponent<Portal>();
        if (portal == null) return;
        if (room.gameObject.layer == 6) {
            roomObject.GetComponent<Portal>().portalEnabled = true;
        } else {
            roomObject.GetComponent<Portal>().portalEnabled = false;
        }
    }

    private void ChangeRoomLayer(Transform room, int layer) {
        foreach (Transform child in room.GetComponentsInChildren<Transform>()) {
            child.gameObject.layer = layer;
            PortalRenderers(child, layer);
        }
    }

    void PortalRenderers(Transform child, int layer) {
        Portal portal = child.GetComponent<Portal>();
        if (portal == null) return;
        portal.transform.GetChild(0).GetComponent<Renderer>().enabled = layer == 10 ? false : true;
    }

    private void Awake() {

        RemoveRoomLayers();
        UpdateRooms(1, 6);


        //TEMP
        var playArea = new HmdQuad_t();
        var chaperone = OpenVR.Chaperone;
        bool success = (chaperone != null) && chaperone.GetPlayAreaRect(ref playArea);

        if (success) {
            Vector3 newScale = new Vector3(Mathf.Abs(playArea.vCorners0.v0 - playArea.vCorners2.v0), Mathf.Abs(playArea.vCorners0.v2 - playArea.vCorners2.v2), this.transform.localScale.y);

            floor0.localScale = newScale;
            floor1.localScale = newScale;
            floor2.localScale = newScale;
            floor3.localScale = newScale;
            floor4.localScale = newScale;
            floor5.localScale = newScale;
            floor01.localScale = newScale;
        }
    }

    //If you are reading this code and have a better way to look for a object with a specific number on it that would be great.
    //This dependence I'm putting on for loops is giving me a headache and makes me discusted to my own code.
    public Room getRoomFromStencil(int stencil) {
        foreach (Transform rooms in transform) {
            var room = rooms.GetComponent<Room>();
            if (room == null) continue;
            if (room.stencilNumber == stencil) return room;
        }
        return null;
    }

}
