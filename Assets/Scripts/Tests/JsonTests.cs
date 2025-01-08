using System.Collections;
using NUnit.Framework;
using Runtime.DataLayer;
using UnityEditor;
using UnityEngine.TestTools;

namespace Tests
{
    public class JsonTests
    {
        private Structure _structure;
        
        [SetUp]
        public void Setup()
        {
            _structure = TestStructureFactory.CreateStructure();
        }

        [Test]
        public void JsonIsNotEmptyString()
        {
            // Arrange
            string json = _structure.ToJson();
            
            // Act
            bool isNotEmpty = !string.IsNullOrEmpty(json);
            
            // Assert
            Assert.IsTrue(isNotEmpty);
        }
        
        [Test]
        public void JsonBeforeAndAfterSerializationAreEqual()
        {
            // Arrange
            string originalJson = _structure.ToJson();
            
            // Act
            string deserializedJson = _structure.ToJson().ToStructure().ToJson();
            
            // Assert
            Assert.AreEqual(originalJson, deserializedJson);
        }
    }
}