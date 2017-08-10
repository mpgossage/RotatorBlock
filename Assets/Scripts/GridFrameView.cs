using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

interface iSelectionRect
{
    Rect GetSelectionRect();
}

// note: there is a ZenjectBinding behaviour on this object
// which allows the interface to be injected into other objects
public class GridFrameView : MonoBehaviour, iSelectionRect
{
    // this will be set in the set in the SettingsInstaller ScriptableObject
    [System.Serializable]
    public struct Settings
    {
        public Sprite sprite;
        public float fadeTime, maxAlpha, minAlpha;
    }

    [Inject]
    Settings settings;

    [Inject]
    iRotationEnabled rotationsEnabled;

    private SpriteRenderer frame;
    private Rect selection;    // points in GRID coords
    private Vector2 startPos;
    private Color currentColoúr;

    [Inject]
    void OnInjected()   // called post injection
    {
        MakeFrame();
        selection = new Rect(-100,-100,-100,-100);        
    }

    public Rect GetSelectionRect()
    {
        return selection;
    }

    void MakeFrame()
    {
        var obj = new GameObject("Frame");
        obj.transform.SetParent(this.transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        frame = obj.AddComponent<SpriteRenderer>();
        frame.drawMode = SpriteDrawMode.Sliced;  // 9 slices :-)
        frame.sprite = settings.sprite;
        frame.sortingOrder = 1000; // at front

        currentColoúr = Color.white;
        currentColoúr.a = (rotationsEnabled.IsRotationEnabled) ? settings.maxAlpha : settings.minAlpha;
    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))    // on click: works with mouse & touch
        {
            Vector2 pos=GetMouseGridPos();
            if (isValidGridPos(pos))
            {
                startPos = pos;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = GetMouseGridPos();
            if (isValidGridPos(pos))
            {
                // store grid coords
                selection = Rect.MinMaxRect(Mathf.Min(startPos.x, pos.x), Mathf.Min(startPos.y, pos.y),
                                            Mathf.Max(startPos.x, pos.x), Mathf.Max(startPos.y, pos.y));
                // update the sprite
                Vector2 centre = (startPos + pos) / 2.0f;
                centre.y = GameGridModel.HEIGHT - centre.y; // grid=>screen offset
                Vector2 size = new Vector2(Mathf.Abs(startPos.x - pos.x)+1, Mathf.Abs(startPos.y - pos.y)+1);
                frame.transform.localPosition = centre;
                frame.size = size;
            }
        }
        // disable/enable rendering
        frame.enabled = selection.xMax >= 0;
        // update Alpha
        float deltaAlpha = Time.deltaTime * (settings.maxAlpha - settings.minAlpha) / settings.fadeTime;
        if (rotationsEnabled.IsRotationEnabled == false)
            deltaAlpha = -deltaAlpha;
        currentColoúr.a = Mathf.Clamp(currentColoúr.a + deltaAlpha, settings.minAlpha, settings.maxAlpha);
        frame.color = currentColoúr;
    }
    // get mouse in GRID position
    private Vector2 GetMouseGridPos()
    {
        // does it touch any of mine?
        Vector3 mouseWorldPos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // raycasts will not work as there is no collisions on the sprites
        Vector3 relPos = mouseWorldPos3D - transform.position;
        // convert to grid (round as the sprite is centred)
        int gridX = Mathf.RoundToInt(relPos.x), gridY = Mathf.RoundToInt(relPos.y);
        return new Vector2(gridX, GameGridModel.HEIGHT - gridY);   // offset for inverted grid etc.
    }
    private bool isValidGridPos(Vector2 pos)
    {
        return (pos.x >= 0 && pos.x < GameGridModel.WIDTH && pos.y >= 0 && pos.y < GameGridModel.HEIGHT);
    }
}
