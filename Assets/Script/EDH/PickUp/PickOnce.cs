using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PickOnce : MonoBehaviour
{
    public int Ticket;

    static void CsvForPick()
    {
        string filePath = "Project_P/Assets/Data/EnumID.csv";
        using (StreamReader sr = new StreamReader(filePath))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] values = line.Split(',');
                foreach (var value in values)
                {
                    Console.Write(value + " ");
                }
                Console.WriteLine();
            }
        }
    }
    public void One()
    {
        if (Ticket == 1)
        {
            PickUpCalculator.IsLeader();
            if(PickUpCalculator.IsLeader()== true)
            {
                Unity.Mathematics.Random Range = new Unity.Mathematics.Random();             
            }
            else
            {

            }
        }
    }
}
