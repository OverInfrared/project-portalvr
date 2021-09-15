using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public bool portalEnabled = true;

    [SerializeField] int mask;

    //Deprecated
    //[SerializeField] Renderer playerRender;

    private GameObject screen;
    private GameObject quads;

    public float portalWidth;
    public float portalHeight;

    public int renderQueue = 1900;

    private void Awake() {
        screen = transform.GetChild(0).gameObject;
        quads = screen.transform.GetChild(0).gameObject;
        SetupQuads(screen, mask);

        SetupScreens();
    }

    private void SetupScreens() {
        transform.localScale = new Vector3(portalWidth, portalHeight, 1);
        screen.transform.position = new Vector3(screen.transform.position.x, portalHeight / 2, screen.transform.position.z);
    }

    //TODO: Hands and object traversal to the portals.
    private void Update() {

        if (portalEnabled) {
            CameraTraverser();
        }

    }

    //The cameras reletive position to the portal will always be inline to rotation, which allows for a simple check of positive to negative when you go through the portal.
    //Two parts, first looks for if the player camera enters the portal, then waits for the player to leave the portal before switching back.
    private bool traversing = false;
    void CameraTraverser() {

        Vector3 cameraRelative = -screen.transform.InverseTransformPoint(Camera.main.transform.position);

        if ((cameraRelative.x < portalWidth / 2 && cameraRelative.x > -(portalWidth / 2)) && (cameraRelative.z > -.2 && cameraRelative.z < .2)) {
            if (!traversing) {
                quads.SetActive(true);
            }
            traversing = true;
        } else {
            quads.SetActive(false);
            traversing = false;
        }

        if (traversing) {
            if (cameraRelative.z < -.1) {
                quads.SetActive(false);
                traversing = false;
                int stencil = screen.GetComponent<Renderer>().material.GetInt("_StencilMask");

                var iworld = transform.root.GetComponent<IWorld>();
                if (iworld == null) return;
                iworld.setActiveRoom(stencil);
            }
        }

    }

    //Setting the material of the screen and its children is used because of the option later to generate these portals in some kind of room
    //The renderQueue is 1901 because the plane in front of the players face always has to be overwriten. This might cause an issue later.
    void SetupQuads(GameObject screen, int mask) {
        Renderer stencilMask = screen.GetComponent<Renderer>();
        stencilMask.material = new Material(Shader.Find("Stencils/StencilMask"));
        stencilMask.material.SetInt("_StencilMask", mask);
        stencilMask.material.renderQueue = renderQueue;
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
