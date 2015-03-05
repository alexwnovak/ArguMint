using System;
using Microsoft.Practices.Unity;

namespace Blargument
{
   public static class ArgumentAnalyzer
   {
      public static T Analyze<T>( string[] arguments ) where T : class, new()
      {
         if ( arguments == null )
         {
            throw new ArgumentException( "Arguments must not be null", "arguments" );
         }

         var typeInspector = Dependency.UnityContainer.Resolve<ITypeInspector>();

         var markedProperties = typeInspector.GetMarkedProperties<T, ArgumentAttribute>();

         if ( markedProperties.Length == 0 )
         {
            throw new MissingAttributesException( "No Argument attributes were found on class: " + typeof( T ).Name );
         }

         throw new NotImplementedException();
      }
   }
}
