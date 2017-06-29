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


    [SerializeField] // for now
    TextAsset levelJson;

    LevelFormat level;

    // Use this for initialization
    void Start ()
    {
        LoadLevel();
        // check if it worked:
        //Debug.LogFormat("Left {0} Right {1}", leftGrid, rightGrid);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadLevel()
    {
        // so simple
        level = JsonUtility.FromJson<LevelFormat>(levelJson.text);
        leftGrid.Init(level.leftGrid);
        rightGrid.Init(level.rightGrid);
    }
}
