using System;
using FluentAssertions;
using Moq;

namespace ArguMint.UnitTests
{
   public class RuleMatcherTests
   {
      public void Match_NoMarkedPropertiesFound_ThrowsArgumentConfigurationException()
      {
         IMarkedProperty<ArgumentAttribute>[] markedProperties = new IMarkedProperty<ArgumentAttribute>[0];

         // Arrange

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedProperties<ArgumentAttribute>( It.IsAny<Type>() ) ).Returns( markedProperties );

         // Act

         object argumentClass = "DoesNotMatterWhatThisIs";

         var ruleMatcher = new RuleMatcher( typeInspectorMock.Object );
         Action match = () => ruleMatcher.Match( argumentClass, null );

         // Assert

         match.ShouldThrow<ArgumentConfigurationException>();
      }
   }
}
