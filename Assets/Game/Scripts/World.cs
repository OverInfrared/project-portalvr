using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class World : MonoBehaviour
{

    //TEMP
    [SerializeField] Transform floor;
    [SerializeField] Transform floor1;
    [SerializeField] Transform floor2;
    

    private void Awake() {
        var playArea = new HmdQuad_t();
        var chaperone = OpenVR.Chaperone;
        bool success = (chaperone != null) && chaperone.GetPlayAreaRect(ref playArea);

        if (success) {
            Vector3 newScale = new Vector3(Mathf.Abs(playArea.vCorners0.v0 - playArea.vCorners2.v0), Mathf.Abs(playArea.vCorners0.v2 - playArea.vCorners2.v2), this.transform.localScale.y);

            floor.localScale = newScale;
            floor1.localScale = newScale;
            floor2.localScale = newScale;
        }
    }

}
