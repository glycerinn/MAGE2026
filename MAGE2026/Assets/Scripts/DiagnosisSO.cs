using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiagnosisSO", menuName = "Game/Diagnosis SO")]
public class DiagnosisSO : ScriptableObject
{
    [System.Serializable]
    public class DiagnosisRule
    {
        public string diagnosis;
        public List<Condition> conditions = new List<Condition>();
    }

    [System.Serializable]
    public class Condition
    {
        public string source;
        public string value;
    }

    public List<DiagnosisRule> rules = new List<DiagnosisRule>();

    public string GetDiagnosis(Dictionary<string, string> observations)
    {
        for (int i = 0; i < rules.Count; i++)
        {
            bool matches = true;

            for (int j = 0; j < rules[i].conditions.Count; j++)
            {
                Condition condition = rules[i].conditions[j];

                if (!observations.TryGetValue(condition.source, out string value))
                {
                    matches = false;
                    break;
                }

                if (value != condition.value)
                {
                    matches = false;
                    break;
                }
            }

            if (matches)
                return rules[i].diagnosis;
        }

        return "";
    }
}