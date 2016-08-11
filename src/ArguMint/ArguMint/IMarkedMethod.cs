using System;

namespace ArguMint
{
   internal interface IMarkedMethod<out T> where T : Attribute
   {
   }
}
