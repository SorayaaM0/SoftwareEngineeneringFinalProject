# **Refer to this document for information on project structure.**

# LANGUAGE:
- Our backend language is C#, this is because we will use .Net Maui for the UI.

# ORGANIZATION:

- A PersonalNotes folder under docs where members can include their own in depth documentation (optional).

- Factories/ --> where our objects will be created (so that it doesn't have to be done manually each time)

- Models/ --> our classes live here (e.g. user, buyer, admin..) 
    - **Keep in mind that a lot of these classes have placeholder lines of code**

- Services/ --> our business logic lives here (factory rules)

- Views/  --> Maui UI components will live here

# The project is currently organized under a src/ directory with logical seperation for Models, Factories, Services, and Views folders. 
# Please keep in mind that the code is organized but not yet inside a full .NET project setup (for the UI)
# A *.csproj file is the configuration file that turns these folders into a real C# project. It will tell
# .NET how to build and run the code. This project does not have one yet, so the next step will be adding it
# before compiling it or running any tests.

# I am pushing this branch now so that we can all view the structure. Once I recieve full confirmation, I will go ahead and add cs.proj so that we can have a proper .NET project.

# Also NOTE: I cannot test this (much less compile it) until we have a *.csproj file with a .NET project!!!