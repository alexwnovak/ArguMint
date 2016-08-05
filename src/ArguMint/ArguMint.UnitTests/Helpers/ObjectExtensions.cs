using System.Reflection;

namespace ArguMint.UnitTests.Helpers
{
   public static class ObjExtensions
   {
      public static object Property( this object obj, string propertyName )
      {
         var propertyInfo = obj.GetType().GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
         return propertyInfo.GetValue( obj );
      }
   }
}
