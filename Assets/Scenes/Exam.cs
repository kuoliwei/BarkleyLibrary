using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(Num(new int[] { 3,5,3,4}, 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int Num(int[] people, int limit)
    {
        Array.Sort(people);
        int i = 0, j = people.Length - 1;
        int boatNum = 0;

        while (i <= j)
        {
            if (people[i] + people[j] <= limit)
            {
                i++;
            }
            j--;
            boatNum++;
        }

        return boatNum;
    }

}
