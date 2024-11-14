using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutInManager : MonoBehaviour
{
    [Header("Left and right control elements")]
    [SerializeField] private Animator _leftAnim;
    [SerializeField] private TextMeshProUGUI _leftTMP;
    [SerializeField] private Image _leftImage; 

    [SerializeField] private Animator _rightAnim;
    [SerializeField] private TextMeshProUGUI _rightTMP;
    [SerializeField] private Image _rightImage;

    [Header("Characters")]
    [SerializeField] private List<Sprite> charaSprites;
    [SerializeField] private List<string> charaSayings;

    public bool begin;
    float testTimer = 35;

    void Start()
    {
        StartCoroutine(DoAutoTrigger());
    }

    // Update is called once per frame
    void Update()
    {
        if(testTimer > 0 && begin)
            testTimer -= Time.deltaTime;
    }

    private void PlayCutIn(bool left, int character)
    {
        if (left)
        {
            _leftTMP.text = charaSayings[character];
            _leftImage.sprite = charaSprites[character];
            _leftAnim.Play("LeftDrift", 0, 0);
        }
        else
        {
            _rightTMP.text = charaSayings[character];
            _rightImage.sprite = charaSprites[character];
            _rightAnim.Play("RighDrift", 0, 0);
        }
    }

    private IEnumerator DoAutoTrigger()
    {
        yield return new WaitUntil(() => testTimer < 30);
        PlayCutIn(true, 0);
        yield return new WaitUntil(() => testTimer < 20);
        PlayCutIn(false, 1);
        yield return new WaitUntil(() => testTimer < 10);
        PlayCutIn(true, 2);
    }
}
