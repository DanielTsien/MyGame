using System.Collections;
using System.Collections.Generic;
using MyGame.Proto;
using Network;
using UI;
using UnityEngine;

public class CreateCharacterPanel : PanelBase
{
    public void OnCreateClick()
    {
        this.SendNetMessage(PacketId.UserCreateCharacterRequest, new UserCreateCharacterRequest
        {
            Class = CharacterClass.Archer,
            Name = "tt",
        });
    }
}
