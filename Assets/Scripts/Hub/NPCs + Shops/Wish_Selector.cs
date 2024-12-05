using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish_Selector : MonoBehaviour
{

    //Make a lit of convos from the scriptable object public
    //Book Wish Book type
    [SerializeField] Book wishBook;
    //Wish Npc
    [SerializeField] NPCInteractor _npc;
  
    void Start()
    {
        // Get NPC Component
        _npc = GetComponent<NPCInteractor>();
        // Lis of convos  = book.general
        List<Conversation> fortunes = wishBook.generalConvos;
        //Debug.Log("Fortunes Size " + fortunes.Count);
        if (fortunes.Count > 0)
        {
            //Generate a random number from 0 to the length of the list
            int generatedIndex = Random.Range(0, fortunes.Count);
            // npc.set convo ()
           // Debug.Log("Selected " + generatedIndex);
            _npc.SetConversation(fortunes[generatedIndex], 0);
        }
        //else
            //Debug.Log("There are No Fortunes when there should be");
    }

}
