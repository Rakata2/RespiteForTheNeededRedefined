using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class NPCSpawner : MonoBehaviour
{
    public GameObject[] ShelterPrefab;
    public GameObject[] FoodPrefab;

    public Transform LeftSpawnPoint;
    public Transform RightSpawnPoint;
    public Transform CenterPoint;

    public float spawnInterval = 3f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        
        GameObject ChosenPrefab;
        Transform selectedSpawnPoint;

        int RequestRandomizer = Random.Range(0, 2);


        if (RequestRandomizer == 0)
        {
            selectedSpawnPoint = LeftSpawnPoint;
            ChosenPrefab = ShelterPrefab[Random.Range(0, ShelterPrefab.Length)];
        }
        else
        {
            selectedSpawnPoint = RightSpawnPoint;
            ChosenPrefab = FoodPrefab[Random.Range(0, FoodPrefab.Length)];
        }

        GameObject NewNPC = Instantiate(ChosenPrefab, selectedSpawnPoint.position, Quaternion.identity);

        NPCMovement MovementScript = NewNPC.GetComponent<NPCMovement>();
        if(MovementScript != null)
        {
            MovementScript.SpawnPoint = selectedSpawnPoint;
            MovementScript.CenterPoint = this.CenterPoint;
        }

    }
}