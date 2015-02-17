# Rest API for EPiServer Commerce #
A simple REST API for EPiServer Commerce for development purposes. This toolset should be treated as a utility box for your development project. 

> **Important!**  This library has not been security tested, and should not be used on production systems. It contains commands that could render your site unusable, and could also expose sensitive data.

**Note:** This is not the official EPiServer Service API, that can be found on: http://world.episerver.com/documentation/Items/EPiServer-Service-API/ 

## Unsupported ##
This code is completely unsupported, some of the methods could irreversibly delete some or all of your Commerce and / or Cms data. Please consume responsibly.

## Note About REST Principles ##
The API mostly does not adhere to REST principles, please see code for usage of the API. It means the methods uses query strings where it should not, HTTP methods are not used consistenly (a GET could change data) and even more worst practises. Pull Requests are welcomed!

## Installation ##
Run the following command in the Nuget Package Manager Console for your web site (not Commerce Manager site):
```
install-package EPiCode.Commerce.RestApi
```
You need to add the EPiServer Nuget Feed to Visual Studio (see http://nuget.episerver.com/en/Feed/)

## Usage ##
You can call the different API methods using any rest client (like curl or custom code) as long as you specify the `apikey` HTTP header as part of the call.

The easiest way to experiment the REST API is to install the Postman App for Chrome and import the collection of API definitions as shown below.

### 1. Install the Postman App ###
Goto https://chrome.google.com/webstore/detail/postman-rest-client/fdmmgilgnpjigdojojpjoooidkmcomcm

![](https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/images/postman-install-app.png)

### 2. Import the API collection ###
Click the "Import Collection" button: 
![](https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/images/postman-import-collection.png)

Import from the following url:
`https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/Commerce-Rest-API-Postman-Collection.txt`
 
### 3. Add Environment ###
Click Manage Environments, add a new one: 
![](https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/images/postman-manage-environment.png)

Specify the host and apikey to use, the keys are important, as the imported collection uses these.

### 4. Add API Key to appSettings ###
Add the following to appSettings in web.config: 
``` 
<add key="EPiCode.Commerce.Rest.ApiKey"
     value="some-hard-to-guess-secret-here" />
```

### 5. Try it out ###
You should now be able to use the different REST methods imported. Try the `Meta Class: Get All` method:
![](https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/images/postman-call-method.png)

**Note!** Make sure you have selected the correct environment before you click Send.

If you want to use Powershell, you can use the `invoke-webrequest` cmdlet:
```
$r = Invoke-WebRequest -Uri http://localhost:49883/api/Price/Get?code=Felsina-Berardenga-Chianti-Classico-2010_1 -Headers @{"apikey"="mysecret"}
```
If you print `$.Content` it returns:
```json
[
{
	"CatalogKey": {
		"ApplicationId": "52186ec3-5f1f-4105-a654-396de70ba003",
		"CatalogEntryCode": "Felsina-Berardenga-Chianti-Classico-2010_1"
	},
	"MarketId": {
		"Value": "NORWAY"
	},
	"CustomerPricing": {
		"PriceTypeId": 0,
		"PriceCode": ""
	},
	"ValidFrom": "2014-10-28T10:33:00Z",
	"ValidUntil": "2028-10-28T09:33:00Z",
	"MinQuantity": 0.000000000,
	"UnitPrice": {
		"Amount": 149.0000,
		"Currency": "NOK"
	}
}
]
```

## Example Usages ##
**Note!** You will be running commands in the context of the current Chrome instance that Postman runs in. You can use this to change the context of your REST calls. Just open a new tab, visit your site and add something to the cart, log in if you want to see your orders etc.

### See the Current Cart ###
Use the "Cart: Get Current Cart" 

Method: `GET {{host}}/api/cartinfo/get` 

Returns:
```json
{
    "__Version": "5.0.0.1",
    "__State": 3,
    "__MetaClass": "ShoppingCart",
    "AddressId": "",
    "SiteId": null,
    "CustomerId": "3153b801-ad9c-4370-a4ff-226fc60abcd4",
    "Owner": "",
    "Name": "Default",
    "CustomerName": "",
    "BillingCurrency": "USD",
    "ApplicationId": "52186ec3-5f1f-4105-a654-396de70ba003",
    "OrderGroupId": 596,
    "MarketId": "DEFAULT",
    "ProviderId": "FrontEnd",
    "SubTotal": 23.95,
    "HandlingTotal": 0,
    "OwnerOrg": "",
    "ShippingTotal": 0,
    "TaxTotal": 0,
    "Status": "",
    "InstanceId": "d31420ad-93ad-4b31-931b-cbd84cd2bb0e",
    "Total": 23.95,
    "AffiliateId": null,
    "OrderForms": [
        {
            "__Version": "5.0.0.1",
            "__State": 3,
            "__MetaClass": "OrderFormEx",
            "MemberClub": null,
            "RMANumber": "",
            "SelectedCategories": null,
            "Status": null,
            "BillingAddressId": null,
            "ReturnType": null,
            "ExchangeOrderGroupId": null,
            "Name": "Default",
            "ProviderId": null,
            "ReturnAuthCode": null,
            "CapturedPaymentTotal": 0,
            "OrderGroupId": 596,
            "ReturnComment": null,
            "DiscountAmount": 0,
            "AuthorizedPaymentTotal": 0,
            "SubTotal": 23.95,
            "HandlingTotal": 0,
            "ShippingTotal": 0,
            "TaxTotal": 0,
            "OrderFormId": 561,
            "Total": 23.95,
            "OrigOrderFormId": null,
            "LineItems": [
                {
                    "__Version": "5.0.0.1",
                    "__State": 3,
                    "__MetaClass": "LineItemEx",
                    "ArticleNumber": "Tops-Tunics-CowlNeck-Chocolate",
                    "Color": "",
                    "ColorImageUrl": "/globalassets/catalogs/fashion/swatches/tops-tunic-cowlneck-chocolate-swatch.jpg",
                    "ImageUrl": "/globalassets/catalogs/fashion/cowlneck.jpg",
                    "ItemSize": "Small",
                    "WineRegion": "",
                    "ParentCatalogEntryId": "Tops-Tunics-CowlNeck-Chocolate",
                    "InStockQuantity": 0,
                    "LineItemDiscountAmount": 0,
                    "PlacedPrice": 23.95,
                    "ReturnReason": null,
                    "MaxQuantity": 50,
                    "ProviderId": null,
                    "ConfigurationId": null,
                    "OrigLineItemId": null,
                    "AllowBackordersAndPreorders": false,
                    "IsInventoryAllocated": false,
                    "ShippingMethodName": null,
                    "Catalog": "Starterkit",
                    "OrderFormId": 561,
                    "DisplayName": "Tops-Tunics-CowlNeck-Chocolate-Small",
                    "CatalogNode": "",
                    "ListPrice": 23.95,
                    "OrderLevelDiscountAmount": 0,
                    "Status": null,
                    "LineItemId": 961,
                    "CatalogEntryId": "Tops-Tunics-CowlNeck-Chocolate-Small",
                    "ReturnQuantity": 0,
                    "ShippingAddressId": "",
                    "OrderGroupId": 596,
                    "Quantity": 1,
                    "WarehouseCode": "default",
                    "PreorderQuantity": 0,
                    "Description": "",
                    "ExtendedPrice": 23.95,
                    "BackorderQuantity": 0,
                    "MinQuantity": 1,
                    "ShippingMethodId": "00000000-0000-0000-0000-000000000000",
                    "InventoryStatus": 1,
                    "LineItemOrdering": "2015-02-17T14:53:38.84Z",
                    "Discounts": []
                }
            ],
            "Discounts": [],
            "Shipments": [],
            "Payments": []
        }
    ],
    "OrderAddresses": [],
    "OrderNotes": []
}
```

### Get Contact Information ###
There are several methods to retrieve information about a customer:

* Contact: Get Contact By Email
* Contact: Get All Contacts
* Contact: Get Contact By Primary Key

Method: `GET {{host}}/api/Contact/get/?email=receiving@unknowdomain.domain`

Returns:
```json
{
    "ProviderUserKey": "receiving",
    "PreferredShippingAddress": null,
    "PreferredBillingAddress": null,
    "CustomerGroup": null,
    "EffectiveCustomerGroup": null,
    "ContactCreditCards": [],
    "ContactAddresses": [],
    "ContactOrganization": null,
    "ExtendedProperties": [
        {
            "_name": "PhoneNumber",
            "_value": null
        },
        {
            "_name": "Category",
            "_value": []
        },
        {
            "_name": "Gender",
            "_value": null
        },
        {
            "_name": "HasPassword",
            "_value": true
        }
    ],
    "Created": "2014-04-03T10:31:37.027+02:00",
    "Modified": "2014-04-03T10:31:37.027+02:00",
    "CreatorId": "270b2401-2450-4a68-8a53-05d846d73a85",
    "ModifierId": "f8e14c14-b414-4830-84ff-3135d70a225a",
    "FullName": "receiving",
    "LastName": "",
    "FirstName": "receiving",
    "MiddleName": null,
    "Password": null,
    "Email": "receiving@unknowdomain.domain",
    "BirthDate": null,
    "LastOrder": null,
    "Code": null,
    "PreferredLanguage": null,
    "PreferredCurrency": null,
    "RegistrationSource": null,
    "OwnerId": null,
    "Owner": null,
    "PreferredShippingAddressId": null,
    "PreferredBillingAddressId": null,
    "UserId": "String:receiving",
    "MetaClassName": "Contact",
    "PrimaryKeyId": "ed0898d9-eda9-4a0b-a136-0b9ba4f352cd",
    "Properties": [
        {
            "_name": "Created",
            "_value": "2014-04-03T10:31:37.027+02:00"
        },
        {
            "_name": "Modified",
            "_value": "2014-04-03T10:31:37.027+02:00"
        },
        {
            "_name": "CreatorId",
            "_value": "270b2401-2450-4a68-8a53-05d846d73a85"
        },
        {
            "_name": "ModifierId",
            "_value": "f8e14c14-b414-4830-84ff-3135d70a225a"
        },
        {
            "_name": "FullName",
            "_value": "receiving"
        },
        {
            "_name": "LastName",
            "_value": ""
        },
        {
            "_name": "FirstName",
            "_value": "receiving"
        },
        {
            "_name": "MiddleName",
            "_value": null
        },
        {
            "_name": "Password",
            "_value": null
        },
        {
            "_name": "Email",
            "_value": "receiving@unknowdomain.domain"
        },
        {
            "_name": "BirthDate",
            "_value": null
        },
        {
            "_name": "LastOrder",
            "_value": null
        },
        {
            "_name": "CustomerGroup",
            "_value": null
        },
        {
            "_name": "Code",
            "_value": null
        },
        {
            "_name": "PreferredLanguage",
            "_value": null
        },
        {
            "_name": "PreferredCurrency",
            "_value": null
        },
        {
            "_name": "RegistrationSource",
            "_value": null
        },
        {
            "_name": "OwnerId",
            "_value": null
        },
        {
            "_name": "Owner",
            "_value": null
        },
        {
            "_name": "PreferredShippingAddressId",
            "_value": null
        },
        {
            "_name": "PreferredShippingAddress",
            "_value": null
        },
        {
            "_name": "PreferredBillingAddressId",
            "_value": null
        },
        {
            "_name": "PreferredBillingAddress",
            "_value": null
        },
        {
            "_name": "UserId",
            "_value": "String:receiving"
        },
        {
            "_name": "PhoneNumber",
            "_value": null
        },
        {
            "_name": "Category",
            "_value": []
        },
        {
            "_name": "Gender",
            "_value": null
        },
        {
            "_name": "HasPassword",
            "_value": true
        }
    ]
}
```

### Get Order Information ###
There are several methods that return order information:
* Order: Get Active Orders
* Order: Get All Orders for Customer
* Order: Get By ID
* Order: Get Order by Tracking Number

**Method:** `GET {{host}}/api/order/get`
**Note!** This returns all order IDs from the database. Do not run this on a production database with many orders in it. 

Returns:
```json
[
    591,
    597,
    598
]
```

**Method:** `GET {{host}}/api/order/get?trackingNumber=PO594`
Returns:
```json
{
    "__Version": "5.0.0.1",
    "__State": 3,
    "__MetaClass": "PurchaseOrder",
    "BackendOrderNumber": null,
    "CardNumberMasked": null,
    "CardTypeName": null,
    "ExpirationDate": null,
    "JeevesId": null,
    "ParentOrderGroupId": 0,
    "PostNordTrackingId": null,
    "TrackingNumber": "PO594",
    "AddressId": "",
    "SiteId": null,
    "CustomerId": "b221df97-2771-4ac1-a042-dd3ab81c0680",
    "Owner": "",
    "Name": "Default",
    "CustomerName": "admin",
    "BillingCurrency": "USD",
    "ApplicationId": "52186ec3-5f1f-4105-a654-396de70ba003",
    "OrderGroupId": 597,
    "MarketId": "DEFAULT",
    "ProviderId": "FrontEnd",
    "SubTotal": 47.9,
    "HandlingTotal": 0,
    "OwnerOrg": "",
    "ShippingTotal": 0,
    "TaxTotal": 0,
    "Status": "InProgress",
    "InstanceId": "25a7352d-601c-4c49-a88d-a86dca25acc3",
    "Total": 47.9,
    "AffiliateId": "00000000-0000-0000-0000-000000000000",
    "OrderForms": [
        {
            "__Version": "5.0.0.1",
            "__State": 3,
            "__MetaClass": "OrderFormEx",
            "MemberClub": false,
            "RMANumber": "",
            "SelectedCategories": null,
            "Status": null,
            "BillingAddressId": "Billing Address",
            "ReturnType": null,
            "ExchangeOrderGroupId": null,
            "Name": "Default",
            "ProviderId": null,
            "ReturnAuthCode": null,
            "CapturedPaymentTotal": 47.9,
            "OrderGroupId": 597,
            "ReturnComment": null,
            "DiscountAmount": 0,
            "AuthorizedPaymentTotal": 0,
            "SubTotal": 47.9,
            "HandlingTotal": 0,
            "ShippingTotal": 0,
            "TaxTotal": 0,
            "OrderFormId": 563,
            "Total": 47.9,
            "OrigOrderFormId": null,
            "LineItems": [
                {
                    "__Version": "5.0.0.1",
                    "__State": 3,
                    "__MetaClass": "LineItemEx",
                    "ArticleNumber": "Tops-Tunics-CowlNeck-Chocolate",
                    "Color": "",
                    "ColorImageUrl": "/globalassets/catalogs/fashion/swatches/tops-tunic-cowlneck-chocolate-swatch.jpg",
                    "ImageUrl": "/globalassets/catalogs/fashion/cowlneck.jpg",
                    "ItemSize": "Small",
                    "WineRegion": "",
                    "ParentCatalogEntryId": "Tops-Tunics-CowlNeck-Chocolate",
                    "InStockQuantity": 8,
                    "LineItemDiscountAmount": 0,
                    "PlacedPrice": 23.95,
                    "ReturnReason": null,
                    "MaxQuantity": 50,
                    "ProviderId": null,
                    "ConfigurationId": null,
                    "OrigLineItemId": null,
                    "AllowBackordersAndPreorders": true,
                    "IsInventoryAllocated": true,
                    "ShippingMethodName": null,
                    "Catalog": "Starterkit",
                    "OrderFormId": 563,
                    "DisplayName": "Tops-Tunics-CowlNeck-Chocolate-Small",
                    "CatalogNode": "",
                    "ListPrice": 23.95,
                    "OrderLevelDiscountAmount": 0,
                    "Status": null,
                    "LineItemId": 963,
                    "CatalogEntryId": "Tops-Tunics-CowlNeck-Chocolate-Small",
                    "ReturnQuantity": 0,
                    "ShippingAddressId": "",
                    "OrderGroupId": 597,
                    "Quantity": 2,
                    "WarehouseCode": "default",
                    "PreorderQuantity": 4,
                    "Description": "",
                    "ExtendedPrice": 47.9,
                    "BackorderQuantity": 6,
                    "MinQuantity": 1,
                    "ShippingMethodId": "00000000-0000-0000-0000-000000000000",
                    "InventoryStatus": 1,
                    "LineItemOrdering": "2015-02-17T14:53:38.84Z",
                    "Discounts": []
                }
            ],
            "Discounts": [],
            "Shipments": [
                {
                    "__Version": "5.0.0.1",
                    "__State": 3,
                    "__MetaClass": "ShipmentEx",
                    "PrevStatus": "Released",
                    "Status": "Packing",
                    "ShippingMethodId": "e348c896-c82d-4065-9b06-07982b12aa04",
                    "OperationKeys": "0:1#ZZZS8PAU7GKDBPAPWK29X4KW8WQQ7VS7MPJ99Y6AWGQ9XVVMMLBX6Y6NWPT9EVZZZCDX6KNG7P9H8GZT5EGZZZZZ",
                    "ShipmentTrackingNumber": "",
                    "ShipmentId": 524,
                    "ShippingAddressId": "Shipping",
                    "ShippingTax": 0,
                    "OrderGroupId": 597,
                    "SubTotal": 47.9,
                    "PickListId": null,
                    "ShippingMethodName": "Free Shipping",
                    "ShippingDiscountAmount": 0,
                    "LineItemIds": "0:2.0000",
                    "OrderFormId": 563,
                    "ShipmentTotal": 0,
                    "WarehouseCode": "default",
                    "Discounts": []
                }
            ],
            "Payments": [
                {
                    "__Version": "5.0.0.1",
                    "__State": 3,
                    "__MetaClass": "DibsPayment",
                    "CardNumberMasked": null,
                    "CardTypeName": null,
                    "PaymentType": 4,
                    "Status": "Processed",
                    "BillingAddressId": "Billing",
                    "ValidationCode": null,
                    "PaymentMethodName": "Pay By Phone",
                    "ProviderTransactionID": null,
                    "CustomerName": null,
                    "OrderGroupId": 597,
                    "PaymentId": 603,
                    "OrderFormId": 563,
                    "AuthorizationCode": null,
                    "PaymentMethodId": "765ec2a1-7e55-4df9-9605-154c6ae95650",
                    "TransactionID": null,
                    "ImplementationClass": "OxxCommerceStarterKit.Core.PaymentProviders.Payment.DibsPayment, OxxCommerceStarterKit.Core",
                    "Amount": 47.9,
                    "TransactionType": "Sale"
                }
            ]
        }
    ],
    "OrderAddresses": [
        {
            "__Version": "5.0.0.1",
            "__State": 3,
            "__MetaClass": "OrderGroupAddressEx",
            "DeliveryServicePoint": null,
            "LastName": "Doe",
            "Organization": null,
            "RegionCode": null,
            "Name": "Billing",
            "PostalCode": "1166",
            "State": null,
            "OrderGroupId": 597,
            "CountryName": null,
            "FaxNumber": null,
            "FirstName": "John",
            "DaytimePhoneNumber": "99887766",
            "Line2": null,
            "CountryCode": "NOR",
            "City": "OSLO",
            "EveningPhoneNumber": null,
            "Email": "info+commerce@episerver.no",
            "OrderGroupAddressId": 1254,
            "RegionName": null,
            "Line1": "Ekebergveien 102"
        },
        {
            "__Version": "5.0.0.1",
            "__State": 3,
            "__MetaClass": "OrderGroupAddressEx",
            "DeliveryServicePoint": "{\"Id\":\"3098258\",\"Name\":\"MATKROKEN LJABRU\",\"Address\":\"LJABRUBAKKEN 1\",\"City\":\"OSLO\",\"PostalCode\":\"1165\"}",
            "LastName": "Doe",
            "Organization": null,
            "RegionCode": null,
            "Name": "Shipping",
            "PostalCode": "1166",
            "State": null,
            "OrderGroupId": 597,
            "CountryName": null,
            "FaxNumber": null,
            "FirstName": "John",
            "DaytimePhoneNumber": null,
            "Line2": null,
            "CountryCode": null,
            "City": "OSLO",
            "EveningPhoneNumber": null,
            "Email": null,
            "OrderGroupAddressId": 1255,
            "RegionName": null,
            "Line1": "Ekebergveien 102"
        },
        {
            "__Version": "5.0.0.1",
            "__State": 3,
            "__MetaClass": "OrderGroupAddressEx",
            "DeliveryServicePoint": null,
            "LastName": "Doe",
            "Organization": "",
            "RegionCode": "",
            "Name": "Billing Address",
            "PostalCode": "1166",
            "State": "",
            "OrderGroupId": 597,
            "CountryName": "",
            "FaxNumber": null,
            "FirstName": "John",
            "DaytimePhoneNumber": "99887766",
            "Line2": "",
            "CountryCode": "NOR",
            "City": "OSLO",
            "EveningPhoneNumber": "",
            "Email": "info+commerce@episerver.no",
            "OrderGroupAddressId": 1260,
            "RegionName": "",
            "Line1": "Ekebergveien 102"
        }
    ],
    "OrderNotes": [
        {
            "OrderGroupId": 597,
            "OrderNoteId": 53,
            "Created": "2015-02-17T15:17:00.253Z",
            "CustomerId": "b221df97-2771-4ac1-a042-dd3ab81c0680",
            "Detail": "Customer asked for discount. Respectfully declined.",
            "Title": "Customer Service Call",
            "Type": ""
        },
        {
            "OrderGroupId": 597,
            "OrderNoteId": 54,
            "Created": "2015-02-17T15:17:02.787Z",
            "CustomerId": "b221df97-2771-4ac1-a042-dd3ab81c0680",
            "Detail": "Sku (0.00 of Tops-Tunics-CowlNeck-Chocolate-Small) is changed on the shipment #524, order total changed to $47.90",
            "Title": "Sku (0.00 of Tops-Tunics...",
            "Type": "System"
        }
    ]
}
```

