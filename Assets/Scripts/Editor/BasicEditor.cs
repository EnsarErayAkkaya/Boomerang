using RR.Services;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace RR.Editor
{
    public class BasicEditor : EditorWindow
    {
        [MenuItem("Basic/Clear PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Basic/Clear Save Data")]
        public static void ClearSaveData()
        {
            string filePath = Application.persistentDataPath + SaveService.saveFile;

            // check if file exists
            if (!File.Exists(filePath))
            {
                Debug.Log( "no " + filePath + " file exists" );
            }
            else
            {
                Debug.Log(filePath + " file exists, deleting..." );

                File.Delete(filePath);

                RefreshEditorProjectWindow();
            }
        }

        [MenuItem("Basic/Open Save Data")]
        public static void OpenDataFolder()
        {
            string filePath = Application.persistentDataPath + SaveService.saveFile;

            string itemPath = filePath.Replace(@"/", @"\");   // explorer doesn't like front slashes
            System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
        }

        static void RefreshEditorProjectWindow()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}
