using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC Interactions/Book")]
public class Book : ScriptableObject
{
    public int bookNumber = 0;
    public List<Conversation> postBossLossConvos;
    public List<Conversation> generalConvos;
}
