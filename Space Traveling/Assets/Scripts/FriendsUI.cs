using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendsUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Button addFriendButton;
    public TMP_Text friendNameText;
    private NetworkManager networkManager;
    void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        addFriendButton.onClick.AddListener(addFriend);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void show()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void hide()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void addFriend()
    {
        string friendName = friendNameText.text.Trim((char)8203);
        AddFriendMessage message = new AddFriendMessage();
        message.friendName = friendName;
        networkManager.sendMessage(message);
    }
}
