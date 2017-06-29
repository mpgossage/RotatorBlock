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

    [SerializeField]
    bool isLeft;

    [Inject]
    Settings settings;
    // this is irritating: we cannot set the Inject(Id) using a variable,
    // so we get both and decide later
    [Inject(Id = "left")]   GameGridModel leftGrid;
    [Inject(Id = "right")]  GameGridModel rightGrid;

    // Use this for initialization
    void Start ()
    {
        // the grid might not be loaded, so wait a bit
        Invoke("MakeTiles", 0.1f);
		
	}
    void MakeTiles()
    {
        GameGridModel grid = (isLeft) ? leftGrid : rightGrid;
        for (int j = 0; j < GameGridModel.HEIGHT; j++)
        {
            for (int i = 0; i < GameGridModel.WIDTH; i++)
            {
                GameObject obj = new GameObject(string.Format("Tile-{0}-{1}", i, j));
                obj.transform.SetParent(this.transform);
                obj.transform.localPosition = new Vector3(i, GameGridModel.HEIGHT - j, 0);
                SpriteRenderer spr = obj.AddComponent<SpriteRenderer>();
                int val = grid.GetGrid(i, j);
                spr.sprite = settings.tiles[val];
            }
        }
    }

}
