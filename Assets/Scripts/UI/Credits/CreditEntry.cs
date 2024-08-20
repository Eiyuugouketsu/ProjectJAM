using System.Collections.Generic;
using UnityEngine;

public class CreditEntry : MonoBehaviour
{
    public string Title { get; set;}
    public List<string> People { get; set; }

    public CreditEntry(string title, List<string> people)
    {
        Title = title;
        People = people;
    }
}
