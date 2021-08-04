using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class GeneticAlgorithm<T>
{
        public List<DNA<T>> Population { get; private set; }
        public int Generation { get; private set; }
        public float BestFitness { get; private set; }

        public float Deltafitness { get; private set; }

       public float DeltaBestfitness { get; private set; }

       public float Counter { get; private set; }

       public float fitsum { get; private set; }

       public T[] BestGenes { get; private set; }

        public int Elitism;
        public float MutationRate;
        public float Starting_MutationRate;
        private List<DNA<T>> newPopulation;
        private Random random;
        private float fitnessSum;
        private int dnaSize;
        private Func<T> getRandomS;
        private Func<T> getRandomN;
        private Func<int, float> fitnessFunction;
        private DNA<T> child_old;



    public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomS, Func<T> getRandomN, Func<int, float> fitnessFunction,
            int elitism, float mutationRate = 0.01f)
        {
            Counter = 0;
            Generation = 1;
            Elitism = elitism;
            MutationRate = mutationRate;
            Starting_MutationRate = mutationRate;
            Population = new List<DNA<T>>(populationSize);
            newPopulation = new List<DNA<T>>(populationSize);
            this.random = random;
            this.dnaSize = dnaSize;
            this.getRandomS= getRandomS;
            this.getRandomN = getRandomN;
            this.fitnessFunction = fitnessFunction;

            child_old = new DNA<T>(dnaSize, random, getRandomS, getRandomN, fitnessFunction, shouldInitGenes: false);

            BestGenes = new T[dnaSize];

            for (int i = 0; i < populationSize; i++)
            {
                Population.Add(new DNA<T>(dnaSize, random, getRandomS, getRandomN, fitnessFunction, shouldInitGenes: true));
            }
        }

        public void NewGeneration(int numNewDNA = 0, bool crossoverNewDNA = false)
        {
            int finalCount = Population.Count + numNewDNA;
            int convergense=0;
            if (finalCount <= 0)
            {
                return;
            }

            if (Population.Count > 0)
            {
                CalculateFitness();
                Population.Sort(CompareDNA);
            }
            newPopulation.Clear();

            for (int i = 0; i < finalCount; i++)
            {
                if (i < Elitism && i < Population.Count)
                {
                    newPopulation.Add(Population[i]);
                }
                else if (i < Population.Count || crossoverNewDNA)
                {

                float gen_0 = float.Parse(child_old.Genes[0].ToString());
                float gen_1 = float.Parse(child_old.Genes[1].ToString());

                DNA<T> parent1 = ChooseParent();
                    DNA<T> parent2 = ChooseParent();

                Console.WriteLine("Parent 1:" + parent1.Genes[0].ToString() + " " + parent1.Genes[1].ToString());
                Console.WriteLine("Parent 2: " + parent2.Genes[0].ToString() + " " + parent2.Genes[1].ToString());

                DNA<T> child = parent1.Crossover(parent2);

              
                child.Mutate(MutationRate);

                Console.WriteLine("child after mutation:" + child.Genes[0].ToString() + " " + child.Genes[1].ToString());

                float gen0 = float.Parse(child.Genes[0].ToString());
                float gen1 = float.Parse(child.Genes[1].ToString());

                float delta_0 = gen0 - gen_0;
                float delta_1 = gen1 - gen_1;

                if (delta_0 == 0 && delta_1 == 0)
                {
                    convergense += 1;
                }

                Console.WriteLine("Convergenece :" + convergense.ToString());

                child_old = child;

                newPopulation.Add(child);
                }
                else
                {
                    newPopulation.Add(new DNA<T>(dnaSize, random, getRandomS, getRandomN, fitnessFunction, shouldInitGenes: true));
                }
            }

            List<DNA<T>> tmpList = Population;
            Population = newPopulation;
            newPopulation = tmpList;

            Generation++;

        Console.WriteLine("Mutation Rate now: " + MutationRate.ToString());

        int fin_elit = finalCount - Elitism - 1;

        if (convergense > (fin_elit*80/10) && MutationRate < 1.1f)
        {

            MutationRate += 0.05f;
            Console.WriteLine("----------------------------Mutation rate INCREASED!-------------------------");
            Console.WriteLine("Mutation Rate now: " + MutationRate.ToString());

            // } else if( MutationRate>Starting_MutationRate && convergense<(finalCount*2/10))
        }
        else if (MutationRate > (Starting_MutationRate/2) && convergense < (fin_elit * 30 / 10))
        {

            MutationRate -= 0.05f;
            Console.WriteLine("---------------------------Mutation rate DECREASED!--------------------------");
            Console.WriteLine("Mutation Rate now: " + MutationRate.ToString());

        }
        }

        private int CompareDNA(DNA<T> a, DNA<T> b)
        {
            if (a.Fitness > b.Fitness)
            {
                return -1;
            }
            else if (a.Fitness < b.Fitness)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private void CalculateFitness()
        {
          fitnessSum = 0;
            DNA<T> best = Population[0];

        /*  Aqui se podria incrementar un mejor mecanizmo de score, basandome en las notas ya puestas organizando y poniendo el max score al mejor individuo*/

        for (int i = 0; i < Population.Count; i++)
            {
              fitnessSum += Population[i].CalculateFitness(i);
           

              if (i>0)
            {
                Deltafitness = Math.Abs(Population[i].Fitness - best.Fitness);
              
                if (Deltafitness <= 1/10 && Deltafitness >= 0) // here we can modify the exatitude 
                {
                    Counter = Counter + 1;
                }
                else
                {
                    Counter = Counter - 1;
                }

                Console.WriteLine("delta: " + Deltafitness.ToString() + " " + "counter" + " " + Counter.ToString());
            }
          



            if (Population[i].Fitness > best.Fitness)
                {
                Console.WriteLine("New best fit : " + best.Fitness.ToString());
                     best = Population[i];
                
            }
            }
       fitsum = fitnessSum;

        BestFitness = best.Fitness;
            best.Genes.CopyTo(BestGenes, 0);
        }

        private DNA<T> ChooseParent()
        {

        double randomNumber = 0;

        if (BestFitness > 0)
        {
            randomNumber = (double)(random.Next(50, 101)) / 100 * BestFitness;
        } else
        {
            randomNumber = (double)(random.Next(150, 201)) / 100 * BestFitness;
        }
          

        Console.WriteLine("Rand Number created:" + randomNumber.ToString());

            for (int i = 0; i < Population.Count; i++)
            {

                if ( Population[i].Fitness >= randomNumber)
                {

                Console.WriteLine("Parent Found ... " + randomNumber.ToString() + "<" + Population[i].Fitness.ToString());
                return Population[i];
                }
           
            }
        
            return null;
        }
    }

