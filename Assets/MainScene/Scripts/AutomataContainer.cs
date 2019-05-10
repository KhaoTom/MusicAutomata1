using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent01 : UnityEvent<int> { }

public class AutomataContainer : MonoBehaviour
{
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
    AudioController audioController = null;

    [SerializeField]
    private IntEvent01 onGenerationCountChanged = null;

    [SerializeField]
    private IntEvent01 onStepChanged = null;

    [SerializeField]
    private IntEvent01 onColumnsCreated = null;

    [SerializeField]
    private ColumnParamChanged onUpdateColumnParamDisplay = null;

    private float tickAccumulator = 0.0f;
    private int step = 0;

    private MusicAutomataSystem system;
    private CellAppearance[,] cellAppearances;

    private bool isPlaying = false;

    private struct ColumnParameter
    {
        public int channel;
        public int param;
        public ColumnParameter(int channel, int param)
        {
            this.channel = channel;
            this.param = param;
        }
    }
    private ColumnParameter[] colParams;

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

        // create params
        {
            colParams = new ColumnParameter[width];
            for (int x = 0; x < width; x++)
            {
                new ColumnParameter();
                
            }

            onColumnsCreated.Invoke(width);

            // TODO: remove this hardcoded nightmare
            colParams[0] = new ColumnParameter(0, 1);
            colParams[1] = new ColumnParameter(0, 2);

            colParams[2] = new ColumnParameter(1, 1);
            colParams[3] = new ColumnParameter(1, 2);

            colParams[4] = new ColumnParameter(2, 1);
            colParams[5] = new ColumnParameter(2, 2);

            colParams[6] = new ColumnParameter(3, 1);
            colParams[7] = new ColumnParameter(3, 2);

            colParams[8] = new ColumnParameter(4, 1);
            colParams[9] = new ColumnParameter(4, 2);

            colParams[10] = new ColumnParameter(5, 1);
            colParams[11] = new ColumnParameter(5, 2);

            colParams[12] = new ColumnParameter(6, 1);
            colParams[13] = new ColumnParameter(6, 2);

            colParams[14] = new ColumnParameter(7, 1);
            colParams[15] = new ColumnParameter(7, 2);

            colParams[16] = new ColumnParameter(8, 1);
            colParams[17] = new ColumnParameter(8, 2);

            colParams[18] = new ColumnParameter(9, 1);
            colParams[19] = new ColumnParameter(9, 2);

            colParams[20] = new ColumnParameter(10, 1);
            colParams[21] = new ColumnParameter(10, 2);

            for (int x = 0; x < width; x++)
            {
                onUpdateColumnParamDisplay.Invoke(x, colParams[x].channel, colParams[x].param);

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
            step = 0;
            onStepChanged.Invoke(step);
            tickAccumulator = 0.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlaying)
            {
                isPlaying = false;
                audioController.StopAll();
            }
            else
            {
                isPlaying = true;
                audioController.PlayAll();
            }
        }

        if (isPlaying)
        {
            if (tickAccumulator > tickInterval)
            {
                tickAccumulator -= tickInterval;

                ++step;
                if (step >= height)
                {
                    step = 0;
                    system.AdvanceGeneration();
                    onGenerationCountChanged.Invoke(system.GenerationCount);
                    system.ForEachCell(SetCellAppearance);
                }

                onStepChanged.Invoke(step);

                int y = height - 1 - step;
                for (int x = 0; x < width; x++)
                {
                    int cv = system.GetCell(x, y);
                    if (cv > 0)
                    {
                        ColumnParameter p = colParams[x];

                        switch (p.param)
                        {
                            case 0:
                                break;
                            case 1:
                                audioController.SetVolume(p.channel, cv * (1.0f / (float)maxValue));
                                break;
                            case 2:
                                audioController.SetTime(p.channel, (cv - 1) * (1.0f / ((float)maxValue)));
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
            tickAccumulator += Time.deltaTime;
        }
    }

    void SetCellAppearance(int x, int y, int val)
    {
        cellAppearances[x, y].SetAppearance(val);
    }

    public void SetTickInterval(float val)
    {
        tickInterval = val;
        tickAccumulator = 0.0f;
    }

    public void SetColumnParams(int columnId, int channelId, int paramId)
    {
        if (columnId < width && channelId < audioController.GetNumberOfSources())
        {
            colParams[columnId] = new ColumnParameter(channelId, paramId);
            onUpdateColumnParamDisplay.Invoke(columnId, colParams[columnId].channel, colParams[columnId].param);
        }
    }
}
