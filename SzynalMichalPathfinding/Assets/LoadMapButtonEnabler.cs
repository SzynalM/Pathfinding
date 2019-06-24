using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class LoadMapButtonEnabler : MonoBehaviour
    {
        private Button thisButton;

        void Start()
        {
            thisButton = GetComponent<Button>();
            if (File.Exists(Path.Combine(Application.dataPath, "SavedMap.json")))
            {
                thisButton.interactable = true;
            }
            else
            {
                thisButton.interactable = false;
            }
        }
    } 
}
