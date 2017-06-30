using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    [SerializeField]
    Sprite[] tiles;

    [SerializeField]
    TextAsset inputText;

    [SerializeField]
    bool isLeft;

    const int WIDTH = 9, HEIGHT = 11;
    private LevelFormat level;

	// Use this for initialization
	void Start ()
    {
        level = JsonUtility.FromJson<LevelFormat>(inputText.text);
        MakeTiles();
	}

    void MakeTiles()
    {
        for (int j=0;j<HEIGHT;j++)
        {
            for(int i=0;i<WIDTH;i++)
            {
                GameObject obj = new GameObject(string.Format("Tile-{0}-{1}",i,j));
                obj.transform.SetParent(this.transform);
                obj.transform.localPosition = new Vector3(i, HEIGHT - j, 0);
                SpriteRenderer spr = obj.AddComponent<SpriteRenderer>();
                int index = j * WIDTH + i;
                int val = (isLeft) ? level.leftGrid[index] : level.rightGrid[index];
                spr.sprite = tiles[val];
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
