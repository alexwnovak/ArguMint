using System;
using System.ComponentModel;

namespace ArguMint
{
   public class ArgumentAnalyzer
   {
      private readonly ITypeInspector _typeInspector;

      public ArgumentAnalyzer() : this( new TypeInspector() )
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
            var markedMethods = _typeInspector.GetMarkedMethods<T, ArgumentsOmittedHandlerAttribute>();

            if ( markedMethods?.Length == 1 )
            {
               markedMethods[0].Invoke( argumentClass );
            }

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
                  var typeConverter = TypeDescriptor.GetConverter( markedProperty.PropertyType );

                  object convertedValue = typeConverter.ConvertFromString( arguments[index] );

                  markedProperty.SetPropertyValue( argumentClass, convertedValue );
               }
            }
            else
            {
               if ( !string.IsNullOrEmpty( markedProperty.Attribute.Argument ) )
               {
                  if ( markedProperty.Attribute.Spacing == Spacing.None )
                  {
                     foreach ( string argument in arguments )
                     {
                        if ( argument.StartsWith( markedProperty.Attribute.Argument ) )
                        {
                           string value = argument.Replace( markedProperty.Attribute.Argument, string.Empty );
                           markedProperty.SetPropertyValue( argumentClass, value );
                        }
                     }
                  }
                  else if ( markedProperty.Attribute.Spacing == Spacing.Postfix )
                  {
                     for ( int index = 0; index < arguments.Length; index++ )
                     {
                        if ( arguments[index] == markedProperty.Attribute.Argument )
                        {
                           string value = arguments[index + 1];
                           markedProperty.SetPropertyValue( argumentClass, value );
                        }
                     }
                  }
               }
            }
         }

         return argumentClass;
      }

      private IMarkedProperty<ArgumentAttribute>[] GetMarkedProperties<T>()
      {
         var markedProperties = _typeInspector.GetMarkedProperties<ArgumentAttribute>( typeof( T ) );

         if ( markedProperties.Length == 0 )
         {
            throw new MissingAttributesException( "No Argument attributes were found on class: " + typeof( T ).Name );
         }

         return markedProperties;
      }
   }
}
