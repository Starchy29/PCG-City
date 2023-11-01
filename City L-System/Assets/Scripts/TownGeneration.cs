using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGeneration : MonoBehaviour
{
    [SerializeField] private int iterations;
    private Dictionary<char, string> ruleset;

    void Start()
    {
        CreateRuleset();
        string formula = GenerateFormula("-", iterations);
        SpawnCity(formula);
    }

    private void CreateRuleset()
    {
        ruleset = new Dictionary<char, string>();

        ruleset['-'] = "L-R-";
    }

    private string GenerateFormula(string startState, int iterations) {
        string current = startState;
        for(int i = 0; i < iterations; i++) {
            string next = "";
            foreach(char letter in current) {
                if(ruleset.ContainsKey(letter)) {
                    next += ruleset[letter];
                }
            }
            current = next;
        }

        return current;
    }

    private void SpawnCity(string formula) {
        Debug.Log(formula);
    }
}
