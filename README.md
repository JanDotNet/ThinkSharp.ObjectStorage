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
      .AddFileLocation("C:\\Temp\\File.xml")    // add a storage location to store/restore the data
          .Zipped()                             // zip serialized object for that file location
          .Encrypt("SecretPassword")            // encrypt zipped data for that file location
      .Build();                                 // create the configured storage
    
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

The API is really simple. There is a ```StorageBuilder``` that provides static methods for configuring and creating an object of type ```IStorage<TData>```. That "storage object" can be used to save and load objects of type ```TData```:

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
    
followed by one serializer to use:

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

    IStorage<Person> storage = StorageBuilder
        .ForType<Person>("StorageName")
        .UsingXmlSerializer()               
        .AddInMemoryLocation().Zipped().Encrypted("secret")
        .AddFileLocation("c:\\Temp\\file.xml")
        .AddEmbeddedResource("pathToEmbeddedResource")
       
The example above create a storage that serializes the object ```Person``` using the .net's ```XmlSerializer```. The storage has 3 locations:
* The in memory location reads or writes the serialized content (zipped and encrypted in the case above) as byte array to a dictionary with the storage's name as key. Therfore another instance of the storage can be used to read the in memory content.
* The file location reads or writes the serialized content from / to the specified file.
* The embedded resource location reads the content from the specified embedded resource. If the embedded resource is not in the same assembly as the ```Person``` class, it is possible to specifie another assembly.

Calling ```storage.Load()``` for the storage created above does the following:
* It checks if there is serialized content for the storage in memory
* If not, it checks if the is a file with the specified content
* if not, it loads the content from the embedded resource.

Calling ```storage.Save(new Person {FirstName = "Hans", LastName = "Meier")``` for the storage above does the following:
* It writes the serialized, zipped and encrypted person object to the in memory location
* It writes the serialied person object to the specified file
* The embedded resource is read-only and will be ignored

### Extendability

The library can be extended by implementing custom serializers, storage locations or stream transformations. The pattern is always the same: Implement one of the interfaces ([ISerializer](https://github.com/JanDotNet/ThinkSharp.ObjectStorage/blob/master/ObjectStorage/Serializers/ISerializer.cs), [IStorageLocation](https://github.com/JanDotNet/ThinkSharp.ObjectStorage/blob/master/ObjectStorage/Locations/IStorageLocation.cs) or [IStreamTransformation](https://github.com/JanDotNet/ThinkSharp.ObjectStorage/blob/master/ObjectStorage/StreamTransformations/IStreamTransformation.cs)) and create extension methods for extending the fuent API. All existing implementations are implemented following that pattern, therefore they can be used as examples:

* [Serializers](https://github.com/JanDotNet/ThinkSharp.ObjectStorage/tree/master/ObjectStorage/Serializers)
* [StorageLocations](https://github.com/JanDotNet/ThinkSharp.ObjectStorage/tree/master/ObjectStorage/Locations)
* [StreamTransformations](https://github.com/JanDotNet/ThinkSharp.ObjectStorage/tree/master/ObjectStorage/StreamTransformations)

## Contributors

Feel free to contribute by
* report bugs
* suggest features
* implement one of the open points in the issue tracker 
