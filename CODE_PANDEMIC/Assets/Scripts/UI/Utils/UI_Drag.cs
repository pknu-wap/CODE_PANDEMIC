using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI_Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas _canvas;
    private RectTransform _rectTransform;
    private bool _isFollowing;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        Transform uiRoot = Managers.UI.UIRoot.transform;
        if (uiRoot != null)
        {
            Transform gameSceneCanvas = Managers.UI.SceneUI.transform;
            if (gameSceneCanvas != null)
            {
                _canvas = gameSceneCanvas.GetComponent<Canvas>();
            }
        }
        _canvas.worldCamera = Camera.main;
    }
    void Update()
    {
        if (!_isFollowing)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        transform.position = mousePos;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        StartFollowing();

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isFollowing)
            return;
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            _canvas.worldCamera,
            out localPoint))
        {
            _rectTransform.localPosition = localPoint;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        StopFollowing();
    }

    public void StartFollowing()
    {
        _isFollowing = true;
    }

    public void StopFollowing()
    {
        _isFollowing = false;
    }
}