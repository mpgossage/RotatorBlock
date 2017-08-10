using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

interface iRotationEnabled
{
    bool IsRotationEnabled { get; }
}

public class RotatorBlockController : MonoBehaviour, iRotationEnabled
{
    [Inject(Id = "left")]
    GameGridModel leftGrid;
    [Inject(Id = "right")]
    GameGridModel rightGrid;
    [Inject]
    Signals.GridUpdated gridUpdated;
    [Inject]
    iSelectionRect selectionRect;
    [Inject]
    iGridTileAnimator animator;

    [SerializeField] // for now
    TextAsset levelJson;

    LevelFormat level;
    private bool isAnimating = false;   // true if user animating
    private bool isFalling = false; // true if system animating falling

    public bool IsRotationEnabled { get { return isAnimating == false && isFalling == false; } }

    // Use this for initialization
    [Inject]
    void OnInject()
    {
        LoadLevel();
        // check if it worked:
        //Debug.LogFormat("Left {0} Right {1}", leftGrid.GetHashCode(), rightGrid.GetHashCode());

    }

    private void Start() // inject is called before start, so we are sure its finished all injections
    {
        gridUpdated.Fire(); // triggers initial setup
    }

    // Update is called once per frame
    void Update ()
    {
        if (IsRotationEnabled)
        {       
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Rotate(false);
            }            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Rotate(true);
            }
        }
    }

    void Rotate(bool clockwise)
    {
        Rect r = selectionRect.GetSelectionRect();
        int left = Mathf.RoundToInt(r.xMin), top = Mathf.RoundToInt(r.yMin),
            right = Mathf.RoundToInt(r.xMax), bottom = Mathf.RoundToInt(r.yMax);
        if (leftGrid.CanRotate(left, top, right, bottom) == false)
            return;

        isAnimating = true;
        System.Action done = () => {
            animator.RewindTweens();    // reset the tweens
            leftGrid.Rotate(left, top, right, bottom, clockwise);
            gridUpdated.Fire();
            this.isAnimating = false;   // animating end
            CheckForFalling();
        };

        if (clockwise)
        {
            // special case LT
            animator.AddTween(left, top, Vector3.right, false, done);
            for (int x = left+1; x < right; x++)  // top row right (not LT)
                animator.AddTween(x, top, Vector3.right);
            for (int y = top; y < bottom; y++)  // right col down
                animator.AddTween(right, y, Vector3.down);
            for (int x = left + 1; x <= right; x++)  // bottom row left (not LB, inc RB)
                animator.AddTween(x, bottom, Vector3.left);
            for (int y = top+1; y <= bottom; y++)  // left col up (not LT, inc LB)
                animator.AddTween(left, y, Vector3.up);
        }
        else
        {
            // special case LT
            animator.AddTween(left, top, Vector3.down, false,done);
            for (int x = left + 1; x <= right; x++)  // top row left (not LT)
                animator.AddTween(x, top, Vector3.left);
            for (int y = top+1; y <= bottom; y++)  // right col up (not RT, inc RB)
                animator.AddTween(right, y, Vector3.up);
            for (int x = left; x < right; x++)  // bottom row left (not RB, inc LB)
                animator.AddTween(x, bottom, Vector3.right);
            for (int y = top + 1; y < bottom; y++)  // left col down (not LT, not LB)
                animator.AddTween(left, y, Vector3.down);
        }   
    }

    void CheckForFalling()
    {
        var pairs = new List<KeyValuePair<int, int>>();
        if (leftGrid.CheckForFalling(pairs))
        {
            System.Action done = () => {
                animator.RewindTweens();    // reset the tweens
                for (int i = 0; i < pairs.Count; i++)
                {
                    leftGrid.MoveDown(pairs[i].Key,pairs[i].Value);
                }
                gridUpdated.Fire();
                this.isFalling = false;   // falling end (for now)
                CheckForFalling();  // check again
            };
            // drop them all
            animator.AddTween(pairs[0].Key, pairs[0].Value,Vector3.down,true,done);
            for(int i=1;i<pairs.Count;i++)
            {
                animator.AddTween(pairs[i].Key, pairs[i].Value, Vector3.down,true);
            }
            this.isFalling = true;   // we are falling
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
