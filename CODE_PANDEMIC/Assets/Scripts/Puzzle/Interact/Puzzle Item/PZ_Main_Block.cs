using UnityEngine;

public class PZ_Main_Block : MonoBehaviour
{
    private Transform _mainBlockTransform;
    private BoxCollider2D _mainBlockCollider;

    public void SetInfo(BlockData data)
    {
        _mainBlockTransform = GetComponent<Transform>();
        _mainBlockCollider = GetComponent<BoxCollider2D>();

        _mainBlockTransform.position = data.Position;

        _mainBlockCollider.offset = data.Offset;
        _mainBlockCollider.size = data.Size;
    }
}