using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    public Scene scene;
    public ITitlePlay titlePlay;

    private void Awake() {
        titlePlay = FindObjectOfType<TitlePanel>();
    }
    public void OnMoveScene(int SceneNum){
        if(SceneNum == -1) SceneManager.LoadSceneAsync("Main");
        else if(SceneNum == 0) {
            titlePlay.TitleSet("Sorting \nAlgorithm");
            AsyncOperation a = SceneManager.LoadSceneAsync("SortScene");
            StartCoroutine(WaitSceneLoad(a));
        }
        else if(SceneNum == 1) {
            titlePlay.TitleSet("Binary \nSearch Tree");
            AsyncOperation a = SceneManager.LoadSceneAsync("TreeScene");
            StartCoroutine(WaitSceneLoad(a));
        }
    }

    public IEnumerator WaitSceneLoad(AsyncOperation a){
        titlePlay.TitleOpen();
        a.allowSceneActivation = false;
        while (true){
            if(a.progress >= 0.9f) {
                yield return new WaitForSeconds(0.5f);
                titlePlay.TitleClose();
                a.allowSceneActivation = true;
            }
            yield return null;
        }

    }
    
}
