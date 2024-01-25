using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] chunks;

    public delegate void GenerateScenario(Vector3 pos);
    public static GenerateScenario Generate;

    private void Awake()
    {
        Generate += GenerateNewPieces;
    }

    public void GenerateNewPieces(Vector3 pos)
    {
        int x = 0;
        foreach (Transform child in transform) { x++; }
        if (x ==5) { Destroy(transform.GetChild(0).gameObject); }
        Transform lastChunk = transform.GetChild(x-1);
        Vector3 position = lastChunk.Find("ScenarioPieceEnd").position;
        GameObject chunk = Instantiate(chunks[0], position, Quaternion.identity);
        chunk.transform.parent = transform;
    }
}
