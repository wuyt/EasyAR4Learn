using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using easyar;
using VideoRecording;

public class RecorderController : MonoBehaviour
{
    /// <summary>
    /// 视频录像对象
    /// </summary>
    public VideoRecorder videoRecorder;
    /// <summary>
    /// 信息提示文本
    /// </summary>
    public Text uiText;
    /// <summary>
    /// 摄像头录制对象
    /// </summary>
    private CameraRecorder cameraRecorder;

    

    private void Awake()
    {
        //录像状态更新事件
        videoRecorder.StatusUpdate += (status, msg) =>
        {
            if (status == RecordStatus.OnStarted)
            {
                uiText.text = "Recording start";
            }
            if (status == RecordStatus.FailedToStart || status == RecordStatus.FileFailed || status == RecordStatus.LogError)
            {
                uiText.text = "Recording Error: " + status + ", details: " + msg;
            }
            Debug.Log("RecordStatus: " + status + ", details: " + msg);
        };
    }
    /// <summary>
    /// 开始录像
    /// </summary>
    public void StartRecorder()
    {
        videoRecorder.StartRecording();
        cameraRecorder = Camera.main.gameObject.AddComponent<CameraRecorder>();
        cameraRecorder.Setup(videoRecorder, null);
    }
    /// <summary>
    /// 停止录像
    /// </summary>
    public void StopRecorder()
    {
        if (videoRecorder.StopRecording())
        {
            uiText.text = "Recording stop " + videoRecorder.FilePath;
        }
        else
        {
            uiText.text = "Recording failed";
        }
        if (cameraRecorder)
        {
            cameraRecorder.Destroy();
        }
    }
}
