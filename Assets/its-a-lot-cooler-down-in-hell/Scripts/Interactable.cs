using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    List<string> lines;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        lines = new List<string>();
        lines.Add("First line");
        lines.Add("Second line");
        lines.Add("Third line");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Talk()
    {
        UIManager.instance.StartDialogue(this);
    }

    public string NextLine()
    {
        return lines[index++];
    }
}
