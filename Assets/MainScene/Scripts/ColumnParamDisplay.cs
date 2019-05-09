using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ColumnParamDisplay : MonoBehaviour
{
    [SerializeField]
    private Text label = null;

    [SerializeField]
    private Button button = null;

    private int columnId = 0;
    private System.Action<int> buttonClickedEvent;
    public void SetColumnId(int newId)
    {
        columnId = newId;
    }

    public void SetLabel(string text)
    {
        label.text = text;
    }

    public string GetLabel()
    {
        return label.text;
    }

    public void AddButtonListener(System.Action<int> buttonClickedEvent)
    {
        this.buttonClickedEvent = buttonClickedEvent;
        button.onClick.AddListener(clickButton);
    }

    private void clickButton()
    {
        buttonClickedEvent.Invoke(columnId);
    }
}
