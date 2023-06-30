using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHEnemyManager : MonoBehaviour
{
    public bool spawnSomeEnemies;
    [Space(5)]

    public float distanceBetween;
    public GameObject fishControllerPrefab;
    public Vector3 followerOffset;
    public int schoolSize;

    private void Update()
    {
        //Currently a manual bool check inside the editor inspector
        if(spawnSomeEnemies)
        {
            GameObject newFishController = Instantiate(fishControllerPrefab, transform.position, transform.rotation);
            FishSchoolController schoolController = newFishController.GetComponent<FishSchoolController>();

            schoolController.fishToSpawn.Clear();

            for (int i = 0; i < schoolSize; i++)
            {
                schoolController.fishToSpawn.Add(null);  // Add new elements to the list
            }

            //Spawn the leader of the school. The rest of the school will follow inside the FishSchoolController
            schoolController.SpawnSchool(followerOffset, distanceBetween);

            //Turn the bool back off so I can be used again.
            spawnSomeEnemies = false;
        }
    }

}
