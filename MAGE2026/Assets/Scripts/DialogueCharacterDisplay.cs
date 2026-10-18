
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueCharacterDisplay : MonoBehaviour
{
    [System.Serializable]
    public class Character
    {
        public string characterName;
        public Sprite sprite;
        public bool showOnRight;
    }

    [Header("Dialogue")]
    public DialogueRunner dialogueRunner;

    [Header("Character Bob")]
    public DialogueBob leftCharacterBob;
    public DialogueBob rightCharacterBob;

    [Header("Character Display")]
    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public TMP_Text characterNameText;

    [Header("Available Characters")]
    public Character[] characters;

    void Start()
    {
        if (dialogueRunner == null)
        {
            Debug.LogWarning("DialogueRunner is not assigned.");
            return;
        }

        dialogueRunner.AddCommandHandler<string>("show_character", ShowCharacter);
        dialogueRunner.AddCommandHandler("hide_character", HideCharacter);
    }

    
    public void ShowCharacter(string characterName)
    {
        foreach (Character character in characters)
        {
            if (character.characterName != characterName)
                continue;

            bool showRight = character.showOnRight;
            Image activeImage = showRight ? rightCharacterImage : leftCharacterImage;
            Image inactiveImage = showRight ? leftCharacterImage : rightCharacterImage;
            DialogueBob activeBob = showRight ? rightCharacterBob : leftCharacterBob;
            DialogueBob inactiveBob = showRight ? leftCharacterBob : rightCharacterBob;

            if (inactiveBob != null)
                inactiveBob.Hide();

            if (inactiveImage != null)
                inactiveImage.enabled = false;

            if (activeImage != null)
            {
                activeImage.sprite = character.sprite;
                activeImage.enabled = character.sprite != null;
            }

            if (activeBob != null && character.sprite != null)
                activeBob.Show();

            if (characterNameText != null)
                characterNameText.text = character.characterName;

            return;
        }

        Debug.LogWarning("Character not found: " + characterName);
    }


    public void HideCharacter()
    {
        if (leftCharacterImage != null)
            leftCharacterImage.enabled = false;

        if (rightCharacterImage != null)
            rightCharacterImage.enabled = false;

        if (characterNameText != null)
            characterNameText.text = "";
    }

    void OnDestroy()
    {
        if (dialogueRunner == null)
            return;

        dialogueRunner.RemoveCommandHandler("show_character");
        dialogueRunner.RemoveCommandHandler("hide_character");
    }
}
