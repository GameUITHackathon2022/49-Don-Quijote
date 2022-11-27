using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
/// <summary>
/// LittleFoxLite is a special version that is prepared for UITGameHackrathon2022 
/// This is a part of LittleFox Template is owned by Ly
/// This shortened version is used for the UITGameHackrathon2022
/// In addition, the remaining parts belong only to the owner
/// Thanks for reading! hope your guys have a good time during UITGameHackrathon2022.
/// From Don Quijote team with love <3
/// </summary>
namespace LittleFoxLite
{
    public class GameManager : MonoBehaviour
    {
        public GameState gameState;
        public static GameManager instance;
        public bool canAttack;
        private void Awake()
        {
            if (instance == null) instance = this;
            gameState = GameState.inCombat;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void PauseGame()
        {
            Time.timeScale = 0;
        }
        public void ResumeGame()
        {
            Time.timeScale = 1;
        }
        public void ExitGame()
        {
            Application.Quit();
        }
        public void OnComBackMenuPres()
        {
            SceneManager.LoadScene(0);
        }
    }
    public enum GameState
    {
        inEvent, inNormal, inCombat
    }
}

