using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    GameObject promptTalk;
    GameObject dialogue;

    string dialogueLine;
    int dialogueLineIndex;
    Text textDialogue;
    Interactable talker;

    public bool isCooledDown;

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
        textDialogue = GameObject.Find("Dialogue/Text").GetComponent<Text>();
        HideTalkPrompt();
        HideDialogue();

        dialogueLineIndex = 0;
        isCooledDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (talker && isCooledDown && Input.GetButton("Fire1"))
        {
            ShowNextLine();
            StartCoroutine(CooldownAct());
        }
    }

    public void ShowTalkPrompt(GameObject interactable)
    {
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
        talker = interactable;
    }

    void EndDialogue()
    {
        textDialogue.text = "";
        dialogueLineIndex = 0;
        dialogueLine = "";
        talker.EndTalk();
        HideDialogue();
        talker = null;
    }

    public void ShowNextLine()
    {
        textDialogue.text = "";
        dialogueLineIndex = 0;
        dialogueLine = talker.NextLine();
        if (!string.IsNullOrEmpty(dialogueLine))
        {
            ShowDialogue();
            StartCoroutine(ScrollLine());
        } 
        else
        {
            EndDialogue();
        }
    }

    IEnumerator ScrollLine()
    {
        yield return new WaitForSeconds(0.01f);
        if (dialogueLineIndex < dialogueLine.Length)
        {
            textDialogue.text += dialogueLine[dialogueLineIndex++];
            StartCoroutine(ScrollLine());
        }
    }

    public IEnumerator CooldownAct()
    {
        isCooledDown = false;
        yield return new WaitForSeconds(0.5f);
        isCooledDown = true;
    }
}
