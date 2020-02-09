using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    List<string> lines;
    int index;
    Player player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
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
        if (index >= lines.Count)
        {
            return null;
        }
        return lines[index++];
    }

    public void EndTalk()
    {
        index = 0;
        player.canMove = true;
    }
}
