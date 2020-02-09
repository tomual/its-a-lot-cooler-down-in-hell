using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    GameObject promptTalk;
    GameObject dialogue;

    string dialogueLine;
    int dialogueLineIndex;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        promptTalk = GameObject.Find("PromptTalk");
        dialogue = GameObject.Find("Dialogue");
        HideTalkPrompt();
        HideDialogue();

        dialogueLineIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTalkPrompt(GameObject interactable)
    {
        Debug.Log(interactable.transform.position);
        GameObject target = interactable;
        RectTransform canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();

        float offsetPosY = target.transform.position.y + 1.5f;
        Vector3 offsetPos = new Vector3(target.transform.position.x, offsetPosY, target.transform.position.z);
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
        promptTalk.transform.localPosition = canvasPos;
        promptTalk.SetActive(true);
    }

    public void HideTalkPrompt()
    {
        promptTalk.SetActive(false);
    }

    public void ShowDialogue()
    {
        dialogue.SetActive(true);
    }

    public void HideDialogue()
    {
        dialogue.SetActive(false);
    }

    public void StartDialogue(Interactable interactable)
    {
        dialogueLineIndex = 0;
        dialogueLine = interactable.NextLine();
        ShowDialogue();
        StartCoroutine(ScrollLine());
    }

    IEnumerator ScrollLine()
    {

        yield return new WaitForSeconds(0.2f);
    }
}
