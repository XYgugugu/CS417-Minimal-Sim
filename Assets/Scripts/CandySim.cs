using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Header("Costs")]
    public float generatorCost = 10;
    public float powerUpCost = 20;
    public float unlockCost = 50;
    public float multiplier = 1.0f;

	public GameObject trophy1, trophy2, trophy3;

	void Start() {
        if (popClickButton != null) popClickButton.gameObject.SetActive(false);
    }

    void Update() {
        beans += (beanRate * multiplier) * Time.deltaTime;
        pops += popRate * Time.deltaTime;

        beanText.text = "JellyBeans: " + Mathf.FloorToInt(beans);

        if (lollipopStation.activeSelf) {
            popText.text = "Lollipops: " + Mathf.FloorToInt(pops);
        } else {
            popText.text = "???";
        }

        UpdateButtonStyle(buyGenButton, beans >= generatorCost);
        UpdateButtonStyle(buyPowerUpButton, beans >= powerUpCost);

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
}