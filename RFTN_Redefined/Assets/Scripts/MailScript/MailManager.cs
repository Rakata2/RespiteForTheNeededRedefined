using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class MailManager : MonoBehaviour
{
    public static MailManager Instance;
    public GameObject MailWindow;
    public Image MailIconButton;
    public Transform ContentBox;

    public Sprite NormalMailSprite;
    public Sprite AlertMailSprite;

    public GameObject MailPrefab;

    private bool HasUnreadMail = false;

    public ClickableObject ComputerObject;

    private void Awake()
    {
        Instance = this;
    }

    public void ReceiveViolationMail(string subject, string descripition)
    {
        HasUnreadMail = true;
        MailIconButton.sprite = AlertMailSprite;
        if(ComputerObject != null)
        {
            ComputerObject.TriggerComputerAlert();
        }
        GameObject newMail = Instantiate(MailPrefab, ContentBox);
        newMail.transform.SetAsFirstSibling();
        MailItem mailscript = newMail.GetComponent<MailItem>();
        mailscript.SetupMail(subject, descripition);

        LayoutRebuilder.ForceRebuildLayoutImmediate(ContentBox.GetComponent<RectTransform>());
    }

    public void ShowMailWindow()
    {
        MailWindow.SetActive(true);
        if(HasUnreadMail)
        {
            HasUnreadMail = false;
            MailIconButton.sprite = NormalMailSprite;
        }
    }

    public void CloseMailWindow()
    {
        MailWindow.SetActive(false);
    }


}
