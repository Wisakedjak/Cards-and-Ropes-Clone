using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject chestHolder;
    public static SceneController instance;
    void Awake()
    {
        instance = this;
        _loadCurrentLevelScene();
    }

    private void _loadCurrentLevelScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"), LoadSceneMode.Additive);
    }

    public void RestartLevel()
    {
        _restartLevel();
    }

    private void _restartLevel()
    {
        _findAndMoveOldChests();

        var loadScene = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("CurrentLevel"), LoadSceneMode.Additive);
        loadScene.completed += _findAndCloseNewChests;
    }

    private void _findAndMoveOldChests()
    {
        var chests = GameObject.FindGameObjectsWithTag("Chest");
        foreach (var t in chests)
        {
            t.transform.parent = chestHolder.transform;
        }

        var solver = GameObject.FindGameObjectWithTag("ObiSolver");
        solver.SetActive(false);
    }

    private void _findAndCloseNewChests(AsyncOperation asyncOperation)
    {
        print("here");
        var chests = GameObject.FindGameObjectsWithTag("Chest");
        foreach (var t in chests)
        {
            if (t.scene==SceneManager.GetSceneAt(2))
            {
                t.SetActive(false);
            }
            
        }
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        var solver = GameObject.FindGameObjectWithTag("ObiSolver");
        solver.SetActive(true);
        GameManager.instance.ReturnCardsAndOpenButtons();
    }

    
    void Update()
    {
        
    }
}
