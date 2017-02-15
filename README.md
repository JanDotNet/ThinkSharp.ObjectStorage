## Synopsis

ObjectStorage is a small extendable library with fluent API for creating storages for .net object.

Features:

* Serializations: XML / JSON
* Storage Locations: File, Embedded Resources (read-only)
* Transformations: optional compression or symmetric encryption
* Simple usage
* Extendable

## Code Example

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

## Installation

ThinkSharp.ObjectStorage is available via NuGet:

    Install-Package ThinkSharp.ObjectStorage
    
## Motivation

The project was started with the goal to create a simple to use library for storing complex user settings / configurations in different locations.

Usages may be:
* Default XML configuration as embedded resource + custom configuration file in user's AppData directory
* Mutliple storages with different priority (e.g. one in the ProgramData directory for all users and one in the AppData directory for special users
* Simple storage for persisting serializable objects

Another personally reason for creating the library was to practice the design of fluent APIs.

## API Reference

### Creating storages

The API is really simple. There is a ```StorageBuilder``` that provides static methods for configuring and creating an object of type ```IStorage<TData>```. That "storage object" can be used to store and load objects of type ```TData```:

    public interface IStorage<TData> where TData : class
    {
        TData Load();
        void Save(TData data);
        void Clear();

        string Name { get; }        
    }
    
### Configuration

The correct configuration is forced by the fluent API. It starts with the specification of the object's type and the name of the storage:

    StorageBuilder.ForType<Person>("StorageName")...
    
followed by the serializer to use:

    ...UsingXmlSerializer()
    ...UsingDataContractJsonSerializer()
    ...UsingDataContractSerializer()
    ...UsingCustonSerializer(customSerializer)
   
The ```customSerializer``` has to implement the following ```ISerializer<TData>``` interface:

    public interface ISerializer<TData> where TData : class
    {
        TData Deserialize(Stream stream);
        Stream Serialize(TData data);
    }
    
After selecting the serializer, locations or transformations (e.g. zip, encryption, ...) may be added to the configuration:

    StorageBuilder
        .ForType<Person>("StorageName")
        .UsingXmlSerializer()
        .Zipped()
        .Encrypted("secret")
        .AddInMemoryLocation()
        .AddFileLocation(fileName)
        .AddEmbeddedResource("pathToEmbeddedResource")
       
The example above create a storage that serializes the object ```Person``` using the .net's ```XmlSerializer```, encrypts the zipped data stream, adds an in memory location and a file location. 

Note that the order of configuring locations and transformations matters:

* if 2 locations were added, the storage uses the second one only if the first one was not available. 
* Adding a transformation between 2 locations, the transformation does only apply to the second location.



The whole configuration of the storage is done by the ```StorageBuilder```

## Contributors

Feel free to contribute by
* report bugs
* suggest features
* implement one of the open points in the issue tracker 
