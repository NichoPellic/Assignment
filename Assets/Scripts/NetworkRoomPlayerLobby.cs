using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lobbyUi = null;
    [SerializeField] private Text[] playerNameTexts = new Text[4]; //Old, can probably be removed
    [SerializeField] private Text[] blueTeamText = new Text[5];
    [SerializeField] private Text[] redTeamText = new Text[5];
    [SerializeField] private Text[] blueReadyText = new Text[5];
    [SerializeField] private Text[] redReadyText = new Text[5];
    [SerializeField] private Text[] playerReadyText = new Text[4]; //Old, can probably be removed
    [SerializeField] private Button startGameButton = null;    

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;

    //Sets player team
    private string team = "blue";

    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerTheReturn room;

    private NetworkManagerTheReturn Room
    {
        get
        {
            if (room != null) return room;

            return room = NetworkManager.singleton as NetworkManagerTheReturn;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);

        lobbyUi.SetActive(true);
    }

    public override void OnStartClient()
    {
        if (Room.RoomPlayers.Count % 2 == 0) team = "blue";

        else team = "red";

        Room.RoomPlayers.Add(this);  

        UpdateDisplay();
    }

    //public override void OnNetworkDestroy()
    //{
    //}

    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!isLocalPlayer)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.isLocalPlayer)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }


        /*
        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for player...";
            playerReadyText[i].text = string.Empty;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyText[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not ready</color>";
        }
        */

        for(int i = 0; i < blueTeamText.Length; i++)
        {
            blueTeamText[i].text = "Waiting for player";
            blueReadyText[i].text = string.Empty;
        }

        for (int i = 0; i < redTeamText.Length; i++)
        {
            redTeamText[i].text = "Waiting for player";
            redReadyText[i].text = string.Empty;
        }

        int incBlue = 0;
        int incRed = 0;

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            if(Room.RoomPlayers[i].team == "blue")
            {
                blueTeamText[incBlue].text = Room.RoomPlayers[i].DisplayName;
                blueReadyText[incBlue++].text = Room.RoomPlayers[i].IsReady ?
                    "<color=green>Ready</color>" :
                    "<color=red>Not ready</color>";
            }

            else
            {
                redTeamText[incRed].text = Room.RoomPlayers[i].DisplayName;
                redReadyText[incRed++].text = Room.RoomPlayers[i].IsReady ?
                    "<color=green>Ready</color>" :
                    "<color=red>Not ready</color>";
            }
        }
    }

    public void HandleReadyStart()
    {
        if (!isLeader) return;

        startGameButton.interactable = startGameButton;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (!Room.IsReadyToStart()) return;

        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) return;

        //Start game here
        Debug.Log("Game start!");
    }
}