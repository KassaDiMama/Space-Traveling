using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void show(int dir)
    {
        transform.localScale = new Vector3(dir * 0.1f, 0.1f, 0.1f);
    }
    public void hide()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
    }
}
