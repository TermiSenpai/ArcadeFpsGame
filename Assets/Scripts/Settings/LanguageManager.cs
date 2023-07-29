using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using TMPro;

public class LanguageManager : MonoBehaviour
{
    private const string LanguagePlayerPrefsKey = "SelectedLanguage";
    
    // Código del idioma predeterminado
    public string defaultLanguage = "en"; 

    public TMP_Dropdown languageDropdown; 

    private void Start()
    {
        StartCoroutine(InitializeLocalization());
    }

    private IEnumerator InitializeLocalization()
    {
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMP_Dropdown.OptionData(locale.name));
        }
        languageDropdown.options = options;

        languageDropdown.value = selected;

        // Attach the OnLanguageChanged method to the Dropdown's onValueChanged event
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

        // Load the last selected language or use the default if none is saved
        string savedLanguage = PlayerPrefs.GetString(LanguagePlayerPrefsKey, defaultLanguage);
        SetSelectedLanguage(savedLanguage);
    }

    void OnLanguageChanged(int index)
    {
        string selectedLanguageCode = LocalizationSettings.AvailableLocales.Locales[index].Identifier.Code;
        ChangeLanguage(selectedLanguageCode);
    }

    void ChangeLanguage(string languageCode)
    {
        Locale selectedLocale = GetLocaleFromCode(languageCode);
        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
            PlayerPrefs.SetString(LanguagePlayerPrefsKey, languageCode);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogError("El idioma con código '" + languageCode + "' no está disponible.");
        }
#endif
    }

    Locale GetLocaleFromCode(string languageCode)
    {
        IReadOnlyList<Locale> availableLocales = LocalizationSettings.AvailableLocales.Locales;
        foreach (var locale in availableLocales)
        {
            if (locale.Identifier.Code == languageCode)
            {
                return locale;
            }
        }
        // Retorna null si el código de idioma no se encuentra en los locales disponibles.
        return null; 
    }

    void SetSelectedLanguage(string languageCode)
    {
        int selectedLanguageIndex = FindLanguageIndex(languageCode);
        languageDropdown.value = selectedLanguageIndex;
    }

    int FindLanguageIndex(string languageCode)
    {
        for (int i = 0; i < languageDropdown.options.Count; i++)
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == languageCode)
            {
                return i;
            }
        }
        return 0; // Retorna 0 si no se encuentra el idioma, mostrando el primer idioma de la lista.
    }
}
