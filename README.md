# Openfort Unity LevelPlay Sample
<div align="center">
    <img
      width="100%"
      height="100%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample17_35e3238c3e.png?updated_at=2024-01-12T10:56:23.117Z"
      alt='Openfort Unity LevelPlay Sample'
    />
</div>

## Overview

This sample project showcases the Openfort integration with [Unity LevelPlay](https://docs.unity.com/monetization-dashboard/en-us/manual/UnityLevelPlay).

The sample includes:
  - [**`ugs-backend`**](https://github.com/openfort-xyz/iap-unity-sample/tree/main/ugs-backend)
    
    A .NET Core project with [Cloud Code C# modules](https://docs.unity.com/ugs/en-us/manual/cloud-code/manual/modules#Cloud_Code_C#_modules) that implement [Openfort C# SDK](https://www.nuget.org/packages/Openfort.SDK/1.0.21) methods. Needs to be hosted in Unity Gaming Services.

  - [**`unity-client`**](https://github.com/openfort-xyz/iap-unity-sample/tree/main/unity-client)

    A Unity sample game that connects to ``ugs-backend`` through [Cloud Code](https://docs.unity.com/ugs/manual/cloud-code/manual). It uses [Openfort Unity SDK](https://github.com/openfort-xyz/openfort-csharp-unity) to have full compatibility with `ugs-backend` responses.

## Workflow diagram

<div align="center">
    <img
      width="100%"
      height="100%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample_workflow_9af5eeba53.png?updated_at=2024-01-15T11:53:31.312Z"
      alt='Openfort Unity LevelPlay Sample diagram'
    />
</div>

## Prerequisites
+ **Get started with Openfort**
  + [Sign in](https://dashboard.openfort.xyz/login) or [sign up](https://dashboard.openfort.xyz/register) and create a new dashboard project

+ **Get started with UGS**
  + [Complete basic prerequisites](https://docs.unity.com/ugs/manual/overview/manual/getting-started#Prerequisites)

+ **Get started with ironSource**:
  + [Sign up](https://developers.is.com/ironsource-mobile/air/sign-up-ironsource/) on the ironSource website

## Set up Openfort dashboard
  
  + [Add an NFT contract](https://dashboard.openfort.xyz/assets/new)
    
    This sample requires an NFT contract to run. We use [0x38090d1636069c0ff1Af6bc1737Fb996B7f63AC0](https://mumbai.polygonscan.com/address/0x38090d1636069c0ff1Af6bc1737Fb996B7f63AC0) (contract deployed in 80001 Mumbai). You can use it for this tutorial too:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/ugs_integration_4_9397f3633b.png?updated_at=2023-12-14T15:59:33.808Z"
      alt='Contract Info'
    />
    </div>
  
  + [Add an ERC20 contract](https://dashboard.openfort.xyz/assets/new)
    
    This sample also requires an ERC20 contract to run. You can [deploy a standard one](https://thirdweb.com/thirdweb.eth/TokenERC20) and then add it to the Openfort dashboard following the same logic as above.

  + [Add a Full Sponsor Policy](https://dashboard.openfort.xyz/policies/new)
    
    We aim to cover gas fees for our players when they mint the NFT (if they have watched the ad video). Set a new gas policy for that:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/ugs_integration_5_ab3d8ad48d.png?updated_at=2023-12-14T15:59:33.985Z"
      alt='Gas Policy'
    />
    </div>

    Add a rule so the NFT contract uses this policy:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/ugs_integration_6_6727e69146.png?updated_at=2023-12-14T15:59:33.683Z"
      alt='NFT Policy Rule'
    />
    </div>

    Add also a rule for the ERC20 contract, as we want to send some ERC20 tokens to the player to be able to test the sample:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/iap_sample_22_aec7863428.png?updated_at=2023-12-31T16:02:32.817Z"
      alt='ERC20 Policy Rule'
    />
    </div>

  + [Add a Fixed Charge Policy](https://dashboard.openfort.xyz/policies/new)

    The players will be charged with 1 in-game ERC20 token when they decide not to watch the ad:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample18_2b75d93690.png?updated_at=2024-01-14T18:31:55.626Z"
      alt='Fixed charge policy'
    />
    </div>

    Add a rule so the policy applies to the NFT contract:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/ugs_integration_6_6727e69146.png?updated_at=2023-12-14T15:59:33.683Z"
      alt='NFT Policy Rule'
    />
    </div>

  + [Add a Developer Account](https://dashboard.openfort.xyz/accounts)

    Enter a name and choose ***Add account***:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/iap_sample_23_74b85444b2.png?updated_at=2023-12-31T16:09:09.921Z"
      alt='Developer account'
    />
    </div>

    This will automatically create a custodial wallet that we'll use to send the ERC20 tokens to the players. **IMPORTANT: Transfer a good amount of tokens from the created ERC20 contract to this wallet to facilitate testing**.

## Set up ironSource

- ### Get Unity Cloud keys

  Before going into ironSource, go to the [Unity Cloud dashboard](https://cloud.unity.com/) and open ***Unity Ads Monetization*** using *Shortcuts*:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample19_7bf501e9a9.png?updated_at=2024-01-14T21:56:19.185Z"
      alt='Unity Cloud dashboard: Unity Ads Monetization'
    />
  </div>
  
  - #### Create a LevelPlay Service Account

    Now go to the ***API management*** section and choose ***Create LevelPlay Service Account***:

    <div align="center">
      <img
        width="50%"
        height="50%"
        src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample12_c2ec3e5dd7.png?updated_at=2024-01-08T17:25:19.881Z"
        alt='Unity Cloud dashboard: Create LevelPlay Service Account'
      />
    </div>

    Copy and save the ***Key ID*** and the ***Secret key*** somewhere safe and choose ***Done***:

    <div align="center">
      <img
        width="50%"
        height="50%"
        src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample13_adb919080c.png?updated_at=2024-01-08T17:25:19.985Z"
        alt='Unity Cloud dashboard: Copy LevelPlay Service Account credentials'
      />
    </div>

  - #### Get Monetization Stats API Access

    Copy and save the API Key. Choose ***Create API Key*** if it's not already there:

    <div align="center">
      <img
        width="50%"
        height="50%"
        src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample31_1139cc3e32.png?updated_at=2024-01-14T22:10:25.975Z"
        alt='Unity Cloud dashboard: copy Monetization Stats API key'
      />
    </div>
  
  - #### Get Organization Core ID

    Copy and save the Organization Core ID:

    <div align="center">
      <img
        width="50%"
        height="50%"
        src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample32_07789b5094.png?updated_at=2024-01-14T22:10:08.028Z"
        alt='Unity Cloud dashboard: copy Organization Core ID'
      />
    </div>

- ### Create an ironSource LevelPlay app
  Go to the [ironSource dashboard](https://platform.ironsrc.com/partners/dashboard) and under the *LevelPlay* section, choose ***Add app*** and enter your app details:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample1_a6eae902d4.png?updated_at=2024-01-08T15:23:19.695Z"
      alt='ironSource: new app'
    />
  </div>

  Select the following settings and choose ***Add app***:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample2_07485439ba.png?updated_at=2024-01-08T15:31:25.290Z"
      alt='ironSource: app details'
    />
  </div>

  Activate ***Rewarded Video*** as an ad unit and choose ***Continue***:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample3_2acaeaa57e.png?updated_at=2024-01-08T15:36:02.386Z"
      alt='ironSource: activate ad units'
    />
  </div>

- ### Set up SDK Networks
  In the *Available Networks* panel select ***Unity Ads***:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample4_549e6a7164.png?updated_at=2024-01-08T15:48:55.693Z"
      alt='ironSource: available networks'
    />
  </div>

  Enable the ***Unity bidder auto-setup*** option, add all the credentials from the [*Get Unity Cloud keys* section](https://github.com/dpradell-dev/openfort-unity-ad-sample#get-unity-cloud-keys) and choose ***Save***:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample5_41d6ab375c.png?updated_at=2024-01-08T17:32:19.205Z"
      alt='ironSource: adding Unity Ads'
    />
  </div> 

  *Unity Ads* will have appeared as a new available network. Choose ***Setup***:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample14_d57379d965.png?updated_at=2024-01-08T17:49:05.283Z"
      alt='ironSource: setup Unity Ads'
    />
  </div>

  Because we enabled the *Unity bidder auto-setup* option, now you can choose ***Add bidder***:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample15_eecec77e16.png?updated_at=2024-01-08T17:49:05.583Z"
      alt='ironSource: add bidder'
    />
  </div>

  The needed information from Unity will be automatically retrieved. Choose ***Save***:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample16_5141cc4b35.png?updated_at=2024-01-08T17:49:05.590Z"
      alt='ironSource: save app'
    />
  </div>

## Set up Unity Cloud

Thanks to the *Unity bidder auto-setup* option, a new project has been automatically created in the [Unity Cloud dashboard](https://cloud.unity.com/). Now your LevelPlay Service Account needs to have some admin roles over this newly created project. Go to ***Administration --> Service accounts*** and choose your account:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample20_9dea7b262c.png?updated_at=2024-01-14T21:56:23.784Z"
      alt='Set up Unity Cloud: service account'
    />
  </div>

Scroll down and choose ***Manage project roles***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample21_50cb464fa0.png?updated_at=2024-01-14T21:56:23.788Z"
      alt='Set up Unity Cloud: manage project roles'
    />
  </div>

Select your project and choose ***Next***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample22_980d7e7834.png?updated_at=2024-01-14T21:56:23.877Z"
      alt='Set up Unity Cloud: select project'
    />
  </div>

In the *Admin* dropdown select:

+ ***Player Resource Policy Editor***
+ ***Project Resource Policy Editor***
+ ***Unity Environments Admin***

In the *LiveOps* dropdown select:

+ ***Cloud Code Script Publisher***
+ ***Triggers Configuration Editor***
+ ***Leaderboards Admin***
+ ***Cloud Code Editor***

Choose ***Save***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample23_b8f548f685.png?updated_at=2024-01-14T21:56:23.983Z"
      alt='Set up Unity Cloud dashboard: add roles'
    />
  </div>

## Set up [`ugs-backend`](https://github.com/openfort-xyz/iap-unity-sample/tree/main/ugs-backend)

- ### Set Openfort dashboard variables

  Open the [solution](https://github.com/openfort-xyz/iap-unity-sample/blob/main/ugs-backend/CloudCodeModules.sln) with your preferred IDE, open [``SingletonModule.cs``](https://github.com/openfort-xyz/iap-unity-sample/blob/main/ugs-backend/CloudCodeModules/SingletonModule.cs) and fill in these variables:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample24_bbba67daca.png?updated_at=2024-01-14T21:56:19.781Z"
      alt='Singleton Module'
    />
    </div>

  - `OfApiKey`: [Retrieve the **Openfort secret key**](https://dashboard.openfort.xyz/apikeys)
  - `OfNftContract`: [Retrieve the **NFT contract API ID**](https://dashboard.openfort.xyz/assets)
  - `OfGoldContract`: [Retrieve the **ERC20 contract API ID**](https://dashboard.openfort.xyz/assets)
  - `OfFullSponsorPolicy`: [Retrieve the **Full Sponsor Policy API ID**](https://dashboard.openfort.xyz/policies)
  - `OfChargeErc20Policy`: [Retrieve the **Fixed Charge Policy API ID**](https://dashboard.openfort.xyz/policies)
  - `OfDevAccount`: [Retrieve the **Developer Account API ID**](https://dashboard.openfort.xyz/accounts)

- ### Package Code
  Follow [the official documentation steps](https://docs.unity.com/ugs/en-us/manual/cloud-code/manual/modules/getting-started#Package_code).
- ### Deploy to UGS
  Follow [the official documentation steps](https://docs.unity.com/ugs/en-us/manual/cloud-code/manual/modules/getting-started#Deploy_a_module_project).

## Set up [``unity-client``](https://github.com/openfort-xyz/iap-unity-sample/tree/main/unity-client)

In Unity go to *Edit --> Project Settings --> Services* and link the ``unity-client`` to your UGS Project:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample25_61a831c3fe.png?updated_at=2024-01-14T21:56:23.781Z"
      alt='Services settings'
    />
</div>

Select your *Environment*:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample26_230b3e9819.png?updated_at=2024-01-14T21:56:22.480Z"
      alt='UGS environment'
    />
</div>

Under *Assets --> Scripts --> Controllers* open the ***AdsController.cs***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample27_45abf61c4b.png?updated_at=2024-01-14T21:56:14.982Z"
      alt='AdsController.cs'
    />
</div>

Fill the **``appKey``** variable with the *ironSource LevelPlay app key* and save the script:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample29_731247fac0.png?updated_at=2024-01-14T21:56:24.804Z"
      alt='ironSource LevelPlay app key'
    />
</div>

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample28_0ead9e68e7.png?updated_at=2024-01-14T21:56:19.186Z"
      alt='ironSource LevelPlay app key'
    />
</div>

## Build to Android

In Unity go to [*Android Player settings*](https://docs.unity3d.com/Manual/class-PlayerSettingsAndroid.html) and make sure *Other Settings* looks like this:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/iap_sample_7_e6ec7eb903.png?updated_at=2023-12-28T07:47:59.386Z"
      alt='Android Player settings'
    />
</div>

Also, make sure to sign the application with a [Keystore](https://docs.unity3d.com/Manual/android-keystore-create.html) in *Publishing Settings*:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/iap_sample_8_ecae38df0e.png?updated_at=2023-12-28T07:47:59.307Z"
      alt='Application Signing'
    />
</div>

Return to *Build Settings* and choose ***Build***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_ads_sample30_2b8dbf10b8.png?updated_at=2024-01-14T22:13:25.912Z"
      alt='Build'
    />
</div>

Send and run the *.apk* on your device. 

## Conclusion

Upon completing the above steps, your Unity game will be fully integrated with Openfort and [Unity LevelPlay](https://docs.unity.com/monetization-dashboard/en-us/manual/UnityLevelPlay). Always remember to test every feature before deploying to guarantee a flawless player experience.

For a deeper understanding of the underlying processes, check out the [tutorial video](https://www.youtube.com/watch?v=vjjvDILS-DU). 

## Get support
If you found a bug or want to suggest a new [feature/use case/sample], please [file an issue](../../issues).

If you have questions, or comments, or need help with code, we're here to help:
- on Twitter at https://twitter.com/openfortxyz
- on Discord: https://discord.com/invite/t7x7hwkJF4
- by email: support+youtube@openfort.xyz
