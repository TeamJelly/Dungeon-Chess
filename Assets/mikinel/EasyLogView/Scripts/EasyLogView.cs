using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace mikinel.easylogview
{
    public class EasyLogView : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private bool isShowMillisecond = true;
        [SerializeField] private bool isAutoScroll = true;
        [SerializeField] private int maxLines = 30;

        private void Awake(){
            if(text == null)
            {
                Debug.LogError($"Please attach TextMeshProUGUI");
                this.enabled = false;
                return;
            }

            if(scrollRect == null)
            {
                Debug.LogError($"Please attach ScrollView");
                this.enabled = false;
                return;
            }

            text.text = String.Empty;

            Application.logMessageReceived += OnLogMessage;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessage;
        }

        private void OnLogMessage(string logText, string stackTrace, LogType type)
        {
            if (text == null)
                return;

            var tmp = text.text;
            TrimLine(ref tmp, maxLines);
            text.text = tmp;

            var time = DateTime.Now;
            var ms = time.Millisecond.ToString().PadLeft(3, '0');

            text.text += isShowMillisecond ?
                $"[ {time.ToLongTimeString()}.{ms} ] " :
                $"[ {time.ToLongTimeString()} ] ";
             
            switch (type){
                case LogType.Warning:
                    text.text += $"<color=#ffff00> {logText} </color> \n";
                    break;
                case LogType.Error:
                    text.text += $"<color=#ff0000> {logText} </color> \n";
                    break;
                default:
                    text.text += $"{logText} \n";
                    break;
            }

            if (isAutoScroll)
            {
                scrollRect.verticalNormalizedPosition = 0;
                scrollRect.horizontalNormalizedPosition = 0;
            }
        }

        public void ClearLog(){
            var time = DateTime.Now;
            var ms = time.Millisecond.ToString().PadLeft(3, '0');

            text.text = isShowMillisecond ?
                $"[ {time.ToLongTimeString()}.{ms} ] Clear Log \n" :
                $"[ {time.ToLongTimeString()} ] Clear Log \n";
        }

        private int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }

        private void TrimLine(ref string s, int maxLine)
        {
            if (CountChar(s, '\n') >= maxLine)
                s = RemoveFirstLine(s);

            if (CountChar(s, '\n') >= maxLine)
                TrimLine(ref s, maxLine);
        }

        private string RemoveFirstLine(string s)
        {
            var pos = s.IndexOf('\n');
            return s.Substring(pos + 1, s.Length - pos - 1);
        }
    }
}