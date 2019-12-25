using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{
    // Start is called before the first frame update
    private NetworkManager networkManager;
    public Button loginButton;
    public Button registerButton;
    public TMP_Text usernameText;
    public TMP_Text passwordText;

    void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        loginButton.onClick.AddListener(OnLoginClick);
        registerButton.onClick.AddListener(OnRegisterClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnLoginClick()
    {
        LoginMessage loginMessage = new LoginMessage();
        loginMessage.username = usernameText.text.Trim((char)8203);
        loginMessage.password = passwordText.text.Trim((char)8203);
        networkManager.sendMessage(loginMessage);
    }
    void OnRegisterClick()
    {
        RegisterMessage registerMessage = new RegisterMessage();
        registerMessage.username = (string)usernameText.text.Trim((char)8203);
        registerMessage.password = (string)passwordText.text.Trim((char)8203);
        Debug.Log(registerMessage.Serialize());
        networkManager.sendMessage(registerMessage);
    }
}
