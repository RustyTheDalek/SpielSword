using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( SpriteRenderer))]
class DiscRotatorEditor : Editor
{

    SpriteRenderer m_Target;
    int m_DiscRotateControlId;
 
     private void OnSceneGUI()
     {
        if(m_DiscRotateControlId == -1)
        {
            m_DiscRotateControlId = GUIUtility.GetControlID(this.GetHashCode(), FocusType.Passive);
        }

        m_Target = (SpriteRenderer)target;

        m_Target.transform.rotation = DiscRotatorHandle.Do(m_DiscRotateControlId, m_Target.transform.position, m_Target.transform.rotation, 4f, 0f);
     }
}
