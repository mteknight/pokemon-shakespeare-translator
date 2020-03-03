# pokemon-shakespeare-translator

## Instructions
1. Navigate to the dir where DockerFile is. (e.g. pokemon-shakespeare-translator\Poketranslator\Poketranslator.API)
2. Build docker image: docker build -t poketranslator .
3. Run docker container: docker run -d -p 8080:80 --name poketranslator poketranslator
4. Open browser and connect to http://localhost:8080/api/pokemon/charizard
