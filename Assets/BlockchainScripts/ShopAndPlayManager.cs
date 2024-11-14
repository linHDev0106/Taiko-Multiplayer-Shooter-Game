using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thirdweb;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopAndPlayManager : MonoBehaviour
{
    public string Address { get; private set; }

    private string receiverAddress = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";

    string NFTAddressSmartContract = "";

    string NFTAddressSmartContractTough = "0x152745Bf926C144A7D79949b1f501da3780D5d90";
    string NFTAddressSmartContractAgile = "0xf81A26fb86C4FD10c09a26d4357a7bFbf4342f1f";
    string NFTAddressSmartContractNoel = "0x81D1c10909AFd0EEd4F0c5dE84DED732d9844632";
    string NFTAddressSmartContractPirate = "0xB37B94417Fa9Bb74B8002a0d25726567DF365DE3";
    string NFTAddressSmartContractHat = "0x203cc95e47D6307CccD7a87962baFD6E49205ae2";

    public Button toughButton;
    public Button agileButton;
    public Button noelButton;
    public Button pirateButton;
    public Button hatButton;

    public Button shopButton;
    public Button playButton;
    public Button openChest;
    public Button backButton;

    public TMP_Text toughBalanceText;
    public TMP_Text agileBalanceText;
    public TMP_Text noelBalanceText;
    public TMP_Text pirateBalanceText;
    public TMP_Text hatBalanceText;

    public TextMeshProUGUI buyingStatusText;
    public TextMeshProUGUI chestOpeningResultValue;

    private void Start()
    {
        shopButton.interactable = false;
        playButton.interactable = false;
        toughButton.interactable = false;
        agileButton.interactable = false;
        CheckNFTBalance();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public async void CheckNFTBalance()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contractTough = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContractTough);
        try
        {
            List<NFT> nftList = await contractTough.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                toughBalanceText.text = "Balance: 00";
            }
            else
            {
                toughBalanceText.text = "Balance: " + nftList.Count;
                CharacterAndItem.Instance.tough = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
            // Handle the error, e.g., show an error message to the user or retry the operation
            shopButton.interactable = true;
            playButton.interactable = true;
        }

        var contractAgile = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContractAgile);
        try
        {
            List<NFT> nftList = await contractAgile.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                agileBalanceText.text = "Balance: 00";
            }
            else
            {
                agileBalanceText.text = "Balance: " + nftList.Count;
                CharacterAndItem.Instance.agile = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
            // Handle the error, e.g., show an error message to the user or retry the operation
            shopButton.interactable = true;
            playButton.interactable = true;
        }

        var contractNoel = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContractNoel);
        try
        {
            List<NFT> nftList = await contractNoel.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                noelBalanceText.text = "Balance: 00";
            }
            else
            {
                noelBalanceText.text = "Balance: " + nftList.Count;
                CharacterAndItem.Instance.noel = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
            // Handle the error, e.g., show an error message to the user or retry the operation
            shopButton.interactable = true;
            playButton.interactable = true;
        }
        var contractPirate = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContractPirate);
        try
        {
            List<NFT> nftList = await contractPirate.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                pirateBalanceText.text = "Balance: 00";
            }
            else
            {
                pirateBalanceText.text = "Balance: " + nftList.Count;
                CharacterAndItem.Instance.pirate = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
            // Handle the error, e.g., show an error message to the user or retry the operation
            shopButton.interactable = true;
            playButton.interactable = true;
        }
        var contractHat = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContractHat);
        try
        {
            List<NFT> nftList = await contractHat.ERC721.GetOwned(Address);
            if (nftList.Count == 0)
            {
                hatBalanceText.text = "Balance: 00";
                shopButton.interactable = true;
                playButton.interactable = true;
            }
            else
            {
                hatBalanceText.text = "Balance: " + nftList.Count;
                shopButton.interactable = true;
                playButton.interactable = true;
                CharacterAndItem.Instance.hat = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
            // Handle the error, e.g., show an error message to the user or retry the operation
            shopButton.interactable = true;
            playButton.interactable = true;
        }
    }

    private void HideAllButton() {
        toughButton.interactable = false;
        agileButton.interactable = false;
        noelButton.interactable = false;
        pirateButton.interactable = false;
        hatButton.interactable = false;
        backButton.interactable = false;
        openChest.interactable = false;
    }

    private void ShowAllButton()
    {
        noelButton.interactable = true;
        pirateButton.interactable = true;
        hatButton.interactable = true;
        backButton.interactable = true;
        openChest.interactable = true;
    }

    public async void BuyNFT(int indexValue) {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        HideAllButton();
        if (indexValue == 1) {
            NFTAddressSmartContract = NFTAddressSmartContractTough;
        }
        else if (indexValue == 2) {
            NFTAddressSmartContract = NFTAddressSmartContractAgile;
        }
        else if (indexValue == 3)
        {
            NFTAddressSmartContract = NFTAddressSmartContractNoel;
        }
        else if (indexValue == 4)
        {
            NFTAddressSmartContract = NFTAddressSmartContractPirate;
        }
        else if (indexValue == 5)
        {
            NFTAddressSmartContract = NFTAddressSmartContractHat;
        }

        var contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        try
        {
            var result = await contract.ERC721.ClaimTo(Address, 1);
            buyingStatusText.text = "Buying...";
            buyingStatusText.gameObject.SetActive(true);

            if (indexValue == 1)
            {
                buyingStatusText.text = "+1 Tough";
                buyingStatusText.gameObject.SetActive(true);

                try
                {
                    List<NFT> nftList = await contract.ERC721.GetOwned(Address);
                    if (nftList.Count == 0)
                    {
                        toughBalanceText.text = "Balance: 00";
                    }
                    else
                    {
                        toughBalanceText.text = "Balance: " + nftList.Count;
                        CharacterAndItem.Instance.tough = true;
                    }
                    ShowAllButton();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
                    // Handle the error, e.g., show an error message to the user or retry the operation
                    ShowAllButton();
                }
            }
            else if (indexValue == 2)
            {
                buyingStatusText.text = "+1 Agile";
                buyingStatusText.gameObject.SetActive(true);
                try
                {
                    List<NFT> nftList = await contract.ERC721.GetOwned(Address);
                    if (nftList.Count == 0)
                    {
                        agileBalanceText.text = "Balance: 00";
                    }
                    else
                    {
                        agileBalanceText.text = "Balance: " + nftList.Count;
                        CharacterAndItem.Instance.agile = true;
                    }
                    ShowAllButton();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
                    // Handle the error, e.g., show an error message to the user or retry the operation
                    ShowAllButton();
                }
            }
            else if (indexValue == 3)
            {
                buyingStatusText.text = "+1 Noel";
                buyingStatusText.gameObject.SetActive(true);
                try
                {
                    List<NFT> nftList = await contract.ERC721.GetOwned(Address);
                    if (nftList.Count == 0)
                    {
                        noelBalanceText.text = "Balance: 00";
                    }
                    else
                    {
                        noelBalanceText.text = "Balance: " + nftList.Count;
                        CharacterAndItem.Instance.noel = true;
                    }
                    ShowAllButton();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
                    // Handle the error, e.g., show an error message to the user or retry the operation
                    ShowAllButton();
                }
            }
            else if (indexValue == 4)
            {
                buyingStatusText.text = "+1 Pirate";
                buyingStatusText.gameObject.SetActive(true);
                try
                {
                    List<NFT> nftList = await contract.ERC721.GetOwned(Address);
                    if (nftList.Count == 0)
                    {
                        pirateBalanceText.text = "Balance: 00";
                    }
                    else
                    {
                        pirateBalanceText.text = "Balance: " + nftList.Count;
                        CharacterAndItem.Instance.pirate = true;
                    }
                    ShowAllButton();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
                    // Handle the error, e.g., show an error message to the user or retry the operation
                    ShowAllButton();
                }
            }
            else if (indexValue == 5)
            {
                buyingStatusText.text = "+1 Hat";
                buyingStatusText.gameObject.SetActive(true);
                try
                {
                    List<NFT> nftList = await contract.ERC721.GetOwned(Address);
                    if (nftList.Count == 0)
                    {
                        hatBalanceText.text = "Balance: 00";
                    }
                    else
                    {
                        hatBalanceText.text = "Balance: " + nftList.Count;
                        CharacterAndItem.Instance.hat = true;
                    }
                    ShowAllButton();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
                    // Handle the error, e.g., show an error message to the user or retry the operation
                    ShowAllButton();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while buying the NFT: {ex.Message}");
            // Optionally, update the UI to inform the user of the error
            //buyingStatusText.text = "Failed to buy NFT. Please try again.";
            //buyingStatusText.gameObject.SetActive(true);
            ShowAllButton();
        }
    }

    //Code chạy Lottery
    private int randomNumber;

    // Hàm để tạo ra số ngẫu nhiên và hiển thị hiệu ứng nhảy số
    public void GenerateRandomNumber()
    {
        randomNumber = UnityEngine.Random.Range(1, 101); // Tạo số ngẫu nhiên từ 1 đến 100
        StartCoroutine(ShowNumberEffect(randomNumber));
    }

    private IEnumerator ShowNumberEffect(int targetNumber)
    {
        int currentNumber = 1;
        while (currentNumber < targetNumber)
        {
            currentNumber++;
            chestOpeningResultValue.text = currentNumber.ToString();
            yield return new WaitForSeconds(0.05f); // Điều chỉnh tốc độ nhảy số tại đây
        }

        // Hiển thị số cuối cùng
        chestOpeningResultValue.text = targetNumber.ToString();

        // Kiểm tra điều kiện debug
        if (targetNumber <= 20)
        {
            Debug.Log("Character1");
            buyingStatusText.text = "Tough Guy";
            buyingStatusText.gameObject.SetActive(true);
            toughButton.interactable = true;
        }
        else
        {
            buyingStatusText.text = "Better luck next time";
            buyingStatusText.gameObject.SetActive(true);
        }
        if (targetNumber <= 5)
        {
            Debug.Log("Character2");
            buyingStatusText.text = "Agile Guy";
            buyingStatusText.gameObject.SetActive(true);
            toughButton.interactable = true;
            agileButton.interactable = true;
        }
    }

    private static float ConvertStringToFloat(string numberStr)
    {
        // Convert the string to a float
        float number = float.Parse(numberStr);

        // Return the float value
        return number;
    }

    public async void SpendETHToBuyNFT() {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        buyingStatusText.text = "Buying...";
        buyingStatusText.gameObject.SetActive(true);
        var userBalance = await ThirdwebManager.Instance.SDK.Wallet.GetBalance();
        float costValue = 0.0002f;
        if (ConvertStringToFloat(userBalance.displayValue) < costValue)
        {
            buyingStatusText.text = "Not Enough ETH";
        }
        else
        {
            HideAllButton();
            try
            {
                // Thực hiện chuyển tiền, nếu thành công thì tiếp tục xử lý giao diện
                await ThirdwebManager.Instance.SDK.Wallet.Transfer(receiverAddress, costValue.ToString());

                // Chỉ thực hiện các thay đổi giao diện nếu chuyển tiền thành công

                GenerateRandomNumber();
                ShowAllButton();
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra
                Debug.LogError($"Lỗi khi thực hiện chuyển tiền: {ex.Message}");
                buyingStatusText.text = "Error. Please try again";
                ShowAllButton();
            }
        }
    }
}
