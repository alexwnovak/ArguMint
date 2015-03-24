using System;
using System.Reflection;
using ArguMint.UnitTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArguMint.UnitTests
{
   [TestClass]
   public class MarkedPropertyTest
   {
      private string DummyProperty
      {
         get;
         set;
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_PropertyInfoIsNull_ThrowsArgumentNullException()
      {
         var markedProperty = new MarkedProperty<DontCareAttribute>( null, new DontCareAttribute() );
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_AttributeIsNull_ThrowsArgumentNullException()
      {
         var propertyInfo = typeof( MarkedPropertyTest ).GetProperty( "DummyProperty", BindingFlags.NonPublic | BindingFlags.Instance );

         var markedProperty = new MarkedProperty<DontCareAttribute>( propertyInfo, null );
      }

      [TestMethod]
      public void PropertyName_HasValidProperty_ReturnsPropertyName()
      {
         const string propertyName = "DummyProperty";

         // Setup

         var propertyInfo = typeof( MarkedPropertyTest ).GetProperty( propertyName, BindingFlags.NonPublic | BindingFlags.Instance );

         // Test

         var markedProperty = new MarkedProperty<DontCareAttribute>( propertyInfo, new DontCareAttribute() );

         Assert.AreEqual( propertyName, markedProperty.PropertyName );
      }

      [TestMethod]
      public void Attribute_HasValidAttribute_ReturnsInstanceProperly()
      {
         var attribute = new DontCareAttribute();

         // Setup

         var propertyInfo = typeof( MarkedPropertyTest ).GetProperty( "DummyProperty", BindingFlags.NonPublic | BindingFlags.Instance );

         // Test

         var markedProperty = new MarkedProperty<DontCareAttribute>( propertyInfo, attribute );

         Assert.AreEqual( attribute, markedProperty.Attribute );
      }

      [TestMethod]
      public void SetProperty_HasValidAttribute_SetsPropertyCorrectly()
      {
         const string oldValue = "OldValue";
         const string newValue = "NewValue";

         // Setup

         var propertyInfo = typeof( DummyAttribute ).GetProperty( "StringProperty", BindingFlags.Public | BindingFlags.Instance );

         // Test

         var dummyAttribute = new DummyAttribute
         {
            StringProperty = oldValue
         };

         var markedProperty = new MarkedProperty<DummyAttribute>( propertyInfo, dummyAttribute );

         markedProperty.SetProperty( dummyAttribute, newValue );

         // Assert

         Assert.AreEqual( newValue, dummyAttribute.StringProperty );
      }
   }
}
