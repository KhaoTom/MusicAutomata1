using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class CellMouseDownEvent : UnityEvent<int, int> { }

public class CellMouseDownEventTrigger : MonoBehaviour
{

    [SerializeField]
    int x;

    [SerializeField]
    int y;

    [SerializeField]
    CellMouseDownEvent events = null;

    private void OnMouseDown()
    {
        events.Invoke(x, y);
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            events.Invoke(x, y);
        }
    }

    public void SetupTrigger(UnityAction<int, int> myEvent, int myX, int myY)
    {
        x = myX;
        y = myY;
        events.AddListener(myEvent);
    }
}
