using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class DNA <T>
    {
        public T[] Genes { get; private set; }
        public float Fitness { get; private set; }

        private Random random;
        private Func<T> getRandomS;
       private Func<T> getRandomN;
          private Func<int, float> fitnessFunction;

        public DNA(int size, Random random, Func<T> getRandomS, Func<T> getRandomN, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
        {
            Genes = new T[size];
            this.random = random;
            this.getRandomS = getRandomS;
        this.getRandomN = getRandomN;
        this.fitnessFunction = fitnessFunction;

            if (shouldInitGenes)
            {
              for (int i = 0; i < Genes.Length; i++)
                {
                    if (i > 0)
                    {
                        Genes[i] = getRandomN();
                    Console.Write("Gen two:" + Genes[i].ToString());
                }
                    else
                    {
                        Genes[i] = getRandomS();
                    Console.Write("Gen one:" + Genes[i].ToString() + " ");
                }
            }

   
            Console.WriteLine("");
            Console.WriteLine("---------------");
        }
        }

        public float CalculateFitness(int index)
        {
            Fitness = fitnessFunction(index);
            return Fitness;
        }

        public DNA<T> Crossover(DNA<T> otherParent)
        {
            DNA<T> child = new DNA<T>(Genes.Length, random, getRandomS, getRandomN, fitnessFunction, shouldInitGenes: false);

        Console.WriteLine("child created:" + CharArrayToString(child.Genes));

        for (int i = 0; i < Genes.Length; i++)
            {
               // child.Genes[i] = random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];

            if (random.NextDouble() < 0.5)
            {
                child.Genes[i] = otherParent.Genes[i];
            } else
            {
                child.Genes[i] = Genes[i];
            }


            }


        Console.WriteLine("child after crossover:" + CharArrayToString(child.Genes));

        return child;
        }

        public void Mutate(float mutationRate)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                if (i > 0)
                {
                    Genes[i] = getRandomN();
                    Console.WriteLine("Mutation on n:" + Genes[i].ToString());
                }
                else
                {
                    Genes[i] = getRandomS();
                    Console.WriteLine("Mutation on s:" + Genes[i].ToString());
                }
                    
                }
            }
        }

    private string CharArrayToString(T[] charArray)
        {
            var sb = new StringBuilder();
            foreach (var c in charArray)
            {
                sb.Append(c + " ");
            }

            return sb.ToString();
        }

    }

