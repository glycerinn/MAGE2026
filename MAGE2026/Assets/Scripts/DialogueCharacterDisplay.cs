
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
    }

    public DialogueRunner dialogueRunner;

    [Header("Character Display")]
    public Image characterImage;
    public TMP_Text characterNameText;

    [Header("Available Characters")]
    public Character[] characters;

    void Start()
    {
        dialogueRunner.AddCommandHandler<string>("show_character", ShowCharacter);
        dialogueRunner.AddCommandHandler("hide_character", HideCharacter);
    }

    public void ShowCharacter(string characterName)
    {
        foreach (Character character in characters)
        {
            if (character.characterName == characterName)
            {
                if (characterImage != null)
                    characterImage.sprite = character.sprite;

                if (characterNameText != null)
                    characterNameText.text = character.characterName;

                if (characterImage != null)
                    characterImage.enabled = character.sprite != null;

                return;
            }
        }

        Debug.LogWarning("Character not found: " + characterName);
    }

    public void HideCharacter()
    {
        if (characterImage != null)
            characterImage.enabled = false;

        if (characterNameText != null)
            characterNameText.text = "";
    }
}
