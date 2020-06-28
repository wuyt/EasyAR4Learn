using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using easyar;
using UnityEngine.UI;
using System;

namespace MSSM
{
    public class LoadMapsController : MonoBehaviour
    {
        private GameController game;
        public SparseSpatialMapWorkerFrameFilter mapWorker;
        public SparseSpatialMapController prefab;
        private string[] mapStatus;
        public Color[] colors;

        public Text text;


        void Awake()
        {
            game = FindObjectOfType<GameController>();
        }

        void Start()
        {
            if (!game)
            {
                return;
            }

            var list = game.LoadMapInfos();
            mapStatus = new string[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                var info = list[i].Split(',');
                var map = Instantiate(prefab);

                map.transform.Find("Cube").GetComponent<MeshRenderer>().material.color=colors[i];

                map.SourceType = SparseSpatialMapController.DataSource.MapManager;
                map.MapManagerSource.ID = info[0];
                map.MapManagerSource.Name = info[1];
                map.MapWorker = mapWorker;

                map.MapLoad += (mapInfo, isSuccess, error) =>
                {
                    if (isSuccess)
                    {
                        text.text = mapInfo.Name + "-->" + "map load success." + Environment.NewLine + text.text;
                    }
                    else
                    {
                        text.text = mapInfo.Name + "-->" + "map load fail." + Environment.NewLine + text.text;
                    }
                };

                map.MapLocalized += () =>
                {
                    text.text = map.MapInfo.Name+"-->"+"Localized."+Environment.NewLine+text.text;
                };

                map.MapStopLocalize += () =>
                {
                    text.text = map.MapInfo.Name+"-->"+"Stopped."+Environment.NewLine+text.text;
                };
            }

            mapWorker.Localizer.startLocalization();
        }

        public void Back()
        {
            SceneManager.LoadScene(0);
        }
    }
}


