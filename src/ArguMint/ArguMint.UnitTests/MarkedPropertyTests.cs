using System;
using System.Reflection;
using ArguMint.UnitTests.Dummies;
using FluentAssertions;
using Xunit;

namespace ArguMint.UnitTests
{
   public class MarkedPropertyTests
   {
      private string DummyProperty
      {
         get;
         set;
      }

      [Fact]
      public void Constructor_PropertyInfoIsNull_ThrowsArgumentNullException()
      {
         Action ctor = () => new MarkedProperty<DontCareAttribute>( null, new DontCareAttribute() );

         ctor.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void Constructor_AttributeIsNull_ThrowsArgumentNullException()
      {
         var propertyInfo = typeof( MarkedPropertyTests ).GetProperty( "DummyProperty", BindingFlags.NonPublic | BindingFlags.Instance );

         Action ctor = () => new MarkedProperty<DontCareAttribute>( propertyInfo, null );

         ctor.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void Constructor_PropertyDoesNotHaveSetter_ThrowsArgumentException()
      {
         // Setup

         var propertyInfo = typeof( DummyAttributePropertyNoSetter ).GetProperty( "StringProperty", BindingFlags.Public | BindingFlags.Instance );

         // Test

         var dummyAttribute = new DummyAttributePropertyNoSetter();

         Action ctor = () => new MarkedProperty<DummyAttributePropertyNoSetter>( propertyInfo, dummyAttribute );

         ctor.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void Constructor_PropertyDoesNotHaveGetter_ThrowsArgumentException()
      {
         // Setup

         var propertyInfo = typeof( DummyAttributePropertyNoGetter ).GetProperty( "StringProperty", BindingFlags.Public | BindingFlags.Instance );

         // Test

         var dummyAttribute = new DummyAttributePropertyNoGetter();

         Action ctor = () => new MarkedProperty<DummyAttributePropertyNoGetter>( propertyInfo, dummyAttribute );

         ctor.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void PropertyName_HasValidProperty_ReturnsPropertyName()
      {
         const string propertyName = "DummyProperty";

         // Setup

         var propertyInfo = typeof( MarkedPropertyTests ).GetProperty( propertyName, BindingFlags.NonPublic | BindingFlags.Instance );

         // Test

         var markedProperty = new MarkedProperty<DontCareAttribute>( propertyInfo, new DontCareAttribute() );

         propertyName.Should().Be( markedProperty.PropertyName );
      }

      [Fact]
      public void Attribute_HasValidAttribute_ReturnsInstanceProperly()
      {
         var attribute = new DontCareAttribute();

         // Setup

         var propertyInfo = typeof( MarkedPropertyTests ).GetProperty( "DummyProperty", BindingFlags.NonPublic | BindingFlags.Instance );

         // Test

         var markedProperty = new MarkedProperty<DontCareAttribute>( propertyInfo, attribute );

         attribute.Should().Be( markedProperty.Attribute );
      }

      [Fact]
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

         markedProperty.SetPropertyValue( dummyAttribute, newValue );

         // Assert

         newValue.Should().Be( dummyAttribute.StringProperty );
      }

      [Fact]
      public void SetProperty_PropertyHasPrivateSetter_AllowsPropertyToBeSet()
      {
         const string newValue = "NewValue";

         // Setup

         var propertyInfo = typeof( DummyAttributePropertyPrivateSetter ).GetProperty( "StringProperty", BindingFlags.Public | BindingFlags.Instance );

         // Test

         var dummyAttribute = new DummyAttributePropertyPrivateSetter();

         var markedProperty = new MarkedProperty<DummyAttributePropertyPrivateSetter>( propertyInfo, dummyAttribute );

         markedProperty.SetPropertyValue( dummyAttribute, newValue );

         // Assert

         newValue.Should().Be( markedProperty.Attribute.StringProperty );
      }
   }
}
