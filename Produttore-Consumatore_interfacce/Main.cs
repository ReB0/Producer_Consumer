using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Produttore_Consumatore_interfacce
{
    class Producer_Consumer
    {


        static void Main(string[] args)
        {

            int buffer_size = 0;
            int n_consumers_rand = 0;   // number of random consumers
            int n_producers_rand = 0;   // number of random producers
            int n_producers_seq = 0;    // number of sequential producers


            Console.Write("Insert the buffer size: ");
            buffer_size = Convert.ToInt32(Console.ReadLine());

            Console.Write("Insert the number of random producers: ");
            n_producers_rand = Convert.ToInt32(Console.ReadLine());


            Console.Write("Insert the number of sequential producers: ");
            n_producers_seq = Convert.ToInt32(Console.ReadLine());

            Console.Write("Insert the numer of random consumers: ");
            n_consumers_rand = Convert.ToInt32(Console.ReadLine());

            
            Buffer.Buffer_Size(buffer_size); 


            List<Produttore_Random> producers_rand = new List<Produttore_Random>();
            List<Consumatore_Random> consumers_rand = new List<Consumatore_Random>();
            List<Produttore_Sequenziale> producers_seq = new List<Produttore_Sequenziale>();


            for (int i = 0; i < n_producers_rand; i++) { producers_rand.Add(new Produttore_Random(i, buffer_size)); }
            for (int j = 0; j < n_consumers_rand; j++) { consumers_rand.Add(new Consumatore_Random(j, buffer_size)); }
            for (int k = 0; k < n_producers_seq; k++) { producers_seq.Add(new Produttore_Sequenziale(k, buffer_size)); }
            
            // Creo i vari thread, uno per ciascun produttore/consumatore

            Thread[] thread = new Thread[n_consumers_rand + n_producers_rand+n_producers_seq];
            
            //inizializzo l'operazione che dovrà compiere ciascun thread

            for (int i = 0; i < n_producers_rand; i++)
            {
                thread[i] = new Thread(new ThreadStart(producers_rand.ElementAt(i).Start));
            }


            for (int j = 0; j < n_consumers_rand; j++)
            {
                thread[j+n_producers_rand] = new Thread(new ThreadStart(consumers_rand.ElementAt(j).Start));
            }

            for (int k = 0; k < n_producers_seq; k++)
            {
                thread[k+n_producers_rand+n_consumers_rand] = new Thread(new ThreadStart(producers_seq.ElementAt(k).Start));
            }


            //Do l'avvio a tutti i thread
            foreach (Thread x in thread)
            {
                x.Start();
            }
            Console.Read();

        }


    }
}
