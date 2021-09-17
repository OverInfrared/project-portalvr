using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQueue : MonoBehaviour
{

    public Renderer floor0;
    public Renderer floor1;
    public Renderer floor2;
    public Renderer floor3;
    public Renderer floor4;
    public Renderer floor5;
    public Renderer floor6;
     
    public Renderer room01;
    public Renderer room02;
    public Renderer room11;
    public Renderer room12;
    public Renderer room13;
    public Renderer room21;
    public Renderer room31;
    public Renderer room32;
    public Renderer room33;
    public Renderer room41;
    public Renderer room51;
    public Renderer room61;

    public Renderer player;

    public int ifloor0;
    public int ifloor1;
    public int ifloor2;
    public int ifloor3;
    public int ifloor4;
    public int ifloor5;
    public int ifloor6;

    public int iroom01;
    public int iroom02;
    public int iroom11;
    public int iroom12;
    public int iroom13;
    public int iroom21;
    public int iroom31;
    public int iroom32;
    public int iroom33;
    public int iroom41;
    public int iroom51;
    public int iroom61;

    public int iplayer;

    private void Update() {
        floor0.material.renderQueue = ifloor0;
        floor1.material.renderQueue = ifloor1;
        floor2.material.renderQueue = ifloor2;
        floor3.material.renderQueue = ifloor3;
        floor4.material.renderQueue = ifloor4;
        floor5.material.renderQueue = ifloor5;
        floor6.material.renderQueue = ifloor6;
              
        room01.material.renderQueue = iroom01;
        room02.material.renderQueue = iroom02;
        room11.material.renderQueue = iroom11;
        room12.material.renderQueue = iroom12;
        room13.material.renderQueue = iroom13;
        room21.material.renderQueue = iroom21;
        room31.material.renderQueue = iroom31;
        room32.material.renderQueue = iroom32;
        room33.material.renderQueue = iroom33;
        room41.material.renderQueue = iroom41;
        room51.material.renderQueue = iroom51;
        room61.material.renderQueue = iroom61;

        player.material.renderQueue = iplayer;
    }

}
