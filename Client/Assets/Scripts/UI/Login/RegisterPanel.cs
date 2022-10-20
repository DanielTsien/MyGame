using Google.Protobuf;
using MyGame.Proto;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class RegisterPanel : PanelBase
    {
        public TMP_InputField UsernameInput;
        public TMP_InputField PasswordInput;
        public Button RegisterBtn;

        private void Start()
        {
            this.RegisterNetEvent(PacketId.UserRegisterResponse, OnRegisterResp);
        }

        private void OnRegisterResp(IMessage msg)
        {
            if (msg is not UserRegisterResponse message) return;
            
            if (message.Result == RESULT.Success)
            {
                SceneManager.LoadScene("CreateCharacter");
            }
            else
            {
                Debug.LogError(message.Errormsg);
            }
            
        }

        public void Register()
        {
            if (!string.IsNullOrEmpty(UsernameInput.text) || !string.IsNullOrEmpty(PasswordInput.text))
            {
                this.SendNetMessage(PacketId.UserRegisterRequest, new UserRegisterRequest
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
