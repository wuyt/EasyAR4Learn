using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using easyar;


public class LocalizeMapController : MonoBehaviour
{
    //稀疏空间地图相关对象
    private ARSession session;
    private SparseSpatialMapWorkerFrameFilter mapWorker;
    private SparseSpatialMapController map;
    /// <summary>
    ///地图 ID输入框
    /// </summary>
    public InputField inputID;
    /// <summary>
    /// 地图名称输入框
    /// </summary>
    public InputField inputName;
    /// <summary>
    /// 文本显示
    /// </summary>
    public Text text;


    void Start()
    {
        //稀疏空间地图初始
        session = FindObjectOfType<ARSession>();
        mapWorker = FindObjectOfType<SparseSpatialMapWorkerFrameFilter>();
        map = FindObjectOfType<SparseSpatialMapController>();

        //如果之前有建立过地图且文本框没有预设值
        if (inputID.text.Length <= 0)
        {
            inputID.text = PlayerPrefs.GetString("MapID", "");
            inputName.text = PlayerPrefs.GetString("MapName", "");
        }

        map.MapLoad += MapLoadBack; //注册地图加载事件
        map.MapLocalized += LocalizedMap;   //注册定位成功事件
        map.MapStopLocalize += StopLocalizeMap; //注册停止定位事件

        StartLocalization();
    }

    /// <summary>
    /// 地图加载反馈
    /// </summary>
    /// <param name="mapInfo">地图信息</param>
    /// <param name="isSuccess">是否成功</param>
    /// <param name="error">错误信息</param>
    private void MapLoadBack(SparseSpatialMapController.SparseSpatialMapInfo mapInfo, bool isSuccess, string error)
    {
        if (isSuccess)
        {
            text.text = "地图" + mapInfo.Name + "加载成功。";
        }
        else
        {
            text.text = "地图加载失败。" + error;
        }
    }
    /// <summary>
    /// 地图定位成功
    /// </summary>
    private void LocalizedMap()
    {
        text.text = "稀疏空间地图定位成功。" + DateTime.Now.ToShortTimeString();
    }
    /// <summary>
    /// 停止地图定位
    /// </summary>
    private void StopLocalizeMap()
    {
        text.text = "稀疏空间地图停止定位。" + DateTime.Now.ToShortTimeString();
    }
    /// <summary>
    /// 开始本地化地图
    /// </summary>
    public void StartLocalization()
    {
        //文本框内容不为空
        if (inputID.text.Length > 0 && inputName.text.Length > 0)
        {
            map.MapManagerSource.ID = inputID.text;
            map.MapManagerSource.Name = inputName.text;
        }
        text.text = "开始本地化地图。";
        mapWorker.Localizer.startLocalization();
    }
    /// <summary>
    /// 停止本地化
    /// </summary>
    public void StopLocalization()
    {
        mapWorker.Localizer.stopLocalization();
    }

}
