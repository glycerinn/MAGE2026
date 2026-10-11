using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiagnosisSO", menuName = "Game/DiagnosisSO")]
public class DiagnosisSO : ScriptableObject
{
    [System.Serializable]
    public class Condition
    {
        public string source;
        public string value;
    }

    [System.Serializable]
    public class DiagnosisRule
    {
        public string diagnosis;
        public List<Condition> conditions = new List<Condition>();
    }

    public List<DiagnosisRule> rules = new List<DiagnosisRule>();

    public string GetDiagnosis(Dictionary<string, string> observations)
    {
        for (int i = 0; i < rules.Count; i++)
        {
            DiagnosisRule rule = rules[i];
            bool matches = true;

            for (int j = 0; j < rule.conditions.Count; j++)
            {
                Condition condition = rule.conditions[j];

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
                return rule.diagnosis;
        }

        return "";
    }

    public string GetRequiredObservation(string source)
    {
        if (rules == null || rules.Count == 0)
            return "";

        for (int i = 0; i < rules[0].conditions.Count; i++)
        {
            Condition condition = rules[0].conditions[i];

            if (condition.source == source)
                return condition.value;
        }

        return "";
    }
}