using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAppearance : MonoBehaviour
{
    [SerializeField]
    GameObject[] appearanceObjects = new GameObject[0];

    int currentValue = 0;

    private void Start()
    {
        foreach(GameObject g in appearanceObjects)
        {
            g.SetActive(false);
        }
        appearanceObjects[currentValue].SetActive(true);
    }

    public void SetAppearance(int value)
    {
        appearanceObjects[currentValue].SetActive(false);
        currentValue = value;
        appearanceObjects[currentValue].SetActive(true);
    }
}
