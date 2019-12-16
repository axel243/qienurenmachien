# QienUrenMachien v1.0.0

Systeem voor urendeclaratie en personeelsadministratie.


## Accounts en Rollen

Het systeem kent de volgende vier rollen:

### Werknemer
Gebruiker account, in staat timesheets te bekijken en in te dienen. Profiel te wijzigen en documenten te uploaden.

### Trainee
Zelfde als een werknemer, enkel maar actief voor 1 jaar.

### Werkgever
Iedere werknemer/trainee is gekoppeld aan een werkgever. De werkgever kan zelf niet inloggen, het account wordt gebruikt om gegevens over de werkgever te beheren. De admin beheert dit.

### Admin
Admin kan als enige accounts aanmaken en heeft overzicht in alle gebruikers en timesheets.

Voor een gebruikeraccount is gekozen voor het Idenity Model. Hieraan zijn de volgende properties toegevoegd.

- Firstname
- Lastname
- Street
- City
- Zipcode
- PhoneNumber
- Country
- ProfileImageUrl
- BankNumber
- NewProfile
- WerkgeverID
- ActiveFrom
- ActiveUntil
  

## Controllers

Het systeem kent de volgende controllers:

### Account
Functionaliteit voor het inloggen, uitloggen en registeren van gebruikers. 

### Administration
Functionaliteit voor de admin dashboard en het beheren van de rollen.

### Data
API voor de tabel en grafiek.

### Home
Functionaliteit voor de Home index.

### Profile
Functionaliteit voor het inzien en wijzigen van een profiel.

### Sheet
Functionaliteit voor het bekijken, indienen, bevestigen en open zetten van timesheets.

### Upload
Functionaliteit voor het uploaden van bestanden.

## Mailserver
De mailserver bevat alle functies voor het versturen van emails.

## Translation
Alle data is het in engels opgeslagen in de database. Binnen deze classe zijn de vertaal functies.

## Hub
SignalR wordt gebruikt voor real-time updates van de grafieken en timesheets. Deze functionaliteit is in de Hub gebouwd.

## Ontwikkeld met

- [ASP.NET MVC](https://dotnet.microsoft.com/apps/aspnet/mvc)
- [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr)
- [Chartsjs](https://www.chartjs.org/)
- [FooTable](https://fooplugins.github.io/FooTable/) 

## Versiebeheer

We gebruiken [SemVer](http://semver.org/) voor versiebeheer.

## Authors

* **Julian Kramer** - [Juless89](https://github.com/Juless89)
* **Ties Wijnker** - [TiesW](https://github.com/TiesW)
* **Mohammed Adda** - [arriff](https://github.com/arriff)
* **Axel Lauffer** - [axel243](https://github.com/axel243)
