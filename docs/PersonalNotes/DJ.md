** D'Andre Johnson 5/6 - 5/7/2026**

## 1. TLDR;
added sqlite packages to the project, and created a database connection class that will be used to connect to the database and perform CRUD operations. This class will be used by the services layer to interact with the database.
models only need parameterless constructors, so I have added those to all models. I have also added a few additional properties to the models that I thought would be useful for the database, such as Id and CreatedAt. These properties will be used as primary keys and timestamps for the database records.
RBAC


## 2. Added packages
--dotnet add package sqlite-net-pcl
--dotnet add package SQLitePCLRaw.bundle_green
--dotnet add package BCrypt.Net-Next <-- for password hashing>

## 3. Database Connection Class
DatabaseService.cs
handles all CRUD operations for the database. It also has a method for hashing and resolving passwords using BCrypt.Net-Next.
I guess I'll mention it here, the database is local, so the only user we all will have is the one stubbed for admin, all other data comes from the database and can be added locally.
The database is stored in a filed called storeapp.db, it was located at C:\Users\name\AppData\Local\User Name\com.team.storeapp\Data\storeapp.db on my machine. it should be a similar path for everyone.
Since we are using SQLite, to actually see the database Im using DB Browser. https://sqlitebrowser.org/dl/


## 4. User Model Changes
User.cs has columns for all user types, UserType is an enum that can be Buyer, Seller, or Admin. This will allow us to have a single table for all users, and we can use the UserType column to differentiate between them. I have also added a PasswordHash property to store the hashed password.
Individual Type classes can be removed if wanted because the single UserType paired with unique user ids makes differentiation easier.
I did this because when implementing the database with each user type in their respective tables the similar IDs (like if buyer and seller both have a UserId of 1) caused a conflict where different accounts could modify each other’s data. With a single User table and a UserType column, we can avoid this issue and have a more streamlined database design.
This same logic carried over for refactoring the wishlist so there was no conflict.

## 5. Passwords
Used BCrypt for password hashing, database stores the hashed password, DatabaseService handles that. When a user registers, their password is hashed and stored in the database. When they log in, the password they enter is hashed and compared to the stored hash to verify their identity. 
Updated password handling, now valid password on registration must be min 6 characters long, contain at least one uppercase letter, one lowercase letter, and one number. Idk why I did this, I realized around the second time I did a test registration that I hate password enforcement lmaooo.

## 6. CartItemDisplay
Wrapper class for displaying cart items in the UI, due to database integration.

## 7. Image Picking
Seller can upload image from machine, the image is stored as a byte array in the database, and can be displayed in the UI. I have not implemented the UI for this yet, but the logic for handling the image upload and storage is in place. The Product model has an Image property that is a byte array, and the DatabaseService has methods for converting images to byte arrays and vice versa.



