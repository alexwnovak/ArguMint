using System;
using Microsoft.Practices.Unity;

namespace ArguMint
{
   public static class ArgumentAnalyzer
   {
      public static T Analyze<T>( string[] arguments ) where T : class, new()
      {
         if ( arguments == null )
         {
            throw new ArgumentException( "Arguments must not be null", "arguments" );
         }

         var argumentClass = new T();

         if ( arguments.Length == 0 )
         {
            return argumentClass;
         }

         var markedProperties = GetMarkedProperties<T>();

         for ( int index = 0; index < arguments.Length; index++ )
         {
            string thisArgument = arguments[index];

            foreach ( var markedProperty in markedProperties )
            {
               if ( thisArgument == markedProperty.Attribute.Argument )
               {
                  markedProperty.SetProperty( argumentClass, true );
               }
            }
         }

         return argumentClass;
      }

      private static IMarkedProperty<ArgumentAttribute>[] GetMarkedProperties<T>()
      {
         var typeInspector = Dependency.UnityContainer.Resolve<ITypeInspector>();

         var markedProperties = typeInspector.GetMarkedProperties<T, ArgumentAttribute>();

         if ( markedProperties.Length == 0 )
         {
            throw new MissingAttributesException( "No Argument attributes were found on class: " + typeof( T ).Name );
         }

         return markedProperties;
      }
   }
}
