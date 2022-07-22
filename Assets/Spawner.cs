using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
     public GameObject[] groups;
     public void spawnNext() {
    // Random Index
    int i = Random.Range(0, groups.Length);

    Instantiate(groups[i],
                transform.position,
                Quaternion.identity);

    }
    
void Start() {
    // Spawn initial Group
    spawnNext();
}
}
