using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float rotationSpeed = 20f;

    private void Start()
    {
        if (PlayerPrefs.GetString("HasWon", "false") == "true")
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
