using System;
using System.Reflection;
using ArguMint.UnitTests.Dummies;
using FluentAssertions;

namespace ArguMint.UnitTests
{
   public class MarkedPropertyTests
   {
      private string DummyProperty
      {
         get;
         set;
      }

      //public void Constructor_PropertyInfoIsNull_ThrowsArgumentNullException()
      //{
      //   // TODO: How should I expect an ArgumentException from a constructor??
      //   var markedProperty = new MarkedProperty<DontCareAttribute>( null, new DontCareAttribute() );
      //}

      //public void Constructor_AttributeIsNull_ThrowsArgumentNullException()
      //{
      //   // TODO: This one too!

      //   var propertyInfo = typeof( MarkedPropertyTests ).GetProperty( "DummyProperty", BindingFlags.NonPublic | BindingFlags.Instance );

      //   var markedProperty = new MarkedProperty<DontCareAttribute>( propertyInfo, null );
      //}

      //public void Constructor_PropertyDoesNotHaveSetter_ThrowsArgumentException()
      //{
      //   // TODO: And this!
      //   // Setup

      //   var propertyInfo = typeof( DummyAttributePropertyNoSetter ).GetProperty( "StringProperty", BindingFlags.Public | BindingFlags.Instance );

      //   // Test

      //   var dummyAttribute = new DummyAttributePropertyNoSetter();

      //   var markedProperty = new MarkedProperty<DummyAttributePropertyNoSetter>( propertyInfo, dummyAttribute );
      //}

      //public void Constructor_PropertyDoesNotHaveGetter_ThrowsArgumentException()
      //{
      //   // TODO: And this!
      //   // Setup

      //   var propertyInfo = typeof( DummyAttributePropertyNoGetter ).GetProperty( "StringProperty", BindingFlags.Public | BindingFlags.Instance );

      //   // Test

      //   var dummyAttribute = new DummyAttributePropertyNoGetter();

      //   var markedProperty = new MarkedProperty<DummyAttributePropertyNoGetter>( propertyInfo, dummyAttribute );
      //}

      public void PropertyName_HasValidProperty_ReturnsPropertyName()
      {
         const string propertyName = "DummyProperty";

         // Setup

         var propertyInfo = typeof( MarkedPropertyTests ).GetProperty( propertyName, BindingFlags.NonPublic | BindingFlags.Instance );

         // Test

         var markedProperty = new MarkedProperty<DontCareAttribute>( propertyInfo, new DontCareAttribute() );

         propertyName.Should().Be( markedProperty.PropertyName );
      }

      public void Attribute_HasValidAttribute_ReturnsInstanceProperly()
      {
         var attribute = new DontCareAttribute();

         // Setup

         var propertyInfo = typeof( MarkedPropertyTests ).GetProperty( "DummyProperty", BindingFlags.NonPublic | BindingFlags.Instance );

         // Test

         var markedProperty = new MarkedProperty<DontCareAttribute>( propertyInfo, attribute );

         attribute.Should().Be( markedProperty.Attribute );
      }

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

         markedProperty.SetPropertyValue( newValue );

         // Assert

         newValue.Should().Be( dummyAttribute.StringProperty );
      }

      public void SetProperty_PropertyHasPrivateSetter_AllowsPropertyToBeSet()
      {
         const string newValue = "NewValue";

         // Setup

         var propertyInfo = typeof( DummyAttributePropertyPrivateSetter ).GetProperty( "StringProperty", BindingFlags.Public | BindingFlags.Instance );

         // Test

         var dummyAttribute = new DummyAttributePropertyPrivateSetter();

         var markedProperty = new MarkedProperty<DummyAttributePropertyPrivateSetter>( propertyInfo, dummyAttribute );

         markedProperty.SetPropertyValue( newValue );

         // Assert

         newValue.Should().Be( markedProperty.Attribute.StringProperty );
      }
   }
}
