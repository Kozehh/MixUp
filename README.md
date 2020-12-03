# Mix Up

- [Information générale](#information-générale)
    - [Outils](#outils)
- [Prérequis](#prérequis)
- [Utilisation](#utilisation)


## Information Générale

Mix Up est un gestionnaire de musique interactif qui facilite la gestion de la musique à des évènements.

### Outils

- VisualStudio 2019
- .NET Core 3.1
- Docker
- MongoDb
- Git
- GitLab
- [localtunnel](https://github.com/localtunnel/localtunnel)

## Prérequis

- Docker et Docker-Compose
- Cellulaire android (Suivre les [étapes](https://docs.microsoft.com/en-us/xamarin/android/get-started/installation/set-up-device-for-development) pour setup l'environnement du cellulaire)
- Xamarin (Suivre les [étapes](https://docs.microsoft.com/en-us/xamarin/get-started/installation/?pivots=windows) pour installer xamarin)
- .NET Core 3.1 [Installer](https://dotnet.microsoft.com/download)


## Utilisation

- Brancher l'appareil Android à l'ordinateur et avoir suivi les étapes en lien plus haut pour setup l'environnement du cellulaire.
- Cloner le code source dans son ordinateur.
- Ouvrir la solution MixUp.sln qui se trouve dans le dossier MixUp avec VisualStudio 2019 (préférablement, car n'a pas été testé avec des versions antérieures)
- Construire et exécuter `docker-compose.yml` en exécutant les commandes suivantes en ligne de commande à la racine du projet où se trouve le fichier
```sh
docker-compose build
docker-compose up
```