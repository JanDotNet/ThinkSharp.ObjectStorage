# ThinkSharp.ObjectStorage
ObjectStorage is a small extendable library with fluent API for creating storages for .net object.

Usage:

    public class Person
    {
      public string FirstName { get; set; }
      public string LastName { get; set; }
    }
    
    IStorage<Person> storage = StorageBuilder
      .ForType<Person>()                        // create storage for the type 'Person'
      .UsingXmlSerializer()                     // use .net's XmlSerializer for serialization
      .Zipped()                                 // zip serialized object
      .Encrypt("SecretPassword")                // encrypt zipped data
      .AddFileLocation("C:\\Temp\\File.xml")    // add a storage location to store/restore the data
      .Build();
    
    storage.Save(new Person { FirstName = "Hugo", LastName = "Oguh" });
    
    var person = storage.Load();
