using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGeneration : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;
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

        // ruleset['T'] = "F<FTB>T>FTB<B"; this one rule accomplishes the layout, but with a lot of overlap

        ruleset['T'] = "F<FRB>T>FLB<B";
        ruleset['L'] = "<FRB>";
        ruleset['R'] = ">FLB<";
        //ruleset['L'] = "F<F B>M>FRB<B";
        //ruleset['R'] = "F<FLB>M>F B<B";
       // ruleset['M'] = "FTB";
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
                    road.transform.position = (startPos + position) / 2;

                    if(Random.value < 0.2f) {
                        Destroy(road);
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
