using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Text genCountText = null;

    public void UpdateGenCount(int amount)
    {
        genCountText.text = amount.ToString();
    }
}
