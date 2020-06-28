using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace MSSM
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance = null;
        private static readonly string pathMapInfo = "/mapinfo.txt";
        [HideInInspector]
        public string InputMapName;

        void Awake()
        {
            //实现单实例
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (this != instance)
            {
                Destroy(gameObject);
                return;
            }
        }

        public void SaveMapInfo(string mapID, string mapName)
        {
            //SaveStringArray(stringArray, Application.persistentDataPath + pathMapInfo);
            List<string> maps = new List<string>();
            try
            {
                maps = LoadMapInfos();
            }
            catch
            {
                Debug.Log("Load MapInfo Error");
            }
            maps.Add(mapID + "," + mapName);
            SaveStringArray(maps.ToArray(), Application.persistentDataPath + pathMapInfo);
        }

        public List<string> LoadMapInfos()
        {
            return LoadStringList(Application.persistentDataPath + pathMapInfo);
        }

        /// <summary>
        /// 保存字符串数组
        /// </summary>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="path">保存路径</param>
        private void SaveStringArray(string[] stringArray, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (var s in stringArray)
                    {
                        writer.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        /// <summary>
        /// 读取文本信息
        /// </summary>
        /// <param name="path">文本路径</param>
        /// <returns>字符串列表</returns>
        private List<string> LoadStringList(string path)
        {
            List<string> list = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        list.Add(reader.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            return list;
        }
    }
}


