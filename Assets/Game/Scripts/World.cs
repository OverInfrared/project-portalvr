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
        //int pRoomStencil = playerRenderer.GetComponent<Renderer>().material.GetInt("_StencilMask");
        playerRenderer.GetComponent<Renderer>().material.SetInt("_StencilMask", stencil);

        RemoveRoomLayers();
        UpdateRoomLayers(stencil, 6);

        //foreach (Transform trans in transform) {
        //    var room = trans.GetComponent<Room>();
        //    if (room == null) continue;
        //    //Debug.Log("Room: " + room.stencilNumber + " Stencil: " + stencil) ;
        //    if (room.stencilNumber == stencil) {
        //        ChangeLayer(trans, 6);
        //    } else {
        //        ChangeLayer(trans, 7);
        //    }
        //}

    }

    private void RemoveRoomLayers() {
        foreach (Transform room in transform) {
            ChangeRoomLayer(room, 10);
        }
    }

    private void UpdateRoomLayers(int stencil, int layer) {
        if (layer == 10) return;
        var room = getRoomFromStencil(stencil);
        if (room.gameObject.layer != 10) return;
        ChangeRoomLayer(room.transform, layer);
        layer++;
        foreach (Transform roomObject in room.GetComponent<Transform>()) {
            if (roomObject.tag.Equals("Portal")) {
                UpdateRoomLayers(roomObject.GetComponent<Portal>().mask, layer);
            }
        }
    }

    private void ChangeRoomLayer(Transform room, int layer) {
        foreach (Transform childs in room.GetComponentsInChildren<Transform>()) {
            childs.gameObject.layer = layer;
        }
    }

    private void Awake() {

        //TEMP
        RemoveRoomLayers();
        UpdateRoomLayers(1, 6);

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
