using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Produttore_Consumatore_interfacce
{


    abstract class Utenti
    {
        public int id { get; set; }  // identificativo dell'utente che accede al buffer
        protected Random rand = new Random((int)DateTime.Now.Ticks);   // generatore di numeri casuali
        
        // protected abstract void Method1(); 
       
    }
    

    class Produttore_Random : Utenti, IProduttore {

        private int value;
        private int buf_size;
        private int in_buffer;

        public Produttore_Random(int idx, int bfsize) {
            id = idx;
            buf_size = bfsize;
        }

        public void Start() {

            Produci();

        }

        public async void Produci()
        {

            while (true)
            {

                Thread.Sleep(new Random(new Random().Next(500)).Next(700, 1400));
                
                value = rand.Next(1000);

                lock (typeof(Buffer))
                {
                    in_buffer = Buffer.Write(value);
                    Stampa_Random_Producer(id, in_buffer);
                }

                if (in_buffer == buf_size) { await Task.Run(() => Buffer.Buffer_is_not_full()); }
                
            }


        }

        static public void Stampa_Random_Producer(int id, int in_buffer){
            
         Console.ForegroundColor = ConsoleColor.DarkYellow;
         Console.Write("Random producer {0} wrote into buffer. {1} values stored in buffer.\n", id, in_buffer);
         Console.ResetColor();

        }



        }

   
     class Consumatore_Random : Utenti,IConsumatore{

        private int in_buffer;
        private int buf_size;

    public Consumatore_Random(int idx, int bfsize) {
         id = idx;
         buf_size = bfsize;

        }

        public void Start()
        {
             Consuma();
        }

        public async void Consuma() {

        while (true)
        {
            Thread.Sleep(new Random(new Random().Next(500)).Next(700, 1400));
                
                lock (typeof(Buffer))
                {
                  in_buffer = Buffer.Read();
                  Stampa_Random_Consumer(id,in_buffer);
                }
                
                if (in_buffer == 0) { await Task.Run(() => Buffer.Buffer_is_not_empty()); }

            }

        }

        static public void Stampa_Random_Consumer(int id, int in_buffer)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Random consumer {0} read from buffer. {1} values stored in buffer.\n", id, in_buffer);
            Console.ResetColor();

        }
        
        
    }
    

   class Produttore_Sequenziale: IProduttore{

        private int value;
        private int id;  
        private int buf_size;
        private int in_buffer;

        public Produttore_Sequenziale(int idx, int bfsize) {
            id = idx;
            buf_size = bfsize;
        }

        public void Start()
        {
           Produci();
        }
        
        public async void Produci(){


            while (true)
            {
                Thread.Sleep(new Random(new Random().Next(500)).Next(700, 1400));
                
                    lock (typeof(Buffer))
                     {
                    value = value + 1;
                    in_buffer = Buffer.Write(value);
                    Stampa_Sequential_Producer(id, in_buffer);
                    }

                if (in_buffer == buf_size) { await Task.Run(() => Buffer.Buffer_is_not_full()); }
                
            }
        }

        static public void Stampa_Sequential_Producer(int id, int in_buffer)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Sequential producer {0} wrote into buffer. {1} values stored in buffer.", id, in_buffer);
            Console.ResetColor();

        }
        

    }


}
