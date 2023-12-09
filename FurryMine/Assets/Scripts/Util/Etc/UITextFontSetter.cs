using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITextFontSetter : MonoBehaviour
{
    public const string PATH_FONT_TEXTMESHPRO = "Assets/Fonts/SDF/PFStardust SDF.asset";

    [MenuItem("CustomMenu/ChangeUITextMeshPro")]
    public static void ChangeFontInTextMeshPro()
    {
        GameObject[] rootObj = GetSceneRootObjects();
        for(int i = 0; i < rootObj.Length; i++)
        {
            GameObject gbj = (GameObject)rootObj[i] as GameObject;
            Component[] com = gbj.transform.GetComponentsInChildren(typeof(TextMeshProUGUI), true);
            foreach(TextMeshProUGUI txt in com)
            {
                txt.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(PATH_FONT_TEXTMESHPRO);
            }
        }
    }

    private static GameObject[] GetSceneRootObjects()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene.GetRootGameObjects();
    }
}
