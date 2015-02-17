# Rest API for EPiServer Commerce #
A simple REST API for EPiServer Commerce for development purposes. This toolset should be treated as a utility box for your development project. 

> **Important!**  This library has not been security tested, and should not be used on production systems. It contains commands that could render your site unusable, and could also expose sensitive data.

**Note:** This is not the official EPiServer Service API, that can be found on: http://world.episerver.com/documentation/Items/EPiServer-Service-API/ 

## Unsupported ##
This code is completely unsupported, some of the methods could irreversibly delete some or all of your Commerce and / or Cms data. Please consume responsibly.

## Note About REST Principles ##
The API mostly does not adhere to REST principles, please see code for usage of the API. It means the methods uses query strings where it should not, HTTP methods are not used consistenly (a GET could change data). 

## Installation ##
Run the following command in the Nuget Package Manager Console for your web site (not Commerce Manager site):
```
install-package EPiCode.CommerceRestApi
```

## Usage ##
The easiest way to use the REST API is to install the Postman App for Chrome and import this collection of API definitions: [https://github.com/BVNetwork/CommerceRestApi](https://github.com/BVNetwork/CommerceRestApi) (this is a JSON file that can be imported directly into Postman).

### Install the Postman App ###
Goto https://chrome.google.com/webstore/detail/postman-rest-client/fdmmgilgnpjigdojojpjoooidkmcomcm

![](https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/images/postman-install-app.png)

### Import the API collection ###
Click the "Import Collection" button: 
![](https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/images/postman-import-collection.png)

Import from the following url:
`https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/Commerce-Rest-API-Postman-Collection.txt`
 
### Add Environment ###
Click Manage Environments, add a new one: 
![](https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/images/postman-manage-environment.png)

Specify the host and apikey to use, the keys are important, as the imported collection uses these.

### Add API Key to appSettings ###
Add the following to appSettings in web.config: 
``` 
<add key="EPiCode.Commerce.Rest.ApiKey"
     value="some-hard-to-guess-secret-here" />
```

### Try it out ###
You should now be able to use the different REST methods imported. Try the `Meta Class: Get All` method:
![](https://raw.githubusercontent.com/BVNetwork/CommerceRestApi/master/doc/images/postman-call-method.png)

**Note!** Make sure you have selected the correct environment before you click Send.