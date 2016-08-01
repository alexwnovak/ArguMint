using System;

namespace ArguMint
{
   public class ArgumentAnalyzer
   {
      private readonly ITypeInspector _typeInspector;

      internal ArgumentAnalyzer() : this( new TypeInspector() )
      {
      }

      internal ArgumentAnalyzer( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      public T Analyze<T>( string[] arguments ) where T : class, new()
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
                  markedProperty.SetPropertyValue( true );
               }
            }
         }

         return argumentClass;
      }

      private IMarkedProperty<ArgumentAttribute>[] GetMarkedProperties<T>()
      {
         var markedProperties = _typeInspector.GetMarkedProperties<T, ArgumentAttribute>();

         if ( markedProperties.Length == 0 )
         {
            throw new MissingAttributesException( "No Argument attributes were found on class: " + typeof( T ).Name );
         }

         return markedProperties;
      }
   }
}
