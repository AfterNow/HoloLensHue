using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuPanel : MonoBehaviour
{

    [Tooltip("Time message should be displayed. Void if message requires user action.")]
    public float MessageExpiration = 3f;

    private UIPanel panel;
    private Image panelImage;

    void Start()
    {
        panel = GetComponent<UIPanel>();
    }

    void OnEnable()
    {
        NotificationManager.newMenu += ShowPanel;
    }

    void OnDisable()
    {
        NotificationManager.newMenu -= ShowPanel;
    }

    private void ShowPanel(Menu menu)
    {
        foreach (Transform child in transform)
        {
            if (child.name == menu.Name)
            {
                child.gameObject.SetActive(true);
                Debug.Log("i was the child found: " + child.name);
            }
            
        }
    }

}
