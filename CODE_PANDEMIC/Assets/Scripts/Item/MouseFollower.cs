using Inventory.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // Input System 사용

public class MouseFollower : MonoBehaviour, IDragHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private UI_InventoryItem _inventoryItem;

    private bool _isFollowing;

    public void Init()
    {
        _isFollowing = false;
       
        Transform uiRoot = GameObject.Find("UI_Root")?.transform;
        if (uiRoot != null)
        {
            Transform gameSceneCanvas = uiRoot.Find("UI_GameScene");
            if (gameSceneCanvas != null)
            {
                _canvas = gameSceneCanvas.GetComponent<Canvas>();
            }
        }
        _canvas.worldCamera = Camera.main;

        _inventoryItem = GetComponentInChildren<UI_InventoryItem>();
        _inventoryItem.Initialize();
        if (_inventoryItem == null)
        {
            Debug.LogError("MouseFollower: UIInventoryItem이 존재하지 않습니다! 인스펙터에서 확인하세요.");

        }
        StopFollowing();
    }

    void Update()
    {
        if (!_isFollowing) return;
        Vector2 mousePos = Mouse.current.position.ReadValue(); // 마우스 좌표 가져오기
        transform.position = mousePos; //  직접 위치 할당!
    }




    public void StartFollowing(Sprite sprite, int quantity)
    {
        gameObject.SetActive(true);
        _isFollowing = true;
        _inventoryItem.SetData(sprite, quantity);
    }

    public void StopFollowing()
    {
        gameObject.SetActive(false);
        _isFollowing = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rectTransform = transform as RectTransform;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)_canvas.transform,
            eventData.position,
            _canvas.worldCamera,
            out localPoint);

        rectTransform.anchoredPosition = localPoint;
    }

}
