using System;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace ArguMint.TestCommon.Dynamic
{
   public class ClassBuilderTests
   {
      [Fact]
      public void Create_DoesNoSetup_TypeIsNull()
      {
         var classBuilder = ClassBuilder.Create();

         classBuilder.Type.Should().BeNull();
      }

      [Fact]
      public void Create_DoesNoSetupButGetsBuilt_TypeExists()
      {
         // Act

         var classBuilder = ClassBuilder.Create();

         classBuilder.Build();

         // Assert

         classBuilder.Type.Should().NotBeNull();
      }

      [Fact]
      public void Create_PassesNullExpression_ThrowsArgumentException()
      {
         var classBuilder = ClassBuilder.Create();

         Action markProperty = () => classBuilder.MarkProperty( "DoesNotMatter", null );

         markProperty.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void Create_ExpressionDoesNotCreateAnObject_ThrowsInvalidOperationException()
      {
         var classBuilder = ClassBuilder.Create();

         Action markProperty = () => classBuilder.MarkProperty( "DoesNotExist", () => null );

         markProperty.ShouldThrow<InvalidOperationException>();
      }

      [Fact]
      public void Create_AddsAttributeToNonExistentProperty_ThrowsInvalidOperationException()
      {
         var classBuilder = ClassBuilder.Create();

         Action markProperty = () => classBuilder.MarkProperty( "DoesNotExist", () => new ObsoleteAttribute() );

         markProperty.ShouldThrow<InvalidOperationException>();
      }

      [Fact]
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

      [Fact]
      public void MarkProperty_AttachesObsoleteAttributeWithNoSettings_AttributeIsAttached()
      {
         const string propertyName = "FileName";

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<string>( propertyName );
         classBuilder.MarkProperty( propertyName, () => new ObsoleteAttribute() );
         classBuilder.Build();

         // Assert

         var propertyInfo = classBuilder.Type.GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
         var attributes = propertyInfo.GetCustomAttributes( typeof( ObsoleteAttribute ), false );

         attributes.Should().HaveCount( 1 );
         attributes[0].Should().BeOfType<ObsoleteAttribute>();
      }

      [Fact]
      public void MarkProperty_AttachesObsoleteAttributeWithConstructorParameters_AttributeIsAttached()
      {
         const string propertyName = "FileName";
         const string message = "Constructor parameter for ObsoleteAttribute";

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<string>( propertyName );
         classBuilder.MarkProperty( propertyName, () => new ObsoleteAttribute( message ) );
         classBuilder.Build();

         // Assert

         var propertyInfo = classBuilder.Type.GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
         var attributes = propertyInfo.GetCustomAttributes( typeof( ObsoleteAttribute ), false );

         var obsoleteAttribute = (ObsoleteAttribute) attributes[0];
         obsoleteAttribute.Message.Should().Be( message );
      }

      [Fact]
      public void MarkProperty_AttachesCustomttributeWithNoConstructorAndOneProperty_AttributeIsAttached()
      {
         const string propertyName = "Character";
         const char charValue = 'X';

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<string>( propertyName );
         classBuilder.MarkProperty( propertyName, () => new AttributeWithNoConstructorButProperty
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

      [Fact]
      public void MarkProperty_AttachesCustomAttributeWithConstructorParametersAndProperties_AttributeIsAttached()
      {
         const string propertyName = "FileName";
         const int integerValue = 5;
         const bool booleanValue = true;

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddProperty<string>( propertyName );
         classBuilder.MarkProperty( propertyName, () => new AttributeWithConstructorParameterAndProperty( integerValue )
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

      [Fact]
      public void AddMethod_AddsVoidMethodNoArguments_CallsHandler()
      {
         const string methodName = "ArgHander";
         bool wasCalled = false;

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddMethod( methodName,
            MethodAttributes.Public,
            typeof( void ),
            Type.EmptyTypes,
            () => wasCalled = true );
         classBuilder.Build();

         // Assert

         var methodInfo = classBuilder.Type.GetMethod( methodName, BindingFlags.Public | BindingFlags.Instance );
         methodInfo.Should().NotBeNull();

         var instance = Activator.CreateInstance( classBuilder.Type );
         methodInfo.Invoke( instance, null );

         wasCalled.Should().BeTrue();
      }

      [Fact]
      public void AddMethod_MapsMethodWithSameNameTwice_ThrowsInvalidOperationException()
      {
         // Act

         var classBuilder = ClassBuilder.Create();

         Action addMethod = () => classBuilder.AddMethod( "ThisIsAddedTwice",
            MethodAttributes.Public,
            typeof( void ),
            Type.EmptyTypes,
            () => { } );

         addMethod();

         addMethod.ShouldThrow<InvalidOperationException>();
      }

      [Fact]
      public void MarkMethod_AddsObsoleteAttributeToMethod_AttributeIsAdded()
      {
         const string methodName = "ArgHander";

         // Act

         var classBuilder = ClassBuilder.Create();
         classBuilder.AddMethod( methodName,
            MethodAttributes.Public,
            typeof( void ),
            Type.EmptyTypes,
            () => { } );
         classBuilder.MarkMethod( methodName, () => new ObsoleteAttribute() );
         classBuilder.Build();

         // Assert

         var methodInfo = classBuilder.Type.GetMethod( methodName, BindingFlags.Public | BindingFlags.Instance );
         var actualAttribute = methodInfo.GetCustomAttribute<ObsoleteAttribute>();

         actualAttribute.Should().NotBeNull();
      }
   }
}
