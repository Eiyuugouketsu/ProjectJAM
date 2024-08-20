using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreditsGenerator : MonoBehaviour
{
    public GameObject contentPanel;
    public GameObject textPrefab;
    public TMP_FontAsset creditsFont;
    public GameObject spacerPrefab;

    public List<CreditEntry> creditsList;
    
    // Start is called before the first frame update
    void Start()
    {
        creditsList = new List<CreditEntry>
        {
            new CreditEntry("Project Lead", new List<string> { "Naiteiru" }),
            new CreditEntry("Programmers", new List<string> { "Swend", "Ams", "Arzpal" }),
            new CreditEntry("Artists", new List<string> { "Nitro", "PerplexVoxel", "Xade", "Muffnbuttn", "Pseudo-Deity" }),
            new CreditEntry("Designers", new List<string> { "Mystify", "Ace S" }),
            new CreditEntry("UI/UX", new List<string> { "Dane", "Klight", "Yetimang19175" }),
            new CreditEntry("Audio", new List<string> { "AvapXia", "SeQuenceAudio", "Zmand97" }),
            new CreditEntry("Quality Assurance", new List<string> { "That One (Sheridan L)", "Scoutamass" }),
            new CreditEntry("Voice Acting", new List<string> { "Wiltee" }),
            new CreditEntry("Fonts", new List<string> { "Coventry-garden", "Parklane", "Sarsaparilla" })
        };


        GenerateCredits();
    }

    void GenerateCredits()
    {
        
        foreach (var entry in creditsList)
        {
            //Debug.Log(entry.Title);
            // title text
            GameObject titleTextObject = Instantiate(textPrefab, contentPanel.transform);
            TextMeshProUGUI titleText = titleTextObject.GetComponent<TextMeshProUGUI>();
            titleText.text = entry.Title;
            titleText.fontStyle = FontStyles.Bold;
            titleText.font = creditsFont;

            GameObject creditContainer = new GameObject("creditContainer");
            creditContainer.transform.SetParent(titleTextObject.transform);
            creditContainer.AddComponent<RectTransform>();

            // people text
            foreach (var person in entry.People)
            {
                GameObject personTextObject = Instantiate(textPrefab, contentPanel.transform);
                personTextObject.transform.SetParent(creditContainer.transform);
                TextMeshProUGUI personText = personTextObject.GetComponent<TextMeshProUGUI>();
                personText.text = person;
                personText.fontStyle = FontStyles.Normal;
                personText.font = creditsFont;
            }
            Instantiate(spacerPrefab, contentPanel.transform);
        }
    }
}
