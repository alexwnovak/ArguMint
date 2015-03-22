using Blargument;
using Moq;

namespace ArguMint.UnitTests
{
   public static class MarkedPropertyHelper
   {
      internal static Mock<IMarkedProperty<ArgumentAttribute>> Create( string argument )
      {
         var argumentAttribute = new ArgumentAttribute( argument );

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();

         markedPropertyMock.Setup( mp => mp.Attribute ).Returns( argumentAttribute );

         return markedPropertyMock;
      }
   }
}
