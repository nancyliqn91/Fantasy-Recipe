# Fantacy Recipe

#### By _Qian Li_ _Joe Wahbeh_ _Max Betich_ 😊

#### This is our c# practice project which creates an app that allows users to keep track of recipes.

## Technologies Used

* C#
* .NET
* HTML
* MVC
* Entity Framework
* MySQL Workbench
* VS code

## Description

* Users can add, edit, delete a recipe, tag and rate recipes. 
* Users can list recipes by highest rated so they can see which ones they like the best.
* Users can see all recipes that use a certain ingredient.
* Users are able to  create an account, log in and log off, amd see account details.

## Setup/Installation Requirements

* _Clone “Fantacy Recipe“ from the repository to your desktop_.
* _Navigate to "Fantacy Recipe" directory via your local terminal command line_.
* Run the app, first navigate to this project's production directory called "FantacyRecipe". 
* Add appsettings.json file, please see the "Database Connection String Setup" instruction below.
* Create the database using the migrations in the "FantacyRecipe" project. Open your shell (e.g., Terminal or GitBash) to the production directory "FantacyRecipe", and `run dotnet ef database update`.
* To optionally create a migration, run the command `dotnet ef migrations add MigrationName` where MigrationName is your custom name for the migration in UpperCamelCase.
* Within the production directory "FantacyRecipe", run `dotnet watch run` in the command line to start the project in development mode with a watcher.
* Open the browser to _https://localhost:5001_. If you cannot access localhost:5001 it is likely because you have not configured a .NET developer security certificate for HTTPS. To learn about this, review this lesson: [Redirecting to HTTPS and Issuing a Security Certificate](https://www.learnhowtoprogram.com/c-and-net/basic-web-applications/redirecting-to-https-and-issuing-a-security-certificate).

## Database Connection String Setup 

* Create an appsetting.json file in the "FantacyRecipe" directory of the project. The example is below.
* Within appsettings.json, put in the following code, replacing the uid and pwd values with your own username and password for MySQL Workbench.


```
University Registrar/UniversityRegistrar/appsettings.json

 {
    "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;
      database=[Your-DATA-BASE];uid=[YOUR-USER-HERE];
      pwd=[YOUR-PASSWORD-HERE];"
    }
 }
```

## Known Bugs

No bugs 

## License
[MIT](license.txt)
Copyright (c) 2023 Qian Li
