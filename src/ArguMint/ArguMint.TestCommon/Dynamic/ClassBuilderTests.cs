using System;
using System.Reflection;
using FluentAssertions;

namespace ArguMint.TestCommon.Dynamic
{
   public class ClassBuilderTests
   {
      public void Create_DoesNoSetup_TypeIsNull()
      {
         var classBuilder = ClassBuilder.Create();

         classBuilder.Type.Should().BeNull();
      }

      public void Create_DoesNoSetupButGetsBuilt_TypeExists()
      {
         // Act

         var classBuilder = ClassBuilder.Create();

         classBuilder.Build();

         // Assert

         classBuilder.Type.Should().NotBeNull();
      }

      public void Create_PassesNullExpression_ThrowsArgumentException()
      {
         var classBuilder = ClassBuilder.Create();

         Action addAttribute = () => classBuilder.AddAttribute( "DoesNotMatter", null );

         addAttribute.ShouldThrow<ArgumentException>();
      }

      public void Create_ExpressionDoesNotCreateAnObject_ThrowsInvalidOperationException()
      {
         var classBuilder = ClassBuilder.Create();

         Action addAttribute = () => classBuilder.AddAttribute( "DoesNotExist", () => null );

         addAttribute.ShouldThrow<InvalidOperationException>();
      }

      public void Create_AddsAttributeToNonExistentProperty_ThrowsInvalidOperationException()
      {
         var classBuilder = ClassBuilder.Create();

         Action addAttribute = () => classBuilder.AddAttribute( "DoesNotExist", () => new ObsoleteAttribute() );

         addAttribute.ShouldThrow<InvalidOperationException>();
      }

      public void AddProperty_CreatesIntProperty_CreatesGetterAndSetterWithCorrectName()
      {
         const string propertyName = "Value";

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<int>( propertyName );
         classBuilder.Build();

         // Assert

         var propertyInfo = classBuilder.Type.GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );

         propertyInfo.Should().NotBeNull();
         propertyInfo.Name.Should().Be( propertyName );
         propertyInfo.PropertyType.Should().Be( typeof( int ) );
         propertyInfo.CanRead.Should().BeTrue();
         propertyInfo.CanWrite.Should().BeTrue();
      }

      public void AddAttribute_AttachesObsoleteAttributeWithNoSettings_AttributeIsAttached()
      {
         const string propertyName = "FileName";

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<string>( propertyName );
         classBuilder.AddAttribute( propertyName, () => new ObsoleteAttribute() );
         classBuilder.Build();

         // Assert

         var propertyInfo = classBuilder.Type.GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
         var attributes = propertyInfo.GetCustomAttributes( typeof( ObsoleteAttribute ), false );

         attributes.Should().HaveCount( 1 );
         attributes[0].Should().BeOfType<ObsoleteAttribute>();
      }

      public void AddAttribute_AttachesObsoleteAttributeWithConstructorParameters_AttributeIsAttached()
      {
         const string propertyName = "FileName";
         const string message = "Constructor parameter for ObsoleteAttribute";

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<string>( propertyName );
         classBuilder.AddAttribute( propertyName, () => new ObsoleteAttribute( message ) );
         classBuilder.Build();

         // Assert

         var propertyInfo = classBuilder.Type.GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
         var attributes = propertyInfo.GetCustomAttributes( typeof( ObsoleteAttribute ), false );

         var obsoleteAttribute = (ObsoleteAttribute) attributes[0];
         obsoleteAttribute.Message.Should().Be( message );
      }

      public void AddAttribute_AttachesCustomttributeWithNoConstructorAndOneProperty_AttributeIsAttached()
      {
         const string propertyName = "Character";
         const char charValue = 'X';

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<string>( propertyName );
         classBuilder.AddAttribute( propertyName, () => new AttributeWithNoConstructorButProperty
         {
            CharValue = charValue
         } );
         classBuilder.Build();

         // Assert

         var propertyInfo = classBuilder.Type.GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
         var attributes = propertyInfo.GetCustomAttributes( typeof( AttributeWithNoConstructorButProperty ), false );

         var forTestAttribute = (AttributeWithNoConstructorButProperty) attributes[0];
         forTestAttribute.CharValue.Should().Be( charValue );
      }

      public void AddAttribute_AttachesCustomAttributeWithConstructorParametersAndProperties_AttributeIsAttached()
      {
         const string propertyName = "FileName";
         const int integerValue = 5;
         const bool booleanValue = true;

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<string>( propertyName );
         classBuilder.AddAttribute( propertyName, () => new AttributeWithConstructorParameterAndProperty( integerValue )
         {
            BooleanValue = true
         } );
         classBuilder.Build();

         // Assert

         var propertyInfo = classBuilder.Type.GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
         var attributes = propertyInfo.GetCustomAttributes( typeof( AttributeWithConstructorParameterAndProperty ), false );

         var forTestAttribute = (AttributeWithConstructorParameterAndProperty) attributes[0];
         forTestAttribute.IntegerValue.Should().Be( integerValue );
         forTestAttribute.BooleanValue.Should().Be( booleanValue );
      }
   }
}
