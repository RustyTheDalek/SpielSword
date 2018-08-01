using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( Minion), true)]
class NewEditorScript : Editor
{

    Minion m_Target;
 
    private void OnSceneGUI()
    {
        m_Target = (Minion)target;

        GUIStyle textStyle = new GUIStyle()
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };

        textStyle.normal.background = GUI.skin.GetStyle("box").normal.background;
        textStyle.normal.textColor = Color.black;

        Handles.Label(m_Target.transform.position + Vector3.up * 1.5f, m_Target.state.ToString(), textStyle);
    }
}
