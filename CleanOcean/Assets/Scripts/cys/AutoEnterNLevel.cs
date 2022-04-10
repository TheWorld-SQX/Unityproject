using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoEnterNLevel : MonoBehaviour {
    private int indexs;
    void Start()
    {
        indexs = SceneManager.GetActiveScene().buildIndex + 1;
    }
    public void NextGame()
    {
        
        SceneManager.LoadScene(indexs);
    }
    public void MainManu()
    {

        SceneManager.LoadScene(0);
    }
}
