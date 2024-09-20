using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Cashout : MonoBehaviour
{
    public GameObject Fish;
    TMP_Text fishName;
    TMP_Text fishAmount;
    public GameObject Content;
    public TMP_Text SummaryText;
    public TMP_Text BossBountyText;
    public TMP_Text TotalText;

    int summary = 0;
    public float BossBounty = 0;
    float total = 0;
    int intTotal = 0;

    public List<string> fishType = new List<string>();
    //fishType will be changed

    // Start is called before the first frame update
    void Start()
    {

        fishName = Fish.transform.Find("FishNameBackground/FishName").GetComponent<TextMeshProUGUI>();
        fishAmount = Fish.transform.Find("FishNameBackground/MoneyBackground/FishAmount").GetComponent<TextMeshProUGUI>();
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
        SummaryText.text = summary.ToString();
        BossBountyText.text = BossBounty.ToString("F1");
        total = (float)summary * BossBounty;
        intTotal = (int)total;
        Debug.Log(total);
        Debug.Log(intTotal);
        TotalText.text = total.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
