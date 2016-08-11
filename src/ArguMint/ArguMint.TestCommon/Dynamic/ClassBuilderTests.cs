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

         Action addAttribute = () => classBuilder.MarkProperty( "DoesNotMatter", null );

         addAttribute.ShouldThrow<ArgumentException>();
      }

      public void Create_ExpressionDoesNotCreateAnObject_ThrowsInvalidOperationException()
      {
         var classBuilder = ClassBuilder.Create();

         Action addAttribute = () => classBuilder.MarkProperty( "DoesNotExist", () => null );

         addAttribute.ShouldThrow<InvalidOperationException>();
      }

      public void Create_AddsAttributeToNonExistentProperty_ThrowsInvalidOperationException()
      {
         var classBuilder = ClassBuilder.Create();

         Action addAttribute = () => classBuilder.MarkProperty( "DoesNotExist", () => new ObsoleteAttribute() );

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
         classBuilder.MarkProperty( propertyName, () => new ObsoleteAttribute() );
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
         classBuilder.MarkProperty( propertyName, () => new ObsoleteAttribute( message ) );
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

      public void AddAttribute_AttachesCustomAttributeWithConstructorParametersAndProperties_AttributeIsAttached()
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

      public void MarkMethod_AddsObsoleteAttributeToMethod_AttributeIsAdded()
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
         classBuilder.MarkMethod( methodName, () => new ObsoleteAttribute() );
         classBuilder.Build();

         // Assert

         var methodInfo = classBuilder.Type.GetMethod( methodName, BindingFlags.Public | BindingFlags.Instance );
         var actualAttribute = methodInfo.GetCustomAttribute<ObsoleteAttribute>();

         actualAttribute.Should().NotBeNull();
      }
   }
}
