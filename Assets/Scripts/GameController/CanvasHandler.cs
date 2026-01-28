using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI subtitles;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subtitle;
    private TextAsset csvFile;
    Dictionary<string, string> translations;
    private void Start()
    {
        GameManager gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        csvFile = Resources.Load<TextAsset>("Translations/" + gameManager.getLocale());
        translations = LoadTranslations();
    }

    public Dictionary<string, string> LoadTranslations()
    {
        Dictionary<string, string> translations = new Dictionary<string, string>();
        if (csvFile != null)
        {
            using (StringReader reader = new StringReader(csvFile.text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Split each line into key and value using the semicolon as a separator
                    string[] parts = line.Split(';');

                    if (parts.Length >= 2)
                    {
                        // Trim leading and trailing whitespaces from key and value
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        // Add the key-value pair to the translations dictionary
                        if (!translations.ContainsKey(key))
                        {
                            translations.Add(key, value);
                        }
                        else
                        {
                            // Handle duplicate keys if needed
                            Debug.LogWarning("Duplicate key found in CSV: " + key);
                        }
                    }
                    else
                    {
                        // Handle lines without both key and value if needed
                        Debug.LogWarning("Invalid line in CSV: " + line);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("CSV file not assigned.");
        }
        return translations;
    }

    public string GetTranslation(string key)
    {
        return (translations.TryGetValue(key, out string value)) ? value : key;
    }

    public void SetSubtitles(string key)
    {
        Color color = subtitles.transform.parent.GetComponent<Image>().color;
        subtitles.transform.parent.GetComponent<Image>().color = new Color(color.r, color.g, color.b, .33f);
        subtitles.text = GetTranslation(key);
    }

    public void SetTitle(string key, string subkey)
    {
        title.text = GetTranslation(key);
        subtitle.text = GetTranslation(subkey);
    }

    public void RemoveSubtitles()
    {
        Color color = subtitles.transform.parent.GetComponent<Image>().color;
        subtitles.transform.parent.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        subtitles.text = "";
    }

    public void RemoveTitle()
    {
        subtitle.text = "";
        title.text = "";
    }

    public void SetTitle() 
    {
    
    }
}
