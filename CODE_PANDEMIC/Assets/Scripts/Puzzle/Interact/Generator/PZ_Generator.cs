using UnityEngine;

public class PZ_Generator : MonoBehaviour, IInteractable
{
    private string _generatorPuzzle = "PZ_Generator_Puzzle_Prefab";

    // 발전기 퍼즐 띄우기
    public void Interact()
    {
        Debug.Log("발전기 상호 작용");

        // 여기에 발전기 퍼즐 띄우는 로직 구현 예정
    }
}