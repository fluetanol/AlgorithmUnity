using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreeUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField     inputField;
    [SerializeField] private TMP_Text           textField;
    [SerializeField] private GameObject         _nodeInfoPrefab;
    [SerializeField] private Transform          _nodeInfoParent;
    [SerializeField] private Transform          _traversalOption;
    [SerializeField] private RectTransform         _nodeInfoUI;

    private static           GameObject         _staticNodeInfoPrefab;
    private static           Transform          _staticNodeInfoParent;
    private static           RectTransform          _staticNodeInfoUI;
    
    private                 ITreeTraversal       _treeTraversal;
    private                 ITreeManage          _treeManage;
    private                 INodeManage          _nodeManage;
    private                 int                 _traversalOptionNum = 0;

    private                 TraversalMode        _currentMode = TraversalMode.PreOrder;

    private void Awake(){
        AlgorithmTreeManager treeManager = FindObjectOfType<AlgorithmTreeManager>();
        _treeManage = treeManager;
        _nodeManage = treeManager;
        _treeTraversal = treeManager;
    }

    private void OnEnable() {
        _staticNodeInfoPrefab = _nodeInfoPrefab;
        _staticNodeInfoParent = _nodeInfoParent;
        _staticNodeInfoUI = _nodeInfoUI;
    }

    public void SetNewTree(int num){
        _treeManage.SetNewTree(num);
    }

    public void AddNode(){
        if(int.TryParse(inputField.text, out int value)){
            Node node = _nodeManage.NewNode(value);
            Edge edge = _nodeManage.NewEdge();
            
            if(_nodeManage.AddNode(node , edge)){
                textField.text = "Success Add Node :" + value;
            }
            else{
                textField.text = "Fail Add Node :" + value;
                Destroy(node.gameObject);
                Destroy(edge.gameObject);
            }
        }
        else textField.text = "Wrong Num";

    }

    public void FindNode(){
        if (int.TryParse(inputField.text, out int value)){
            if(_nodeManage.IsExistNode(value, out Node node)){
                textField.text = "Find!";
                ShowNodeInfoUI(0.3f, 0.5f);
                node.OnNodePress(0.5f);
            }
            else textField.text = "NotFound";
        }
        else textField.text = "Wrong Num";
    }

    public void RemoveNode(){
        if (int.TryParse(inputField.text, out int value)){
            (GameObject, GameObject) RemoveObject = _nodeManage.RemoveNode(value);
            
            if (RemoveObject.Item1 == null) textField.text = "NotFound";
            else {
                Destroy(RemoveObject.Item1);
                Destroy(RemoveObject.Item2);
                textField.text = "Remove!";
            }
        }
    }

    public void SetRootNodeValue(){
        if (int.TryParse(inputField.text, out int value) && _treeManage.GetTreeNodeCount() == 1){
            _treeManage.SetRootValue(value);
        }
    }

    public void PanelNumberMove(bool reverse){
        _traversalOption.GetChild(_traversalOptionNum).gameObject.SetActive(false);
        if (reverse) {
            _traversalOptionNum -= 1;
            if(_traversalOptionNum<0) _traversalOptionNum = 3;
        }
        else _traversalOptionNum = (_traversalOptionNum+1)%4;
        _traversalOption.GetChild(_traversalOptionNum).gameObject.SetActive(true);
    }


    public void StepTraversal(int mode){
        if(_currentMode != (TraversalMode)mode){
            TraversalReset();
            _currentMode = (TraversalMode)mode;
            _treeTraversal.SetTraversalMode(_currentMode);
        }
        _treeTraversal.EnumerateStepTraversal();
    }

    public void UpdateTraversal(int mode){
        TraversalReset();
        _currentMode = (TraversalMode)mode;
        _treeTraversal.SetTraversalMode(_currentMode);
        _treeTraversal.EnumerateCoroutineTraversal();
    }

    //시각적 처리 초기화
    private void TraversalReset()
    {
        for (int k = _nodeInfoParent.childCount - 1; k >= 0; k--)
            Destroy(_nodeInfoParent.GetChild(k).gameObject);
        _treeManage.ResetRecentNode();
    }

    public static void CloseNodeInfoUI(float deltaTime){
        _staticNodeInfoUI.DOAnchorPosX(0, deltaTime).SetEase(Ease.InOutQuad).onComplete
                 += () => _staticNodeInfoUI.gameObject.SetActive(false);
    }

    public static void ShowNodeInfoUI(float widthRatio, float deltaTime){
        float moveX = Screen.width * widthRatio;
        _staticNodeInfoUI.sizeDelta = new Vector2(moveX, _staticNodeInfoUI.sizeDelta.y);
        if (!_staticNodeInfoUI.gameObject.activeSelf)
        {
            _staticNodeInfoUI.DOAnchorPosX(-moveX, deltaTime).SetEase(Ease.InOutQuad).OnStart(() =>
            _staticNodeInfoUI.gameObject.SetActive(true));
        }
    }


    public static void InstantiateNodeInfo(int value){
        GameObject g = Instantiate(_staticNodeInfoPrefab, _staticNodeInfoParent);
        g.GetComponent<TMP_Text>().text =value.ToString();
    }

}
