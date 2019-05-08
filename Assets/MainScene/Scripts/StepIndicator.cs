using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepIndicator : MonoBehaviour
{
    [SerializeField]
    private float stepDistance = 1.1f;

    [SerializeField]
    private float startingY = 0.0f;

    private Vector3 startPos;

    private int step;
    private Vector3 stepStartPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(transform.position.x, startingY, transform.position.z);
        transform.position = startPos;
    }

    public void SetStep(int step)
    {
        this.step = step;
        transform.position = new Vector3(startPos.x, startPos.y + (step * stepDistance), startPos.z);
    }
}
