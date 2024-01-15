using System;
using System.Collections.Generic;
using System.Numerics;
using Cysharp.Threading.Tasks;
using Nethereum.Util;
using TMPro;
using Unity.Services.CloudCode;
using UnityEngine;
using UnityEngine.UI;

public class MintController : BaseController
{
    [Header("HUD")]
    public Button mintButton;
    public Button inventoryButton;
    public TextMeshProUGUI balanceValue;
    public GameObject defaultTxCostLabel;
    public GameObject sponsoredTxCostLabel;

    private bool _sponsored = false;
    
    public void Activate()
    {
        viewPanel.SetActive(true);
        GetErc20Balance();
    }
    
    #region GAME_EVENT_HANDLERS
    public void AdsController_OnAdWatched_Handler(bool adWatched)
    {
        CloudCodeMessager.Instance.OnMintNftSuccessful += CloudCodeMessager_OnMintNftSuccessful_Handler;
        
        // If the player has watched the ad, we will sponsor the mint transaction.
        // If not, he will pay the gas fees with test tokens
        _sponsored = adWatched;
        
        GetErc20Balance();

        if (_sponsored)
        {
            sponsoredTxCostLabel.SetActive(true);
        }
        else
        {
            defaultTxCostLabel.SetActive(true);
        }
        
        viewPanel.SetActive(true);
    }
    #endregion
    
    #region CLOUD_CODE_METHODS
    private async void MintNFT()
    {
        statusText.Set("Minting NFT...");
        mintButton.interactable = false;
        inventoryButton.interactable = false;

        try
        {
            var functionParams = new Dictionary<string, object> {{"sponsored", _sponsored}};
            await CloudCodeService.Instance.CallModuleEndpointAsync(CurrentCloudModule, "MintNFT", functionParams);
            // Let's wait for the message from backend --> Inside SubscribeToCloudCodeMessages()
        }
        catch (Exception e)
        {
            // Check if the string contains the word "timeout"
            bool containsTimeout = e.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase);

            if (containsTimeout)
            {
                Debug.Log("Timeout, waiting for cloud message...");
            }
            else
            {
                Console.WriteLine(e);
                statusText.Set("Transaction failed.");
            
                mintButton.interactable = true;
                inventoryButton.interactable = true;
                
                // Just in case
                GetErc20Balance();
                throw;
            }
        }
    }
    
    private async void GetErc20Balance()
    {
        try
        {
            var balance = await CloudCodeService.Instance.CallModuleEndpointAsync<string>(CurrentCloudModule, "GetErc20Balance");

            if (string.IsNullOrEmpty(balance))
            {
                balanceValue.text = "0.00";
            }
            else
            {
                // The amount in wei. Assuming it comes in wei
                BigInteger balanceInWei = BigInteger.Parse(balance);
                // Assuming decimals is the number of decimal places for the token
                int decimals = 18;
                // Convert to tokens using Nethereum
                decimal amountInTokens = UnitConversion.Convert.FromWei(balanceInWei, decimals);
                
                // Format the decimal value with two decimal places
                string formattedAmount = amountInTokens.ToString("0.00");
                
                balanceValue.text = formattedAmount;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private void CloudCodeMessager_OnMintNftSuccessful_Handler()
    {
        statusText.Set("Transaction successful.");
        
        mintButton.interactable = true;
        inventoryButton.interactable = true;

        // We only sponsor the transaction 1 time
        if (_sponsored)
        {
            _sponsored = false;
            // We set the correct label
            sponsoredTxCostLabel.SetActive(false);
            defaultTxCostLabel.SetActive(true);
        }
        
        GetErc20Balance();
    }
    #endregion
}