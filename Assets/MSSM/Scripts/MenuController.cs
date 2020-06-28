using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace MSSM
{
    public class MenuController : MonoBehaviour
    {
        private GameController game;
        public InputField input;
        public Text maps;

        void Awake()
        {
            game = FindObjectOfType<GameController>();
        }

        void Start()
        {
            if (game)
            {
                var list = game.LoadMapInfos();
                foreach (var item in list)
                {
                    maps.text = item + Environment.NewLine + maps.text;
                }
            }
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void Load()
        {
            SceneManager.LoadScene("LoadMaps");
        }

        public void Create()
        {
            if (!string.IsNullOrEmpty(input.text))
            {
                game.InputMapName = input.text;
                SceneManager.LoadScene("CreateMap");
            }
        }
    }
}

