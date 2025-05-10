using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PZ_Main_Block : MonoBehaviour
{
    private Transform _mainBlockTransform;
    private BoxCollider2D _mainBlockCollider;

    public void SetInfo(BlockData data)
    {
        _mainBlockTransform = GetComponent<Transform>();
        _mainBlockCollider = GetComponent<BoxCollider2D>();

        _mainBlockTransform.position = data.Pos;

        _mainBlockCollider.offset = data.Offset;
        _mainBlockCollider.size = data.Size;
    }

    public IEnumerator DestroyThisObject()
    {
        List<SpriteRenderer> spriteRenders = new List<SpriteRenderer>();

        GetComponentsInChildren(false, spriteRenders);

        float currentTime = 0f;

        while (currentTime < 1)
        {
            currentTime += Time.fixedDeltaTime;

            for (int index = 0; index < spriteRenders.Count; index++)
            {
                Color color = spriteRenders[index].color;
                color.a -= 0.02f;
                spriteRenders[index].color = color;
            }

            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}