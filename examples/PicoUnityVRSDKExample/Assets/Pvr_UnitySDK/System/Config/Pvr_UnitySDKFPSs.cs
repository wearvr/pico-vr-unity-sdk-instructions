///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKFPSs
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription: The FPS .
///////////////////////////////////////////////////////////////////////////////
using UnityEngine; 
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Pvr_UnitySDKFPSs : MonoBehaviour
{
    public Text fpsText; 
     
    private float fps = 60;
 
    void Start()
    {
        fps = 60; 
    }
 
    void Update()
    {
        if (fpsText != null)
        {
            fpsText.text = "FPS:" + ShowFps();
           // fpsText.text += "---"+ ShowFps1();
        }
    }  

    public string ShowFps()
    {
        float interp = Time.deltaTime / (0.5f + Time.deltaTime);
        if (float.IsNaN(interp) || float.IsInfinity(interp))
        {
            interp = 0;
        }
        float currentFPS = 1.0f / Time.deltaTime;
        if (float.IsNaN(currentFPS) || float.IsInfinity(currentFPS))
        {
            currentFPS = 0;
        }
        if (float.IsNaN(fps) || float.IsInfinity(fps))
        {
            fps = 0;
        }
        fps = Mathf.Lerp(fps, currentFPS, interp);

        return (Mathf.RoundToInt(fps) + "fps");
    }
    int frameno = 0;
    float sum = 0.0f;

      public string ShowFps1()
    {
        frameno++;
        float interp = Time.deltaTime / (0.5f + Time.deltaTime);
        if (float.IsNaN(interp) || float.IsInfinity(interp))
        {
            interp = 0;
        }
        float currentFPS = 1.0f / Time.deltaTime;
        if (float.IsNaN(currentFPS) || float.IsInfinity(currentFPS))
        {
            currentFPS = 0;
        }
        if (float.IsNaN(fps) || float.IsInfinity(fps))
        {
            fps = 0;
        }     
       
		fps = Mathf.Lerp(fps, currentFPS, interp);
        sum += fps;
        fps = sum / (frameno * 1.0f);
		 if (frameno == 10000)
        {            
            frameno = 0;
            sum = 0;
			fps = 0;
        }
        return (Mathf.RoundToInt(fps) + "fps");
    }
}
