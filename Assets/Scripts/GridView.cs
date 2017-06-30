using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridView : MonoBehaviour
{
    // this will be set in the set in the SettingsInstaller ScriptableObject
    [System.Serializable]
    public struct Settings 
    {
        public Sprite[] tiles;
    }

    [Inject]
    Settings settings;

    // this is irritating: we cannot set the Inject(Id) using a variable,
    // so we get both and decide later
    [Inject(Id = "left")]   GameGridModel leftGrid;
    [Inject(Id = "right")]  GameGridModel rightGrid;

    [Inject]
    Signals.GridUpdated onGridUpdated;

    [SerializeField]
    bool isLeft;

    private SpriteRenderer[,] grid;

    [Inject]
    void OnInjected()   // called post injection
    {
        onGridUpdated += OnGridUpdated;
        MakeTiles();
        OnGridUpdated();
    }

    private void OnDestroy()
    {
        onGridUpdated -= OnGridUpdated;
    }

    void OnGridUpdated()
    {
        GameGridModel inputgrid = (isLeft) ? leftGrid : rightGrid;
        for (int j = 0; j < GameGridModel.HEIGHT; j++)
        {
            for (int i = 0; i < GameGridModel.WIDTH; i++)
            {
                int val = inputgrid.GetGrid(i, j);
                grid[i,j].sprite = settings.tiles[val];
            }
        }
    }
    void MakeTiles()
    {
        grid = new SpriteRenderer[GameGridModel.WIDTH, GameGridModel.HEIGHT];
        for (int j = 0; j < GameGridModel.HEIGHT; j++)
        {
            for (int i = 0; i < GameGridModel.WIDTH; i++)
            {
                GameObject obj = new GameObject(string.Format("Tile-{0}-{1}", i, j));
                obj.transform.SetParent(this.transform);
                obj.transform.localPosition = new Vector3(i, GameGridModel.HEIGHT - j, 0);
                grid[i, j] = obj.AddComponent<SpriteRenderer>();
            }
        }
    }

}
