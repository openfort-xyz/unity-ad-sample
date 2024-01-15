using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudCode;
using UnityEngine;
using UnityEngine.Events;

public class TokensController : BaseController
{
    public UnityEvent<bool> onTokensGranted;

    public TextMeshProUGUI balanceValue;
    
    #region GAME_EVENT_HANDLERS
    public void AuthController_OnAuthSuccess_Handler()
    {
        CloudCodeMessager.Instance.OnGrantTokensSuccessful += CloudCodeMessager_OnGrantTokensSuccessful_Handler;
        GrantTestTokens(1);
    }

    private void OnDisable()
    {
        CloudCodeMessager.Instance.OnGrantTokensSuccessful -= CloudCodeMessager_OnGrantTokensSuccessful_Handler;
    }
    #endregion
    
    #region CLOUD_CODE_METHODS
    private async void GrantTestTokens(decimal amount)
    {
        statusText.Set("Granting test tokens...");

        try
        {
            var functionParams = new Dictionary<string, object> {{"amount", amount}};
            await CloudCodeService.Instance.CallModuleEndpointAsync(CurrentCloudModule, "GrantTestTokens", functionParams);
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
                statusText.Set("Tokens not granted.");
                onTokensGranted.Invoke(false);
                throw;
            }
        }
    }
    
    private void CloudCodeMessager_OnGrantTokensSuccessful_Handler()
    {
        Debug.Log("Tokens granted.");
        onTokensGranted?.Invoke(true);
        
        // TODO We should get erc20 balance correctly like we do in MintController.cs
        balanceValue.text = "1.00";
    }
    #endregion
}
