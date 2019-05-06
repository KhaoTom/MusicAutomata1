using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutomataContainer : MonoBehaviour
{
    [System.Serializable]
    public class IntEvent01 : UnityEvent<int> { }

    [SerializeField]
    private int width = 16;

    [SerializeField]
    private int height = 16;

    [SerializeField]
    private int maxValue = 2;

    [SerializeField]
    private GameObject cellPrefab = null;

    [SerializeField]
    private float cellObjSpacing = 1.1f;

    [SerializeField]
    private float tickInterval = 1.0f;

    [SerializeField]
    private IntEvent01 onGenerationCountChanged;

    private float tickAccumulator = 0.0f;

    private MusicAutomataSystem system;
    private CellAppearance[,] cellAppearances;

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        system = new MusicAutomataSystem(width, height, maxValue);

        // create cell gameobjects
        {
            cellAppearances = new CellAppearance[width, height];
            Vector3 pos = transform.position;
            Transform t = transform;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject g = Instantiate<GameObject>(cellPrefab, t);
                    g.transform.position = new Vector3(pos.x + x * cellObjSpacing, pos.y + y * cellObjSpacing, pos.z);
                    cellAppearances[x, y] = g.GetComponent<CellAppearance>();
                    g.GetComponent<CellMouseDownEventTrigger>().SetupTrigger(IncrementCell, x, y);
                }
            }
        }
    }

    void IncrementCell(int x, int y)
    {
        Debug.Log($"Incrementing cell at: {x},{y}");
        int val = system.IncrementCell(x, y);
        cellAppearances[x, y].SetAppearance(val);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            system.AdvanceGeneration();
            onGenerationCountChanged.Invoke(system.GenerationCount);
            system.ForEachCell(SetCellAppearance);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            system.Restart();
            onGenerationCountChanged.Invoke(system.GenerationCount);
            system.ForEachCell(SetCellAppearance);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isPlaying = !isPlaying;
        }

        if (isPlaying)
        {
            if (tickAccumulator > tickInterval)
            {
                tickAccumulator -= tickInterval;
                system.AdvanceGeneration();
                onGenerationCountChanged.Invoke(system.GenerationCount);
                system.ForEachCell(SetCellAppearance);
            }
            tickAccumulator += Time.deltaTime;
        }
    }

    void SetCellAppearance(int x, int y, int val)
    {
        cellAppearances[x, y].SetAppearance(val);
    }
}
