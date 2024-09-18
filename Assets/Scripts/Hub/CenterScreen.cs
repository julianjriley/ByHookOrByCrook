using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translation = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.Log(translation);
        this.transform.position = new Vector3(translation.x, translation.y, 0);
    }
}
