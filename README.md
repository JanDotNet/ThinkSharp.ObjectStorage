# ThinkSharp.ObjectStorage
ObjectStorage is a small extendable library with fluent API  creating storages for .net object.

Usage:

    public class Person
    {
      public string FirstName { get; set; }
      public string LastName { get; set; }
    }
    
    IStorage<Person> storage = StorageBuilder.
      .ForType<Person>()
      .UsingXmlSerializer()
      .AddFileLocation("C:\\Temp")
      .Build();
      
    storage.Save(new Person { FirstName = "Hugo", LastName = "Oguh" });
    var person = storage.Load();

