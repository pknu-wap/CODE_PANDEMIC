using UnityEngine;

public class PZ_Elevator_Screen : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _floorImage;
    [SerializeField] private Sprite _1Sprite;
    [SerializeField] private Sprite _2Sprite;
    [SerializeField] private Sprite _3Sprite;

    public void Setting(ElevatingMap floorInfo)
    {
        switch (floorInfo)
        {
            case ElevatingMap.Hospital1:
                _floorImage.sprite = _1Sprite;
                break;

            case ElevatingMap.Hospital2:
                _floorImage.sprite = _2Sprite;
                break;

            case ElevatingMap.Hospital3:
                _floorImage.sprite = _3Sprite;
                break;
        }
    }
}