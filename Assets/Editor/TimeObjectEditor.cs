using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( TimeObject))]
class TimeObjectEditor : Editor
{
    TimeObject m_Target;

    private void OnSceneGUI()
    {
        m_Target = (TimeObject)target;

        GUIStyle textStyle = new GUIStyle()
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };

        textStyle.normal.textColor = Color.white;

        Handles.Label(m_Target.transform.position, 
            "Time State: " + m_Target.tObjectState.ToString() + 
            "\nTotal Frames: " + m_Target.TotalFrames +
            "\nCurrent Frame: " + m_Target.currentFrame +
            "\nStart Frame: " + m_Target.startFrame +
            "\nFinish Frame: " + m_Target.finishFrame, textStyle);

    }
}
