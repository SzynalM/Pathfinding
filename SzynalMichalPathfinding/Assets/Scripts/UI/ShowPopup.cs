using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ShowPopup : MonoBehaviour
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
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1, 0.5f);
        }

        public void HideMessage()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0, 0.5f);
        }
    }

}