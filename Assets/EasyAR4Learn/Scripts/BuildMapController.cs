using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using easyar;
using System;

public class BuildMapController : MonoBehaviour
{
    //稀疏空间地图相关对象
    private ARSession session;
    private SparseSpatialMapWorkerFrameFilter mapWorker;
    private SparseSpatialMapController map;
    /// <summary>
    /// 保存按钮
    /// </summary>
    private Button btnSave;
    /// <summary>
    /// 显示文本
    /// </summary>
    private Text text;

    void Start()
    {
        //稀疏空间地图初始
        session = FindObjectOfType<ARSession>();
        mapWorker = FindObjectOfType<SparseSpatialMapWorkerFrameFilter>();
        map = FindObjectOfType<SparseSpatialMapController>();
        //注册追踪状态变化事件
        session.WorldRootController.TrackingStatusChanged += OnTrackingStatusChanged;
        //初始化保存按钮
        btnSave = GameObject.Find("/Canvas/Button").GetComponent<Button>();
        btnSave.onClick.AddListener(Save);
        btnSave.interactable = false;
        if (session.WorldRootController.TrackingStatus == MotionTrackingStatus.Tracking)
        {
            btnSave.interactable = true;
        }
        else
        {
            btnSave.interactable = false;
        }
        //初始化显示文本
        text = GameObject.Find("/Canvas/Panel/Text").GetComponent<Text>();
    }

    /// <summary>
    /// 保存地图方法
    /// </summary>
    private void Save()
    {
        btnSave.interactable = false;
        //注册地图保存结果反馈事件
        mapWorker.BuilderMapController.MapHost += SaveMapHostBack;
        //保存地图
        try
        {
            //保存地图
            mapWorker.BuilderMapController.Host("LearnMap" + DateTime.Now.ToString("yyyyMMddHHmm"), null);
            text.text = "开始保存地图，请稍等。";
        }
        catch (Exception ex)
        {
            btnSave.interactable = true;
            text.text = "保存出错：" + ex.Message;
        }
    }

    /// <summary>
    /// 保存地图反馈
    /// </summary>
    /// <param name="mapInfo">地图信息</param>
    /// <param name="isSuccess">成功标识</param>
    /// <param name="error">错误信息</param>
    private void SaveMapHostBack(SparseSpatialMapController.SparseSpatialMapInfo mapInfo, bool isSuccess, string error)
    {
        if (isSuccess)
        {
            PlayerPrefs.SetString("MapID", mapInfo.ID);
            PlayerPrefs.SetString("MapName", mapInfo.Name);
            text.text = "地图保存成功。\r\nMapID：" + mapInfo.ID + "\r\nMapName：" + mapInfo.Name;
        }
        else
        {
            btnSave.interactable = true;
            text.text = "地图保存出错：" + error;
        }
    }

    /// <summary>
    /// 摄像机状态变化
    /// </summary>
    /// <param name="status">状态</param>
    private void OnTrackingStatusChanged(MotionTrackingStatus status)
    {
        if (status == MotionTrackingStatus.Tracking)
        {
            btnSave.interactable = true;
            text.text = "进入跟踪状态。";
        }
        else
        {
            btnSave.interactable = false;
            text.text = "退出跟踪状态。" + status.ToString();
        }
    }
}
