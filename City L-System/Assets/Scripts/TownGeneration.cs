using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGeneration : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;
    [SerializeField] private GameObject BuildingPrefab;
    [SerializeField] private int iterations;
    private Dictionary<char, string> ruleset;

    void Start()
    {
        CreateRuleset();
        string formula = GenerateFormula("T>T>T>T", iterations);
        SpawnCity(formula);
    }

    private void CreateRuleset()
    {
        ruleset = new Dictionary<char, string>();

        ruleset['T'] = "F<FRB>T>FLB<B";
        ruleset['L'] = "<FRB>";
        ruleset['R'] = ">FLB<";
    }

    private string GenerateFormula(string startState, int iterations) {
        string current = startState;
        for(int i = 0; i < iterations; i++) {
            string next = "";
            foreach(char letter in current) {
                if(ruleset.ContainsKey(letter)) {
                    next += ruleset[letter];
                } else {
                    next += letter;
                }
            }
            current = next;
        }

        return current;
    }

    private void SpawnCity(string formula) {
        float roadLength = RoadPrefab.transform.localScale.x - RoadPrefab.transform.localScale.y;
        float rotation = 0;
        Vector3 position = Vector3.zero;

        foreach(char letter in formula) {
            Vector3 direction = new Vector3(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad), 0);

            switch(letter) {
                case 'F':
                    GameObject road = Instantiate(RoadPrefab);
                    road.transform.rotation = Quaternion.Euler(0, 0, rotation);

                    Vector3 startPos = position;
                    position += direction * roadLength;
                    Vector3 roadMiddle = (startPos + position) / 2;
                    road.transform.position = roadMiddle;

                    // place adjacent buildings
                    float distFromRoad = 0.3f;
                    Vector3 roadDirection = (position - startPos).normalized;
                    Vector3 end = roadMiddle + (roadLength / 2 - distFromRoad) * roadDirection;
                    Vector3 start = roadMiddle + (roadLength / 2 - distFromRoad) * -roadDirection;
                    if (Random.value < 0.2f) {
                        // delete some roads for a more interesting layout
                        Destroy(road);
                        Instantiate(BuildingPrefab).transform.position = end;
                        Instantiate(BuildingPrefab).transform.position = start;
                    } else {
                        Vector3 perp = new Vector3(-roadDirection.y, roadDirection.x, 0);
                        Vector3 left = distFromRoad * perp;
                        Vector3 right = distFromRoad * -perp;

                        Instantiate(BuildingPrefab).transform.position = roadMiddle + left;
                        Instantiate(BuildingPrefab).transform.position = roadMiddle + right;
                        Instantiate(BuildingPrefab).transform.position = end + left;
                        Instantiate(BuildingPrefab).transform.position = end + right;
                        Instantiate(BuildingPrefab).transform.position = start + left;
                        Instantiate(BuildingPrefab).transform.position = start + right;
                    }
                    break;

                case 'B':
                    position += -direction * roadLength;
                    break;
                    
                case '<':
                    rotation += 90f;
                    break;

                case '>':
                    rotation -= 90f;
                    break;
            }
        }
    }
}
