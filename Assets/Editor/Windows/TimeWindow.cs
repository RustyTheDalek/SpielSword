using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class TimeWindow : EditorWindow
{
    [MenuItem("Window/TimeWindow")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TimeWindow));
    }

    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            GUILayout.Label("Time: " + TimeObjectManager.t);
            GUILayout.Label("TimeState: " + TimeObjectManager.timeState);
            GUILayout.Label("TimeScale: " + Time.timeScale);
            GUILayout.Label("Start T: " + TimeObjectManager.startT);
            GUILayout.Label("Prev T: " + TimeObjectManager.prevT);
            GUILayout.Label("Delta T: " + TimeObjectManager.DeltaT);

            if (GUILayout.Button("Rewind Time"))
            {
                TimeObjectManager.ReverseTime();
            }
        }
        else
        {
            GUILayout.Label("Waiting for Play-mode");
        }
    }

    private void Update()
    {
        Repaint();
    }
}
