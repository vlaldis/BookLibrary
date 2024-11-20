# BookLibrary

Sample book library

## Requirements
1. Create an ASP.NET Core application for managing borrowed books.
0. Use database models for users and books and a data layer that will enable communication with the database (e.g. using Entity Framework Core).
0. The endpoint should support:
  - Creating a new book
  - Getting details of an existing book by ID number
  - Updating an existing book
  - Deleting a book
  - Borrowing a book
  - Confirming the return of a borrowed book
0. Ensure validation of input data.

## Bonus tasks
  - Covering the code with tests (unit tests, integration tests).
  - Implementing the functionality of automatically sending user comments before the book return deadline. (send an email to fake)

## Implementation details
- store for storing documents in specific format
- update logic
- get in specific format, eg. ```https://localhost:32770/Document/{id}/xml```
- storage abstraction
- document formatting abstraction

## TODOs
- authentication and authorization is to be done, as I'm not yet familiar with it's implementation in Asp.Net core
- SSL ccertificate is only for development, should be substituted by production certificate
- more unit tests (only sample is provided)
