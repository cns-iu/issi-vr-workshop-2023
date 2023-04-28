using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVtoSO : MonoBehaviour
{
    private static string root = "Assets/Editor/CSVs";

    [MenuItem("Utilities/IngestCorrelationMatrices")]
    public static void GetCorrelationMatrices()
    {

        foreach (string file in Directory.GetFiles(root, "*.csv"))
        {
            string folder = Path.GetFileName(file).Split('.')[0];

            string[] allLines = File.ReadAllLines(file);

            string[] colHeaders = allLines[0].Split(',');
            List<string> nodeIds = new List<string>();

            foreach (var h in colHeaders)
            {
                if (h == "HANDLE") continue;
                nodeIds.Add(h);
            }

            foreach (var line in allLines)
            {
                string[] splitData = line.Split(',');
                if (splitData[0] == "HANDLE") continue;


                string node = splitData[0];


                CorrelationMatrix matrix = ScriptableObject.CreateInstance<CorrelationMatrix>();
                matrix.id = node;

                for (int i = 0; i < nodeIds.Count; i++)
                {
                    bool isFloat = float.TryParse(splitData[i + 1], out float value);

                    matrix.rows.Add(new MatrixCell(nodeIds[i], value, isFloat));

                }




                AssetDatabase.CreateAsset(matrix, $"Assets/Resources/CorrelationMatrices/{folder}/{node}.asset");

            }

        }

        AssetDatabase.SaveAssets();

    }
}
