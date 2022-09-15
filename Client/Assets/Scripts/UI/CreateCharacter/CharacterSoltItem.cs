using MyGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSoltItem : MonoBehaviour
{
    private TMP_Text m_nameTxt;
    private TMP_Text m_classTxt;
    private TMP_Text m_levelTxt;
    private Button m_createBtn;

    private void Awake()
    {
        m_nameTxt = transform.Find("NameTxt").GetComponent<TMP_Text>();
        m_classTxt = transform.Find("ClassTxt").GetComponent<TMP_Text>();
        m_levelTxt = transform.Find("LevelTxt").GetComponent<TMP_Text>();
        m_createBtn = transform.Find("CreateBtn").GetComponent<Button>();
    }

    public void Init(Character info)
    {
        m_createBtn.gameObject.SetActive(false);
        m_nameTxt.text = info.Name;
        m_classTxt.text = info.Class.ToString();
        m_levelTxt.text = $"Lv.{info.Level}";
    }
}
