using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TwitchChatSettingsUI : MonoBehaviour
{
    public InputField usernameInput;
    public InputField channelNameInput;
    public InputField passwordInput;

    public TwitchChat twitchChat;

    public TextMeshProUGUI failText;
    public GameObject twitchMenu;
    void Update()
    {
        // Tab between text Credentials inputs
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                if(EventSystem.current.currentSelectedGameObject != null)
                {
                    Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                    if (selectable != null)
                        selectable.Select();
                }
            }
            else
            {
                if(EventSystem.current.currentSelectedGameObject != null)
                {
                    Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                    if (selectable != null)
                        selectable.Select();
                }
            }
        }
    }

    public void Connect()
    {
        TwitchChat.TwitchCredentials credentials = new TwitchChat.TwitchCredentials
        {
            ChannelName = channelNameInput.text.ToLower(),
            Username = usernameInput.text.ToLower(),
            Password = passwordInput.text
        };

        if(twitchChat.Connect(credentials))
        {
            Debug.Log("OH YEAH BABY");
            twitchMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Now I'm sad. :(");
            failText.gameObject.SetActive(true);
        }
    }
}
