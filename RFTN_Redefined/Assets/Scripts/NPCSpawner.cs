using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefab;

    public Transform SpawnPoint;
    public Transform CenterPoint;

    public float spawnInterval = 3f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        int RandomIndex = Random.Range(0, npcPrefab.Length);
        GameObject ChosenPrefab = npcPrefab[RandomIndex];
        GameObject NewNPC = Instantiate(ChosenPrefab, SpawnPoint.position, Quaternion.identity);  
        NPCMovement MovementScript = NewNPC.GetComponent<NPCMovement>();
        if (MovementScript != null)
        {
            MovementScript.SpawnPoint = this.SpawnPoint;
            MovementScript.CenterPoint = this.CenterPoint;  
        }
    }
}