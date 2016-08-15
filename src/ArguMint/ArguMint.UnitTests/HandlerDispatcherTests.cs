using System;
using FluentAssertions;
using Moq;
using ArguMint.TestCommon.Helpers;

namespace ArguMint.UnitTests
{
   public class HandlerDispatcherTests
   {
      public void DispatchArgumentsOmitted_ArgumentClassIsNull_ThrowsArgumentException()
      {
         // Arrange
         
         var typeInspectorMock = new Mock<ITypeInspector>();

         // Act

         var handlerDispatcher = new HandlerDispatcher( typeInspectorMock.Object );

         Action dispatchArgumentsOmitted = () => handlerDispatcher.DispatchArgumentsOmitted( null );

         dispatchArgumentsOmitted.ShouldThrow<ArgumentException>();
      }

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

      public void DispatchArgumentError_ArgumentClassIsNull_ThrowsArgumentException()
      {
         var handlerDispatcher = new HandlerDispatcher( null );

         Action dispatchArgumentError = () => handlerDispatcher.DispatchArgumentError( null );

         dispatchArgumentError.ShouldThrow<ArgumentException>();
      }
   }
}
