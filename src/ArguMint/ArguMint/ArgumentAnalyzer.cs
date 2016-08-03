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
            throw new ArgumentException( "Arguments must not be null", nameof( arguments ) );
         }

         var argumentClass = new T();

         if ( arguments.Length == 0 )
         {
            return argumentClass;
         }

         var markedProperties = GetMarkedProperties<T>();

         foreach ( var markedProperty in markedProperties )
         {
            if ( markedProperty.Attribute.Position != ArgumentPosition.Any )
            {
               int index = markedProperty.Attribute.Position.ToIndex();

               if ( arguments.Length >= index + 1 )
               {
                  markedProperty.SetPropertyValue( argumentClass, arguments[index] );
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
