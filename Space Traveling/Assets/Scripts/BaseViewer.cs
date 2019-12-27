using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BaseViewer : MonoBehaviour
{
    // Start is called before the first frame update
    IsometricGrid grid;
    Transform gridParent;
    void Start()
    {
        // grid = IsometricGrid.Deserialize(PlayerPrefs.GetString("friendBaseData"));
        // grid.placeGrid(gridParent);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void goBack()
    {
        SceneManager.LoadScene("GameMap");
    }
}
