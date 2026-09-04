# Unique Email : Prevent duplicate registration with the same email.

Using `IsUnique()`

## 1- API

**Create User → Controller Check → Is the email already available?**

* **1- No:** 409 Conflict + With the message `"Email is already registered."` displayed
* **2- yes:** Database → UNIQUE Email → Create User

## 2- Rejection also occurs from SQL

**SQL Server → Repetition is rejected.**

## 3- HasIndex()

The index helps the database search for emails faster
especially when checking if the email already exists or not

## 4- Migracion

After modifying the UserDbContext to update the database
I created a migration to record the latest changes, which made the email unique.

# What I Learned

Through this task, I learned how to:

* Prevent duplicate user registration
* Check whether a user already exists before creating an account
* Return an appropriate HTTP status code for duplicate data
* Configure a unique constraint for the email field
* Use Entity Framework Core migrations to apply database changes
