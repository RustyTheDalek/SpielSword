using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

/// <summary>
/// Script summary
/// </summary>
public class EditorCreator : Editor 
{
	[MenuItem("Assets/Create/Editor Script")]
    static void CreateEditorScript()
    {
        string copyPath = "Assets/Editor/NewEditorScript.cs";
        Debug.Log("Creating new Editor Sript");
        if (!File.Exists(copyPath)) //if File doesn't exist
        {
            using (StreamWriter outFile = new StreamWriter(copyPath))
            {
                outFile.WriteLine("using UnityEngine;");
                outFile.WriteLine("using UnityEditor;");
                outFile.WriteLine("");
                outFile.WriteLine("[CustomEditor( typeof( /*ObjectName*/))]");
                outFile.WriteLine("class NewEditorScript : Editor");
                outFile.WriteLine("{");
                outFile.WriteLine(" //ObjectName myObject");
                outFile.WriteLine(" ");
                outFile.WriteLine(" private void OnSceneGUI()");
                outFile.WriteLine(" {");
                outFile.WriteLine("     ");
                outFile.WriteLine(" }");
                outFile.WriteLine("}");
            }
        }
        else
        {

            Debug.Log("File exists, creating NewEditorScrpt");
        }

        AssetDatabase.Refresh();
    }
}
