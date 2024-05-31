using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameConsole : MonoBehaviour
{
    private Dictionary<string, (string value, string color)> debugLogs = new Dictionary<string, (string, string)>();
    public TextMeshProUGUI text;
    private bool logsUpdated = false;

    private void Update()
    {
        if (logsUpdated)
        {
            UpdateUIText();
            logsUpdated = false;
        }
    }

    private void OnEnable()
    {
        Application.logMessageReceived += Handlelog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Handlelog;
    }

    private void Handlelog(string logstring, string stackTrace, LogType type)
    {
        string[] splitString = logstring.Split(new char[] { ':' }, 2);  // 첫 번째 콜론만 분리
        string debugKey = splitString[0].Trim();
        string debugValue = splitString.Length > 1 ? splitString[1].Trim() : "";

        string colorCode = GetColorCode(type);

        if (debugLogs.ContainsKey(debugKey))
        {
            debugLogs[debugKey] = ($"{debugValue}", colorCode);
        }
        else
        {
            debugLogs.Add(debugKey, ($"{debugValue}", colorCode));
        }

        logsUpdated = true;  // 로그가 업데이트되었음을 표시
    }

    private string GetColorCode(LogType type)
    {
        switch (type)
        {
            case LogType.Warning:
                return "yellow";
            case LogType.Error:
            case LogType.Exception:
                return "red";
            case LogType.Log:
            default:
                return "white";
        }
    }

    private void UpdateUIText()
    {
        string displayText = "";
        foreach (KeyValuePair<string, (string value, string color)> log in debugLogs)
        {
            if (log.Value.value == "")
            {
                displayText += $"<color={log.Value.color}>{log.Key}</color>\n";
            }
            else
            {
                displayText += $"<color={log.Value.color}>{log.Key}: {log.Value.value}</color>\n";
            }
        }
        text.text = displayText;
    }
}
