using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using easyar;
using System;

namespace MSSM
{
    public class CreateMapController : MonoBehaviour
    {
        private GameController game;
        public Button btnSave;
        public Text text;
        public ARSession session;
        /// <summary>
        /// /// 稀疏空间工作框架
        /// </summary>
        public SparseSpatialMapWorkerFrameFilter mapWorker;
        /// <summary>
        /// 稀疏空间地图
        /// </summary>
        public SparseSpatialMapController map;

        void Awake()
        {
            game = FindObjectOfType<GameController>();
        }
        // Start is called before the first frame update
        void Start()
        {
            btnSave.interactable = false;
            session.WorldRootController.TrackingStatusChanged += OnTrackingStatusChanged;
            if (session.WorldRootController.TrackingStatus == MotionTrackingStatus.Tracking)
            {
                btnSave.interactable = true;
            }
            else
            {
                btnSave.interactable = false;
            }
        }
        /// <summary>
        /// 跟踪状态事件
        /// </summary>
        /// <param name="status"></param>
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
                text.text = "跟踪状态异常";
            }
        }
        /// <summary>
        /// 保存地图
        /// </summary>
        public void SaveMap()
        {
            btnSave.interactable = false;
            //地图保存结果反馈
            mapWorker.BuilderMapController.MapHost += (mapInfo, isSuccess, error) =>
            {
                if (isSuccess)
                {
                    game.SaveMapInfo(mapInfo.ID,mapInfo.Name);
                    text.text = "地图保存成功。";
                }
                else
                {
                    text.text = "地图保存出错：" + error;
                    btnSave.interactable = true;
                }
            };
            try
            {
                //保存地图
                mapWorker.BuilderMapController.Host(game.InputMapName, null);
                text.text = "开始保存地图，请稍等。";
            }
            catch (Exception ex)
            {
                text.text = "保存出错：" + ex.Message;
                btnSave.interactable = true;
            }
        }
        public void Back()
        {
            SceneManager.LoadScene(0);
        }
    }
}

