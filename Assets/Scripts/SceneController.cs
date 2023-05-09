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
        if (PlayerPrefs.GetInt("CurrentLevel")<11)
        {
            SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("CurrentLevel"), LoadSceneMode.Additive);
         
        }
        else
        {
            SceneManager.LoadSceneAsync(11, LoadSceneMode.Additive);
            
        }
    }

    public void RestartLevel()
    {
        _restartLevel();
    }

    private void _restartLevel()
    {
        _findAndMoveOldChests();

        if (PlayerPrefs.GetInt("CurrentLevel")<11)
        {
            var loadScene = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("CurrentLevel"), LoadSceneMode.Additive);
            loadScene.completed += _findAndCloseNewChests;
        }
        else
        {
            var loadScene = SceneManager.LoadSceneAsync(11, LoadSceneMode.Additive);
            loadScene.completed += _findAndCloseNewChests;
        }
        
        
    }

    private void _findAndMoveOldChests()
    {
        var chests = GameObject.FindGameObjectsWithTag("Chest");
        foreach (var t in chests)
        {
            t.transform.parent = chestHolder.transform;
        }

        var solver = GameObject.FindGameObjectWithTag("ObiSolver");
        Destroy(solver);
    }

    private void _findAndCloseNewChests(AsyncOperation asyncOperation)
    {
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

    public void OpenNextLevel()
    {
        _openNextLevel();
    }

    private void _openNextLevel()
    {
        _destroyOldChest();
        if (PlayerPrefs.GetInt("CurrentLevel")<11)
        {
            var loadScene = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("CurrentLevel"), LoadSceneMode.Additive);
            loadScene.completed += _unloadOldScene;
        }
        else
        {
            var loadScene = SceneManager.LoadSceneAsync(11, LoadSceneMode.Additive);
            loadScene.completed += _unloadOldScene;
        }
    }

    private void _destroyOldChest()
    {
        for (int i = 0; i < chestHolder.transform.childCount; i++)
        {
            chestHolder.transform.GetChild(i).gameObject.SetActive(false); 
        }
        
        var solver = GameObject.FindGameObjectWithTag("ObiSolver");
        Destroy(solver);
    }

    private void _unloadOldScene(AsyncOperation asyncOperation)
    {
        var unloadSceneAsync = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        unloadSceneAsync.completed += _openSolver;
        
    }

    private void _openSolver(AsyncOperation asyncOperation)
    {
        var solver = GameObject.FindGameObjectWithTag("ObiSolver");
        //solver.SetActive(true);
        GameManager.instance.ReturnCardsAndOpenButtons();
    }

    
    void Update()
    {
        
    }
}
