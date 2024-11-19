using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoSelector : MonoBehaviour
{
    private NPCInteractor _npcInteractor;
    [Tooltip("The amount of failure that takes you beyond any of the programmed post-boss convos")]
    [SerializeField] private int maxTries;
    [Tooltip("The 3 books of conversations an NPC has for before boss 1 / 2 / 3")]
    [SerializeField] private List<Book> _books;
    private int returnIndex;
    public enum NPCType { Rod, Bait, Bag };
    public NPCType NPCharaType;
    void Start()
    {
        // Get the interactor and choose the appropriate book to look for a conversation in
        
    }

    private void Awake()
    {
        // If the tracker list in save data is empty
        if(getIsConvoHad().Count == 0 || GameManager.Instance.GamePersistent.NPCBossNumber != GameManager.Instance.GamePersistent.BossNumber)
        {
            GameManager.Instance.GamePersistent.NPCBossNumber = GameManager.Instance.GamePersistent.BossNumber;
            clearIsConvoHad();
            int fullConvoCount = _books[GameManager.Instance.GamePersistent.BossNumber].generalConvos.Count + _books[GameManager.Instance.GamePersistent.BossNumber].postBossLossConvos.Count;
            for (int i = 0; i < fullConvoCount + 1; i++)
            {
                getIsConvoHad().Add(false); // Fill it to the brim
            }
        }

        _npcInteractor = GetComponent<NPCInteractor>();
        Conversation convo = SelectConversation(_books[GameManager.Instance.GamePersistent.BossNumber]);
        _npcInteractor.SetConversation(convo, returnIndex);
    }

    private Conversation SelectConversation(Book book)
    {
        int lossCount = GameManager.Instance.GamePersistent.LossCounter;
        // First, we check and see if there are any conversation that should occur after a certain amount of losses to the boss
        for(int i = 0; i < book.postBossLossConvos.Count; i++)
        {
            // If we're at the final index and need to check against a maxTries var
            if(i+1 == book.postBossLossConvos.Count)
            {
                if (book.postBossLossConvos[i].RequiredBossLossCount <= lossCount && lossCount < maxTries)
                {
                    if (!getIsConvoHad()[i])
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
                    if (!getIsConvoHad()[i])
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
                if(conversation.GeneralPriority == k && !getIsConvoHad()[book.postBossLossConvos.Count + index]) 
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
        markNoNewGenConvos();
        for(int l = 0; l < book.generalConvos.Count; l++)
        {
            if(book.generalConvos[l].GeneralPriority == 0)
            {
                getIsConvoHad()[l + book.postBossLossConvos.Count] = false;
                
            }
        }
        returnIndex = book.postBossLossConvos.Count;
        return book.generalConvos[0];
    }

    private List<bool> getIsConvoHad()
    {
        if(NPCharaType == NPCType.Rod)
        {
            return GameManager.Instance.GamePersistent.IsConvoHadRod;
        }
        else if (NPCharaType == NPCType.Bait)
        {
            return GameManager.Instance.GamePersistent.IsConvoHadBait;
        }
        else
        {
            return GameManager.Instance.GamePersistent.IsConvoHadBag;
        }
    }

    private void markNoNewGenConvos()
    {
        if (NPCharaType == NPCType.Rod)
        {
            GameManager.Instance.GamePersistent.AllConvosHadRod = true;
        }
        else if (NPCharaType == NPCType.Bait)
        {
            GameManager.Instance.GamePersistent.AllConvosHadBait = true;
        }
        else
        {
            GameManager.Instance.GamePersistent.AllConvosHadBag = true;
        }
    }

    private void clearIsConvoHad()
    {
        if (NPCharaType == NPCType.Rod)
        {
            GameManager.Instance.GamePersistent.IsConvoHadRod = new List<bool>();
            GameManager.Instance.GamePersistent.AllConvosHadRod = false;
        }
        else if (NPCharaType == NPCType.Bait)
        {
            GameManager.Instance.GamePersistent.IsConvoHadBait = new List<bool>();
            GameManager.Instance.GamePersistent.AllConvosHadBait = false;
        }
        else
        {
            GameManager.Instance.GamePersistent.IsConvoHadBag = new List<bool>();
            GameManager.Instance.GamePersistent.AllConvosHadBag = false;
        }
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
