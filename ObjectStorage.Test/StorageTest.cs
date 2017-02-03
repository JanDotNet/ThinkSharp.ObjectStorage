using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkSharp.ObjectStorage;
using System.Runtime.Serialization;
using ThinkSharp.ObjectStorage.FluentApi;

namespace ObjectStorage.Test
{
    [TestClass]
    public class StorageTest
    {
        [TestMethod]
        public void Test_FileStorage_SaveLoad()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .AddFileLocation(fileName)
                    .Build();

                var test = storage.Load();
                Assert.IsNull(test);

                // case: file does not exist
                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));

                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);

                // case: file does exist
                storage.Save(new Test { PropA = "C", PropB = "D" });
                Assert.IsTrue(File.Exists(fileName));

                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("C", test.PropA);
                Assert.AreEqual("D", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_Save_DirectoryNotExist()
        {
            var fileName = Path.Combine("Test1", "Test2", GetTestSpecificFileName());

            foreach (var storageBuilder in GetStorageBuilders())
            {
                if (File.Exists(fileName))
                    File.Delete(Path.GetFullPath(fileName));

                var storage = storageBuilder
                    .AddFileLocation(fileName)
                    .Build();

                var test = storage.Load();
                Assert.IsNull(test);

                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));

                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_SaveLoad_Readonly()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var saveNeverStorage = storageBuilder
                    .AddFileLocation(fileName)
                    .AsReadonly()
                    .Build();

                var storage = storageBuilder
                    .AddFileLocation(fileName)
                    .Build();

                var test = saveNeverStorage.Load();
                Assert.IsNull(test);

                saveNeverStorage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsFalse(File.Exists(fileName));

                File.WriteAllText(fileName, "ABC");

                saveNeverStorage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.AreEqual("ABC", File.ReadAllText(fileName));

                storage.Save(new Test { PropA = "A", PropB = "B" });

                test = saveNeverStorage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_SaveLoad_Default()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .AddFileLocation(fileName)
                    .WithDefault(new Test {PropA = "F", PropB = "G"})
                    .Build();

                Assert.IsFalse(File.Exists(fileName));

                var test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("F", test.PropA);
                Assert.AreEqual("G", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_Encryption()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .Encrypted("abc")
                    .AddFileLocation(fileName)
                    .Build();

                var test = storage.Load();
                Assert.IsNull(test);

                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));

                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_Compression_Zip()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .Zipped()
                    .AddFileLocation(fileName)
                    .Build();

                var test = storage.Load();
                Assert.IsNull(test);

                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));

                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_Compression_GZip()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .GZipped()
                    .AddFileLocation(fileName)
                    .Build();

                var test = storage.Load();
                Assert.IsNull(test);

                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));

                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_Compression_Encryption()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .Zipped()
                    .Encrypted("Hello World")
                    .AddFileLocation(fileName)
                    .Build();

                var test = storage.Load();
                Assert.IsNull(test);

                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));

                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_InMemory()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .Zipped()
                    .Encrypted("Hello World")
                    .AddInMemoryLocation("Test")
                    .AddFileLocation(fileName)
                    .Build();

                var test = storage.Load();
                Assert.IsNull(test);

                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));
                File.Delete(fileName);
                Assert.IsFalse(File.Exists(fileName));

                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_WithDefault()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .Zipped()
                    .Encrypted("Hello World")
                    .AddFileLocation(fileName).WithDefault(new Test { PropA = "C", PropB = "D" })
                    .Build();

                var test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("C", test.PropA);
                Assert.AreEqual("D", test.PropB);

                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));
                
                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);

                File.Delete(fileName);
                test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("C", test.PropA);
                Assert.AreEqual("D", test.PropB);
            }
        }

        [TestMethod]
        public void Test_FileStorage_Clear()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .Zipped()
                    .Encrypted("Hello World")
                    .AddFileLocation(fileName)
                    .Build();

                storage.Save(new Test { PropA = "A", PropB = "B" });
                Assert.IsTrue(File.Exists(fileName));

                var test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
                storage.Clear();
                Assert.IsFalse(File.Exists(fileName));
                test = storage.Load();
                Assert.IsNull(test);
            }
        }

        [TestMethod]
        public void Test_InMemoryLocal_Clear()
        {
            var fileName = GetTestSpecificFileName();

            foreach (var storageBuilder in GetStorageBuilders())
            {
                File.Delete(fileName);

                var storage = storageBuilder
                    .Zipped()
                    .Encrypted("Hello World")
                    .AddInMemoryLocation(fileName)
                    .Build();

                storage.Save(new Test { PropA = "A", PropB = "B" });

                var test = storage.Load();
                Assert.IsNotNull(test);
                Assert.AreEqual("A", test.PropA);
                Assert.AreEqual("B", test.PropB);
                storage.Clear();

                test = storage.Load();
                Assert.IsNull(test);
            }
        }

        [TestMethod]
        public void Test_InMemoryLocal_WithDefaultValue_DefaultValueModified()
        {
           
            foreach (var storageBuilder in GetStorageBuilders())
            {
                var defaultValue = new Test {PropA = "Prop1", PropB = "Prop2"};

                var storage = storageBuilder
                    .AddInMemoryLocation().WithDefault(defaultValue)
                    .Build();

                var actual = storage.Load();

                Assert.AreEqual("Prop1", actual.PropA);
                Assert.AreEqual("Prop2", actual.PropB);

                defaultValue.PropA = "Prop1_Mod";
                defaultValue.PropB = "Prop2_Mod";

                Assert.AreEqual("Prop1", actual.PropA);
                Assert.AreEqual("Prop2", actual.PropB);

                actual.PropA = "Prop1_Mod2";
                actual.PropB = "Prop2_Mod2";

                actual = storage.Load();

                Assert.AreEqual("Prop1", actual.PropA);
                Assert.AreEqual("Prop2", actual.PropB);
            }
        }

        [TestMethod]
        public void Test_BigTestFile()
        {
            var fileName = GetTestSpecificFileName();

            var storage = StorageBuilder
                .ForType<TestBigClass>()
                .UsingDataContractJsonSerializer()
                .GZipped()
                .AddFileLocation("Big_ContractJsonSerializer_gzipped.xml")
                .Build();

            storage.Save(new TestBigClass
            {
                PropA = "I've been looking for a simple Java algorithm to generate a pseudo-random alpha-numeric string. In my situation it would be used as a unique session/key identifier that would be unique over 500K+ generation (my needs don't really require anything much more sophisticated). Ideally, I would be able to specify a length depending on my uniqueness needs. For example, a generated string of length 12 might look something like",
                PropB = "This works by choosing 130 bits from a cryptographically secure random bit generator, and encoding them in base-32. 128 bits is considered to be cryptographically strong, but each digit in a base 32 number can encode 5 bits, so 128 is rounded up to the next multiple of 5. This encoding is compact and efficient, with 5 random bits per character. Compare this to a random UUID, which only has 3.4 bits per character in standard layout, and only 122 random bits in total.",
                PropC = "@weisjohn That's a good idea. You can do something similar with the second method, by removing the digits from symbols and using a space instead; you can control the average  length by changing the number of spaces in symbols (more occurrences for shorter words). For a really over-the-top fake text solution, you can use a Markov chain!",
                PropD = "@kamil, I looked at the source code for RandomStringUtils, and it uses an instance of java.util.Random instantiated without arguments. The documentation for java.util.Random says it uses current system time if no seed is provided. This means that it can not be used for session identifiers/keys since an attacker can easily predict what the generated session identifiers are at any given time",
                PropF = "@Inshallah : You are (unnecessarily) overengineering the system. While I agree that it uses time as seed, the attacker has to have the access to following data to to actually get what he wants 1. Time to the exact millisecond, when the code was seeded 2. Number of calls that have occurred so far 3. Atomicity for his own call (so that number of calls-so-far ramains same) If your attacker has all three of these things, then you have much bigger issue at hand",
                PropG = "The first 4 bits are the version type and 2 for the variant so you get 122 bits of random. So if you want to you can truncate from the end to reduce the size of the UUID. It's not recommended but you still have loads of randomness, enough for your 500k records easy."
            });
        }
        private static IEnumerable<ICompEncryptionCompressionLocation<Test>> GetStorageBuilders()
        {
            Console.WriteLine("UsingXmlSerializer...");
            yield return StorageBuilder.ForType<Test>("XmlSerializer").UsingXmlSerializer();
            Console.WriteLine("UsingDataContractJsonSerializer...");
            yield return StorageBuilder.ForType<Test>("JsonSerializer").UsingDataContractJsonSerializer();
            Console.WriteLine("UsingDataContractSerializer...");
            yield return StorageBuilder.ForType<Test>("DataContractSerializer").UsingDataContractSerializer();
        }
        private static string GetTestSpecificFileName([CallerMemberName] string name = "null") => $"{name}.xml";
    }

    [DataContract]
    public class Test
    {
        [DataMember]
        public string PropA { get; set; }
        [DataMember]
        public string PropB { get; set; }
    }

    [DataContract]
    public class TestBigClass
    {
        [DataMember]
        public string PropA { get; set; }
        [DataMember]
        public string PropB { get; set; }
        [DataMember]
        public string PropC { get; set; }
        [DataMember]
        public string PropD { get; set; }
        [DataMember]
        public string PropF { get; set; }
        [DataMember]
        public string PropG { get; set; }
    }
}
