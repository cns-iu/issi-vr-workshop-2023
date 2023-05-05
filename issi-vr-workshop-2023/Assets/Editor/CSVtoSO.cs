using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class CSVtoSO : MonoBehaviour
{
    private static string root = "Assets/Editor";

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
                if (h == "") continue;
                nodeIds.Add(h);
            }

            for (int i = 1; i < allLines.Length; i++)
            {
                string[] splitLine = allLines[i].Split(',');
                string id = splitLine[0];


                CorrelationMatrix matrix = ScriptableObject.CreateInstance<CorrelationMatrix>();
                matrix.id = id;

                for (int j = 0; j < nodeIds.Count; j++)
                {
                    bool isFloat = float.TryParse(splitLine[j + 1], out float value);

                    matrix.rows.Add(new MatrixCell(nodeIds[j], value, isFloat));

                }

                AssetDatabase.CreateAsset(matrix, $"Assets/ResourcesCorrelationMatrices/{id}.asset");

            }

            AssetDatabase.SaveAssets();

        }
    }
}
