using UnityEngine;
using System.Collections;

public class GameStateController : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            player.SetActive(true);
        }
    }
}
