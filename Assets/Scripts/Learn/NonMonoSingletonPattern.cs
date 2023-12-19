

public class NonMonoSingletonPattern
{
     public static NonMonoSingletonPattern Instance
     {
          get
          {
               if (_instance == null)
               {
                    _instance = new NonMonoSingletonPattern();
               }

               return _instance;
          }
     }

     private static NonMonoSingletonPattern _instance;
     
     public int coins { get; private set; }
}