using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RotatorBlockController : MonoBehaviour
{
    [Inject(Id = "left")]
    GameGridModel leftGrid;
    [Inject(Id = "right")]
    GameGridModel rightGrid;
    [Inject]
    Signals.GridUpdated gridUpdated;
    [Inject]
    iSelectionRect selectionRect;

    [SerializeField] // for now
    TextAsset levelJson;

    LevelFormat level;

    // Use this for initialization
    [Inject]
    void OnInject()
    {
        LoadLevel();
        // check if it worked:
        //Debug.LogFormat("Left {0} Right {1}", leftGrid.GetHashCode(), rightGrid.GetHashCode());

        gridUpdated.Fire(); // triggers initial setup
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rect r = selectionRect.GetSelectionRect();
            int left = Mathf.RoundToInt(r.xMin), top = Mathf.RoundToInt(r.yMin),
                right = Mathf.RoundToInt(r.xMax), bottom = Mathf.RoundToInt(r.yMax);
            Debug.LogFormat("RotateLeft {0} {1} {2} {3}", left,top,right,bottom);
            leftGrid.Rotate(left,top,right,bottom,false);
            gridUpdated.Fire();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Rect r = selectionRect.GetSelectionRect();
            int left = Mathf.RoundToInt(r.xMin), top = Mathf.RoundToInt(r.yMin),
                right = Mathf.RoundToInt(r.xMax), bottom = Mathf.RoundToInt(r.yMax);
            Debug.LogFormat("RotateRight {0} {1} {2} {3}", left, top, right, bottom);
            leftGrid.Rotate(left, top, right, bottom, true);
            gridUpdated.Fire();
        }

    }

    void LoadLevel()
    {
        // so simple
        level = JsonUtility.FromJson<LevelFormat>(levelJson.text);
        leftGrid.Init(level.leftGrid);
        rightGrid.Init(level.rightGrid);
    }
}
