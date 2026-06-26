using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class NPCSpawner : MonoBehaviour
{
    public GameObject[] ShelterPrefab;

    public Transform LeftSpawnPoint;
    public Transform CenterPoint;
    public Transform ExitPointShelter;
    public Transform ExitPointShelterFailed;
    public NPCMovement CurrentNPC;

    public float spawnInterval = 3f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => CurrentNPC == null);

            yield return new WaitForSeconds(spawnInterval);


            Transform selectedSpawnPoint = LeftSpawnPoint;
            GameObject ChosenPrefab = ShelterPrefab[Random.Range(0, ShelterPrefab.Length)];
            GameObject NewNPC = Instantiate(ChosenPrefab, selectedSpawnPoint.position, Quaternion.identity);

            NPCMovement MovementScript = NewNPC.GetComponent<NPCMovement>();
            if (MovementScript != null)
            {
                MovementScript.SpawnPoint = selectedSpawnPoint;
                MovementScript.CenterPoint = this.CenterPoint;

                MovementScript.ExitPointShelter = this.ExitPointShelter;
                MovementScript.ExitPointShelterFailed = this.ExitPointShelterFailed;

                CurrentNPC = MovementScript;
            }
        }
    }
}