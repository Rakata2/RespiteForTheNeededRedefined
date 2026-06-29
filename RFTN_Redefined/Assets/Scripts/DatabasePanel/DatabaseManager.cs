using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    public List<IdentityProfile> ShuffledProfiles;
    public int TotalPage = 4;

    public List<IdentityProfile> AllProfiles = new List<IdentityProfile>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ShuffleDatabase();
        }
        else Destroy(gameObject);
    }


    private void ShuffleDatabase()
    {
        ShuffledProfiles = new List<IdentityProfile>(AllProfiles);
        for (int i = 0; i < ShuffledProfiles.Count; i++)
        {
            IdentityProfile temp = ShuffledProfiles[i];
            int randomIndex = Random.Range(i, ShuffledProfiles.Count);
            ShuffledProfiles[i] = ShuffledProfiles[randomIndex];
            ShuffledProfiles[randomIndex] = temp;
        }
    }

    public bool IsNPCIsVisibleInDatabse(IdentityProfile profile)
    {
        if (profile == null) return false;

        if (ShuffledProfiles == null || ShuffledProfiles.Count == 0) ShuffleDatabase();

        int MaxPickedSlots = TotalPage * 2; //4 * 2 = 8, 8 max data, no more

        for (int i = 0; i < MaxPickedSlots; i++)
        {
            if (i >= ShuffledProfiles.Count) break;

            if (ShuffledProfiles[i] == profile) return true;
        }
        return false;
    }
}
