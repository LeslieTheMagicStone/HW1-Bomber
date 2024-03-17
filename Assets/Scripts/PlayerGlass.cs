using UnityEngine;

public class PlayerGlass : MonoBehaviour
{
    public Material sunglass;

    private void Start()
    {
        if (PlayerPrefs.GetString("HasWon", "false") == "true")
        {
            GetComponent<MeshRenderer>().material = sunglass;
        }
    }
}
