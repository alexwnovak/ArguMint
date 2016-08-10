using System;
using System.Collections.Generic;

namespace ArguMint.TestCommon.Dynamic
{
   public static class GlobalCallbackTable
   {
      private static readonly Dictionary<string, Action> _callbackTable = new Dictionary<string, Action>();

      public static void Add( string name, Action action ) => _callbackTable[name] = action;

      public static void Call( string name ) => _callbackTable[name]();
   }
}
