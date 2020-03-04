# pokemon-shakespeare-translator

## Instructions
1. Navigate to the solution directory. (e.g. pokemon-shakespeare-translator\Poketranslator)
2. Build docker image: docker build -f .\Poketranslator.API\Dockerfile -t poketranslator .
3. Run docker container: docker run -d -p 8080:80 --name poketranslator poketranslator
4. Open browser and connect to http://localhost:8080/api/pokemon/charizard

## Remarks
1. The free Pokemon api has a hard limit of 5 request per hour, which is nowhere near what to expect from a production environment. For the sake of simplicity it is just failing and yielding a 500 error.
In a production environment, where there might be a reasonable restriction, there should be some mechanism in place to deal with this in a more controlled fashion, if needed.
2. Another option to try to reduce the amount of calls to the Pokemon api might be implemening chaching. There are, of course, other benefits for the Poketranslator api as well.