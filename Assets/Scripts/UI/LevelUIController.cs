using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    public GameObject playerUI;
    public GameController gameController;
    int levelId;
    List<GameObject> levels = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        levelId = gameController.levelId;
        foreach (Transform item in transform)
        {
            levels.Add(item.gameObject);
        }
        SetLevelPlayer();
    }

    public void SetLevelPlayer() {
        levelId = gameController.levelId;
        playerUI.transform.SetParent(levels[levelId-1].transform);
        playerUI.transform.localPosition = new Vector3(0,playerUI.transform.localPosition.y,0);
    }
}
