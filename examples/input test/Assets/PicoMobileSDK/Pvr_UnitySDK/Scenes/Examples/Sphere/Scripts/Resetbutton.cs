using UnityEngine;
using System.Collections;

public class Resetbutton : MonoBehaviour {

    public void DemoResetTracking()
    {
        if (Pvr_UnitySDKManager.SDK != null)
        {
            if (Pvr_UnitySDKManager.pvr_UnitySDKSensor != null)
            {
                Pvr_UnitySDKManager.pvr_UnitySDKSensor.ResetUnitySDKSensor();
            }
            else
            {
                Pvr_UnitySDKManager.SDK.pvr_UnitySDKEditor.ResetUnitySDKSensor();
            }
            
        }

    }   

}
