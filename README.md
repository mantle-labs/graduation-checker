# graduation-checker template
This is a template of a project using [Tracker](https://www.mantleblockchain.com/tracker) & [Keeper](https://www.mantleblockchain.com/keeper)
to validate the legitimacy of the content and the issuance of an academic degree.
By storing their students' certificates in the blockchain on issuance,
a University could validate if a certificate has been issued or not and validate if it has been falsified or not.
This app's relational data is stored on the blockchain with Mantle Tracker,
while the comparison feature is powered by Mantle Keeper.
Both of these products rely on the blockchain to ensure data integrity & immutability over time.

With that implementation, companies could validate a potential candidate's credentials 
by asking the University they claim they went to.

## Before you start
You might find it easier to implement this project after reading our [documentation](https://developer.mantleblockchain.com/docs).

## Functionalities
- [x] Create new academic programs with a REST API
- [x] Create new universities with a REST API
- [x] Validate an academic certificate from a form and show the differences if necessary
- [x] List programs filtered by university, degree and graduation year
- [ ] Upload certificates with the app (must use Keeper directly to do so)
- [ ] User creation procedure

## Prerequisites

To fully use this application, you will need to have both a Keeper and Tracker product instance.
Then, you will need to create a folder in Keeper that will be used solely to deposit certificates.
Your certificates must follow the same structure and fields than the examples in `backend/GraduationChecker/testdata`.

### Backend
You will need the update the following values in `backend/GraduationChecker/Config/AppSettings.cs` to run the backend project:

``` c#
public class AppSettings
{
     // Mantle API's URL: This should be left as is
    public readonly string MantleApiUrl = "https://api.mantleblockchain.com";

     // Your Mantle Tracker Product Id, you can find it with the following HTTP call: GET http://api.mantleblockchain.com/products
    public readonly string MantleTrackerProductId = "{product-id}";

     // Your Mantle Keeper Product Id, you can find it with the following HTTP call: GET http://api.mantleblockchain.com/products
    public readonly string MantleKeeperProductId = "{product-id}";

     // You Mantle Keeper Folder Id, you will need to create a folder in which the certificates will be stored
     // See https://api.mantleblockchain.com/documentation/index.html#operation/CreateFolder for more details
    public readonly string MantleKeeperFolderId = "{folder-id}";

    // The API key of a user that has admin rights on both your Keeper and Tracker instances
    // See https://api.mantleblockchain.com/documentation/index.html#operation/CreateApiKey for more details
    public readonly string ApiKey = "{api-key}";
}
```

### Frontend
You might need the update the following value in `frontend/src/services/apiService.js` to run the front end project:

``` javascript
{
    // The URL of the C# backend, this might change if you use a different port, or if you don't host the backend on your local machine
    export const baseURL = 'http://localhost:43056';
}
```

### Initial setup
The app will only work with predefined universities and programs.
Fortunately, these can be easily created using the backend applications's REST API.

#### Create universities

You can create new universities with a `POST /api/universities` HTTP call.

Here are examples that work with our example certificates in `backend/GraduationChecker/testdata`.

```
POST /api/universities HTTP/1.1
Host: localhost:43056
Content-Type: application/json
{
	shortName: "ETS",
	longName: "Ecole de Technologie Superieure"
}

POST /api/universities HTTP/1.1
Host: localhost:43056
Content-Type: application/json
{
	shortName: "HEC",
	longName: "Hautes Etudes Commerciales"
}
```

#### Create academic programs

You can create new academic programs with a `POST /api/programs` HTTP call.

Here are examples that work with our example certificates in `backend/GraduationChecker/testdata`.

```
POST /api/programs HTTP/1.1
Host: localhost:43056
Content-Type: application/json
{
	universityName: "HEC",
	shortName: "MBA",
	longName: "Business Administration",
	degree: "Master",
	year: "2018"
}

POST /api/programs HTTP/1.1
Host: localhost:43056
Content-Type: application/json
{
	universityName: "ETS",
	shortName: "BCompSc",
	longName: "Computer Science",
	degree: "Bachelor",
	year: "2019"
}
```

## Getting started
### Install dependencies:
- [Node.js](https://nodejs.org/en/)
- [.NET Core](https://dotnet.microsoft.com/download)

### Run the application:
##### Frontend:
1. Go in the frontend directory
2. Run the following command: `npm install`
3. Then, run the following command to serve locally with a watch: `npm run dev`
4. To deploy the app and minify it, run the following command: `npm run build`

##### Backend:
There's two ways to run the backend:
1. Use your favorite IDE to run the backend
2. Go in this directory `backend/GraduationChecker` and run this command : `dotnet run`

## Mantle API calls
In this section, we will explain how we used [Tracker](https://www.mantleblockchain.com/tracker) and [Keeper](https://www.mantleblockchain.com/keeper) in our application.

**DATA MODELING**

Create a new Program or University
`POST /tracker/{product-id}/multiassets`

List Programs or Universities
`GET /tracker/{product-id}/multiassets`

Find a specific Program or University
`GET /tracker/{product-id}/multiassets/{id}`

List Programs and Universities' specifications
`GET /tracker/{product-id}/assets`

**CERTIFICATE COMPARISON**

List all potential certificates
`GET /keeper/{product-id}/files?folderId={folderId}`

List all versions for a certificate
`GET /keeper/{product-id}/files/{fileId}/versions`

Compare a certificate against the original or a version
`POST /keeper/{product-id}/files/{fileId}/compare/original`
`POST /keeper/{product-id}/files/{fileId}/versions/compare/{versionId}`

## More docs
- [Mantle knowledge base](https://developer.mantleblockchain.com/docs)
- [Mantle API documentation](https://api.mantleblockchain.com/documentation)
