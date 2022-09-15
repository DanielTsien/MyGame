using Google.Protobuf;
using MyGame.Proto;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoginPanel : PanelBase
    {
        public TMP_InputField UsernameInput;
        public TMP_InputField PasswordInput;
        public Button LoginBtn;

        private void Start()
        {
            this.RegisterNetEvent(PacketId.UserLoginResponse, OnLoginResp);
        }

        private void OnLoginResp(IMessage msg)
        {
            if (msg is not UserLoginResponse message) return;

            if (message.Result == Result.Success)
            {
                SceneManager.LoadScene("CreateCharacter");
            }
            else
            {
                Debug.LogError(message.Errormsg);
            }
        }

        public void Login()
        {
            if (!string.IsNullOrEmpty(UsernameInput.text) || !string.IsNullOrEmpty(PasswordInput.text))
            {
                this.SendNetMessage(PacketId.UserLoginRequest, new UserLoginRequest
                {
                    Username = UsernameInput.text,
                    Passward = PasswordInput.text
                });
                UsernameInput.text = "";
                PasswordInput.text = "";
            }
        }
    }
}
