﻿using System;

namespace ArguMint
{
   internal interface IMarkedMethod<out T> where T : Attribute
   {
      T Attribute
      {
         get;
      }

      string Name
      {
         get;
      }

      Type[] ParameterTypes
      {
         get;
      }

      void Invoke( object instance );
      void Invoke( object instance, object[] parameters );
   }
}
