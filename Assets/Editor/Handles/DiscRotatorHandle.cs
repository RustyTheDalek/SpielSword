using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DiscRotatorHandle
{
    private static Vector2 s_StartMousePosition, s_CurrentMousePosition;
    static float s_RotationDist;
    static Quaternion s_StartRotation;
    static Vector3 s_StartPositionDisc;

	public static Quaternion Do(int id, Vector3 position, Quaternion rotation, float size, float snap)
    {
        Quaternion discRotation = rotation * Quaternion.Euler(0, 0, 0);

        Event evt = Event.current;
        switch (evt.GetTypeForControl(id))
        {
            case EventType.Layout:
                Handles.CircleHandleCap(id, position, discRotation, size, EventType.Layout);
                break;

            case EventType.MouseDown:
                //Am I closest to thingy
                if ((HandleUtility.nearestControl == id && evt.button == 0) && GUIUtility.hotControl == 0)
                {
                    GUIUtility.hotControl = id;
                    s_CurrentMousePosition = s_StartMousePosition = evt.mousePosition;
                    s_StartRotation = rotation;
                    s_StartPositionDisc = HandleUtility.ClosestPointToDisc(position, Vector3.forward, size) - position;
                    evt.Use();
                    EditorGUIUtility.SetWantsMouseJumping(1);
                }
                break;

            case EventType.MouseDrag:

                if (GUIUtility.hotControl == id)
                {
                    s_CurrentMousePosition += evt.delta;

                    s_RotationDist = s_CurrentMousePosition.x - s_StartMousePosition.x;

                    rotation = Quaternion.AngleAxis(s_RotationDist * -1, Vector3.forward) * s_StartRotation;
                    GUI.changed = true;
                    evt.Use();

                }
                break;

            case EventType.MouseUp:

                if (GUIUtility.hotControl == id && (evt.button == 0 || evt.button == 2))
                {
                    GUIUtility.hotControl = 0;
                    evt.Use();
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    s_RotationDist = 0;
                }
                break;

            case EventType.MouseMove:

                if (id == HandleUtility.nearestControl)
                    HandleUtility.Repaint();
                break;

            case EventType.Repaint:

                Color temp = Color.white;

                if (id == GUIUtility.hotControl)
                {
                    temp = Handles.color;

                    Handles.color = Handles.selectedColor * new Color(1f, 1f, 1f, .3f);
                    Handles.DrawSolidArc(position, Vector3.forward, s_StartPositionDisc, s_RotationDist * -1, size);
                    Handles.color = Handles.selectedColor;
                }
                else if (id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
                {
                    temp = Handles.color;
                    Handles.color = Handles.preselectionColor;
                }

                Handles.CircleHandleCap(id, position, discRotation, size, EventType.Repaint);

                if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
                    Handles.color = temp;

                break;
        }

        return rotation;
    }
}
