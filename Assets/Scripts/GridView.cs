using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

interface iGridTileAnimator
{
    void AddTween(int x, int y, Vector3 delta, bool linear=false, System.Action onComplete = null);
    void RewindTweens();
}

public class GridView : MonoBehaviour, iGridTileAnimator
{
    // this will be set in the set in the SettingsInstaller ScriptableObject
    [System.Serializable]
    public struct Settings 
    {
        public Sprite[] tiles;
        public float TweenTime;
        public Sprite outerGrid;
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
    private SpriteRenderer outerGrid;

    [Inject]
    void OnInjected()   // called post injection
    {
        onGridUpdated += OnGridUpdated;
        MakeTiles();
        MakeOuterGrid();
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
                var spr = grid[i, j];
                int val = inputgrid.GetGrid(i, j);
                spr.sprite = settings.tiles[val];
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

    void MakeOuterGrid()
    {
        GameObject obj = new GameObject("OuterGrid");
        obj.transform.SetParent(this.transform);
        outerGrid = obj.AddComponent<SpriteRenderer>();
        outerGrid.drawMode = SpriteDrawMode.Tiled;
        outerGrid.sprite = settings.outerGrid;
        outerGrid.size = new Vector2(1+GameGridModel.WIDTH, 1+GameGridModel.HEIGHT);
        obj.transform.localPosition = new Vector3(GameGridModel.WIDTH/2, (GameGridModel.HEIGHT+1)/2,0);
    }

    public void AddTween(int x, int y, Vector3 delta, bool linear=false, System.Action onComplete = null)
    {
        var trans = grid[x, y].transform;
        Vector3 tgtpos = trans.position+delta;
        var tween = trans.DOMove(tgtpos, settings.TweenTime);
        if (linear)
        {
            tween = tween.SetEase(Ease.Linear);
        }
        if (onComplete != null)
        {
            tween.OnComplete(()=>onComplete());
        }
    }
    public void RewindTweens()
    {
        DOTween.RewindAll();
    }
}
