# AppFitness

Application web pour créer ses séances d'entraînement de musculation et suivre sa progression sur chaque exercice.

Projet réalisé pour l'obtention du certificat de développeur à l'IDEC (2024-2025). La version présentée lors du certificat est conservée sous le tag [`certificat-idec-2025`](../../tree/certificat-idec-2025).

## Fonctionnalités

- **Exercices** : créer, modifier, supprimer un exercice et l'associer au muscle principalement travaillé
- **Séances** : composer ses propres séances avec autant d'exercices que nécessaire
- **Suivi** : enregistrer les séries (répétitions et poids) et voir la meilleure performance de chaque exercice
- **Progression** : consulter l'historique d'un exercice
- **Impression** : imprimer une séance ou la progression d'un exercice

## Technologies

- C# / ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQLite
- Bootstrap

## Architecture

Le projet est organisé en trois couches :

| Projet | Rôle |
|---|---|
| `AppFitness` | Interface utilisateur : contrôleurs et vues MVC |
| `EntityFitness` | Couche métier : entités et règles de validation |
| `DbFitness` | Données : base SQLite `DbFitness.db` |

Les entités principales sont `Muscle`, `NomExercice`, `Exercice`, `Serie`, `NomSeance` et `Seance`. Le modèle de données (MCD et MLD) est décrit dans [`MCD_MLD.docx`](MCD_MLD.docx).

## Lancer le projet

Prérequis : le [SDK .NET 8](https://dotnet.microsoft.com/download).

1. Dans `EntityFitness/Entities/SqliteContext.cs`, adapter le chemin `Data Source=...` vers l'emplacement de `DbFitness/DbFitness.db` sur votre machine.
2. Lancer l'application :

```bash
dotnet run --project AppFitness/AppFitness.csproj
```

Ou ouvrir `AppFitness.sln` dans Visual Studio et appuyer sur F5.

## Documentation

Le rapport du projet, avec le mode d'emploi détaillé, est disponible dans [`JE_Projet_Idec_2025.pdf`](JE_Projet_Idec_2025.pdf).

## Améliorations prévues

- Système de login
- Temps de repos entre les exercices
- Chronomètre pour suivre la durée de l'entraînement
- Images des muscles et des exercices
- Graphiques de progression
