using System.ComponentModel;

namespace ArguMint
{
   internal class PositionalRule : IArgumentRule
   {
      public void Match( object argumentClass, IMarkedProperty<ArgumentAttribute> property, string[] arguments )
      {
         if ( property.Attribute.Position != ArgumentPosition.Any )
         {
            int index = property.Attribute.Position.ToIndex();

            if ( arguments.Length >= index + 1 )
            {
               var typeConverter = TypeDescriptor.GetConverter( property.PropertyType );

               object convertedValue = typeConverter.ConvertFromString( arguments[index] );

               property.SetPropertyValue( argumentClass, convertedValue );
            }
         }
      }
   }
}
