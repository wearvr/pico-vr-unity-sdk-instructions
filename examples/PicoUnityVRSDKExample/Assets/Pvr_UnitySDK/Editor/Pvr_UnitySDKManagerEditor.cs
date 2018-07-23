using UnityEngine;
using UnityEditor;
using Pvr_UnitySDKAPI;

[CustomEditor(typeof(Pvr_UnitySDKManager))]
public class Pvr_UnitySDKManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.changed = false;

        //GUI style 设置
        GUIStyle firstLevelStyle = new GUIStyle(GUI.skin.label);
        firstLevelStyle.alignment = TextAnchor.UpperLeft;
        firstLevelStyle.fontStyle = FontStyle.Bold;
        firstLevelStyle.fontSize = 12;
        firstLevelStyle.wordWrap = true;

        //inspector 所在 target 
        Pvr_UnitySDKManager manager = (Pvr_UnitySDKManager)target;

        //一级分层标题 1
        GUILayout.Space(10);
        EditorGUILayout.LabelField("ConfigFile Setting", firstLevelStyle);
        EditorGUILayout.LabelField("Current Build Target ： " + EditorUserBuildSettings.activeBuildTarget.ToString() + "\n", firstLevelStyle);
        GUILayout.Space(10);

        //一级分层标题 2
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Render Texture Setting", firstLevelStyle);
        GUILayout.Space(10);

        manager.RtAntiAlising = (RenderTextureAntiAliasing)EditorGUILayout.EnumPopup("Render Texture Anti-Aliasing ", manager.RtAntiAlising);
        manager.RtBitDepth = (RenderTextureDepth)EditorGUILayout.EnumPopup("Render Texture Bit Depth   ", manager.RtBitDepth);
        manager.RtFormat = (RenderTextureFormat)EditorGUILayout.EnumPopup("Render Texture Format", manager.RtFormat);

        manager.RtSizeWH = EditorGUILayout.FloatField("Render Texture Size",manager.RtSizeWH);


        //一级分层标题 1
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Show FPS", firstLevelStyle);
        GUILayout.Space(10);
        manager.ShowFPS = EditorGUILayout.Toggle("Show FPS in Scene", manager.ShowFPS);
        GUILayout.Space(10);



        GUILayout.Space(10);
        EditorGUILayout.LabelField("Screen Fade", firstLevelStyle);
        GUILayout.Space(10);
        manager.ScreenFade = EditorGUILayout.Toggle("Screen Fade", manager.ScreenFade);
        GUILayout.Space(10);


        GUILayout.Space(10);
        EditorGUILayout.LabelField("Enable 6 Dof ", firstLevelStyle);
        GUILayout.Space(10);
        manager.Enable6Dof = EditorGUILayout.Toggle("Enable 6 Dof ", manager.Enable6Dof);
        GUILayout.Space(10);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
        }

        // 保存序列化数据，否则会出现设置数据丢失情况
        serializedObject.ApplyModifiedProperties();
    }

}
