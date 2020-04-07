using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using easyar;
using System;

public class ImageCloudRecognizerController : MonoBehaviour
{
    private Text text;
    private CloudRecognizerFrameFilter cloudRecognizer;


    void Awake()
    {
        text = GameObject.Find("/Canvas/Text").GetComponent<Text>();
        cloudRecognizer = FindObjectOfType<CloudRecognizerFrameFilter>();

        cloudRecognizer.CloudUpdate += (status, targets) =>
        {
            text.text = "Cloud Recognizer status " + status.ToString() + Environment.NewLine + "targets count:" + targets.Count;

            foreach (var t in targets)
            {
                text.text += Environment.NewLine +
                "uid:" + t.uid() + Environment.NewLine +
                "name:" + t.name();
            }

            text.text += Environment.NewLine + Time.time;
        };
    }
}
