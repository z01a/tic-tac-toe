using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogBox : Singleton<DialogBox>
{
    [SerializeField]
    private TextMeshProUGUI messageText;
    
    [SerializeField]
    private Button okButton;

    [SerializeField]
    private Button cancelButton;

    private Action _onOk;
    private Action _onCancel;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public void Show(string message, Action onOk = null, Action onCancel = null)
    {
        _onOk = onOk;
        _onCancel = onCancel;

        messageText.text = message;

        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        okButton.onClick.AddListener(OnOkPressed);
        cancelButton.onClick.AddListener(OnCancelPressed);

        cancelButton.gameObject.SetActive(onCancel != null);

        gameObject.SetActive(true);
    }

    private void OnOkPressed()
    {
        // TODO: This should be moves somewhere else
        SoundManager.Instance.PlaySFX("click2");
        _onOk?.Invoke();
        Close();
    }

    private void OnCancelPressed()
    {
        // TODO: This should be moves somewhere else
        SoundManager.Instance.PlaySFX("click2");
        _onCancel?.Invoke();
        Close();
    }

    private void Close()
    {
        // TODO: This should be moves somewhere else
        SoundManager.Instance.PlaySFX("click1");
        gameObject.SetActive(false);
    }
}
