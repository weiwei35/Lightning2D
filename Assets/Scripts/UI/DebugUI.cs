using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public GameObject debugUI;
    public void ShowDebug () {
        debugUI.SetActive(true);
    }
    public void CloseDebug () {
        debugUI.SetActive(false);
    }
}
