using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ArmorHelp
{
    public class ErrorsFinder : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _text;

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            //output = logString;
            //stack = stackTrace;
            if (type == LogType.Exception || type == LogType.Error)
            {
                _text.text += $"{logString} \n";
                _text.text += $"{stackTrace} \n";
            }

        }

    }
}

