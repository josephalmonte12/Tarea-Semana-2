using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulacionCarrera
{
    public partial class Form1 : Form
    {
        Random rand = new Random();

        int[] arreglo = new int[5000];
        //int[] arreglo = new int[100];
        float[] time = new float[5];

        public int BuscarGanador(float[] arr)
        {
            int pos = 0;
            float menor = arr[pos];

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < menor)
                {
                    menor = arr[i];
                    pos = i;
                };
            }
            return pos;

        }

        public void Generar()
        {
            BoxArregloPrincipal.Text = "";
            for (int i = 0; i < arreglo.Length; i++)
            {
                arreglo[i] = rand.Next(1, 100000); // Genera números aleatorios entre 1 y 999
                BoxArregloPrincipal.Text += "[" + arreglo[i] + "] ";
            }
        }
        public void Limpiar()
        {
            boxBinaria.Text = boxBurbuja.Text = boxQuickSort.Text = boxSecuencial.Text = boxInsercion.Text = "";
            labelQuikSort.Text = labelBinaria.Text = labelBurbuja.Text = labelInsercion.Text = labelSecuencial.Text = "";
            labelResultados.Visible = false;
            labelQuikSort.ForeColor = labelBinaria.ForeColor = labelBurbuja.ForeColor = labelInsercion.ForeColor = labelSecuencial.ForeColor = Color.Black;
            labelCantMemoria.Text = "Cantidad de memoria consumida:";
            labelCantMemoria.Visible = false;
        }

        public static int BuscarSecuencial(int[] arreglo, int elemento)
        {
            for (int i = 0; i < arreglo.Length; i++)
            {
                if (arreglo[i] == elemento)
                {
                    return i; // Elemento encontrado, retorna el índice
                }
            }
            return -1; // Elemento no encontrado
        }

        public static void Insercion(int[] arreglo)
        {
            int n = arreglo.Length;
            for (int i = 1; i < n; i++)
            {
                int elementoActual = arreglo[i];
                int j = i - 1;

                // Mover elementos del arreglo[0..i-1] que son mayores que el elemento actual
                while (j >= 0 && arreglo[j] > elementoActual)
                {
                    arreglo[j + 1] = arreglo[j];
                    j--;
                }

                // Colocar el elemento actual en su posición correcta
                arreglo[j + 1] = elementoActual;
            }
        }

        public static int BuscarBinaria(int[] arreglo, int elemento)
        {
            int izquierda = 0;
            int derecha = arreglo.Length - 1;

            while (izquierda <= derecha)
            {
                int medio = izquierda + (derecha - izquierda) / 2;

                if (arreglo[medio] == elemento)
                {
                    return medio; // Elemento encontrado, retorna el índice
                }
                else if (arreglo[medio] < elemento)
                {
                    izquierda = medio + 1;
                }
                else
                {
                    derecha = medio - 1;
                }
            }

            return -1; // Elemento no encontrado
        }

        public static void Burbuja(int[] arreglo)
        {
            int n = arreglo.Length;
            bool cambio;

            do
            {
                cambio = false;
                for (int i = 1; i < n; i++)
                {
                    if (arreglo[i - 1] > arreglo[i])
                    {
                        // Intercambia los elementos si están en el orden incorrecto
                        int temp = arreglo[i - 1];
                        arreglo[i - 1] = arreglo[i];
                        arreglo[i] = temp;
                        cambio = true;
                    }
                }
            } while (cambio);
        }

        public static void Ordenar(int[] arreglo, int izquierda, int derecha)
        {
            if (izquierda < derecha)
            {
                int particion = Particionar(arreglo, izquierda, derecha);

                // Ordenar recursivamente los subarreglos izquierdo y derecho
                Ordenar(arreglo, izquierda, particion - 1);
                Ordenar(arreglo, particion + 1, derecha);
            }
        }
        private static int Particionar(int[] arreglo, int izquierda, int derecha)
        {
            int pivote = arreglo[derecha];
            int i = izquierda - 1;

            for (int j = izquierda; j < derecha; j++)
            {
                if (arreglo[j] < pivote)
                {
                    i++;
                    // Intercambiar arreglo[i] y arreglo[j]
                    int temp = arreglo[i];
                    arreglo[i] = arreglo[j];
                    arreglo[j] = temp;
                }
            }

            // Colocar el pivote en su posición correcta
            int tempPivote = arreglo[i + 1];
            arreglo[i + 1] = arreglo[derecha];
            arreglo[derecha] = tempPivote;

            return i + 1;
        }

        public Form1()
        {
            InitializeComponent();

            Limpiar();
            Generar();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            Limpiar();

            int[] arreglo2 = arreglo;
            bool cambio;

            do
            {
                cambio = false;
                for (int i = 1; i < arreglo2.Length; i++)
                {
                    if (arreglo2[i - 1] > arreglo2[i])
                    {
                        // Intercambia los elementos si están en el orden incorrecto
                        int temp = arreglo[i - 1];
                        arreglo2[i - 1] = arreglo2[i];
                        arreglo2[i] = temp;
                        cambio = true;
                    }
                }
            } while (cambio);
            labelCantMemoria.Visible = true;
            labelResultados.Visible = true;
            var stopwatch = new Stopwatch();
            int busqueda = int.Parse(txtBuscar.Text.ToString());

            // Medir el uso de memoria antes de ejecutar la búsqueda secuencial
            long memoriaInicialBusquedaSecuencial = GC.GetTotalMemory(true);
            // Crear tareas para ejecutar los métodos en paralelo
            stopwatch.Start();
            var tareaBusquedaSecuencial = Task.Run(() => BuscarSecuencial(arreglo, busqueda));
            stopwatch.Stop();
            Console.WriteLine("Tiempo de ejecución de búsqueda secuencial: " + stopwatch.Elapsed);
            if (tareaBusquedaSecuencial.Result < 0) boxSecuencial.Text = "Elemento NO encontrado";
            else boxSecuencial.Text = "Elemento encontrado en la posicion " + tareaBusquedaSecuencial.Result;
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds.ToString());
            time[0] = float.Parse(stopwatch.Elapsed.TotalMilliseconds.ToString());
            // Medir el uso de memoria después de ejecutar la búsqueda secuencial
            long memoriaFinalBusquedaSecuencial = GC.GetTotalMemory(true);
            long memoriaConsumidaBusquedaSecuencial = memoriaFinalBusquedaSecuencial - memoriaInicialBusquedaSecuencial;
            Console.WriteLine("Memoria consumida en búsqueda secuencial: " + memoriaConsumidaBusquedaSecuencial + " bytes");
            labelSecuencial.Text = "Secuencial: " + stopwatch.Elapsed + " (" + (float.Parse(memoriaConsumidaBusquedaSecuencial.ToString()) / 1000000) + " MB)";
            float memoriaSecuencial = (float.Parse(memoriaConsumidaBusquedaSecuencial.ToString()) / 1000000);


            stopwatch.Restart();
            long memoriaInicialInsercion = GC.GetTotalMemory(true);
            var tareaInsercion = Task.Run(() => Insercion(arreglo.Clone() as int[]));
            stopwatch.Stop();
            Console.WriteLine("Tiempo de ejecución de Ordenamiento por Insercion: " + stopwatch.Elapsed);
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds.ToString());
            time[1] = float.Parse(stopwatch.Elapsed.TotalMilliseconds.ToString());
            long memoriaFinalMetodoInsercion = GC.GetTotalMemory(true);
            long memoriaConsumidaMetodoInsercion = memoriaFinalMetodoInsercion - memoriaInicialInsercion;
            Console.WriteLine("Memoria consumida en metodo de Insercion: " + memoriaConsumidaMetodoInsercion + " bytes");
            labelInsercion.Text = "Metodo por Insercion: " + stopwatch.Elapsed + " (" + (float.Parse(memoriaConsumidaMetodoInsercion.ToString()) / 1000000) + " MB)"; ;
            float memoriaInsercion = (float.Parse(memoriaConsumidaMetodoInsercion.ToString()) / 1000000);


            long memoriaInicialBinaria = GC.GetTotalMemory(true);
            stopwatch.Restart();
            var tareaBusquedaBinaria = Task.Run(() => BuscarBinaria(arreglo2, busqueda));
            stopwatch.Stop();
            Console.WriteLine("Tiempo de ejecución de Busqueda Binaria: " + stopwatch.Elapsed);
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds.ToString());
            if (tareaBusquedaBinaria.Result < 0) boxBinaria.Text = "Elemento NO encontrado";
            else boxBinaria.Text = "Elemento encontrado en la posicion " + tareaBusquedaBinaria.Result;
            time[2] = float.Parse(stopwatch.Elapsed.TotalMilliseconds.ToString());
            long memoriaFinalBusquedaBinaria = GC.GetTotalMemory(true);
            long memoriaConsumidaBusquedaBinaria = memoriaFinalBusquedaBinaria - memoriaInicialBinaria;
            Console.WriteLine("Memoria consumida en metodo de Binaria: " + memoriaConsumidaBusquedaBinaria + " bytes");
            labelBinaria.Text = "Busqueda Binaria: " + stopwatch.Elapsed + " (" + (float.Parse(memoriaConsumidaBusquedaBinaria.ToString()) / 1000000) + " MB)"; ; ;
            float memoriaBinaria = (float.Parse(memoriaConsumidaBusquedaBinaria.ToString()) / 1000000);


            long memoriaInicialBurbuja = GC.GetTotalMemory(true);
            stopwatch.Restart();
            var tareaBurbuja = Task.Run(() => Burbuja(arreglo.Clone() as int[]));
            stopwatch.Stop();
            Console.WriteLine("Tiempo de ejecución de ordenamiento burbuja: " + stopwatch.Elapsed);
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds.ToString());
            time[3] = float.Parse(stopwatch.Elapsed.TotalMilliseconds.ToString());
            long memoriaFinalMetodoBurbuja = GC.GetTotalMemory(true);
            long memoriaConsumidaMetodoBurbuja = memoriaFinalMetodoBurbuja - memoriaInicialBurbuja;
            Console.WriteLine("Memoria consumida en metodo de Burbuja: " + memoriaConsumidaMetodoBurbuja + " bytes");
            labelBurbuja.Text = "Metodo Burbuja: " + stopwatch.Elapsed + " (" + (float.Parse(memoriaConsumidaMetodoBurbuja.ToString()) / 1000000) + " MB)";
            float memoriaBurbuja = (float.Parse(memoriaConsumidaMetodoBurbuja.ToString()) / 1000000);


            long memoriaInicialQuikSort = GC.GetTotalMemory(true);
            stopwatch.Restart();
            var tareaQuickSort = Task.Run(() => Ordenar(arreglo.Clone() as int[], 0, arreglo.Length - 1));
            stopwatch.Stop();
            Console.WriteLine("Tiempo de ejecución de ordenamiento quiksort: " + stopwatch.Elapsed);
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds.ToString());
            time[4] = float.Parse(stopwatch.Elapsed.TotalMilliseconds.ToString());
            long memoriaFinalMetodoQuikSort = GC.GetTotalMemory(true);
            long memoriaConsumidaMetodoQuikSort = memoriaFinalMetodoQuikSort - memoriaInicialQuikSort;
            Console.WriteLine("Memoria consumida en metodo de QuikSort: " + memoriaConsumidaMetodoQuikSort + " bytes");
            labelQuikSort.Text = "QuikSort: " + stopwatch.Elapsed + " (" + (float.Parse(memoriaConsumidaMetodoQuikSort.ToString()) / 1000000) + " MB)"; ;
            float memoriaquik = (float.Parse(memoriaConsumidaMetodoQuikSort.ToString()) / 1000000);
            labelCantMemoria.Text += " " + (memoriaBinaria + memoriaBurbuja + memoriaquik + memoriaInsercion + memoriaSecuencial).ToString() + " MB";
            // Esperar a que todas las tareas se completen
            Task.WaitAll(tareaBusquedaSecuencial, tareaInsercion, tareaBusquedaBinaria, tareaBurbuja, tareaQuickSort);

            for (int i = 0; i < arreglo.Length; i++)
            {

                boxQuickSort.Text += "[" + arreglo2[i] + "] ";
            }
            boxBurbuja.Text = boxQuickSort.Text;
            boxInsercion.Text = boxQuickSort.Text;

            /*
            0- Secuencial
            1- Insercion
            2- Binaria
            3- Burbuja
            4- QuikSort
             */

            switch (BuscarGanador(time))
            {
                case 0:
                    labelSecuencial.ForeColor = Color.Green;
                    labelSecuencial.Text += " (GANADOR)";
                    break;
                case 1:
                    labelInsercion.ForeColor = Color.Green;
                    labelInsercion.Text += " (GANADOR)";
                    break;
                case 2:
                    labelBinaria.ForeColor = Color.Green;
                    labelBinaria.Text += " (GANADOR)";
                    break;
                case 3:
                    labelBurbuja.ForeColor = Color.Green;
                    labelBurbuja.Text += " (GANADOR)";
                    break;
                case 4:
                    labelQuikSort.ForeColor = Color.Green;
                    labelQuikSort.Text += " (GANADOR)";
                    break;

            }
        }

        private void btnGenerardatos_Click(object sender, EventArgs e)
        {
            Limpiar();
            Generar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
