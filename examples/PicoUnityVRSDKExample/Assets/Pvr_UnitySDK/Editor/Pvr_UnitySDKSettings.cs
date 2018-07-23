///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  CO.,Ltd. All Rights Reserved.
// file: Pvr_UnitySDKSettings
// author: AiLi.Shang
// date:  2017/01/11
// discription: Pvr_UnitySDK Settings
// class:   1 Pvr_UnitySDKParameter
//          2 Pvr_UnitySDKQualitySettings
//          3 Pvr_UnitySDKExportPackage
//          4 Pvr_UnitySDKAndriodBuilSetting
//          5 Pvr_UnitySDKLogWindowsWizard
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;  
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Xml.Linq;

public class Pvr_UnitySDKParameter
{
    public const string Pvr_UnitySDKName = "Pvr_UnitySDK";

}

 [InitializeOnLoad]
public class Pvr_UnitySDKQualitySettings
{
  
    public static string Pvr_UnityGetcurrentBuildTarget()
    {
        string returnstring = "Sorry !!  This platform we are NOT support.";
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                returnstring = "Android Platform";
                Pvr_UnitySDKManager.platform = Pvr_UnitySDKAPI.PlatForm.Android;

                break;
            case BuildTarget.StandaloneWindows:
                returnstring = "Win Platform";
                Pvr_UnitySDKManager.platform = Pvr_UnitySDKAPI.PlatForm.Win;
                break;
            case BuildTarget.StandaloneWindows64:
                returnstring = "Win64 Platform";
                Pvr_UnitySDKManager.platform = Pvr_UnitySDKAPI.PlatForm.Win;
                break;
            case BuildTarget.iOS:
                returnstring = "IOS Platform";
                Pvr_UnitySDKManager.platform = Pvr_UnitySDKAPI.PlatForm.IOS;
                break; 
            default:
                Pvr_UnitySDKManager.platform = Pvr_UnitySDKAPI.PlatForm.Notsupport;
                break;
        }
        return returnstring;
    }
    static bool xmlUpdate = false;
   [InitializeOnLoadMethod]
    static void UnitySDKQualitySettings()    
    {
        Pvr_UnityGetcurrentBuildTarget();
        if (!xmlUpdate)
        {

#if UNITY_5
         UpdateXML("0", "Assets/Plugins/Android/AndroidManifest.xml");
#else
         UpdateXML("1", "Assets/Plugins/Android/AndroidManifest.xml");
#endif
         xmlUpdate = true;
        }
        SetvSyncCount();  
    }


    private static void UpdateXML(string Value,string m_sXmlPath)
    {
        XNamespace android = "http://schemas.android.com/apk/res/android";
        if (File.Exists(m_sXmlPath))
        {
         
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(m_sXmlPath);
            XmlNodeList nodeList;
            XmlElement root = xmlDoc.DocumentElement;             
            nodeList = root.SelectNodes("/manifest/application/meta-data");

            foreach (XmlNode node in nodeList)
            {
               
                if (node.Attributes.GetNamedItem("name", android.NamespaceName) != null)   
                {
                    if (node.Attributes.GetNamedItem("name", android.NamespaceName).Value == "platform_high")
                    {
                        if (node.Attributes.GetNamedItem("value", android.NamespaceName).Value == Value)
                        {
                            Debug.Log("无须修改");
                        }
                        else
                        {
                            node.Attributes.GetNamedItem("value", android.NamespaceName).Value = Value;
                            xmlDoc.Save(m_sXmlPath); 
                        }

                        return;
                    }
                } 
            }
            XmlNode applicationNode = xmlDoc.SelectSingleNode("/manifest/application"); 
            XmlElement xmlEle = xmlDoc.CreateElement("meta-data");  
            Debug.Log(android.NamespaceName.ToString());
            xmlEle.SetAttribute( "name", android.NamespaceName,"platform_high");
            xmlEle.SetAttribute( "value", android.NamespaceName, Value);      
            applicationNode.AppendChild(xmlEle);    
            xmlDoc.Save(m_sXmlPath); 
        }
    }
    static void SetvSyncCount()
    {
        QualitySettings.vSyncCount = 0;
        int currentLevel = QualitySettings.GetQualityLevel();
        for (int i = currentLevel; i >= 1; i--)
        {
            QualitySettings.DecreaseLevel(true);
            QualitySettings.vSyncCount = 0;
        }
        QualitySettings.SetQualityLevel(currentLevel, true);
        for (int i = currentLevel; i < 10; i++)
        {
            QualitySettings.IncreaseLevel(true);
            QualitySettings.vSyncCount = 0;
        }

        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new UnityEngine.Rendering.GraphicsDeviceType[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2, UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });

    }
}


[InitializeOnLoad, CanEditMultipleObjects]
public class Pvr_UnitySDKSettings : Editor
{

    [SerializeField]
    public static string[] needAbilities;
    const string MenuItemName = Pvr_UnitySDKParameter.Pvr_UnitySDKName + "/ExportPackage Select";
    [MenuItem(MenuItemName)]
    static void pvr_ExportPackage()
    {  
        ScriptableWizard.DisplayWizard<Pvr_UnitySDKAbilitiesSlecetWindows>("Change ActiveBuildTarget to Android!", "Close","SAVE");  
    }

    public static void Pvr_ExportPackage()
    {
        
        AssetDatabase.ExportPackage(new string[] {
            "Assets/Pvr_UnitySDK",
            "Assets/Pvr_Sensor",
            "Assets/Pvr_Brightness",
            "Assets/Pvr_BLESPP",
            "Assets/Pvr_Audio",
            "Assets/Pvr_Audio3D",
        }, "Pvr_UnitySDK.unitypackage", ExportPackageOptions.Recurse);
        EditorApplication.Exit(0);

        /*
        if (PlayerPrefs.HasKey("Length") )
        {

            int length = PlayerPrefs.GetInt("Length");
            if (length != 0)
            {
                needAbilities = new string[length];
                for (int i = 0; i < length; i++)
                {
                    needAbilities[i] = PlayerPrefs.GetString(i.ToString());
                }
            } 
            AssetDatabase.ExportPackage(needAbilities, "Pvr_UnitySDK.unitypackage", ExportPackageOptions.Recurse);
            EditorApplication.Exit(0);
        }
        else
            EditorApplication.Exit(-1);
            */

    }
}


[InitializeOnLoad]
public class Pvr_UnitySDKBuilSetting : Editor
{
    void Start()
    {
        PlayerSettings.MTRendering = true;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.companyName = Pvr_UnitySDKParameter.Pvr_UnitySDKName;
#if UNITY_5
        PlayerSettings.mobileMTRendering = false;
#else
        //PlayerSettings.SetMobileMTRendering(BuildTargetGroup.Android, false);
#endif
        PlayerSettings.productName = Pvr_UnitySDKParameter.Pvr_UnitySDKName;
        PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Enabled;//Disabled;
        PlayerSettings.defaultIsFullScreen = true;
        EditorUserBuildSettings.activeBuildTargetChanged += OnChangePlatform;
        SetvSyncCount();
    }

    static Pvr_UnitySDKBuilSetting()
    {
        EditorUserBuildSettings.activeBuildTargetChanged += OnChangePlatform;    
    }

    static void OnChangePlatform()
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            PerformAndroidAPKBuild();
            SetvSyncCount();
        }
		if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS) {
			PerformIOSAppBuild();
		}
    }

    const string MenuItemName = Pvr_UnitySDKParameter.Pvr_UnitySDKName + "/Android APK Setting";
    [MenuItem(MenuItemName)]
    static void PerformAndroidAPKBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        PlayerSettings.MTRendering = true;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.companyName = Pvr_UnitySDKParameter.Pvr_UnitySDKName;
#if UNITY_5
        PlayerSettings.mobileMTRendering = false;
#else
       // PlayerSettings.SetMobileMTRendering(BuildTargetGroup.Android, false);
#endif
        PlayerSettings.productName = Pvr_UnitySDKParameter.Pvr_UnitySDKName;
        //PlayerSettings.applicationIdentifier = "com.cn." + Pvr_UnitySDKParameter.Pvr_UnitySDKName;
        QualitySettings.vSyncCount = 0;  
		PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new UnityEngine.Rendering.GraphicsDeviceType[]{ UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2, UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3});

    }
	const string MenuItemNameIOS = Pvr_UnitySDKParameter.Pvr_UnitySDKName + "/IOS App Setting";
	[MenuItem(MenuItemNameIOS)]

	static void PerformIOSAppBuild()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
		PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS,new UnityEngine.Rendering.GraphicsDeviceType[]{ UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2});
		//PlayerSettings.SetApiCompatibilityLevel (BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_2_0);
		PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneOnly;	
		//PlayerSettings.applicationIdentifier = "com.cn.IOSdemo";// + Pvr_UnitySDKParameter.Pvr_UnitySDKName;
		QualitySettings.vSyncCount = 0;  
	}
    static void SetvSyncCount()
    {
        QualitySettings.vSyncCount = 0;
        int currentLevel = QualitySettings.GetQualityLevel();
        for (int i = currentLevel; i >= 1; i--)
        {
            QualitySettings.DecreaseLevel(true);
            QualitySettings.vSyncCount = 0;
        }
        QualitySettings.SetQualityLevel(currentLevel, true);
        for (int i = currentLevel; i < 10; i++)
        {
            QualitySettings.IncreaseLevel(true);
            QualitySettings.vSyncCount = 0;
        }
      }
}

[InitializeOnLoad]
public class Pvr_UnitySDKLogWindowsWizard : ScriptableWizard
{
    void OnWizardUpdate()
    {
        helpString = "Hi Pay Attention to you target platform!!!";
    }
    void OnWizardCreate() { }
}


public class Pvr_UnitySDKAbilitiesSlecetWindows : ScriptableWizard
{
    List<string> Abilitieslist = new List<string>();
    [SerializeField]
    public bool Pvr_Sensor = false;
    [SerializeField]
    public bool Pvr_Brightness = false;
    [SerializeField]
    public bool Pvr_BLESPP = false;
    [SerializeField]
    public bool Pvr_Audio = false;
    [SerializeField]
    public bool Pvr_AM3D = false; 

    void OnWizardCreate() { }
    void OnWizardUpdate()
    {
        //-----1
        if (Pvr_Sensor)
        {
            if (!Abilitieslist.Contains("Assets/Pvr_Sensor"))
            {
                Abilitieslist.Add("Assets/Pvr_Sensor");

            }
        }
        else
        {
            if (Abilitieslist.Contains("Assets/Pvr_Sensor"))
                Abilitieslist.Remove("Assets/Pvr_Sensor");
        }
        //----- 2
        if (Pvr_Brightness)
        {
            if (!Abilitieslist.Contains("Assets/Pvr_Brightness"))
                Abilitieslist.Add("Assets/Pvr_Brightness");
        }
        else
        {
            if (Abilitieslist.Contains("Assets/Pvr_Brightness"))
                Abilitieslist.Remove("Assets/Pvr_Brightness");
        }
        //----- 3
        if (Pvr_BLESPP)
        {
            if (!Abilitieslist.Contains("Assets/Pvr_BLESPP"))
                Abilitieslist.Add("Assets/Pvr_BLESPP");
        }
        else
        {
            if (Abilitieslist.Contains("Assets/Pvr_BLESPP"))
                Abilitieslist.Remove("Assets/Pvr_BLESPP");
        }



        //-----  4
        if (Pvr_Audio)
        {
            if (!Abilitieslist.Contains("Assets/Pvr_Audio"))
                Abilitieslist.Add("Assets/Pvr_Audio");
        }
        else
        {
            if (Abilitieslist.Contains("Assets/Pvr_Audio"))
                Abilitieslist.Remove("Assets/Pvr_Audio");
        }
        //-----  5
        if (Pvr_AM3D)
        {
            if (!Abilitieslist.Contains("Assets/Pvr_Audio3D"))
                Abilitieslist.Add("Assets/Pvr_Audio3D");
        }
        else
        {
            if (Abilitieslist.Contains("Assets/Pvr_Audio3D"))
                Abilitieslist.Remove("Assets/Pvr_Audio3D");
        }
        if (!Abilitieslist.Contains("Assets/Pvr_UnitySDK"))
            Abilitieslist.Add("Assets/Pvr_UnitySDK");
    }
                                
    void OnWizardOtherButton()
    {
        Pvr_UnitySDKSettings.needAbilities = Abilitieslist.ToArray();
        PlayerPrefs.SetInt("Length", Abilitieslist.Count);
        for (int i = 0; i < Abilitieslist.Count; i++)
        {
            PlayerPrefs.SetString(i.ToString(), Pvr_UnitySDKSettings.needAbilities[i]);
        }
    }
                                   
}


