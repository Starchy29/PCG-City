using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject TopLeft;
    [SerializeField] private GameObject TopRight;
    [SerializeField] private GameObject BottomLeft;
    [SerializeField] private GameObject BottomRight;
    private static Dictionary<Vector2Int, List<Vector3>> placedSpots = new Dictionary<Vector2Int, List<Vector3>>();

    void Start() {
        // destroy buildings placed on the same spot
        Vector2Int region = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        if(placedSpots.ContainsKey(region)) {
            foreach(Vector3 position in placedSpots[region]) {
                if(Vector3.Distance(position, transform.position) <= 0.1f) {
                    Destroy(gameObject);
                    return;
                }
            }        
        } else {
            placedSpots[region] = new List<Vector3>();
        }

        // register this building location
        placedSpots[region].Add(transform.position);

        // randomly choose which corners to eliminate
        int numRemoved = 0;
        GameObject[] corners = new GameObject[4] { TopLeft, TopRight, BottomLeft, BottomRight };
        foreach(GameObject corner in corners) {
            if(Random.value <= 0.3f - 0.1f * numRemoved) {
                Destroy(corner);
                numRemoved++;
            }
        }
    }
}
