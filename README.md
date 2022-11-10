

# Enteractive.SDK.Demo


## Quick Start

-   Download latest Release
    
-   Clone the repo: git clone [https://github.com/enteractive/sdkdemo.git](https://github.com/enteractive/sdkdemo.git)
    
-   Install Enteractive SDK: [https://www.nuget.org/packages/Enteractive.SDK](https://www.nuget.org/packages/Enteractive.SDK)
    
-   Install Enteractive SDK with Package Manager: Install-Package Enteractive.SDK
    

## Introduction

Enteractive SDK Demo project is a demonstration of how to implement and utilize the features the Enteractive.SDK package offers. The aim of this demo is to facilitate and simplify the data transfers and data synchronisation between Enteractive and its operators. The Enteractive SDK Demo project can be adapted with minimal effort to accommodate the business requirements of our partners.

### Requirements

-   Enteractive SDK package  
    Available on NuGet https://www.nuget.org/packages/Enteractive.SDK/
    
-   Enteractive API credentials  
    Contact Enteractive sales team for more information [support team](mailto:%20itsupport@enteractive.se)
    
## Customising the demo

### Enteractive Client API credentials  
The Enteractive’s Client API credentials are required to be able to initialise and operate the Enteractive SDK package. The credentials are stored in the appsettings.json config file, please update these settings with your credentials.

### Update the Brand Name mapping  
The MapBrandNames() method in the PlayerImport.cs and PlayerUpdate.cs classes is used to ensure that the brand names passed to Enteractive are correct and the players will be correctly associated with their respective brand. Update the mapping of the MapBrandNames() method with the appropriate brand names.

### Data source integration  
Currently the GetData() method in the PlayerImport.cs and PlayerUpdate.cs classes is using mock player data for demonstration purposes, this method should be updated to retrieve data from your data source.


## About The Code

### Player Import class

The PlayerImport.cs class demonstrates the process of importing new players onto Enteractive’s ReActivation Cloud.

Flow:

1.  Get player data from a relevant data source     
2.  Validate the data to make sure that a call project for the players’ country, brand and campaign type exists on Enteractive.    
3.  Generate the add players request object (Enteractive.SDK.Models.V2.Requests.AddPlayersRequest) with the validated player data.    
4.  Process request using the Enteractive.SDK’s AddPlayers(AddPlayersRequest request) method in the Player Class.    
5.  Await the result from the AddPlayers method and check if the request was successful.
    

#### Player Update class

The PlayerUpdate.cs class demonstrates the process of updating the information of players which have already been imported on the Enteractive’s ReActivation Cloud.

Flow:

1.  Retrieve the Player Checklist from Enteractive SDK EnteractiveClient.PlayersBulk.GetPlayerCheckList() 
2.  Get player data for the players present in the Checklist
3.  Generate a sync player request object for each player   (Enteractive.SDK.Models.V2.Requests. SyncPlayer)   
4.  Check players eligibility and close any ineligible players
5.  Process the request using the Enteractive.SDK’s SyncPlayers(SyncPlayersRequest request) method in the PlayersBulk.cs class.
6.  Await the result from the SyncPlayers method and check if the request was successful.
      

## Support 

For any support or questions about this demo, feel free to contact our technical team [support team](mailto:%20itsupport@enteractive.se)
