using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : MonoBehaviour
{
    [Header("Deletion")]
    [Tooltip("Attacks that always take the same amount of time should delete themselves.")]
    public bool DeletesItself = true;
    public float Lifetime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if (DeletesItself) {
            Invoke("DeleteSelf", Lifetime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DeleteSelf() {
        Destroy(this.gameObject);
    }
}
