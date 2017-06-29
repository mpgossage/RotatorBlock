using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelLoad : MonoBehaviour
{
    [SerializeField]
    private TextAsset toLoad;

	// Use this for initialization
	void Start ()
    {
        string data = toLoad.text; // get the text
        Debug.Log("Raw json " + data);
        LevelFormat tl = JsonUtility.FromJson<LevelFormat>(data);
        Debug.Log("Looking good: len "+tl.leftGrid.Length);

        // testing obfuscation on data
        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(data);
        string encodedText = System.Convert.ToBase64String(bytesToEncode);
        Debug.Log("Encode: " + encodedText.Length+" "+encodedText);

    }
}
