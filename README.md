# OptoBasicNotes
Optoma Basic notes system for senior pretask


## Setup

- Setup a Sql Server database with whatever name you choose

- Open both Solutions
  - /OptoBasicNotes/OptoBasicNotes.sln
  - /OptoBasicNotesApi/OptoBasicNotesApi.sln

- Navigate to OptoBasicNotesApi appsettings.config file and update the connection string to what is suitable for your newly created database

- For the OptoBasicNotesApi projet open the 'Nuget Package Manager Console' 
   - change the target project to OptoBasicNotesApi.Core
   - run the update-database command
