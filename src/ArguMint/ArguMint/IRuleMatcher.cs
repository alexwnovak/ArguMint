﻿namespace ArguMint
{
   internal interface IRuleMatcher
   {
      void Match( object argumentClass, ArgumentToken[] arguments );
   }
}
