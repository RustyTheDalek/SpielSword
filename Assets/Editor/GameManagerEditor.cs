using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor( typeof( GameManager))]
class GameManagerEditor : Editor
{
    GameManager m_target;

    bool keepUnlocks = false;

    private void OnSceneGUI()
    {
        m_target = (GameManager)target;

        Handles.BeginGUI();
        {
            if (GameManager.gManager.hats != null)
            {
                GUIStyle boxStyle = new GUIStyle("box");

                string[] hatNames = GameManager.gManager.hats.GetAllAssetNames();

                GUILayout.BeginArea(new Rect(10, 10, 200, 20 + 20 * hatNames.Length), boxStyle);
                {
                    GUILayout.Label("Unlock Hats");

                    if (GUILayout.Button(keepUnlocks == true ? "Save Unlocks" : "Don't Save Unlock"))
                    {
                        keepUnlocks = !keepUnlocks;
                    }

                    foreach (string hat in hatNames)
                    {
                        if (GUILayout.Button(GameManager.gManager.hats.LoadAsset<Hat>(hat)._Name))
                        {
                            GameManager.gManager.UnlockHat(hat, keepUnlocks);
                        }
                    }
                }
            }
                GUILayout.EndArea();
        }
        Handles.EndGUI();
    }
}
