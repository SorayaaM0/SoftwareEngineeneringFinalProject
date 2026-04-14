# These are my personal implementation notes, refer to this file if you are looking for a more in depth explanation about implementations, and what I have personally contributed.

I will be implementing all key classes, you will see them listed under Models/

I have included a PersonalNotes folder where we can add our personal documentation notes. Feel free to use it, but it is not a requirement. I do this for organization and documentation purposes.

Under src I have added an additional folder structure where all our classes will live. I have also included files for Iliyah's implementation of the classes. 

Of course, each file contains ONLY the class specified by its given name. (Each class gets its own .cs file)

I have also added a "Services" and "Views" folder where we will add our factory pattern (logic) and the Maui project (UI). Naming conventions match that of Iliyah's 1.2 phase document. A "Factories" folder has been created as well for object creation. Added some blank files under Services.

## TLDR; 4/14/26
1. So.. what's been implemented?
    - A full model layer including inheritance, composition, aggregation, that matches UML and is C# backend ready for **ALL CLASSES**
    - Factory Logic for the following:
        1. User
           - This includes all user types such as buyer, seller, and admin
        3. Product
        4. Shopping Cart
        5. Wishlist

2. What's left?
    At this point of the process Iliyah and I have to do all Unit testing and test results with sets of test cases, and the communication
    logs.
    To keep things simple, I will be creating a branch that only has Models, and Factories done. An additional branch with unit testing will be created shortly after this branch. 

# The project is currently organized under a src/ directory with logical seperation for Models, Factories, Services, and Views folders. 
# Please keep in mind that the code is organized but not yet inside a full .NET project setup (for the UI)
# A *.csproj file is the configuration file that turns these folders into a real C# project. It will tell
# .NET how to build and run the code. This project does not have one yet, so the next step will be adding it
# before compiling it or running any tests.

# I am pushing this branch now so that we can all view the structure. Once I recieve full confirmation, I will go ahead and add cs.proj so that we can have a proper .NET project.

# Also NOTE: I cannot test this (much less compile it) until we have a *.csproj file with a .NET project!!!

**And of course, reach out if you have any questions that have not been covered here.**