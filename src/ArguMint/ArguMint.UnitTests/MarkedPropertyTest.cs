using System;
using System.Reflection;
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
   }
}
