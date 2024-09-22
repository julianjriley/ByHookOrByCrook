using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC Interactions/Conversation")]
public class Conversation : ScriptableObject
{
    [TextArea(1,2)] public List<string> lines;

    public bool IsConversationHad;

    public int RequiredBossLossCount = 0;
    public int GeneralPriority = 0;
}
