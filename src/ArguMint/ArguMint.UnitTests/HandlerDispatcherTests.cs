using System;
using FluentAssertions;
using Moq;
using Xunit;
using ArguMint.TestCommon.Helpers;

namespace ArguMint.UnitTests
{
   public class HandlerDispatcherTests
   {
      [Fact]
      public void DispatchArgumentsOmitted_ArgumentClassIsNull_ThrowsArgumentException()
      {
         // Arrange

         var typeInspectorMock = new Mock<ITypeInspector>();

         // Act

         var handlerDispatcher = new HandlerDispatcher( typeInspectorMock.Object );

         Action dispatchArgumentsOmitted = () => handlerDispatcher.DispatchArgumentsOmitted( null );

         dispatchArgumentsOmitted.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void DispatchArgumentsOmitted_FindsOneMarkedMethod_CallsTheMarkedMethod()
      {
         // Arrange

         var markedMethodMock = new Mock<IMarkedMethod<ArgumentsOmittedHandlerAttribute>>();
         var markedMethods = ArrayHelper.Create( markedMethodMock.Object );

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedMethods<ArgumentsOmittedHandlerAttribute>( It.IsAny<Type>() ) ).Returns( markedMethods );

         // Act

         object argumentClassDoesNotMatter = 12345;

         var handlerDispatcher = new HandlerDispatcher( typeInspectorMock.Object );

         handlerDispatcher.DispatchArgumentsOmitted( argumentClassDoesNotMatter );

         // Assert

         markedMethodMock.Verify( mm => mm.Invoke( argumentClassDoesNotMatter ), Times.Once() );
      }

      [Fact]
      public void DispatchArgumentsOmitted_FindsMultipleMarkedMethods_ThrowsArgumentConfigurationException()
      {
         // Arrange

         var markedMethodMock = new Mock<IMarkedMethod<ArgumentsOmittedHandlerAttribute>>();
         var markedMethods = ArrayHelper.Create( markedMethodMock.Object, markedMethodMock.Object );

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedMethods<ArgumentsOmittedHandlerAttribute>( It.IsAny<Type>() ) ).Returns( markedMethods );

         // Act

         object argumentClassDoesNotMatter = 12345;

         var handlerDispatcher = new HandlerDispatcher( typeInspectorMock.Object );

         Action dispatchArgumentsOmitted = () => handlerDispatcher.DispatchArgumentsOmitted( argumentClassDoesNotMatter );

         // Assert

         dispatchArgumentsOmitted.ShouldThrow<ArgumentConfigurationException>();
      }

      [Fact]
      public void DispatchArgumentError_ArgumentClassIsNull_ThrowsArgumentException()
      {
         var handlerDispatcher = new HandlerDispatcher( null );

         Action dispatchArgumentError = () => handlerDispatcher.DispatchArgumentError( null, new ArgumentError( ArgumentErrorType.Unspecified ) );

         dispatchArgumentError.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void DispatchArgumentError_FindsOneHandlerWithNoParameters_CallsHandler()
      {
         // Arrange

         var markedMethodMock = new Mock<IMarkedMethod<ArgumentErrorHandlerAttribute>>();
         markedMethodMock.SetupGet( mm => mm.ParameterTypes ).Returns( new Type[0] );
         var markedMethods = ArrayHelper.Create( markedMethodMock.Object );

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedMethods<ArgumentErrorHandlerAttribute>( It.IsAny<Type>() ) ).Returns( markedMethods );

         // Act

         object argumentClassDoesNotMatter = 12345;

         var handlerDispatcher = new HandlerDispatcher( typeInspectorMock.Object );

         handlerDispatcher.DispatchArgumentError( argumentClassDoesNotMatter, new ArgumentError( ArgumentErrorType.Unspecified ) );

         // Assert

         markedMethodMock.Verify( mm => mm.Invoke( argumentClassDoesNotMatter ), Times.Once() );
      }

      [Fact]
      public void DispatchArgumentError_FindsHandlerWithErrorParameter_CallsHandler()
      {
         const ArgumentErrorType errorType = ArgumentErrorType.TypeMismatch;

         // Arrange

         var markedMethodMock = new Mock<IMarkedMethod<ArgumentErrorHandlerAttribute>>();
         markedMethodMock.SetupGet( mm => mm.ParameterTypes ).Returns( ArrayHelper.Create( typeof( ArgumentError ) ) );
         var markedMethods = ArrayHelper.Create( markedMethodMock.Object );

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedMethods<ArgumentErrorHandlerAttribute>( It.IsAny<Type>() ) ).Returns( markedMethods );

         // Act

         object argumentClassDoesNotMatter = 12345;

         var handlerDispatcher = new HandlerDispatcher( typeInspectorMock.Object );

         handlerDispatcher.DispatchArgumentError( argumentClassDoesNotMatter, new ArgumentError( errorType ) );

         // Assert

         markedMethodMock.Verify( mm => mm.Invoke( argumentClassDoesNotMatter,
            It.Is<object[]>( a => ((ArgumentError) a[0]).ErrorType == errorType ) ),
            Times.Once() );
      }

      [Fact]
      public void DispatchArgumentError_FindsMultipleErrorHandlers_ThrowsArgumentConfigurationException()
      {
         // Arrange

         var markedMethodMock = new Mock<IMarkedMethod<ArgumentErrorHandlerAttribute>>();
         var markedMethods = ArrayHelper.Create( markedMethodMock.Object, markedMethodMock.Object );

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedMethods<ArgumentErrorHandlerAttribute>( It.IsAny<Type>() ) ).Returns( markedMethods );

         // Act

         object argumentClassDoesNotMatter = 12345;

         var handlerDispatcher = new HandlerDispatcher( typeInspectorMock.Object );

         Action dispatchArgumentError = () => handlerDispatcher.DispatchArgumentError( argumentClassDoesNotMatter, new ArgumentError( ArgumentErrorType.Unspecified ) );

         // Assert

         dispatchArgumentError.ShouldThrow<ArgumentConfigurationException>();
      }
   }
}
