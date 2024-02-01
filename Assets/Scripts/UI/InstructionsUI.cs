using System;
using UnityEngine;
using UnityEngine.UI;
public class InstructionsUI : MonoBehaviour
{
    [SerializeField] private Text messageText;
    [SerializeField] private Button nextButton;
    [SerializeField] private EventClick eventClick;
    [SerializeField] private string[] instructionsArray;
    [SerializeField] private FireExtinguisher fireExtinguisher;

    private Text currentText;
    private string textToWrite;
    private int characterIndex;
    private int instructionIndex;
    private float timePerCharacter;
    private float timer;
    private bool ableToClick3DObjects;
    private bool invisibleCharacters;


    private void Start()
    {
        AddEventListeners();

        nextButton.onClick.AddListener(() =>
        {
            messageText.text = "";
            instructionIndex++;

            if (instructionIndex < instructionsArray.Length - 1)
            {
                AddWriter(messageText, instructionsArray[instructionIndex], .04f, true);
            }
            if (instructionIndex == 1)
            {
                fireExtinguisher.showBoltUI();
                ableToClick3DObjects = true;
            }
            if (instructionIndex == instructionsArray.Length)
            {
                Hide();
            }
            HideButton();
        });
        AddWriter(messageText, instructionsArray[instructionIndex], .04f, true);
    }
    private void Update()
    {
        if (currentText != null)
        {
            timer -= Time.deltaTime;
            while (timer <= 0f)
            {
                timer += timePerCharacter;
                characterIndex++;
                string text = textToWrite.Substring(0, characterIndex);
                if (invisibleCharacters)
                {
                    text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                }
                currentText.text = text;

                if (characterIndex >= textToWrite.Length)
                {
                    currentText = null;
                    if (!nextButton.gameObject.activeInHierarchy && instructionIndex == 0) ShowButton();
                    return;
                }
            }
        }
    }

    private void AddEventListeners()
    {
        eventClick.OnBoltClicked += EventClick_OnBoltClicked;
        eventClick.OnNozzleClicked += EventClick_OnNozzleClicked;
        eventClick.OnFirstTimeHeightChange += EventClick_OnFirstTimeHeightChange;
        eventClick.OnFirstTimeUseExtinguisher += EventClick_OnFirstTimeUseExtinguisher;
    }

    private void EventClick_OnFirstTimeUseExtinguisher(object sender, EventArgs e)
    {
        NextInstruction();
        nextButton.GetComponentInChildren<Text>().text = "Zamknij";
        ShowButton();
    }

    private void EventClick_OnFirstTimeHeightChange(object sender, EventArgs e)
    {
        NextInstruction();
    }

    private void EventClick_OnNozzleClicked(object sender, EventArgs e)
    {
        NextInstruction();
    }

    private void EventClick_OnBoltClicked(object sender, EventArgs e)
    {
        NextInstruction();
    }

    private void NextInstruction()
    {
        messageText.text = "";
        instructionIndex++;
        AddWriter(messageText, instructionsArray[instructionIndex], .04f, true);
    }

    public void AddWriter(Text text, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        this.currentText = text;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        this.invisibleCharacters = invisibleCharacters;
        characterIndex = 0;
    }

    // Activate/Deactivate objects
    public void ShowButton(){ nextButton.gameObject.SetActive(true);}
    public void HideButton() { nextButton.gameObject.SetActive(false);}
    private void Hide() { gameObject.SetActive(false); }


    // Getters
    public bool IsAbleToClick3DObjects() { return ableToClick3DObjects; }
}
