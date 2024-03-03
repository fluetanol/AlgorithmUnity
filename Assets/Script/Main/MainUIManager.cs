using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    public Scene scene;
    public void OnMoveScene(int SceneNum){
        if(SceneNum == -1) SceneManager.LoadScene("Main");
        else if(SceneNum == 0) SceneManager.LoadScene("SortScene");
        else if(SceneNum == 1) SceneManager.LoadScene("TreeScene");
    }
}
