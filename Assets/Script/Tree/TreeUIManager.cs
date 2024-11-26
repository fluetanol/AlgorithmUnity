using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private Button             _findNodeButton;   
    [SerializeField] private Button             _removeNodeButton; 


    [SerializeField] private GameObject         _nodeInfoPrefab;
    [SerializeField] private Transform          _nodeInfoParent;
    [SerializeField] private Transform          _traversalOption;

    [Header("Panel")]
    [SerializeField] private RectTransform      _nodeInfoPanel;
    [SerializeField] private TextFieldPanel    _textFieldPanel;

    //static managed;
    private static           GameObject         _staticNodeInfoPrefab;
    private static           Transform          _staticNodeInfoParent;
    
    //dependency injection interface
    private                 ITreeTraversal       _treeTraversal;
    private                 ITreeManage          _treeManage;
    private                 INodeManage          _nodeManage;

    private                 int                 _traversalOptionNum = 0;
    private                 TraversalMode        _currentMode = TraversalMode.PreOrder;


    //이거 bit처리로 바꿀 필요가 있음
    private                 bool                _isShowNodeInfoClose = true;
    private                 bool                _isTextFieldClose = true;   


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
        _addNodeButton.onClick.AddListener(() => OnAddClick(0.5f));
        _findNodeButton.onClick.AddListener(() => OnFindClick(0.5f));
        _removeNodeButton.onClick.AddListener(() => OnRemoveClick(0.5f));
        _textFieldPanel.OpenButton.onClick.AddListener(() => ShowTextFieldPanelUI(0.5f));
        _textFieldPanel.InputField.onValueChanged.AddListener((s) => OnAddValueChanged(s));
    }

    void Start(){
    }

    public void SetNewTree(int num){
        _treeManage.SetNewTree(num);
    }

    public void AddNode(int value){
        Node node = _nodeManage.NewNode(value);
        Edge edge = _nodeManage.NewEdge();
        if(_nodeManage.AddNode(node , edge)){
            _textField.text = "Success Add Node :" + value;
        }
        else{
            _textField.text = "Fail Add Node :" + value;
            ObjectPool.DestoyPoolObject(node.gameObject, ObjectPoolType.Node);
            ObjectPool.DestoyPoolObject(edge.gameObject, ObjectPoolType.Edge);
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
        print("remove!????");
        GameObject RemoveObject = _nodeManage.RemoveNode(value);
        print("remove!?");
        if (RemoveObject== null) _textField.text = "NotFound";
        else {
            print("remove!");
            ObjectPool.DestoyPoolObject(RemoveObject, ObjectPoolType.Node);
            //Destroy(RemoveObject.Item1);
            //Destroy(RemoveObject.Item2);
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

    //NodeInfoPanel을 여닫는 기능
    public void ShowNodeInfoUI(float deltaTime){
        if(!_isShowNodeInfoClose) return;
        _isShowNodeInfoClose = false;
        _nodeInfoPanel.MovePanelUIByHorizontal(deltaTime, -_nodeInfoPanel.sizeDelta.x + 1, true);
    }

    public void CloseNodeInfoUI(float deltaTime){
        if (_isShowNodeInfoClose) return;
        _isShowNodeInfoClose = true;
        Camera.main.DOCameraZoom(4, deltaTime);
        _nodeInfoPanel.MovePanelUIByHorizontal(deltaTime, _nodeInfoPanel.sizeDelta.x, true);
    }

    //TextFieldPanel을 여닫는 기능
    public void ShowTextFieldPanelUI(float deltaTime){
        if (!_isTextFieldClose)  return;
        _isTextFieldClose = false;

        RectTransform rect = _textFieldPanel.GetComponent<RectTransform>();
        rect.MovePanelUIByVertical(deltaTime, rect.sizeDelta.y);
        _textFieldPanel.ResetUIListener(ETextFieldUIType.OpenButton);
        _textFieldPanel.OpenButton.onClick.AddListener(() => CloseTextFieldPanelUI(0.5f));
    }

    public void CloseTextFieldPanelUI(float deltaTime){
        if(_isTextFieldClose) return;
        _isTextFieldClose = true;
        InputManager.current.SetMouseDeltaAllow(true);
        _textFieldPanel.GetComponent<RectTransform>().
        MovePanelUIByVertical(deltaTime, _textFieldPanel.OriginAnchor.y);
        
        _textFieldPanel.ResetAllValue();
        _textFieldPanel.ResetUIListener(ETextFieldUIType.OpenButton);
        _textFieldPanel.OpenButton.onClick.AddListener(() => ShowTextFieldPanelUI(0.5f));
    }

    //TextFieldPanel의 confirm 버튼 기능
    public void OnAddClick(float deltaTime){
        _textFieldPanel.ResetUIListener(ETextFieldUIType.ConfirmButton, ETextFieldUIType.InputField);
        _textFieldPanel.onValueChangedListener((s) => OnAddValueChanged(s));
    }

    public void OnFindClick(float deltaTime){
        FindNode(int.Parse(_textFieldPanel.InputField.text));
        _textFieldPanel.ResetUIListener(ETextFieldUIType.ConfirmButton, ETextFieldUIType.InputField);
        _textFieldPanel.onValueChangedListener((s) => OnFindValueChanged(s));
    }

    public void OnRemoveClick(float deltaTime){
        _textFieldPanel.ResetUIListener(ETextFieldUIType.ConfirmButton, ETextFieldUIType.InputField);
        _textFieldPanel.onValueChangedListener((s) => OnRemoveValueChanged(s));
    }


    //TextFieldPanel의 inputField 기능
    public void OnAddValueChanged(string s){
        Button button = _textFieldPanel.ConfirmButton;
        if(TryParseAndButtonInteractable(s, button, out int n)){
            _textFieldPanel.ResetUIListener(ETextFieldUIType.ConfirmButton);
            _textFieldPanel.ConfirmButton.onClick.AddListener(() => AddNode(n));
        }
    }

    public int OnFindValueChanged(string s){
        Button button = _textFieldPanel.ConfirmButton;
        if (TryParseAndButtonInteractable(s, button, out int n)){
            FindNode(n);
            _textFieldPanel.ResetUIListener(ETextFieldUIType.ConfirmButton);
            _textFieldPanel.ConfirmButton.onClick.AddListener(() => FindNode(int.Parse(_textFieldPanel.InputField.text)));
        }
        return n;
    }

    public void OnRemoveValueChanged(string s){
        Button button = _textFieldPanel.ConfirmButton;
        if (TryParseAndButtonInteractable(s, button, out int n)){
            _textFieldPanel.ResetUIListener(ETextFieldUIType.ConfirmButton);
            _textFieldPanel.ConfirmButton.onClick.AddListener(() => RemoveNode(n));
        }
    }


    private bool TryParseAndButtonInteractable(string s, Button button, out int n){
        bool result = int.TryParse(s, out n);  
        button.interactable = result;
        return result;
    }


    public static void InstantiateNodeInfo(int value){
        GameObject g = Instantiate(_staticNodeInfoPrefab, _staticNodeInfoParent);
        g.GetComponent<TMP_Text>().text =value.ToString();
    }

}


public static class NodeMove{
    
}