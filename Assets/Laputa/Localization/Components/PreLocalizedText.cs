using UnityEngine;

namespace Laputa.Localization.Components
{
    public class PreLocalizedText : MonoBehaviour
    {
        [SerializeField] private string currentText;
        [SerializeField] private LanguageLocalizedData languageLocalizedData;
        [SerializeField] private LocalizedDataText localizedDataText;
    }
}
