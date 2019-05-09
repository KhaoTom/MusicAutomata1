using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class ColumnParamChanged : UnityEvent<int, int, int> { }

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Text genCountText = null;

    [SerializeField]
    private GameObject ColumnParamEditPanel = null;

    [SerializeField]
    private GameObject ColumnParamDisplaysPanel = null;

    [SerializeField]
    private GameObject ColumnParamDisplayPrefab = null;

    [SerializeField]
    private InputField paramsInput = null;

    [SerializeField]
    private ColumnParamChanged onColumnParamChanged = null;

    private int activeColumnId = 0;

    private ColumnParamDisplay[] columnParamDisplays;

    public void UpdateGenCount(int amount)
    {
        genCountText.text = amount.ToString();
    }

    public void CreateColumnParamDisplays(int numColumns)
    {
        columnParamDisplays = new ColumnParamDisplay[numColumns];
        for (int i = 0; i < numColumns; i++)
        {
            GameObject g = Instantiate<GameObject>(ColumnParamDisplayPrefab, ColumnParamDisplaysPanel.transform);
            columnParamDisplays[i] = g.GetComponent<ColumnParamDisplay>();
            columnParamDisplays[i].SetColumnId(i);
            columnParamDisplays[i].AddButtonListener(OpenColumnParamEditPanel);
        }
    }

    public void ChangeColumnParams()
    {
        string newParams = paramsInput.text;
        if (newParams.Length >= 2)
        {
            int channel = System.Convert.ToInt32(newParams.Substring(0,1), 16);
            int channelParam = System.Convert.ToInt32(newParams.Substring(1, 1), 16);
            //columnParamDisplays[activeColumnId].SetLabel(newParams);
            onColumnParamChanged.Invoke(activeColumnId, channel, channelParam);
        }
    }

    void OpenColumnParamEditPanel(int columnId)
    {
        activeColumnId = columnId;
        paramsInput.text = columnParamDisplays[columnId].GetLabel();
        ColumnParamEditPanel.SetActive(true);
    }

    public void CloseColumnEditPanel()
    {
        ColumnParamEditPanel.SetActive(false);
    }

    public void UpdateColumnLabel(int columnId, int channelId, int paramId)
    {
        columnParamDisplays[columnId].SetLabel($"{System.Convert.ToString(channelId, 16).ToUpper()}{System.Convert.ToString(paramId, 16).ToUpper()}");
    }
}
