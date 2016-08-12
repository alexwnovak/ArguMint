using System;
using FluentAssertions;
using Moq;

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
   }
}
