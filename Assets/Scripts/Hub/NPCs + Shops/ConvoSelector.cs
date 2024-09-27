using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoSelector : MonoBehaviour
{
    private Interactor _npcInteractor;
    [Tooltip("The amount of failure that takes you beyond any of the programmed post-boss convos")]
    [SerializeField] private int maxTries;
    [Tooltip("The 3 books of conversations an NPC has for before boss 1 / 2 / 3")]
    [SerializeField] private List<Book> _books;
    private int returnIndex;
    void Start()
    {
        // Get the interactor and choose the appropriate book to look for a conversation in
        
    }

    private void Awake()
    {
        // If the tracker list in save data is empty
        if(GameManager.Instance.SaveData.IsConvoHad.Count == 0)
        {
            int fullConvoCount = _books[GameManager.Instance.SaveData.bossNumber].generalConvos.Count + _books[GameManager.Instance.SaveData.bossNumber].postBossLossConvos.Count;
            for (int i = 0; i < fullConvoCount; i++)
            {
                GameManager.Instance.SaveData.IsConvoHad.Add(false); // Fill it to the brim
            }
        }

        _npcInteractor = GetComponent<Interactor>();
        Conversation convo = SelectConversation(_books[GameManager.Instance.SaveData.bossNumber]);
        _npcInteractor.SetConversation(convo, returnIndex);
    }

    private Conversation SelectConversation(Book book)
    {
        int lossCount = GameManager.Instance.SaveData.lossCounter;
        // First, we check and see if there are any conversation that should occur after a certain amount of losses to the boss
        for(int i = 0; i < book.postBossLossConvos.Count; i++)
        {
            // If we're at the final index and need to check against a maxTries var
            if(i+1 == book.postBossLossConvos.Count)
            {
                if (book.postBossLossConvos[i].RequiredBossLossCount <= lossCount && lossCount < maxTries)
                {
                    if (!GameManager.Instance.SaveData.IsConvoHad[i])
                    {
                    // If we haven't had this conversation, it's high time to slot it in
                        returnIndex = i;
                        return book.postBossLossConvos[i];
                    }
                }
            }
            // Otherwise, if we're moving up the list with room to spare
            else
            {
                if (book.postBossLossConvos[i].RequiredBossLossCount <= lossCount && lossCount < book.postBossLossConvos[i + 1].RequiredBossLossCount)
                {
                    if (!GameManager.Instance.SaveData.IsConvoHad[i])
                    {
                    // If we haven't had this conversation, it's high time to slot it in
                        returnIndex = i;
                        return book.postBossLossConvos[i];
                    }
                }
            }  
        }

        // If it turns out there were no post-boss conversations to return, we instead pivot to our general priority list
        // First, we sort it by general priority
        ConvoComparer comparer = new ConvoComparer();
        book.generalConvos.Sort(comparer);

        // This sorts the list from lowest to highest priority. We store the highest priority for iteration purposes.
        int highPriority = book.generalConvos[book.generalConvos.Count - 1].GeneralPriority;

        // We iterate through the priorities, from X to 0
        for(int k = highPriority; k >= 0; k--)
        {
            List<Conversation> tempList = new List<Conversation>();
            foreach(Conversation conversation in book.generalConvos)
            {
                int index = book.generalConvos.FindIndex(x => x == conversation);
                // If this is a high priority conversation we haven't had, add it to the tempList
                if(conversation.GeneralPriority == k && !GameManager.Instance.SaveData.IsConvoHad[book.postBossLossConvos.Count + index])
                {
                    tempList.Add(conversation);
                }
            }

            if(tempList.Count == 1) // If there's only one viable convo of this priority, return it
            {
                int index = book.generalConvos.FindIndex(x=>x == tempList[0]);
                returnIndex = index + book.postBossLossConvos.Count;
                return tempList[0];
            }
            else if(tempList.Count > 1) // Otherwise, randomly pick one from the pool
            {
                int index = Random.Range(0, tempList.Count);
                int trueIndex = book.generalConvos.FindIndex(x => x == tempList[index]);
                returnIndex = trueIndex + book.postBossLossConvos.Count;
                return tempList[index];
            }
        }

        // If we get here, to the very end, without any conversation returned, we clear the Had booleans of ALL the 0 priorities
        // and just return the first one
        for(int l = 0; l < book.generalConvos.Count; l++)
        {
            if(book.generalConvos[l].GeneralPriority == 0)
            {
                GameManager.Instance.SaveData.IsConvoHad[l + book.postBossLossConvos.Count] = false;
                
            }
        }
        returnIndex = book.postBossLossConvos.Count;
        return book.generalConvos[0];
    }

    #region COMPARER CLASS
    public class ConvoComparer : IComparer<Conversation>
    {
        public int Compare(Conversation x, Conversation y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater.
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the
                    // lengths of the two strings.
                    //
                    int retval = x.GeneralPriority.CompareTo(y.GeneralPriority);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // return the first one
                        //
                        return 1;
                    }
                }
            }
        }
    }
    #endregion
}