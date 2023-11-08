using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    private static Dictionary<Vector3, GameObject> placedBuildings = new Dictionary<Vector3, GameObject>();

    void Start() {
        if(placedBuildings.ContainsKey(transform.position)) {
            Destroy(gameObject);
            Debug.Log("destroyed duplicate");
            return;
        }

        placedBuildings[transform.position] = gameObject;
    }
}
