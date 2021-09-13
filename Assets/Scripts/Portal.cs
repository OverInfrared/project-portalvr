using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Portal : MonoBehaviour
{

    public bool enabled = true;

    [SerializeField] int frontMask;
    [SerializeField] int backMask;
 
    [SerializeField] GameObject frontScreen;
    [SerializeField] GameObject backScreen;

    [SerializeField] Renderer playerRender;

    private GameObject quad0;
    private GameObject quad1;
    private float portalWidth;

    private void Awake() {

        portalWidth = frontScreen.transform.localScale.x;
        quad0 = frontScreen.transform.GetChild(0).gameObject;
        quad1 = backScreen.transform.GetChild(0).gameObject;
        SetupQuads(frontScreen, frontMask);
        SetupQuads(backScreen, backMask);
    }

    //TODO: Hands and object traversal to the portals.
    private void Update() {

        if (enabled) {
            CameraTraverser();
        }

    }

    //The cameras reletive position to the portal will always be inline to rotation, which allows for a simple check of positive to negative when you go through the portal.
    //Two parts, first looks for if the player camera enters the portal, then waits for the player to leave the portal before switching back.
    private bool traversing = false;
    void CameraTraverser() {

        Vector3 cameraRelative = -frontScreen.transform.InverseTransformPoint(Camera.main.transform.position);

        if (cameraRelative.x < portalWidth / 2 && cameraRelative.x > -(portalWidth / 2)) {
            if (!traversing && cameraRelative.z > -.2 && cameraRelative.z < .2) {
                GameObject quad = cameraRelative.z > 0 ? quad0 : quad1;
                quad.SetActive(true);
                traversing = true;
            } else {
                quad0.SetActive(false);
                quad1.SetActive(false);
                traversing = false;
            }
        }

        if (traversing) {
            if (cameraRelative.z < -.1 || cameraRelative.z > .1) {
                quad0.SetActive(false);
                quad1.SetActive(false);
                traversing = false;
                int stencil = cameraRelative.z < 0 ? frontScreen.GetComponent<Renderer>().material.GetInt("_StencilMask") :
                    backScreen.GetComponent<Renderer>().material.GetInt("_StencilMask");

                playerRender.material.SetInt("_StencilMask", stencil);
            }
        }
        
    }

    //Setting the material of the screen and its children is used because of the option later to generate these portals in some kind of room
    //The renderQueue is 1901 because the plane in front of the players face always has to be overwriten. This might cause an issue later.
    void SetupQuads(GameObject screen, int mask) {
        Renderer stencilMask = screen.GetComponent<Renderer>();
        stencilMask.material = new Material(Shader.Find("Stencils/StencilMask"));
        stencilMask.material.SetInt("_StencilMask", mask);
        stencilMask.material.renderQueue = 1901;
        ChangeMaterial(stencilMask.material, screen);
        screen.transform.GetChild(0).gameObject.SetActive(false);
    }

    void ChangeMaterial(Material newMat, GameObject screen) {
        Renderer[] children;
        children = screen.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children) {
            rend.material = newMat;
        }
    }

}
