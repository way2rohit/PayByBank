How to run it.

build docker image by running below command from command terminal (Install Docker on windows install docker desktop)
--------------------------------------------------------------------------------------------------------------------------

Make sure docker deamon is running

1. docker build -t pokedex-image -f Dockerfile .
2. docker create --name core-pokedex pokedex-image
3. docker start core-pokedex 
4. docker run -d -p 80:80 pokedex-image
5. open in browser http://localhost/pokemon/steelix , for translation browse http://localhost/pokemon/translated/steelix


To run locally in windows 10 , (Visual Studio 2019 is required)
----------------------------------------------------------
1. Clone the repository in local.
2. Open the pokedex.sln file and run into IIS Express.


Production
1. Please update the thrid party URL's in appSettings.Production.Json 
2. We can also change the Default Log Level from Information to Error.
3. Currenlty custom error codes are not added. For production to monitor the errors we need to log the custom error codes.
Each error senarios can have a unique error code.The error codes then can be used to create datadog monitor to capture errors 
and raising incidients.




