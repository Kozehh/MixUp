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
- NUnit
- [localtunnel](https://github.com/localtunnel/localtunnel)

## Prérequis

- Cellulaire android (Suivre les [étapes](https://docs.microsoft.com/en-us/xamarin/android/get-started/installation/set-up-device-for-development) pour setup l'environnement du cellulaire)
- Installer [Xamarin](https://docs.microsoft.com/en-us/xamarin/get-started/installation/?pivots=windows)
- Installer [.NET Core 3.1] (https://dotnet.microsoft.com/download)
- Installer [Git](https://git-scm.com/downloads)


## Utilisation

- Brancher l'appareil Android à l'ordinateur et avoir suivi les étapes en lien plus haut pour setup l'environnement du cellulaire.
- Cloner le code source dans son ordinateur.
- Ouvrir la solution MixUp.sln qui se trouve dans le dossier MixUp avec VisualStudio 2019 (préférablement, car n'a pas été testé avec des versions antérieures)
- **Pour faire du port fowarding et pouvoir tester avec 2 android emulators**
Aller à la racine du répertoire android-sdk/platform-tools contenant l'application adb.exe
Ouvrir un terminal de commande windows et entrer les commandes suivantes :
adb devices -l // permet de voir l'id des emulators
adb -t 'deviceid' forward tcp:11000 tcp:11000 // permet de foward le port sur la machine host au port local de lemulator choisi par son id
