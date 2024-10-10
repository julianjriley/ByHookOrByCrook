using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Cashout : MonoBehaviour
{
    public GameObject Fish;
    TMP_Text fishName;
    TMP_Text fishAmount;
    Image fishImage;
    public GameObject Content;
    public TMP_Text SummaryText;
    public TMP_Text BossBountyText;
    public TMP_Text TotalText;

    int summary = 0;
    float BossBounty = 0;
    float total = 0;
    int intTotal = 0;

    public List<string> fishType = new List<string>();
    //fishType will be changed

    // Start is called before the first frame update
    void Start()
    {
        BossBounty = GameManager.Instance.ScenePersistent.BossPerformanceMultiplier;
        fishName = Fish.transform.Find("FishNameBackground/FishName").GetComponent<TextMeshProUGUI>();
        fishAmount = Fish.transform.Find("FishNameBackground/MoneyBackground/FishAmount").GetComponent<TextMeshProUGUI>();
        fishImage = Fish.transform.Find("FishImage").GetComponent<Image>();
        
        Debug.Log(GameManager.Instance.ScenePersistent.Loadout.Count + " FISH AMOUNT");
        foreach(Item item in GameManager.Instance.ScenePersistent.Loadout)
        {
            
            fishName.text = item.GetItemName();
            fishAmount.text = item.GetCost().ToString();
            fishImage.sprite = item.GetSprite();
            summary += item.GetCost();
            Instantiate(Fish, Content.transform);
        }
        /*

        for (int i = 0; i < fishType.Count; i++){
            Instantiate(Fish, Content.transform);
            switch(fishType[i]){
                case "Salmon":
                    fishName.text = "Salmon";
                    int salmonAmt = 100;
                    fishAmount.text = salmonAmt.ToString();
                    summary += salmonAmt;
                    break;
                case "Tuna":
                    fishName.text = "Tuna";
                    int tunaAmt = 200;
                    fishAmount.text = tunaAmt.ToString();
                    summary += tunaAmt;
                    break;
                case "Sucker Puncher":
                    fishName.text = "Sucker Puncher";
                    int suckerAmt = 402;
                    fishAmount.text = suckerAmt.ToString();
                    summary += suckerAmt;
                    break;
            }
        }
        */
        SummaryText.text = summary.ToString();
        BossBountyText.text = BossBounty.ToString("F1");
        total = (float)summary * BossBounty;
        intTotal = (int)total;
        Debug.Log(total);
        Debug.Log(intTotal);
        TotalText.text = total.ToString();
        GameManager.Instance.GamePersistent.Gill += intTotal;
    }

    public void GoToHub()
    {
        SceneManager.LoadScene("PROTO_Hub");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
