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
               object convertedValue = ValueConverter.Convert( arguments[index], property.PropertyType );

               if ( convertedValue == null )
               {
                  throw new ArgumentErrorException();
               }

               property.SetPropertyValue( argumentClass, convertedValue );
            }
            else
            {
               throw new ArgumentErrorException();
            }
         }
      }
   }
}
