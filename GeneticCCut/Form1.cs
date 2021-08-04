//Created by Juan Salvador Medina - 2021

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticCCut
{
    public partial class Form1 : Form
    {
        private GeneticAlgorithm<float> ga;

        private System.Random random;

        bool time_opt = false;

        int dnasize = 2;

        int populationSize =150;
        float mutationRate = 0.5f;
        int elitism = 5;


        float Yv = 0;
        float b1 = 0;

        float Yp = 0;
        float b2 = 0;
        float np = 0;

        float NR = 0;
        float YR = 0;
        float b3 = 0;

        float b6 = 0;
        float b7 = 0;

        public Form1()
        {
            InitializeComponent();

          
        }


        void Updater()
        {
            ga.NewGeneration();

            UpdateText(ga.BestGenes, ga.BestFitness, ga.Generation, ga.Population.Count, (j) => ga.Population[j].Genes);

        }

        private float GetRandomS()
        {
             int k = 0; //decimal coef.

             string s_min_s = textBox1.Text.Replace(".", ",");
             string s_max_s = textBox2.Text.Replace(".", ",");

             float s_min = (float) Math.Round(float.Parse(s_min_s),2);
             float s_max = (float) Math.Round(float.Parse(s_max_s),2);

             s_min_s = s_min.ToString();
             s_max_s = s_max.ToString();

             int s_min_d = 0;
             int s_max_d = 0;

             if (s_min_s.IndexOf(",") > 0)
             {
                s_min_d = s_min_s.Substring(s_min_s.IndexOf(",") + 1).Length;
             }


             if (s_max_s.IndexOf(",") > 0)
             {
                s_max_d = s_max_s.Substring(s_max_s.IndexOf(",") + 1).Length;
             }


             if (s_max_d != 0 || s_min_d != 0)
             {
                 if (s_max_d >= s_min_d)
                 {
                     k = s_max_d;
                 }
                 else
                 {
                     k = s_min_d;
                 }

             }

             float i = 0;
             int aux = 0;

             if (k > 0)
             {
                float min_b = (s_min * (float) Math.Pow(10, k));
                float max_b = (s_max * (float) Math.Pow(10, k));

                 int min = (int)(min_b);
                 int max = (int)(max_b);

                 aux =  random.Next(min, max + 1);

                 i = aux/((float) Math.Pow(10, k));
                
                //for debug
                
                /*
                if (i == 0)
                {
                    MessageBox.Show("min:" + min.ToString());
                }*/
            }
            else
             {
                int min = int.Parse(textBox1.Text);
                int max = int.Parse(textBox2.Text);

                //if is less than 50 then we work with 2 decimals

                if (max < 50)
                {
                    min = (min * (int) Math.Pow(10, 2));
                    max = (max * (int) Math.Pow(10, 2));
                    aux = random.Next(min, max + 1);
                    i = aux / ((float)Math.Pow(10, 2));
                }
                else
                {
                    i = random.Next(min, max + 1);
                }

             }

            return i;
        }

        private float GetRandomN()
        {
            int k = 0; //transformation coef.

            string n_min_s = textBox3.Text.Replace(".", ",");
            string n_max_s = textBox4.Text.Replace(".", ",");

            float n_min = (float)Math.Round(float.Parse(n_min_s), 2);
            float n_max = (float)Math.Round(float.Parse(n_max_s), 2);

            n_min_s = n_min.ToString();
            n_max_s = n_max.ToString();

            int n_min_d = 0;
            int n_max_d = 0;

            if (n_min_s.IndexOf(",") > 0)
            {
               n_min_d = n_min_s.Substring(n_min_s.IndexOf(",") + 1).Length;
            }


            if (n_max_s.IndexOf(",") > 0)
            {
                n_max_d = n_max_s.Substring(n_max_s.IndexOf(",") + 1).Length;
            }


            if (n_max_d != 0 || n_min_d != 0)
            {
                if (n_max_d >= n_min_d)
                {
                    k = n_max_d;
                }
                else
                {
                    k = n_min_d;
                }

            }

            float i = 0;
            int aux = 0;

            if (k > 0)
            {

                float min_b = (n_min * (float)Math.Pow(10, k));
                float max_b = (n_max * (float)Math.Pow(10, k));
                int min = (int)(min_b);
                int max = (int)(max_b);

                aux = random.Next(min, max + 1);

                i = aux / ((float)Math.Pow(10, k));

            }
            else
            {
                int min = int.Parse(textBox3.Text);
                int max = int.Parse(textBox4.Text);

                //if is less than 9999 then we work with 1 decimal

                if (max < 9999)
                {
                    min = (min * (int) Math.Pow(10, 1));
                    max = (max * (int) Math.Pow(10, 1));
                    aux = random.Next(min, max + 1);
                    i = aux / ((float)Math.Pow(10, 1));
                }
                else
                {
                    i = random.Next(min, max + 1);
                }
            }

            return i;
        }


        private float FitnessFunction(int index)
        {
            float score = 0;
            DNA<float> dna = ga.Population[index];

            //IMPORTANT ----> x1 is n, gen 1, and x2 is s, gen 0 
            //x1 = ln(n) and x2=ln(100*s)

            float x1 = (float)Math.Log(dna.Genes[1]);
            float x2 = (float)Math.Log(dna.Genes[0] * 100);

            float ecuation1 = Ec1_builder(x1, x2);
            float ecuation2 = Ec2_builder(x1, x2);
            float ecuation3 = Ec3_builder(x1, x2);

            float ecuation6 = Ec6_builder(x1, x2);
            float ecuation7 = Ec7_builder(x1, x2);

            Console.WriteLine("---------------");
       
            // CHECKING FOR THE LIMITS


            if (ecuation1 <= b1 && ecuation2 <= b2 && ecuation3 <= b3 && ecuation6 >= b6 && ecuation7 <= b7 )
            {
                    score += dna.Genes[0]/ ((float) Math.Round(float.Parse(textBox2.Text.Replace(".",",")),2));
                    score += dna.Genes[1]/ ((float) Math.Round(float.Parse(textBox4.Text.Replace(".", ",")),2));
                    score = score / 2;

            }
            else
            {

                // We should rate based on how close to the objetive is

                ArrayList Delta = new ArrayList();

                float delta1 = ecuation1 - b1;
                float delta2 = ecuation2 - b2;
                float delta3 = ecuation3 - b3;

                float delta6 = b6 - ecuation6;
                float delta7 = ecuation7 - b7;


                Delta.Add(delta1);
                Delta.Add(delta2);
                Delta.Add(delta3);

                Delta.Add(delta6);
                Delta.Add(delta7);

                float delta_sum=0;

                for (int i=0;  i<Delta.Count; i++)
                {
                    if ((float) Delta[i] > 0)
                    {
                        delta_sum += (float)Delta[i];
                    }
                   
                }

                if (delta_sum < 0)
                {
                    delta_sum = 0;
                }

                if (delta_sum > 1000)
                {
                    score = 1 ;
                } else
                {
                    score = delta_sum / 1000;
                }

                score = (score*-1);

            }


                    if (score >= 0)
                {
                    score = ((float)Math.Pow(2, score) - 1) / (2 - 1);
                } else
                {
                    score = ((float)Math.Pow(2, Math.Abs(score)) - 1) / (2 - 1);
                    score = score * -1;
                }

            score = (float)Math.Round(score, 4);

          

            Console.WriteLine("// BEST DATA TILL NOW -> Best score:" + ga.BestFitness.ToString());
            Console.WriteLine("// Data from now ->" + "index: " + index.ToString() + " score:" + score.ToString() + " S:" + dna.Genes[0].ToString() + " n:" + dna.Genes[1].ToString());
            Console.WriteLine("// Ecuation 1:" + ecuation1.ToString() + "<=" + b1.ToString() + " Ecuation 2:" + ecuation2.ToString() + "<=" + b2.ToString());
            Console.WriteLine("// Ecuation 3:" + ecuation3.ToString() + "<=" + b3.ToString());
            Console.WriteLine("// Ecuation 6:" + ecuation6.ToString() + ">=" + b6.ToString());
            Console.WriteLine("// Ecuation 7:" + ecuation7.ToString() + "<=" + b7.ToString());

            return score;
            
        }


        private void UpdateText(float[] bestGenes, float bestFitness, int generation, int populationSize, Func<int, float[]> getGenes)
        {
            //  label2.Text = CharArrayToString(bestGenes);

            label2.Text = "Best Genes: " + FloatArrayToString(bestGenes);
            label3.Text = "Best fitness score: " + bestFitness.ToString();

            float x1 = (float)Math.Log(ga.BestGenes[1]);
            float x2 = (float)Math.Log(ga.BestGenes[0] * 100);

            float ecuation1 = Ec1_builder(x1, x2);
            float ecuation2 = Ec2_builder(x1, x2);
            float ecuation3 = Ec3_builder(x1, x2);

            float ecuation6 = Ec6_builder(x1, x2);
            float ecuation7 = Ec7_builder(x1, x2);

            label16.Text = "Ecuation 1: " + ecuation1.ToString() + "<= " + b1.ToString();
            label20.Text = ecuation1.ToString() + "<= " + b1.ToString();

            label17.Text = "Ecuation 2: " + ecuation2.ToString() + "<= " + b2.ToString();
            label21.Text = ecuation2.ToString() + "<= " + b2.ToString();

            label58.Text = "Ecuation 3: " + ecuation3.ToString() + "<= " + b3.ToString();
            label28.Text = ecuation3.ToString() + "<= " + b3.ToString();

            label59.Text = "Ecuation 6: " + ecuation6.ToString() + ">= " + b6.ToString();
            label44.Text = ecuation6.ToString() + ">= " + b6.ToString();

            label60.Text = "Ecuation 7: " + ecuation7.ToString() + "<= " + b7.ToString();
            label45.Text = ecuation7.ToString() + "<= " + b7.ToString();

            label4.Text = "Generation: " + generation.ToString();


        }

        private float Ec1_builder( float x1, float x2)
        {
            return (float)Math.Round((x1 + (Yv*x2)), 2);
        }

        private float Ec2_builder( float x1, float x2)
        {
            return (float)Math.Round((((1 - np) * x1) + (Yp * x2)), 2);
        }

        private float Ec3_builder(float x1, float x2)
        {
            return (float)Math.Round(((-1*NR *x1 ) + (YR * x2)), 2);
        }

        private float Ec6_builder(float x1, float x2)
        {
            return (float)Math.Round((x1+x2), 2);
        }

        private float Ec7_builder(float x1, float x2)
        {
            return (float)Math.Round((x1+x2), 2);
        }

        private string FloatArrayToString(float[] floatArray)
        {
            var sb = new StringBuilder();
            int i = 0;

            foreach (var c in floatArray)
            {

                string gen = "";

                if (i==0)
                {
                    gen = "| S: ";
                }else
                {
                    gen = "n: ";
                }

                sb.Append(gen);
                sb.Append(c);
                sb.Append("  |  ");
                i++;
            }

            return sb.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            


        }

        private void button1_Click(object sender, EventArgs e)
        {

            random = new System.Random();
            ga = new GeneticAlgorithm<float>(populationSize, dnasize, random, GetRandomS,GetRandomN, FitnessFunction, elitism, mutationRate);

            /*float x1 = (float) Math.Log(ga.BestGenes[1]);
            float x2 = (float) Math.Log(ga.BestGenes[0] * 100);

            float ecuation1 = Ec1_builder(x1, x2);
            float ecuation2 = Ec2_builder(x1, x2);
            float ecuation3 = Ec3_builder(x1, x2);

            float ecuation6 = Ec6_builder(x1, x2);
            float ecuation7 = Ec7_builder(x1, x2);*/

            // while (ga.BestFitness < 0 || ga.Counter < 100 || ecuation1 > b1 || ecuation2 > b2 || ecuation3 > b3 ||  ecuation6 < b6 || ecuation7 > b7)

            if (radioButton1.Checked)
            {
                time_opt = true;
            }

            while ( ga.BestFitness < 0 || ga.Counter < 100 )
            {
                if (ga.Counter > 10000)
                {
                    break;
                }
                Updater();
            }

            groupBox1.Visible = true;

           if (ga.BestFitness==0)
            {
                MessageBox.Show("Генетический алгоритм сходился на 0! Запустите программу снова");
            }

            if (ga.BestFitness < 0)
            {
                MessageBox.Show("Программа была остановлена, потому что решение забирает слишком долго, это может быть признаком невозможного решения, попробуйте снова запускать программу или попробуйте изменить параметры");
            }


            button5.Enabled = false;
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
    
            //Ecuation seven

            float Vs_max = float.Parse(Cleaner(textBox26.Text));

            //Ecuation six

            float Vs_min = float.Parse(Cleaner(textBox25.Text));

            //Ecuation five

            float n_max = float.Parse(Cleaner(textBox4.Text));

            //Ecuation four

            float n_min = float.Parse(Cleaner(textBox3.Text));

            //Ecuation three
                  NR = float.Parse(Cleaner(textBox18.Text));
                  YR = float.Parse(Cleaner(textBox17.Text));
            float Ra = float.Parse(Cleaner(textBox21.Text));
            float CR = float.Parse(Cleaner(textBox15.Text));
            float XR = float.Parse(Cleaner(textBox16.Text));
            float Kn = float.Parse(Cleaner(textBox32.Text));

            //Ecuation two
                  np = float.Parse(Cleaner(textBox20.Text));
                  Yp = float.Parse(Cleaner(textBox19.Text));
            float Xp = float.Parse(Cleaner(textBox22.Text));
            float N_st = float.Parse(Cleaner(textBox14.Text));
            float n_st = float.Parse(Cleaner(textBox13.Text));
            float Cp = float.Parse(Cleaner(textBox23.Text));

            // Ecuation one
            float Cv = float.Parse(Cleaner(textBox5.Text));
            float Kv = float.Parse(Cleaner(textBox6.Text));
                  Yv = float.Parse(Cleaner(textBox7.Text));
            float Xv = float.Parse(Cleaner(textBox8.Text));
            float m = float.Parse(Cleaner(textBox9.Text));
            float t = float.Parse(Cleaner(textBox24.Text));
            float D = float.Parse(Cleaner(textBox10.Text));
            float T = float.Parse(Cleaner(textBox11.Text));



            b1 = (float) Math.Round(Math.Log((Cv*Kv*((float) Math.Pow(100,Yv))*1000) / ((float)Math.PI * D * ((float)Math.Pow(T, m)) * ((float)Math.Pow(t, Xv)))),2);
            b2 = (float) Math.Round(Math.Log((N_st * 60000 * n_st * ((float)Math.Pow(Math.PI * D, np)) * 1000 * ((float)Math.Pow(100, Yp))) / (Cp * Kn *(float)Math.PI * D * ((float)Math.Pow(1000, np)) * ((float)Math.Pow(t, Xp)))), 2);
            b3 = (float) Math.Round(Math.Log( (Ra * ((float) Math.Pow(100,YR)) * ((float)Math.Pow(Math.PI * D, NR)))/ (CR*((float) Math.Pow(t,XR)) * ((float)Math.Pow(1000, NR))) ),2);

            float b4 = (float) Math.Round(Math.Log(n_min),2);
            float b5 = (float) Math.Round(Math.Log(n_max),2);

            b6 = (float) Math.Round(Math.Log( 100 * Vs_min), 2);
            b7 = (float) Math.Round(Math.Log( 100 * Vs_max), 2);

            button5.Enabled = true;

            label69.Text= "b1: " + b1.ToString()+ ";  b2: " + b2.ToString() + ";  b3: " + b3.ToString() + ";  b4: " + b4.ToString() +";  b5: " + b5.ToString() + ";  b6: " + b6.ToString() + ";  b7: " + b7.ToString();

        }

        private string Cleaner (string text )
        {
            return text.Replace(".", ",");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ec_1 ec = new ec_1();
            ec.Show();
                

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ec_2 ec2 = new ec_2();
            ec2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            random = new Random();
            MessageBox.Show(GetRandomS().ToString());
           // GetRandomN();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            float Vs_min = float.Parse(Cleaner(textBox25.Text));
            float Vs_max = float.Parse(Cleaner(textBox26.Text));

            float n_min = float.Parse(Cleaner(textBox3.Text));
            float n_max = float.Parse(Cleaner(textBox4.Text));

            float S_min = ((float) Math.Round((Vs_min/n_max),2));
            float S_max = ((float) Math.Round((Vs_max/n_min),2));

            float b8 = (float) Math.Round(Math.Log(100*S_min),2);
            float b9 = (float) Math.Round(Math.Log(100*S_max),2);

            textBox1.Text = S_min.ToString();
            textBox2.Text = S_max.ToString();

            label69.Text = label69.Text + "; b8: " + b8.ToString() + "; b9: " + b9.ToString();

            button1.Enabled = true;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox11.Text = "18,5";
            textBox10.Text = "30";
            textBox5.Text = "415";
            textBox6.Text = "1";
            textBox24.Text = "1";
            textBox8.Text = "0,4";
            textBox9.Text = "0,4";
            textBox7.Text = "0,4";

            textBox13.Text = "0,75";
            textBox14.Text = "22,4";
            textBox23.Text = "40";
            textBox20.Text = "0";
            textBox19.Text = "0,75";
            textBox22.Text = "1";
            textBox32.Text = "1";

            textBox15.Text = "31,2";
            textBox16.Text = "0,478";
            textBox21.Text = "0,8";
            textBox17.Text = "0,443";
            textBox18.Text = "0,66";

            textBox25.Text = "165";
            textBox26.Text = "16500";

            textBox4.Text = "8100";
            textBox3.Text = "400";

            button2.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
   
}
