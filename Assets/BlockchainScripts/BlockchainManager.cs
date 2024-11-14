using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using Thirdweb;
using System.Numerics;
using System;

namespace Visyde
{
    public class BlockchainManager : MonoBehaviourPunCallbacks
    {
        public string Address { get; private set; }

        private bool player1Ready = false;
        private bool player2Ready = false;
        private bool player3Ready = false;
        private bool player4Ready = false;
        private bool player5Ready = false;
        private bool player6Ready = false;
        private bool player7Ready = false;
        private bool player8Ready = false;

        private string abiString = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_player1\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player2\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player3\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player4\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player5\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player6\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player7\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player8\",\"type\":\"address\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[],\"name\":\"getBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"index\",\"type\":\"uint256\"}],\"name\":\"getPlayer\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"hasWithdrawnOneEther\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"players\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"withdrawAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"withdrawOneEther\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"stateMutability\":\"payable\",\"type\":\"receive\"}]";
        private string contractFactoryABIString = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_player1\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player2\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player3\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player4\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player5\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player6\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player7\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"_player8\",\"type\":\"address\"}],\"name\":\"createGame\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"games\",\"outputs\":[{\"internalType\":\"contract PlayerGame\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getGames\",\"outputs\":[{\"internalType\":\"contract PlayerGame[]\",\"name\":\"\",\"type\":\"address[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_player1\",\"type\":\"address\"}],\"name\":\"getLastGameForPlayer1\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        private string escrowContractAddress = "";
        private string contractFactoryAddress = "0xe69C9077b3F12939EFCd673171A4489f0f1370da";

        private string defaultAddress = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";

        private string player1Address = "";
        private string player2Address = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";
        private string player3Address = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";
        private string player4Address = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";
        private string player5Address = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";
        private string player6Address = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";
        private string player7Address = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";
        private string player8Address = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";

        public Button startButton;
        public Button placeBetButton;
        public Button claimButton;

        public TMP_Text player1AddressText;
        public TMP_Text player2AddressText;
        public TMP_Text player3AddressText;
        public TMP_Text player4AddressText;
        public TMP_Text player5AddressText;
        public TMP_Text player6AddressText;
        public TMP_Text player7AddressText;
        public TMP_Text player8AddressText;

        public TMP_Text player1StatusValueText;
        public TMP_Text player2StatusValueText;
        public TMP_Text player3StatusValueText;
        public TMP_Text player4StatusValueText;
        public TMP_Text player5StatusValueText;
        public TMP_Text player6StatusValueText;
        public TMP_Text player7StatusValueText;
        public TMP_Text player8StatusValueText;

        public TMP_Text waitingToInitializeText;

        public Button createSmartContractButton;

        private bool escrowContractCreated = false;

        private string escrowContractAddressServer = "";

        private string currentPlayerAddress = "";

        public TMP_Text escrowSmartContractAddressText;
        public TMP_Text escrowContractBalanceText;

        private void Start()
        {
            UpdateCurerntPlayerAddress();
            claimRewardButton.gameObject.SetActive(false);
        }

        public async void UpdateCurerntPlayerAddress()
        {
            currentPlayerAddress = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        }

        private void Update()
        {
            UpdatePlayerAddressDisplay();
            DisplayCreateEscrowContractButton();
            if (escrowContractCreated == true)
            {
                waitingToInitializeText.text = "";
                placeBetButton.gameObject.SetActive(true);
                createSmartContractButton.interactable = false;
            }
            UpdatePlayerReadyStatus();
            DisplayStartGameButton();
            ShowClaimButton();
        }

        public async void CreateEscrowSmartContract()
        {
            var contract = ThirdwebManager.Instance.SDK.GetContract(
            contractFactoryAddress,
            contractFactoryABIString
            );
            await contract.Write("createGame",
                player1Address,
                player2Address,
                player3Address,
                player4Address,
                player5Address,
                player6Address,
                player7Address,
                player8Address
                );
            escrowContractAddress = await contract.Read<string>("getLastGameForPlayer1", player1Address);
            escrowSmartContractAddressText.text = escrowContractAddress;

            photonView.RPC("SetEscrowContractCreated", RpcTarget.All, true); // Notify all players
            photonView.RPC("SetEscrowContractAddressServer", RpcTarget.All, escrowContractAddress); // Notify all players
        }

        public async void UpdatePlayerAddress()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                player1Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
                photonView.RPC("SetPlayer1Address", RpcTarget.All, player1Address); // Notify all players
            }
            else
            {
                if (player2Address == defaultAddress)
                {
                    player2Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
                    photonView.RPC("SetPlayer2Address", RpcTarget.All, player2Address); // Notify all players
                }
                else if (player3Address == defaultAddress)
                {
                    player3Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
                    photonView.RPC("SetPlayer3Address", RpcTarget.All, player3Address); // Notify all players
                }
                else if (player4Address == defaultAddress)
                {
                    player4Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
                    photonView.RPC("SetPlayer4Address", RpcTarget.All, player4Address); // Notify all players
                }
                else if (player5Address == defaultAddress)
                {
                    player5Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
                    photonView.RPC("SetPlayer5Address", RpcTarget.All, player5Address); // Notify all players
                }
                else if (player6Address == defaultAddress)
                {
                    player6Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
                    photonView.RPC("SetPlayer6Address", RpcTarget.All, player6Address); // Notify all players
                }
                else if (player7Address == defaultAddress)
                {
                    player7Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
                    photonView.RPC("SetPlayer7Address", RpcTarget.All, player7Address); // Notify all players
                }
                else if (player8Address == defaultAddress)
                {
                    player8Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
                    photonView.RPC("SetPlayer8Address", RpcTarget.All, player8Address); // Notify all players
                }
            }
        }

        decimal ConvertWeiToEther(BigInteger wei)
        {
            // 1 Ether = 10^18 Wei
            BigInteger ether = wei / BigInteger.Parse("1000000000000000000"); // Chia số Wei cho 1 Ether
            return (decimal)ether; // Chuyển đổi về decimal để hiển thị
        }

        public async void BetMoney()
        {
            if (string.IsNullOrEmpty(escrowContractAddressServer))
            {
                return;
            }
            escrowContractAddress = escrowContractAddressServer;
            //Kiểm tra xem nếu đã >= 2 thì không cho transfer tiếp
            var escrowContract = ThirdwebManager.Instance.SDK.GetContract(
               escrowContractAddress,
               abiString
               );
            BigInteger balanceValue = await escrowContract.Read<BigInteger>("getBalance");

            // Chuyển đổi từ Wei sang Ether
            decimal etherAmount = ConvertWeiToEther(balanceValue);
            escrowContractBalanceText.text = etherAmount.ToString();
            if (etherAmount >= 8)
            {
                return;
            }

            placeBetButton.interactable = false;

            try
            {
                await ThirdwebManager.Instance.SDK.Wallet.Transfer(escrowContractAddress, "0.0001");

                balanceValue = await escrowContract.Read<BigInteger>("getBalance");
                // Chuyển đổi từ Wei sang Ether
                etherAmount = ConvertWeiToEther(balanceValue);
                escrowContractBalanceText.text = etherAmount.ToString();

                // Check if this player is the master client
                if (PhotonNetwork.IsMasterClient)
                {
                    player1Ready = true; // Set player1Ready to true
                    photonView.RPC("SetPlayerReady1", RpcTarget.All, player1Ready); // Notify all players

                }
                else if (currentPlayerAddress == player2Address)
                {
                    player2Ready = true; // Set player2Ready to true
                    photonView.RPC("SetPlayerReady2", RpcTarget.All, player2Ready); // Notify all players
                }
                else if (currentPlayerAddress == player3Address)
                {
                    player3Ready = true; // Set player3Ready to true
                    photonView.RPC("SetPlayerReady3", RpcTarget.All, player3Ready); // Notify all players
                }
                else if (currentPlayerAddress == player4Address)
                {
                    player4Ready = true; // Set player4Ready to true
                    photonView.RPC("SetPlayerReady4", RpcTarget.All, player4Ready); // Notify all players
                }
                else if (currentPlayerAddress == player5Address)
                {
                    player5Ready = true; // Set player5Ready to true
                    photonView.RPC("SetPlayerReady5", RpcTarget.All, player5Ready); // Notify all players
                }
                else if (currentPlayerAddress == player6Address)
                {
                    player6Ready = true; // Set player6Ready to true
                    photonView.RPC("SetPlayerReady6", RpcTarget.All, player6Ready); // Notify all players
                }
                else if (currentPlayerAddress == player7Address)
                {
                    player7Ready = true; // Set player7Ready to true
                    photonView.RPC("SetPlayerReady7", RpcTarget.All, player7Ready); // Notify all players
                }
                else if (currentPlayerAddress == player8Address)
                {
                    player8Ready = true; // Set player8Ready to true
                    photonView.RPC("SetPlayerReady8", RpcTarget.All, player8Ready); // Notify all players
                }
            }
            catch (Exception ex)
            {
                // Log the error to Unity Console
                Debug.LogError($"An error occurred: {ex.Message}");
                placeBetButton.interactable = true;
            }
        }

        private void UpdatePlayerAddressDisplay()
        {
            Player[] numberOfPlayerInGame = GameManager.instance.punPlayersAll;
            if (numberOfPlayerInGame.Length == 2)
            {
                player1AddressText.text = player1Address;
                player1AddressText.gameObject.SetActive(true);
                if (player2Address != defaultAddress)
                {
                    player2AddressText.text = player2Address;
                    player2AddressText.gameObject.SetActive(true);
                }
            }
            else if (numberOfPlayerInGame.Length == 3)
            {
                player1AddressText.text = player1Address;
                player1AddressText.gameObject.SetActive(true);
                if (player2Address != defaultAddress)
                {
                    player2AddressText.text = player2Address;
                    player2AddressText.gameObject.SetActive(true);
                }
                if (player3Address != defaultAddress)
                {
                    player3AddressText.text = player3Address;
                    player3AddressText.gameObject.SetActive(true);
                }
            }
            else if (numberOfPlayerInGame.Length == 4)
            {
                player1AddressText.text = player1Address;
                player1AddressText.gameObject.SetActive(true);
                if (player2Address != defaultAddress)
                {
                    player2AddressText.text = player2Address;
                    player2AddressText.gameObject.SetActive(true);
                }
                if (player3Address != defaultAddress)
                {
                    player3AddressText.text = player3Address;
                    player3AddressText.gameObject.SetActive(true);
                }
                if (player4Address != defaultAddress)
                {
                    player4AddressText.text = player4Address;
                    player4AddressText.gameObject.SetActive(true);
                }
            }
            else if (numberOfPlayerInGame.Length == 5)
            {
                player1AddressText.text = player1Address;
                player1AddressText.gameObject.SetActive(true);
                if (player2Address != defaultAddress)
                {
                    player2AddressText.text = player2Address;
                    player2AddressText.gameObject.SetActive(true);
                }
                if (player3Address != defaultAddress)
                {
                    player3AddressText.text = player3Address;
                    player3AddressText.gameObject.SetActive(true);
                }
                if (player4Address != defaultAddress)
                {
                    player4AddressText.text = player4Address;
                    player4AddressText.gameObject.SetActive(true);
                }
                if (player5Address != defaultAddress)
                {
                    player5AddressText.text = player5Address;
                    player5AddressText.gameObject.SetActive(true);
                }
            }
            else if (numberOfPlayerInGame.Length == 6)
            {
                player1AddressText.text = player1Address;
                player1AddressText.gameObject.SetActive(true);
                if (player2Address != defaultAddress)
                {
                    player2AddressText.text = player2Address;
                    player2AddressText.gameObject.SetActive(true);
                }
                if (player3Address != defaultAddress)
                {
                    player3AddressText.text = player3Address;
                    player3AddressText.gameObject.SetActive(true);
                }
                if (player4Address != defaultAddress)
                {
                    player4AddressText.text = player4Address;
                    player4AddressText.gameObject.SetActive(true);
                }
                if (player5Address != defaultAddress)
                {
                    player5AddressText.text = player5Address;
                    player5AddressText.gameObject.SetActive(true);
                }
                if (player6Address != defaultAddress)
                {
                    player6AddressText.text = player6Address;
                    player6AddressText.gameObject.SetActive(true);
                }
            }
            else if (numberOfPlayerInGame.Length == 7)
            {
                player1AddressText.text = player1Address;
                player1AddressText.gameObject.SetActive(true);
                if (player2Address != defaultAddress)
                {
                    player2AddressText.text = player2Address;
                    player2AddressText.gameObject.SetActive(true);
                }
                if (player3Address != defaultAddress)
                {
                    player3AddressText.text = player3Address;
                    player3AddressText.gameObject.SetActive(true);
                }
                if (player4Address != defaultAddress)
                {
                    player4AddressText.text = player4Address;
                    player4AddressText.gameObject.SetActive(true);
                }
                if (player5Address != defaultAddress)
                {
                    player5AddressText.text = player5Address;
                    player5AddressText.gameObject.SetActive(true);
                }
                if (player6Address != defaultAddress)
                {
                    player6AddressText.text = player6Address;
                    player6AddressText.gameObject.SetActive(true);
                }
                if (player7Address != defaultAddress)
                {
                    player7AddressText.text = player7Address;
                    player7AddressText.gameObject.SetActive(true);
                }
            }
            else if (numberOfPlayerInGame.Length == 8)
            {
                player1AddressText.text = player1Address;
                player1AddressText.gameObject.SetActive(true);
                if (player2Address != defaultAddress)
                {
                    player2AddressText.text = player2Address;
                    player2AddressText.gameObject.SetActive(true);
                }
                if (player3Address != defaultAddress)
                {
                    player3AddressText.text = player3Address;
                    player3AddressText.gameObject.SetActive(true);
                }
                if (player4Address != defaultAddress)
                {
                    player4AddressText.text = player4Address;
                    player4AddressText.gameObject.SetActive(true);
                }
                if (player5Address != defaultAddress)
                {
                    player5AddressText.text = player5Address;
                    player5AddressText.gameObject.SetActive(true);
                }
                if (player6Address != defaultAddress)
                {
                    player6AddressText.text = player6Address;
                    player6AddressText.gameObject.SetActive(true);
                }
                if (player7Address != defaultAddress)
                {
                    player7AddressText.text = player7Address;
                    player7AddressText.gameObject.SetActive(true);
                }
                if (player8Address != defaultAddress)
                {
                    player8AddressText.text = player8Address;
                    player8AddressText.gameObject.SetActive(true);
                }
            }
        }

        [PunRPC]
        private void SetPlayer1Address(string player1AddressValue)
        {
            player1Address = player1AddressValue;
        }
        [PunRPC]
        private void SetPlayer2Address(string player2AddressValue)
        {
            player2Address = player2AddressValue;
        }
        [PunRPC]
        private void SetPlayer3Address(string player3AddressValue)
        {
            player3Address = player3AddressValue;
        }
        [PunRPC]
        private void SetPlayer4Address(string player4AddressValue)
        {
            player4Address = player4AddressValue;
        }
        [PunRPC]
        private void SetPlayer5Address(string player5AddressValue)
        {
            player5Address = player5AddressValue;
        }
        [PunRPC]
        private void SetPlayer6Address(string player6AddressValue)
        {
            player6Address = player6AddressValue;
        }
        [PunRPC]
        private void SetPlayer7Address(string player7AddressValue)
        {
            player7Address = player7AddressValue;
        }
        [PunRPC]
        private void SetPlayer8Address(string player8AddressValue)
        {
            player8Address = player8AddressValue;
        }
        [PunRPC]
        private void SetEscrowContractCreated(bool setValue)
        {
            escrowContractCreated = setValue;
        }
        [PunRPC]
        private void SetEscrowContractAddressServer(string escrowContractAddressServerValue)
        {
            escrowContractAddressServer = escrowContractAddressServerValue;
        }

        private void DisplayCreateEscrowContractButton()
        {
            Player[] numberOfPlayerInGame = GameManager.instance.punPlayersAll;
            if (numberOfPlayerInGame.Length == 2)
            {
                if (player1Address != "" && player2Address != defaultAddress)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        createSmartContractButton.gameObject.SetActive(true);
                    }
                    else {
                        waitingToInitializeText.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 3)
            {
                if (player1Address != "" && player2Address != defaultAddress && player3Address != defaultAddress)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        createSmartContractButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        waitingToInitializeText.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 4)
            {
                if (player1Address != "" && player2Address != defaultAddress && player3Address != defaultAddress &&
                    player4Address != defaultAddress)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        createSmartContractButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        waitingToInitializeText.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 5)
            {
                if (player1Address != "" && player2Address != defaultAddress && player3Address != defaultAddress &&
                    player4Address != defaultAddress && player5Address != defaultAddress)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        createSmartContractButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        waitingToInitializeText.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 6)
            {
                if (player1Address != "" && player2Address != defaultAddress && player3Address != defaultAddress &&
                   player4Address != defaultAddress && player5Address != defaultAddress && player6Address != defaultAddress)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        createSmartContractButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        waitingToInitializeText.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 7)
            {
                if (player1Address != "" && player2Address != defaultAddress && player3Address != defaultAddress &&
                   player4Address != defaultAddress && player5Address != defaultAddress && player6Address != defaultAddress &&
                   player7Address != defaultAddress)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        createSmartContractButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        waitingToInitializeText.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 8)
            {
                if (player1Address != "" && player2Address != defaultAddress && player3Address != defaultAddress &&
                   player4Address != defaultAddress && player5Address != defaultAddress && player6Address != defaultAddress &&
                   player7Address != defaultAddress && player8Address != defaultAddress)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        createSmartContractButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        waitingToInitializeText.gameObject.SetActive(true);
                    }
                }
            }
        }
        [PunRPC]
        private void SetPlayerReady1(bool isPlayer1Ready)
        {
            player1Ready = isPlayer1Ready; // Update local player1Ready state
        }

        [PunRPC]
        private void SetPlayerReady2(bool isPlayer2Ready)
        {
            player2Ready = isPlayer2Ready; // Update local player2Ready state
        }
        [PunRPC]
        private void SetPlayerReady3(bool isPlayer3Ready)
        {
            player3Ready = isPlayer3Ready; // Update local player3Ready state
        }
        [PunRPC]
        private void SetPlayerReady4(bool isPlayer4Ready)
        {
            player4Ready = isPlayer4Ready; // Update local player4Ready state
        }
        [PunRPC]
        private void SetPlayerReady5(bool isPlayer5Ready)
        {
            player5Ready = isPlayer5Ready; // Update local player5Ready state
        }
        [PunRPC]
        private void SetPlayerReady6(bool isPlayer6Ready)
        {
            player6Ready = isPlayer6Ready; // Update local player6Ready state
        }
        [PunRPC]
        private void SetPlayerReady7(bool isPlayer7Ready)
        {
            player7Ready = isPlayer7Ready; // Update local player7Ready state
        }
        [PunRPC]
        private void SetPlayerReady8(bool isPlayer8Ready)
        {
            player8Ready = isPlayer8Ready; // Update local player7Ready state
        }

        private void UpdatePlayerReadyStatus()
        {
            if (player1Ready == true)
            {
                player1StatusValueText.text = "Player 1: Ready";
                player1StatusValueText.gameObject.SetActive(true);
            }
            if (player2Ready == true)
            {
                player2StatusValueText.text = "Player 2: Ready";
                player2StatusValueText.gameObject.SetActive(true);
            }
            if (player3Ready == true)
            {
                player3StatusValueText.text = "Player 3: Ready";
                player3StatusValueText.gameObject.SetActive(true);
            }
            if (player4Ready == true)
            {
                player4StatusValueText.text = "Player 4: Ready";
                player4StatusValueText.gameObject.SetActive(true);
            }
            if (player5Ready == true)
            {
                player5StatusValueText.text = "Player 5: Ready";
                player5StatusValueText.gameObject.SetActive(true);
            }
            if (player6Ready == true)
            {
                player6StatusValueText.text = "Player 6: Ready";
                player6StatusValueText.gameObject.SetActive(true);
            }
            if (player7Ready == true)
            {
                player7StatusValueText.text = "Player 7: Ready";
                player7StatusValueText.gameObject.SetActive(true);
            }
            if (player8Ready == true)
            {
                player8StatusValueText.text = "Player 8: Ready";
                player8StatusValueText.gameObject.SetActive(true);
            }
        }
        private void DisplayStartGameButton()
        {
            Player[] numberOfPlayerInGame = GameManager.instance.punPlayersAll;
            if (numberOfPlayerInGame.Length == 2)
            {
                if (player1Ready && player2Ready)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        startButton.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 3)
            {
                if (player1Ready && player2Ready && player3Ready)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        startButton.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 4)
            {
                if (player1Ready && player2Ready && player3Ready && player4Ready)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        startButton.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 5)
            {
                if (player1Ready && player2Ready && player3Ready && player4Ready && player5Ready)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        startButton.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 6)
            {
                if (player1Ready && player2Ready && player3Ready && player4Ready && player5Ready && player6Ready)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        startButton.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 7)
            {
                if (player1Ready && player2Ready && player3Ready && player4Ready && player5Ready && player6Ready && player7Ready)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        startButton.gameObject.SetActive(true);
                    }
                }
            }
            else if (numberOfPlayerInGame.Length == 8)
            {
                if (player1Ready && player2Ready && player3Ready && player4Ready && player5Ready && player6Ready && player7Ready && player8Ready)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        startButton.gameObject.SetActive(true);
                    }
                }
            }
        }

        public GameObject bettingPanel;

        public void StartGame()
        {
            photonView.RPC("StartGameRPC", RpcTarget.All);
        }

        [PunRPC]
        private void StartGameRPC()
        {
            bettingPanel.SetActive(false); // Hide the betting panel for all players
            GameManager.instance.StartPlayingGame(); // Start the game for all players
            Debug.Log("Game started for all players."); // Debug log
        }

        public async void ClaimReward()
        {
            if (string.IsNullOrEmpty(escrowContractAddressServer))
            {
                return;
            }
            escrowContractAddress = escrowContractAddressServer;
            var contract = ThirdwebManager.Instance.SDK.GetContract(
              escrowContractAddress,
              abiString
              );
            BigInteger balanceValue = await contract.Read<BigInteger>("getBalance");
            decimal etherAmount = ConvertWeiToEther(balanceValue);
            if (etherAmount <= 0)
            {
                return;
            }
            claimButton.interactable = false;
            try
            {
                await contract.Write("withdrawAll");
                balanceValue = await contract.Read<BigInteger>("getBalance");
                etherAmount = ConvertWeiToEther(balanceValue);
            }
            catch (Exception ex)
            {
                // Log the error to Unity Console
                Debug.LogError($"An error occurred: {ex.Message}");
                claimButton.interactable = true;
            }

        }

        public Button claimRewardButton;

        private void ShowClaimButton() {
            if (!string.IsNullOrEmpty(GameManager.instance.winnerName))
            {
                //Debug.Log("GameManager.instance.winnerName: " + GameManager.instance.winnerName);
                //Debug.Log("photonView.IsMine: "+ photonView.IsMine);
                if (PhotonNetwork.LocalPlayer.NickName == GameManager.instance.winnerName)
                {
                    // Gán playerName từ PhotonNetwork.LocalPlayer.NickName
                    string playerName = PhotonNetwork.LocalPlayer.NickName;
                    Debug.Log("playerName: " + playerName);

                    // Kiểm tra nếu người chơi hiện tại là người đứng đầu
                    Debug.Log("Đây là nhân vật của tôi và tôi là người chơi đứng đầu.");
                    // Thực hiện các thiết lập hoặc điều khiển cần thiết cho nhân vật của mình
                    claimRewardButton.gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("Đây không phải là nhân vật của tôi.");
                    // Ẩn hoặc vô hiệu hóa các thành phần không cần thiết cho nhân vật của người chơi khác
                }
            }
            else
            {
                
            }
        }
    }
}