using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToShop : MonoBehaviour
{
    public void BackToShopButton()
    {
        SceneManager.LoadScene("ShopAndPlay");
    }
}
