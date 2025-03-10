using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourData : MonoBehaviour
{
    public string name;
    public int age;

    [SerializeField] private int hp;
    [SerializeField] HashSet<int> Tests;


    private void OnEnable()
    {
        
    }
}
