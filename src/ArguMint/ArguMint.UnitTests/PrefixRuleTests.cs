using ArguMint.TestCommon.Helpers;
using Moq;

namespace ArguMint.UnitTests
{
   public class PrefixRuleTests
   {
      public void Match_HasPrefixWithNoSpaceArgument_MatchesArgument()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "/prefixNoSpace:";
         const string value = "ArgumentValue";
         var argumentAttribute = new ArgumentAttribute( prefix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( string ) );

         // Act

         string argument = $"{prefix}{value}";

         var prefixRule = new PrefixRule();

         prefixRule.Match( argumentClass, markedPropertyMock.Object, ArrayHelper.Create( argument ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, value ), Times.Once() );
      }
   }
}
