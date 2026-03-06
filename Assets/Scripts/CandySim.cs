using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class CandySim : MonoBehaviour
{
    [Header("Resource: Jellybeans")]
    public float beans = 0;
    public float beanRate = 1.0f;
    public TextMeshProUGUI beanText;
    public Transform beanJarVisual;

    [Header("Resource: Lollipops")]
    public float pops = 0;
    public float popRate = 0;
    public TextMeshProUGUI popText;
    public GameObject lollipopStation;
	public Button popClickButton;

    [Header("Planting Settings")]
    public GameObject pressPrefab;
    public Transform spawnArea;

    [Header("Buttons")]
    public Button buyGenButton;
    public Button buyPowerUpButton;
    public Button unlockButton;
    public Button prestigeButton;

    [Header("Costs")]
    public float generatorCost = 10;
    public float powerUpCost = 20;
    public float unlockCost = 50;
    public float multiplier = 1.0f;

    [Header("Prestige")]
    public int prestigeLevel = 0;
    public float prestigeBonus = 1.0f;

    [Header("UI Metrics")]
    public TextMeshProUGUI prestigeText;
    public TextMeshProUGUI rateText;
    public TextMeshProUGUI genCostText;
    public TextMeshProUGUI genCountText;
    public TextMeshProUGUI efficiencyText;

	public GameObject trophy1, trophy2, trophy3;
    private float currTime = 0f;

	void Start() {
        LoadGame();

        if (popClickButton != null) {
            popClickButton.gameObject.SetActive(lollipopStation.activeSelf);
        }
    }

    void Update() {
        beans += (beanRate * multiplier * prestigeBonus) * Time.deltaTime;
        pops += popRate * Time.deltaTime;
        currTime += Time.deltaTime;

        beanText.text = "JellyBeans: " + Mathf.FloorToInt(beans);

        if (lollipopStation.activeSelf) {
            popText.text = "Lollipops: " + Mathf.FloorToInt(pops);
        } else {
            popText.text = "???";
        }
        
        float totalRate = beanRate * multiplier * prestigeBonus;
        rateText.text = "Rate: " + totalRate.ToString("F1") + "/s";

        int genCount = Mathf.FloorToInt((beanRate - 1.0f) / 2.0f);
        genCountText.text = "Generators: " + genCount;

        genCostText.text = "Next Generator Cost: " + Mathf.FloorToInt(generatorCost);
    
        efficiencyText.text = "Efficiency: " + (multiplier * 100f).ToString("F0") + "%";

        float bonusPct = (prestigeBonus - 1f) * 100f;
        prestigeText.text = "Prestige Lvl: " + prestigeLevel + " (+" + bonusPct.ToString("F0") + "%)";

        UpdateButtonStyle(buyGenButton, beans >= generatorCost);
        UpdateButtonStyle(buyPowerUpButton, beans >= powerUpCost);
        UpdateButtonStyle(prestigeButton, beans >= 500);

        if (unlockButton.gameObject.activeSelf) {
            UpdateButtonStyle(unlockButton, beans >= unlockCost);
        }

        if (beanJarVisual != null) {
            float fillAmount = Mathf.Clamp(beans / 100f, 0.1f, 2.0f);
            beanJarVisual.localScale = new Vector3(1, fillAmount, 1);
        }

		if (beans >= 50) trophy1.SetActive(true);
		if (beans >= 100) trophy2.SetActive(true);
		if (beans >= 200) trophy3.SetActive(true);

        if (currTime >= 30f) {
            SaveGame();
            currTime = 0f;
        }
    }

    void OnApplicationQuit() { 
        SaveGame(); 
    }
    void OnApplicationPause(bool pause) { 
        if (pause) {
            SaveGame(); 
        }
    }

    void UpdateButtonStyle(Button btn, bool canAfford) {
        if (btn != null) {
            btn.image.color = canAfford ? Color.green : Color.gray;
        }
    }

    public void BuyGenerator() {
        if (beans >= generatorCost) {
            beans -= generatorCost;
            beanRate += 2.0f;

            Vector3 randomPos = spawnArea.position + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
            Instantiate(pressPrefab, randomPos, Quaternion.identity);

            generatorCost *= 1.5f;
            Debug.Log("Generator Planted!");
        }
    }

    public void BuyPowerUp() {
        if (beans >= powerUpCost) {
            beans -= powerUpCost;
            multiplier *= 1.5f;
            Debug.Log("Multiplier Active!");
        }
    }

    public void UnlockLollipops() {
        if (beans >= unlockCost) {
            beans -= unlockCost;
            lollipopStation.SetActive(true);
            popRate = 0.5f;

			if (popClickButton != null) popClickButton.gameObject.SetActive(true);
            
            unlockButton.gameObject.SetActive(false); 
            Debug.Log("Lollipop Wing Unlocked!");
        }
    }

	public void ClickLollipop() {
		pops += 3;
		Debug.Log("Manual Lollipop Created!");
	}

    public void SaveGame() {
        PlayerPrefs.SetFloat("SavedBeans", beans);
        PlayerPrefs.SetFloat("SavedPops", pops);
        PlayerPrefs.SetFloat("SavedBeanRate", beanRate);
        PlayerPrefs.SetFloat("SavedPopRate", popRate);
        PlayerPrefs.SetFloat("SavedMultiplier", multiplier);
        PlayerPrefs.SetFloat("SavedGenCost", generatorCost);
        PlayerPrefs.SetInt("SavedPrestige", prestigeLevel);
        PlayerPrefs.SetInt("LollipopUnlocked", lollipopStation.activeSelf ? 1 : 0);
        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
        Debug.Log("Game Saved");
    }

    void LoadGame() {
        if (!PlayerPrefs.HasKey("SavedBeans")) {
            return;
        }

        beans = PlayerPrefs.GetFloat("SavedBeans");
        pops = PlayerPrefs.GetFloat("SavedPops");
        beanRate = PlayerPrefs.GetFloat("SavedBeanRate");
        popRate = PlayerPrefs.GetFloat("SavedPopRate");
        multiplier = PlayerPrefs.GetFloat("SavedMultiplier");
        generatorCost = PlayerPrefs.GetFloat("SavedGenCost");
        prestigeLevel = PlayerPrefs.GetInt("SavedPrestige", 0);
        
        prestigeBonus = 1.0f + (prestigeLevel * 0.1f);
        bool isUnlocked = (PlayerPrefs.GetInt("LollipopUnlocked") == 1);
        lollipopStation.SetActive(isUnlocked);

        if (unlockButton != null) {
            unlockButton.gameObject.SetActive(!isUnlocked); 
        } 

        if (PlayerPrefs.HasKey("QuitTime")) {
            long temp = Convert.ToInt64(PlayerPrefs.GetString("QuitTime"));
            DateTime quitTime = DateTime.FromBinary(temp);
            TimeSpan timeAway = DateTime.Now - quitTime;
            float secondsAway = (float)timeAway.TotalSeconds;

            beans += (beanRate * multiplier * prestigeBonus) * secondsAway;
            pops += popRate * secondsAway;

            Debug.Log($"You earned {Mathf.FloorToInt((beanRate * multiplier * prestigeBonus) * secondsAway)} beans while away.");
        }
    }

    public void PerformPrestige() {
        if (beans < 500) {
            return;
        }
        
        prestigeLevel++;
        prestigeBonus += 0.1f; 

        beans = 0;
        pops = 0;
        beanRate = 1.0f;
        popRate = 0;
        multiplier = 1.0f;
        generatorCost = 10;
        unlockCost = 50;

        foreach (GameObject gen in GameObject.FindGameObjectsWithTag("Generator")) {
            Destroy(gen);
        }

        lollipopStation.SetActive(false);
        if (unlockButton != null) {
            unlockButton.gameObject.SetActive(true);
        }

        trophy1.SetActive(false);
        trophy2.SetActive(false);
        trophy3.SetActive(false);

        PlayerPrefs.SetInt("SavedPrestige", prestigeLevel);
        SaveGame(); 

        Debug.Log("Prestige Level: " + prestigeLevel);
    }
}