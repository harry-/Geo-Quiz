# Geo Quiz

An application that makes use of the Google Maps API to display a random country. To make this proof of concept project more interesting, some random country names are displayed along with the map. A point is awarded each time the user guesses the displayed country's name correctly.

## Prerequisites

Sign up to the [Google Cloud Platform](https://cloud.google.com).

Generate an API key [here](https://developers.google.com/maps/documentation/static-maps/). Replace the content of the file ApiKeyRENAMETHIS.txt with your newly generated key and rename the file to ApiKey.txt

Install the [SimpleHashing.net] (https://www.nuget.org/packages/SimpleHashing.Net/) nuget package.

There are two modes: Online and Offline. Online mode requires an SQL Server Database. Find the sql scripts to set it up in database.sql. You probably will also have to set up a corresponding server login with the rights to read and write data and also to execute procedures.

## Compilation

Included with the repository is the Visual Studio project file GeoQuiz.csproj. Ideally you just need to open it and press F5. I compiled my version with Visual Studio 2017 community edtion. If you are using anything else, you are on your own.

## Execution

All you need to do is run the file GeoQuiz.exe that should have been created in bin\Release.

## Usage

Self explanatory.

## Acknowledgments

SimpleHashing.net is written and mantained by Ilya Chernomordik(https://github.com/ilya-git).