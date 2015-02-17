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
install-package EPiCode.CommerceRestApi
```
You need to add the EPiServer Nuget Feed to Visual Studio (see http://nuget.episerver.com/en/Feed/)

## Usage ##
The easiest way to use the REST API is to install the Postman App for Chrome and import the collection of API definitions as shown below.

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