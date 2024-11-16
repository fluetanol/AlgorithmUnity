using DG.Tweening;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TreeUIManager : MonoBehaviour
{
    public static TreeUIManager current{get; private set;}

    [Header("UIElement")]
    [SerializeField] private TMP_Text           _textField;
    [SerializeField] private Button             _addNodeButton;
    [SerializeField] private GameObject         _nodeInfoPrefab;
    [SerializeField] private Transform          _nodeInfoParent;
    [SerializeField] private Transform          _traversalOption;

    [Header("Panel")]
    [SerializeField] private RectTransform      _nodeInfoPanel;
    [SerializeField] private AddPanel           _addPanel;
    [SerializeField] private FindRemovePanel    _findRemovePanel;

    //static managed;
    private static           GameObject         _staticNodeInfoPrefab;
    private static           Transform          _staticNodeInfoParent;
    
    //dependency interface
    private                 ITreeTraversal       _treeTraversal;
    private                 ITreeManage          _treeManage;
    private                 INodeManage          _nodeManage;

    private                 int                 _traversalOptionNum = 0;
    private                 TraversalMode        _currentMode = TraversalMode.PreOrder;


    //이거 bit처리로 바꿀 필요가 있음
    private                 bool                _isShowNodeInfoClose = true;
    private                 bool                _isAddNodePanelClose = true;



    private void Awake(){
        current = this;
        AlgorithmTreeManager treeManager = FindObjectOfType<AlgorithmTreeManager>();
        _treeManage = treeManager;
        _nodeManage = treeManager;
        _treeTraversal = treeManager;
    }

    private void OnEnable() {
        _staticNodeInfoPrefab = _nodeInfoPrefab;
        _staticNodeInfoParent = _nodeInfoParent;
        //_addNodeButton.onClick.AddListener(delegate{ShowAddPanelUI(0.1f, 0.5f);});
        _addNodeButton.onClick.AddListener(() => ShowAddPanelUI(0.5f));
        _addPanel.InputField.onValueChanged.AddListener((s) => OnAddValueChanged(s));
        _addPanel.addButton.onClick.AddListener(() => AddNode(int.Parse(_addPanel.NodeUI.NodeValueText.text)));
        _findRemovePanel._InputField.onValueChanged.AddListener((s) => OnFindValueChanged(s));
    }

    public void SetNewTree(int num){
        _treeManage.SetNewTree(num);
    }

    public void AddNode(int value){
        EventSystem.current.SetSelectedGameObject(null);
        Node node = _nodeManage.NewNode(value);
        Edge edge = _nodeManage.NewEdge();
        if(_nodeManage.AddNode(node , edge)){
            _textField.text = "Success Add Node :" + value;
        }
        else{
            _textField.text = "Fail Add Node :" + value;
            Destroy(node.gameObject);
            Destroy(edge.gameObject);
        }
    }

    public void FindNode(int value){
        if(_nodeManage.IsExistNode(value, out Node node)){
            _textField.text = "Find!";
            Vector3 pos = node.transform.position;
            pos.z = -10;
            Camera.main.DOCameraMove(pos, 0.5f);
        }
        else _textField.text = "NotFound";
    }

    public void RemoveNode(int value){
        (GameObject, GameObject) RemoveObject = _nodeManage.RemoveNode(value);
        if (RemoveObject.Item1 == null) _textField.text = "NotFound";
        else {
            Destroy(RemoveObject.Item1);
            Destroy(RemoveObject.Item2);
            _textField.text = "Remove!";
        }
    }

    public void SetRootNodeValue(int value){
        if (_treeManage.GetTreeNodeCount() == 1){
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
    private void TraversalReset(){
        for (int k = _nodeInfoParent.childCount - 1; k >= 0; k--)
            Destroy(_nodeInfoParent.GetChild(k).gameObject);
        _treeManage.ResetRecentNode();
    }

    public void FocusNode(Vector3 NodePosition, float deltaTime){
        Camera.main.DOCameraMove(NodePosition, deltaTime);
        Camera.main.DOCameraZoom(2.5f, deltaTime);
    }

    public void ShowNodeInfoUI(float widthRatio, float deltaTime){
        if(!_isShowNodeInfoClose) return;
        _isShowNodeInfoClose = false;
        _nodeInfoPanel.ShowPanelUI(widthRatio, deltaTime, 0);
    }

    public void CloseNodeInfoUI(float deltaTime){
        if (_isShowNodeInfoClose) return;
        _isShowNodeInfoClose = true;
        Camera.main.DOCameraZoom(4, deltaTime);
        _nodeInfoPanel.ClosePanelUI(deltaTime);
    }

    public void ShowAddPanelUI(float deltaTime){
        if(!_isAddNodePanelClose) return;
        InputManager.current.SetMouseDeltaAllow(false);
        _isAddNodePanelClose = false;
        _addPanel.GetComponent<CanvasGroup>().ShowPanelGroupUIByAlpha(1, deltaTime);
    }

    public void CloseAddPanelUI(float deltaTime){
        if (_isAddNodePanelClose) return;
        InputManager.current.SetMouseDeltaAllow(true);
        _isAddNodePanelClose = true;
        _addPanel.GetComponent<CanvasGroup>().ClosePanelGroupUIByAlpha(deltaTime);
    }

    public void ShowFindPanel(float deltaTime){
        _findRemovePanel.GetComponent<RectTransform>().ShowPanelUI(0.5f, deltaTime, 3);
    }



    public void OnAddValueChanged(string s){
        if(int.TryParse(s, out int result)){
            if(!_addPanel.addButton.interactable) _addPanel.addButton.interactable = true;
            TMP_InputField inputField = _addPanel.InputField;
            _addPanel.NodeUI.NodeValueText.text = result.ToString();
        }else{
            _addPanel.addButton.interactable = false;
            _addPanel.NodeUI.NodeValueText.text = "X";
        }
    }

    public void OnFindValueChanged(string s){
        if(int.TryParse(s, out int result)){
            FindNode(result);
        }else{
            _findRemovePanel._findButton.interactable = false;

        }
    }

    public static void InstantiateNodeInfo(int value){
        GameObject g = Instantiate(_staticNodeInfoPrefab, _staticNodeInfoParent);
        g.GetComponent<TMP_Text>().text =value.ToString();
    }

}


public static class NodeMove{
    
}