using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.InputSystem;
using FMODUnity;

public class HubTTRLManager : MonoBehaviour
{
    protected InputAction _interactAction;

    [Header("Player")]
    [SerializeField] private GameObject _hubPlayer;
    private HubMovement _pMovement;

    [Header("Skipper")]
    [SerializeField] EventReference dialogueSound;
    [SerializeField] private GameObject _skipper;
    [SerializeField] private GameObject _sign;
    [SerializeField] private GameObject _poof;


    [Header("NPCs")]
    [SerializeField] private GameObject _activeNPCs;
    [SerializeField] private GameObject _hidingNPCs;

    [Header("UI")]
    [SerializeField] private GameObject _tutorialView;

    [SerializeField] private TextMeshProUGUI _speaker;
    [SerializeField] private TextMeshProUGUI _convoText;
    [SerializeField] private Conversation _conversation;
    [SerializeField] private Animator _textBoxAnim;

    [SerializeField] private GameObject _starrySky;
    [SerializeField] private CanvasGroup _void;
    [SerializeField] private Animator _bearAnim;
    bool isSkippingLine;

    [Header("Cameras")]
    [Tooltip("The main hub camera, and the camera focused on this specific interaction")]
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _interactCamera;

    void Start()
    {
        if (GameManager.Instance.GamePersistent.IsTutorialHub)
        {
            _tutorialView.SetActive(true);
            _interactAction = InputSystem.actions.FindAction("Interact");
            _pMovement = _hubPlayer.GetComponent<HubMovement>();

            // Set NPCs
            _activeNPCs.SetActive(false);
            _hidingNPCs.SetActive(true);

            StartCoroutine(DoTutorial());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // SCENE CHANGE LOG
        // Grouping the NPCs into "Active NPCs"
        // Making a new group that's "Hiding NPCs"
        // Camera bounds
        // Tutorial view disabled on start

    private IEnumerator DoTutorial()
    {
        // All of the shops are toggled off, and all the NPCs are swapped, by default when IsTutorialHub is active
        // Player cannot move
        _pMovement.IsIdle = true;
        // Set camera
        _interactCamera.SetActive(true);
        _mainCamera.SetActive(false);


        // Player falls from the sky ---------------------
        // Fade out from black
        while(_void.alpha > 0f)
        {
            _void.alpha -= Time.deltaTime;
            yield return null;
        }
        // Play "bear falling"
        yield return new WaitForSeconds(1f);
        _bearAnim.Play("Fall", 0, 0);
        yield return new WaitForSeconds(4f);
        // Cut to black
        _void.alpha = 1.0f;
        Destroy(_starrySky);
        yield return new WaitForSeconds(1f);
        // Fade out from black onto Skipper talking
        
        // Text box & skipper sprite appears, gab gab gab ------------------
        // Stop other SFX
        SoundManager.Instance.CleanButSpare("Hub", false);

        // Show the text box
        _textBoxAnim.Play("Appear", 0, 0);

        // Start the conversation
        foreach (string line in _conversation.lines)
        {
            
            if (line.StartsWith("Y"))
            {
                while (_void.alpha > 0f)
                {
                    _void.alpha -= Time.deltaTime;
                    yield return null;
                }
            }
            if (line.StartsWith("W"))
            {
                _speaker.text = "Skipper";
            }
            isSkippingLine = false;
            SoundManager.Instance.InitializeDialogue(dialogueSound); //start dialogue sound
            SoundManager.Instance.SetGlobalParameter("MusicDown", 1);//turn music down
            StartCoroutine(DoTextEscapeSubroutine());
            for (int i = 0; i < line.Length; i++)
            {
                
                

                if (!isSkippingLine)
                {
                    if (line[i].Equals(' '))
                    {
                        i++;
                    }
                    _convoText.text = line.Substring(0, i);

                    yield return new WaitForSeconds(.07f);
                }
                else
                {
                    _convoText.text = line;
                    break;
                }

            }
            SoundManager.Instance.StopDialogue();//stop dialogue sound
            StopCoroutine(DoTextEscapeSubroutine());
            _convoText.text = line;
            yield return new WaitUntil(() => !_interactAction.IsPressed()); // Make the player lift the button so they don't hold through
            yield return new WaitUntil(() => _interactAction.IsPressed());
        }
        // Hide the text box
        _textBoxAnim.Play("Disappear", 0, 0);

        // Reset camera
        _mainCamera.SetActive(true);
        _interactCamera.SetActive(false);
        

        // Give player movement back
        _pMovement.IsIdle = false;

        // Bring music back to full volume
        SoundManager.Instance.SetGlobalParameter("MusicDown", 0);

        // Skipper disappears, leaving a sign behind --------------------------
        _skipper.SetActive(false);
        _sign.SetActive(true);
        _poof.SetActive(true);
        _poof.GetComponent<Animator>().Play("Poof", 0, 0);
        yield return new WaitForSeconds(7f / 12f);
        _poof.SetActive(false);

        // Player is allowed to walk to the dock, explore, all that; hub portion of the tutorial is over
        yield return null;
    }
    private IEnumerator DoTextEscapeSubroutine()
    {
        yield return new WaitUntil(() => !_interactAction.IsPressed()); // Make the player lift the button so they don't hold through
        yield return new WaitUntil(() => _interactAction.IsPressed());
        isSkippingLine = true;
    }
}
