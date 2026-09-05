1- Factory :
A factory is a method for organizing the generation of objects that are part of the same family
these objects share an interface or base class, and the factory chooses and builds the right object based on the type that is needed.

2-
It focuses on creating a complex object in an organized and clear manner, especially when it has many or optional properties
example :

var user = new UserBuilder()
    .SetUsername("Naseem")
    .SetEmail("naseem@gmail.com")
    .SetPassword("123456")
    .Build();
