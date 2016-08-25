namespace ArguMint
{
   public class ArgumentError
   {
      public ArgumentErrorType ErrorType
      {
         get;
      }

      public ArgumentError( ArgumentErrorType errorType )
      {
         ErrorType = errorType;
      }
   }
}
