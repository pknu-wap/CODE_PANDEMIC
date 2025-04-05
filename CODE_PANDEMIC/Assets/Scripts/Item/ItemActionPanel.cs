using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionPanel : UI_Base
{
 
    public void AddButton(string name, Action onclickAction)
    {
      
        
        Managers.Resource.Instantiate("ItemActionButton", transform,callback:(obj) =>
       {
           
           Button button = obj.GetComponent<Button>();
           TMPro.TMP_Text buttonText = obj.GetComponentInChildren<TMPro.TMP_Text>();

           buttonText.text = name;

           button.onClick.AddListener(() =>
           {
               onclickAction?.Invoke();
               RemoveOldButtons();
           });
       }
      );
       
    }
    public void Toggle(bool val)
    {
        if (val == true) RemoveOldButtons();
        gameObject.SetActive(val);
    }

    private void RemoveOldButtons()
    {
       foreach(Transform transformChildObjects in transform)
        {
            Destroy(transformChildObjects.gameObject);
        }
    }
}
