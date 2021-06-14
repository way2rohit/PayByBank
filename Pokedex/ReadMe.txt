How to run it.

To run locally , (Visual Studio 2019 is required)
1. Clone the repository in local.
2. Open the pokedex.sln file and run into IIS Express.

To run using Doker
1. Clone the repository in local.
2. Open the pokedex.sln file and run using Doker. (Docker desktop is required)


Production
1. Please update the thrid party URL's in appSettings.Prod.Json 
2. Currenlty custom error codes are not added. For production to monitor the errors we need to log the custom error codes.
Each error senarios can have a unique error code.The error codes then can be used to create datadog monitor to capture errors 
and raising incidients.




