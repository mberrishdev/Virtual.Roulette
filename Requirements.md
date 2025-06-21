# Virtual Roulette

[Github](https://github.com/giorgi-kobaidze/virtual-roulette-assignment)

## Introduction

To complete the assignment, please create a private repository in your GitHub account and add the user **giorgi-kobaidze** as a collaborator once everything's done.

## Assignment Details

Your task is to write a back-end service for a **Virtual Roulette** game. It's **not required** to build a front-end client, the back-end service alone will suffice. This service should support the following features:

### Authentication

Users must be able to sign in using their username and password. After successful authentication, they should be granted access to invoke the service's supported endpoints. The choice of authentication method is entirely up to you.

### Getting User's Balance

Users must be able to request their account balance, which should be returned in **US dollar cents**.

### Placing a Bet

Users must be able to place bets using funds from their balance. Each bet, including the selected numbers and the wagered amount, will be submitted from the client application as a specific **JSON** string (see example below).

After receiving a bet from the client, the bet string must be analysed for validation. This validation can be done using the provided **“RouletteBetAnalyzer”** library for C#.

### Example

Import the library: **ge.singular.roulette.dll**

```csharp
// Example bet string received from the client:
string bet = "[{\"T\": \"v\", \"I\": 20, \"C\": 1, \"K\": 1}]";

// Check the bet string validity 
IsBetValidResponse validityCheck = CheckBets.IsValid(bet);

// As a response, you will receive whether the bet string is correct or not, and bet amount made by the user (in cents.)
Console.WriteLine("Is the bet valid: " + validityCheck.getIsValid() + "the bet amount in cents is: " + validityCheck.getBetAmount());
```

After the bet is successfully validated, both the bet string and the bet amount must be stored in the database.

Next, a winning number (between 0 and 36) should be generated using a **secure random** algorithm.

Aftet that, the outcome of the bet must be determined. You should check whether the user has won anything or not, which again could be done using the provided library:

```csharp
int winNum = 12; // the winning number, generated using a secure random algorithm

int estWin = CheckBets.EstimateWin(bet, winNum); // determines whether or not the user has won anything

Console.WriteLine("User has won: " + estWin + " cents."); // If "estWin" equals to 0, it means that the user has lost
```

Finally the winning number and the amount by the user must be stored in the database.

The result should be returned to the user in the following format:
* Status: indicates whether the bet was valid and accepted.
* Spin ID: a unique identifier for the spin, generated automatically, server-side.
* Winning number: the number generated for the spin.
* Won amount: the amount the user has won in US dollar cents.

## Other Features

### Retrieving Game History

Users must be able to request their game history. The response returned to the client should contain a list of past bets, each containing the following information:
* Spin ID: the unique identifier of the spin.
* Bet amount: the amount wagered in US dollar cents.
* Won amount: the amount won in US dollar cents.
* Date & Time: the timestamp of when the bet was placed.

### User Session Tracking

The server-side application must be able to track whether the user is connected to the back-end. In other words, if the user is active. If the user is not active for more than 5 minutes, they must be automatically signed out from the system.

### Jackpot

The system should support jackpot feature. Jackpot is calculated the following way: each time a user places a bet, a 1% of the bet amount is added to the jackpot.

Whenever the jackpot amount changes, the updated value must be automatically pushed to all connected clients in **real time**.

Additionally the service must provide an endpoint that allows clients to request the current jackpot amount at any time.

### Recording IP Address

Each time a user places a bet, the server-side application should record the user’s IP address and date & time when the bet was made.

## Technologies, Frameworks, Libraries, etc.

* You can use any of the following frameworks: **.NET 8 or .NET 9**

* The service must follow the **REST API** principles as much as possible.

* Thorough unit tests are required. For Unit Testing, use **xUnit**.

* A communication protocol between client and server must be **http / https**.

* All communication must be in JSON format.

* **MySQL** must be used as a database management system. It's up to you to decide whether you use code-first or database-first approach. However using **Dapper** is preferable for data manipulation.

## Other Notes

* Database connection must be able to handle thousands of simultaneous connections, requests from concurrent users.

* Data transfer footprint between server and client must be as small as possible.

* Service should have a proper **Swagger** documentation.

* Code must have relevant comments; Endpoints must have thorough descriptions, that should also be visible in the Swagger documentation.

* The solution must contain a well-structured README file, containing all the information about the project.

* Code should be well-structured and organized.

* Naming conventions must be strictly followed (both on the application and database side).

* API versioning should be supported.

* Use **Git** for version control. Commit messages should be clear and self-explanatory. Ensure code changes are logically grouped and divided into meaningful, well-structured commits.

* Avoid anti-patterns and bad practices as much as possible.


## Appendix

Pre-generated bet strings for testing purposes:

```json
[ { "T": "v", "I": 10, "C": 1, "K": 1 }, { "T": "n", "I": 11, "C": 1, "K": 1 }, { "T": "n", "I": 8, "C": 1, "K": 1 }, { "T": "n", "I": 5, "C": 6, "K": 1 }, { "T": "n", "I": 19, "C": 1, "K": 1 }, { "T": "n", "I": 16, "C": 1, "K": 1 }, { "T": "n", "I": 14, "C": 1, "K": 1 }, { "T": "s", "I": 18, "C": 1, "K": 1 } ]
```

```json
[ { "T": "v", "I": 17, "C": 1, "K": 1 }, { "T": "n", "I": 20, "C": 1, "K": 1 }, { "T": "n", "I": 23, "C": 1, "K": 1 }, { "T": "n", "I": 26, "C": 1, "K": 1 } ]
```

```json
[ { "T": "n", "I": 17, "C": 1, "K": 1 }, { "T": "n", "I": 19, "C": 1, "K": 1 }, { "T": "n", "I": 22, "C": 1, "K": 1 } ]
```

```json
[ { "T": "n", "I": 14, "C": 1, "K": 10 }, { "T": "n", "I": 17, "C": 1, "K": 10 } ]
```

```json
[ { "T": "v", "I": 20, "C": 1, "K": 1 } ]
```

***Good luck and happy coding!***