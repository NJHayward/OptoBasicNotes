# OptoBasicNotes
Optoma Basic notes system for senior pretask


## Setup

- Setup a Sql Server database with whatever name you choose

- Open both Solutions and build them (when i did this on the work machine the optoma-azure nuget package location was causing some errors but building it a couple of times it worked..  if you get these errors.. its something to do with our multiple locations of nuget sources getting confused.. but works after a few attempts)
  - /OptoBasicNotes/OptoBasicNotes.sln
  - /OptoBasicNotesApi/OptoBasicNotesApi.sln

- Navigate to OptoBasicNotesApi appsettings.config file and update the connection string to what is suitable for your newly created database

- For the OptoBasicNotesApi projet open the 'Nuget Package Manager Console' 
   - change the target project to OptoBasicNotesApi.Core
   - run the update-database command
