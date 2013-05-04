using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRtCacheHelper;

namespace TestDemoApp
{
    public class Poco
    {
        public Poco() { }
        public int IntProp { get; set; }
        public string StringProp { get; set; }
        public override bool Equals(object obj)
        {
            Poco x = obj as Poco;
            if (x == null) { return false; }
            return x.IntProp == this.IntProp && x.StringProp == this.StringProp;
        }
    }
    public class Poco2
    {
        public Poco2() { }
        public int IntProp { get; set; }
        public string StringProp { get; set; }
        public override bool Equals(object obj)
        {
            Poco2 x = obj as Poco2;
            if (x == null) { return false; }
            return x.IntProp == this.IntProp && x.StringProp == this.StringProp;
        }
    }
    [TestClass]
    public class ObjectStorageHelperTest
    {
        private string handle = "SomeHandle";
        private string handle2 = "AnotherHandle";

        [TestMethod]
        public async Task SaveAndLoadTwoObjectsOfDifferentTypesUsingTheSameHandle()
        {
            var poco = new Poco() { IntProp = 1, StringProp = "one" };
            var poco2 = new Poco2() { IntProp = 2, StringProp = "two" };
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);
            var osh2 = new ObjectStorageHelper<Poco2>(StorageType.Local);

            await osh.SaveAsync(poco, handle);
            await osh2.SaveAsync(poco2, handle);
            var result = await osh.LoadAsync(handle);
            var result2 = await osh2.LoadAsync(handle);

            Assert.AreEqual(poco, result);
            Assert.AreEqual(poco2, result2);
        }

        [TestMethod]
        public async Task SaveAndLoadTwoObjectsOfSameTypeUsingDifferentHandles()
        {
            var poco = new Poco() { IntProp = 1, StringProp = "one" };
            var anotherPoco = new Poco() { IntProp = 2, StringProp = "two" };
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);

            await osh.SaveAsync(poco, handle);
            await osh.SaveAsync(anotherPoco, handle2);
            var result = await osh.LoadAsync(handle);
            var anotherResult = await osh.LoadAsync(handle2);

            Assert.AreEqual(poco, result);
            Assert.AreEqual(anotherPoco, anotherResult);
        }

        [TestMethod]
        public async Task SaveObject()
        {
            //Instantiate an object that we want to save
            var myPoco = new Poco();
            //new up ObjectStorageHelper specifying that we want to interact with the Local storage folder
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);
            //Save the object (via XML Serialization) to the specified folder, asynchronously
            await osh.SaveAsync(myPoco);

            //No assertion. Just want to make sure that this completes successfully!
        }
        [TestMethod]
        public async Task SaveObjectUsingAHandle()
        {
            //Instantiate an object that we want to save
            var myPoco = new Poco();
            //new up ObjectStorageHelper specifying that we want to interact with the Local storage folder
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);
            //Save the object (via XML Serialization) to the specified folder, asynchronously
            await osh.SaveAsync(myPoco, handle);

            //No assertion. Just want to make sure that this completes successfully!

        }

        [TestMethod]
        public async Task SaveAndLoadListOfPocos()
        {
            var listOfPocos = new List<Poco>();
            listOfPocos.Add(new Poco());
            listOfPocos.Add(new Poco());

            var osh = new ObjectStorageHelper<List<Poco>>(StorageType.Local);
            await osh.SaveAsync(listOfPocos);

            var result = await osh.LoadAsync();

            CollectionAssert.AreEqual(listOfPocos, result);
        }
        [TestMethod]
        public async Task SaveAndLoadListOfPocosUsingAHandle()
        {
            var listOfPocos = new List<Poco>();
            listOfPocos.Add(new Poco());
            listOfPocos.Add(new Poco());

            var osh = new ObjectStorageHelper<List<Poco>>(StorageType.Local);
            await osh.SaveAsync(listOfPocos, handle);

            var result = await osh.LoadAsync(handle);

            CollectionAssert.AreEqual(listOfPocos, result);
        }

        [TestMethod]
        public async Task SaveAndLoadPoco()
        {
            var poco = new Poco() { IntProp = 1, StringProp = "one" };
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);

            await osh.SaveAsync(poco);
            var result = await osh.LoadAsync();

            Assert.AreEqual(poco, result);
        }
        [TestMethod]
        public async Task SaveAndLoadPocoUsingAHandle()
        {
            var poco = new Poco() { IntProp = 1, StringProp = "one" };
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);

            await osh.SaveAsync(poco, handle);
            var result = await osh.LoadAsync(handle);

            Assert.AreEqual(poco, result);
        }

        [TestMethod]
        public async Task SaveListOfObjectsWhenObjectClassIsDefinedInAnotherProject()
        {
            var listOfPocos = new List<Poco>();
            listOfPocos.Add(new Poco());
            listOfPocos.Add(new Poco());

            var osh = new ObjectStorageHelper<List<Poco>>(StorageType.Local);
            await osh.SaveAsync(listOfPocos);
            var result = await osh.LoadAsync();

            CollectionAssert.AreEqual(listOfPocos, result);
        }

        [TestMethod]
        public async Task AttemptToLoadObjectThatDoesNotExist()
        {
            //new up ObjectStorageHelper specifying that we want to interact with the Local storage folder
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);

            //First ensure that it does not exist
            await osh.DeleteAsync();

            //Get the object from the storage folder
            var myPoco = await osh.LoadAsync();
            Assert.AreEqual(null, myPoco);
        }
        [TestMethod]
        public async Task AttemptToLoadObjectUsingAHandleThatDoesNotExist()
        {
            //new up ObjectStorageHelper specifying that we want to interact with the Local storage folder
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);

            //First ensure that it does not exist
            await osh.DeleteAsync(handle);

            //Get the object from the storage folder
            var myPoco = await osh.LoadAsync(handle);
            Assert.AreEqual(null, myPoco);
        }


        /// <summary>
        /// Check that LoadASync() returns null after a file has been created and then deleted using DeleteASync().
        /// </summary>
        [TestMethod]
        public async Task SaveAndDeletePoco()
        {
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);
            var poco = new Poco();
            await osh.SaveAsync(poco);
            await osh.DeleteAsync();

            var result = await osh.LoadAsync();

            Assert.AreEqual(result, default(Poco));
        }

        [TestMethod]
        public async Task SaveAndDeletePocoWithAHandle()
        {
            var osh = new ObjectStorageHelper<Poco>(StorageType.Local);
            var poco = new Poco();
            await osh.SaveAsync(poco, handle);
            await osh.DeleteAsync(handle);

            var result = await osh.LoadAsync(handle);

            Assert.AreEqual(result, default(Poco));
        }

        [TestMethod]
        public async Task HandleUnSerializableObject()
        {
            var dictionaryObj = new Dictionary<string, string>();
            dictionaryObj.Add("key", "value");
            string errMsg = null;
            try
            {
                var osh = new ObjectStorageHelper<Dictionary<string, string>>(StorageType.Local);
                await osh.SaveAsync(dictionaryObj);
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }
            Assert.AreEqual(true, errMsg.Contains("is not supported because it implements IDictionary"));
        }
    }
}
