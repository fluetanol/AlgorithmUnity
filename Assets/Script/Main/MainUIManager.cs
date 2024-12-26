using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    public Scene      Scene;
    public ITitlePlay TitlePlay;

    private void Awake() {
        TitlePlay = FindObjectOfType<TitlePanel>();
    }

    public void OnMoveScene(int SceneNum){
        if(SceneNum == -1) SceneManager.LoadSceneAsync("Main");
        else if(SceneNum == 0) {
            TitlePlay.TitleSet("Sorting \nAlgorithm");
            AsyncOperation a = SceneManager.LoadSceneAsync("SortScene");
            StartCoroutine(WaitSceneLoad(a));
        }
        else if(SceneNum == 1) {
            TitlePlay.TitleSet("Binary \nSearch Tree");
            AsyncOperation a = SceneManager.LoadSceneAsync("TreeScene");
            StartCoroutine(WaitSceneLoad(a));
        }
    }

    public IEnumerator WaitSceneLoad(AsyncOperation a){
        TitlePlay.TitleOpen();
        a.allowSceneActivation = false;
        while (true){
            if(a.progress >= 0.9f) {
                yield return new WaitForSeconds(0.5f);
                TitlePlay.TitleClose();
                a.allowSceneActivation = true;
            }
            yield return null;
        }

    }
    
}
