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
            if (GameManager.gManager.Hats != null)
            {
                GUIStyle boxStyle = new GUIStyle("box");

                GUILayout.BeginArea(new Rect(10, 10, 200, 20 + 20 * GameManager.gManager.Hats.Count), boxStyle);
                {
                    GUILayout.Label("Unlock Hats");

                    if (GUILayout.Button(keepUnlocks == true ? "Save Unlocks" : "Don't Save Unlock"))
                    {
                        keepUnlocks = !keepUnlocks;
                    }

                    foreach (KeyValuePair<string, Hat> hat in GameManager.gManager.Hats)
                    {
                        if (GUILayout.Button(hat.Key))
                        {
                            GameManager.gManager.UnlockHat(hat.Key, keepUnlocks);
                        }
                    }
                }
            }
                GUILayout.EndArea();
        }
        Handles.EndGUI();
    }
}
