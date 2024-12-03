using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactLoad : MonoBehaviour
{
    [SerializeField, Tooltip("Used to call scene transitions.")]
    private SceneTransitionsHandler _transitionsHandler;

    public string SceneName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _transitionsHandler.LoadScene(SceneName);
        }
    }
}
