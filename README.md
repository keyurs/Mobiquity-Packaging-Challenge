# Welcome to Packaging Challenge By Keyur Shah
This project is addressing solution for requirements provided in [Packaging Challenge](https://github.com/keyurs/Mobiquity-Packaging-Challenge/blob/main/Com.Mobiquity.API/Upload/PackagingChallenge.pdf) 

### Dependencies  
- .NET Core 3.1 (Framework) https://dotnet.microsoft.com/download/dotnet/3.1
- Swagger (Tool for API) 
- NUnit (Package for TDD)
- Azure Web App (Deployed solution on Azure Web App)
 Azure Demo is available at https://keyurshahpackagingchallenge.azurewebsites.net/swagger/index.html#/Packer/Packer_UploadFile

> For code editor, I prefer Visual Studio Code or Visual Studio 2019+ 

> This project designed on .NET Core 3.1 so this project can compile and run on Linux, Windows, Ubantu or Mac
> Make sure you have installed .NET Core 3.1 framework https://dotnet.microsoft.com/download/dotnet/3.1
 
# Project Details

Solution is designed in three sub projects
- API (Com.Mobiquity.API)
- Packer Library (Com.Mobiquity.Packer)
- NUnit (Com.Mobiquity.NUnitTests)

### API
API project is consist of PackerController which contains all API routes used right now.
> **Note** : I have not implemented JWT token auth or any other auth for this simple solution, so api can work without auth, in case you like to update it with Auth support, feel free to reach me on my email

##### There are four APIs calls designed here
- **api/Packer/uploadInputFile** : API will allow user to upload the file with input data of all package use cases
Sample file is accessible here, [Sample Packaging Input File](https://github.com/keyurs/Mobiquity-Packaging-Challenge/blob/main/Com.Mobiquity.API/Upload/SamplePackagingInputFile.txt)
API return absolute path which can be used in another API call "pack" to evaluate the business logic to find best matching pack as per maxWeight 
As per requirements we required to pass absolute path as input, so this API help to upload file for test with different use cases and output absolute path help to evaluate those test cases in file 

- **api/Packer/pack** : API will allow user to evaluate uploaded file and get the results with best matched indexes for each package as per their items provided in input file
Return result will be multiple line string output having index of items for each rows

- **api/Packer/packReturnJson** : API is designed additionally (Not asked in document) but its for get the idea of complete object and result in Json format, So when its required to design frontend for display complete use case of package max weight, their items and matched output index then this API will useful to get complete objects output as Json
- **api/Packer/packSingle** : API is just for test single package, instead upload file and process whole bunch, its also possible to test single package use case with this API call.
Internally logic function is common between all the API calls which used to evaluate package calculation

### Packer Library
Packer Library project is consist of actual business entities and logic for all use cases calculation to fine best match package items support in package.
Required models class are under /EntitiesModel
Service class "PackerService" (along with its Interface IPackerService) is under /Service

##### There are two model class
* Package
* PackageItem

##### PackerService 
- **pack** : Public Function is used to process file and generate package object list with processed output result of best matching items indexs for package 
Sample file which used as input can accessible here, [Sample Packaging Input File](https://github.com/keyurs/Mobiquity-Packaging-Challenge/blob/main/Com.Mobiquity.API/Upload/SamplePackagingInputFile.txt)

- **packSingle** : Public Function is used to process single line use case. This function is useful when its required to test single package case. So instead uploading file it can be process individually too

- **parseItemsLine** : Private Function used to parse and validate individual package data into file, this function used internally after parsed lines from file.
Also this function is having logic for find best match package items

- **parseInputFile** : Private Function which read file and process all line items, and generate results and return result as Package object list along with output.

### NUnitTest
Project consist of Unit test cases for all functions of Packers library
Is right now using actual test case file provided in requirement to verify business logic of library. The sample input file is stored in project "UploadTest" folder

##### There are three unit test cases created
* Inputfile_CheckAllRowsResults
* SingleLine_CheckCorrectMatch
* SingleLine_NO_or_EmptyMatch

**Output of Test case execution is as below :**
![NUnit output](https://github.com/keyurs/Mobiquity-Packaging-Challenge/blob/main/Com.Mobiquity.API/Upload/NUnitoutput.png?raw=true)

## Demo

![NUnit output](https://github.com/keyurs/Mobiquity-Packaging-Challenge/blob/main/Com.Mobiquity.API/Upload/KeyurShahPackagingChallenge.gif?raw=true)
