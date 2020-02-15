using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class VRDebug : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private UnityEngine.UI.Text m_Text;

    private StringBuilder m_StringBuilder = new StringBuilder();

    // Use this for initialization
    private void Start()
    {
        Application.logMessageReceived += OnUnityLog;
    }

    private void OnUnityLog(string condition, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
            case LogType.Warning:
            case LogType.Log:
            case LogType.Assert:
                m_StringBuilder.AppendLine(condition);
                m_StringBuilder.AppendLine(stackTrace);
                m_Text.text = m_StringBuilder.ToString();
                break;
        }
    }
}
