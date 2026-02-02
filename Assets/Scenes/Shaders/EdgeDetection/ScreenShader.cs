using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShader : MonoBehaviour
{
    [SerializeField] private Material m;
    
    void Start()
    {
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //if(Input.GetKey(KeyCode.LeftShift))
            Graphics.Blit(src, dest, m);
        //else
        //{
            //Graphics.Blit(src, dest);
        //}
    }
}
