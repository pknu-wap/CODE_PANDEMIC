using UnityEngine;

public class PZ_Parking : MonoBehaviour
{
    private bool[,] _emptyPlace = new bool[6, 6];

    private void Start()
    {
        _emptyPlace[2, 0] = true;

        _emptyPlace[4, 1] = true;

        _emptyPlace[3, 2] = true;
        _emptyPlace[4, 2] = true;
        _emptyPlace[5, 2] = true;

        _emptyPlace[1, 4] = true;

        _emptyPlace[3, 5] = true;
        _emptyPlace[5, 5] = true;
    }
}