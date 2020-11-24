using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InputField nameInputField = null;
    [SerializeField] private Button joinLobbyButton = null; 

    public static string DisplayName { get; set; }

    private const string PlayerName = "DefaultName";

    private void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        if(!PlayerPrefs.HasKey(PlayerName)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerName);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        //joinLobbyButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerName, DisplayName);
    }
}
