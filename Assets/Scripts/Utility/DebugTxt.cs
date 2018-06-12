#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helps disable and Debug text
/// Created by : Ian Jones - 13/04/18
/// </summary>
public class DebugTxt : MonoBehaviour
{

    Text classTxt;

    // Use this for initialization
    void Start()
    {
        classTxt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        classTxt.enabled = DebugPnl.debugText;
    }
}
#endif