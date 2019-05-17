using UnityEngine;
using UnityEditor;
using Pvr_UnitySDKAPI;
[CustomEditor(typeof(Pvr_UnitySDKManager))]
public class Pvr_UnitySDKManagerEditor : Editor
{

    public delegate void HeadDofChanged(string dof);
    public static event HeadDofChanged HeadDofChangedEvent;

    static int QulityRtMass = 0; 
    public delegate void Change(int Msaa);
    public static event Change MSAAChange;


    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        
        GUIStyle firstLevelStyle = new GUIStyle(GUI.skin.label);
        firstLevelStyle.alignment = TextAnchor.UpperLeft;
        firstLevelStyle.fontStyle = FontStyle.Bold;
        firstLevelStyle.fontSize = 12;
        firstLevelStyle.wordWrap = true;

        Pvr_UnitySDKManager manager = (Pvr_UnitySDKManager)target;
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("ConfigFile Setting", firstLevelStyle);
        EditorGUILayout.LabelField("Current Build Target ： " + EditorUserBuildSettings.activeBuildTarget.ToString() + "\n", firstLevelStyle);
        GUILayout.Space(10);
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Render Texture Setting", firstLevelStyle);
        GUILayout.Space(10);

        manager.RtAntiAlising = (RenderTextureAntiAliasing)EditorGUILayout.EnumPopup("Render Texture Anti-Aliasing ", manager.RtAntiAlising);
        manager.RtBitDepth = (RenderTextureDepth)EditorGUILayout.EnumPopup("Render Texture Bit Depth   ", manager.RtBitDepth);
        manager.RtFormat = (RenderTextureFormat)EditorGUILayout.EnumPopup("Render Texture Format", manager.RtFormat);
        
        //manager.RtSizeWH = EditorGUILayout.FloatField("Render Texture Size",manager.RtSizeWH);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Render Texture Level", firstLevelStyle);
        GUILayout.Space(10);
        manager.DefaultRenderTexture = EditorGUILayout.Toggle("Use Default Render Texture", manager.DefaultRenderTexture);
        GUILayout.Space(10);

        if (!manager.DefaultRenderTexture)
        {
            GUILayout.Space(10);
            manager.RtLevel = (RenderTextureLevel)EditorGUILayout.EnumPopup("Render Texture Level", manager.RtLevel);
            GUILayout.Space(10);
        }

        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Show FPS", firstLevelStyle);
        GUILayout.Space(10);
        manager.ShowFPS = EditorGUILayout.Toggle("Show FPS in Scene", manager.ShowFPS);
        GUILayout.Space(10);
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Show SafePanel", firstLevelStyle);
        GUILayout.Space(10);
        manager.ShowSafePanel = EditorGUILayout.Toggle("Show SafePanel", manager.ShowSafePanel);
        GUILayout.Space(10);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Screen Fade", firstLevelStyle);
        GUILayout.Space(10);
        manager.ScreenFade = EditorGUILayout.Toggle("Screen Fade", manager.ScreenFade);
        GUILayout.Space(10);


        GUILayout.Space(10);
        EditorGUILayout.LabelField("Head Pose", firstLevelStyle);
        manager.HeadDofNum = (HeadDofNum) EditorGUILayout.EnumPopup(manager.HeadDofNum);
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Hand Pose", firstLevelStyle);
        manager.HandDofNum = (HandDofNum)EditorGUILayout.EnumPopup(manager.HandDofNum);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("6Dof Position Reset", firstLevelStyle);
        GUILayout.Space(10);
        manager.SixDofRecenter = EditorGUILayout.Toggle("Enable 6Dof Position Reset", manager.SixDofRecenter);
        GUILayout.Space(10);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Use Default Range", firstLevelStyle);
        GUILayout.Space(10);
        manager.DefaultRange = EditorGUILayout.Toggle("Use Default Range", manager.DefaultRange);
        GUILayout.Space(10);

        if (!manager.DefaultRange)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Custom Range", firstLevelStyle);
            GUILayout.Space(10);
            manager.CustomRange = EditorGUILayout.FloatField("Range", manager.CustomRange);
            GUILayout.Space(10);
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Moving Ratios", firstLevelStyle);
        GUILayout.Space(10);
        manager.MovingRatios = EditorGUILayout.FloatField("Ratios", manager.MovingRatios);
        GUILayout.Space(10);

        if (GUI.changed)
        {
            QulityRtMass = (int)Pvr_UnitySDKManager.SDK.RtAntiAlising;
            if (QulityRtMass == 1)
            {
                QulityRtMass = 0;
            }
            if (MSAAChange != null)
            {
                MSAAChange(QulityRtMass);
            }
            var headDof = (int) manager.HeadDofNum;
            if (HeadDofChangedEvent != null)
            {
                if (headDof == 0)
                {
                    HeadDofChangedEvent("3dof");
                }
                else
                {
                    HeadDofChangedEvent("6dof");
                }
                
            }
            EditorUtility.SetDirty(manager);
#if !UNITY_5_2
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager
                .GetActiveScene());
#endif
        }

        serializedObject.ApplyModifiedProperties();
    }

}
