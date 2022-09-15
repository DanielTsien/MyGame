using System.Collections.Generic;
using MyGame;
using MyGame.Proto;
using Network;
using UI;

public class SelectCharacterPanel : PanelBase
{
    public List<CharacterSoltItem> CharacterItem;

    private int m_selectedIdx;
    
    private void Start()
    {
        var characters = this.GetModel<IUserModel>().GetCharacters();
        for (int i = 0; i < CharacterItem.Count; i++)
        {
            CharacterItem[i].Init(characters[i]);
        }
    }

    public void OnStartClicked()
    {
        this.SendNetMessage(PacketId.UserGameEnterRequest, new UserGameEnterRequest
        {
            CharacterIdx = m_selectedIdx
        });
    }
    
}
