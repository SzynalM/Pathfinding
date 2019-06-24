using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class ShowPopup : MonoBehaviour, IWindowMessage
{
    [SerializeField]
    private TextMeshProUGUI windowText;
    private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowMessage(ErrorOccuredSignal generationDataNotValidatedInfo)
    {
        windowText.text = generationDataNotValidatedInfo.textToDisplay;
        canvasGroup.interactable = true;
        canvasGroup.DOFade(1, 0.5f);
    }

    public void HideMessage()
    {
        canvasGroup.interactable = false;
        canvasGroup.DOFade(0, 0.5f);
    }
}
