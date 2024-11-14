using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectWalletManager : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("ShopAndPlay");
    }
}
