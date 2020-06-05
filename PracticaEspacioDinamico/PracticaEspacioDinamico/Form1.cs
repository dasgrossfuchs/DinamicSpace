using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaEspacioDinamico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ///////////////////////VARIABLES GLOBALES INICIO////////////////////////////
        Label[] lbl;//tabla de bloques
        List<int> activos;//procesos en ejecucion
        int[] procesos;//bloques de espacio disponibles
        List<int> coordenadas;//bloques que estan libres u ocupados
        bool error;
        ///////////////////////VARIABLES GLOBALES FIN////////////////////////////
        private void Form1_Load(object sender, EventArgs e)//se inicializan los valores de la forma.
        {
            lbl = new Label[100];//se declaran 100 labels para la estructura de la tabla de bloques
            create();//se corre la funcion create, la cual crea la tabla que representa los bloques
            activos = new List<int>();//se inicializa la lista de procesos activos
            procesos = new int[100];//se cuentan con 100 bloques ocuparan
            for (int i = 0; i < 100; i++)//se inicializan como libres
            {
                procesos[i] = 0;
            }
            //Ya que el programa no es utilizable hasta que el 
            //usuario declare las variables se deshabilitan botones
            button1.Enabled = false;
            button3.Enabled = false;
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        //El metodo evita caracteres no numericos
        //ya que solo se necesita de numeros positivos enteros
        //
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public void create()//crear interfaz y tabla
        {
            ///////////////////////Interfaz/INICIO////////////////////////
            for (int i = 0; i < 10; i++)//creacion de labels horizontales
            {
                Label lba = new Label();
                int x = 27 + (25 * i);
                int y = 10;
                lba = new Label();
                lba.Location = new Point(x, y);
                lba.Name = "Guia" + i;
                lba.Text = "-" + (i + 1).ToString() + "-";
                lba.AutoSize = true;
                // set other properties
                this.Controls.Add(lba);
            }
            for (int i = 0; i < 10; i++)//creacion de labels verticales
            {
                Label lba = new Label();
                int x;
                if (i == 9) x = 5;
                else x = 10;
                int y = 33 + (25 * i);
                lba = new Label();
                lba.Location = new Point(x, y);
                lba.Name = "Guib" + i;
                lba.Text = (i + 1).ToString();
                lba.AutoSize = true;
                // set other properties
                this.Controls.Add(lba);
                if (i != 9)
                {
                    lba = new Label();
                    lba.Location = new Point(12, y + 13);
                    lba.Name = "Guic" + i;
                    lba.Text = "|";
                    lba.AutoSize = true;
                    // set other properties
                    this.Controls.Add(lba);
                }
            }
            ///////////////////////Interfaz/FIN////////////////////////
            ///////////////////////Tabla/INICIO///////////////////////////
            for (int i = 0; i < 10; i++)
            {
                for (int ii = 0; ii < 10; ii++)
                {
                    int x = 27 + (25 * ii);
                    int y = 33 + (25 * i);
                    lbl[(ii + i * 10)] = new Label();
                    lbl[(ii + i * 10)].Location = new Point(x, y);
                    //los cuadros son del 1 al 100 "lbl_"
                    lbl[(ii + i * 10)].Name = "lbl" + (ii + i * 10);
                    lbl[(ii + i * 10)].Text = "0";
                    lbl[(ii + i * 10)].AutoSize = true;
                    this.Controls.Add(lbl[(ii + i * 10)]);
                }
            }
            //se dibujan las lineas de la tabla
            pictureBox1.SendToBack();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics grphx = Graphics.FromImage(pictureBox1.Image);
            Pen pluma = new Pen(Color.Black, 1);
            for (int i = 0; i < 11; i++)
            {
                grphx.DrawLine(pluma, 0 + (25 * i), 0, 0 + (25 * i), 250);
                grphx.DrawLine(pluma, 0, 0 + (25 * i), 250, (25 * i) + 0);
            }
        }
        ///////////////////////Tabla/FIN////////////////////////////////////////////
        ///////////////////////HABILITAR CONTROL INICIO////////////////////////////

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { button3.Enabled = true; }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        { button1.Enabled = true; }

        ///////////////////////HABILITAR CONTROL FIN////////////////////////////
        /*
        Se realiza un check para ver cuar de los radiobuttons esta presionado,
        dependiendo de la eleccion del usuario se captura un numero diferente el cual 
        hace la eleccion de estilo de alojamiento.

        Despues de esto para evitar problemas el codigo se asegura de que las 
        opciones estan ingresadas por el usuario.

        Finalmente hace un recorrido de los procesos activos en la lista de procesos 
        y selecciona el primer numero de proceso mas pequeño que no se encuentre en
        la lista.
        ###########################First Fit#########################################
        1.-
        El codigo hace un recorrido del arreglo donde estan almacenados los procesos 
        representados en memoria.
        2.-
        Elije el primer encuentro con el numero de bloques libres ("0") necesarios 
        para almacenar en memoria.
        3.-
        Finalmente al encontrar los espacios guarda estos a la lista de coordenadas
        para ser utilizados posteriormente.
        3.1.-
        En caso de no encontrar el espacio necesario manda mensaje ("No se pudo
        almacenar, espacio insuficiente, se requiere vaciar memoria") y el proceso 
        de agregar a memoria termina
        ###########################Best Fit##########################################
        1.-
        El codigo recorre y guarda en la lista la cantidad de espacios necesarios 
        no consecutivos que se encuentran vacios ("0").
        2.-
        Se agregan a la lista Coordenadas para ser utilizados posteriormente
        2.1.-
        En caso de no encontrar el espacio necesario manda mensaje ("No se pudo
        almacenar, espacio insuficiente, se requiere vaciar memoria") y el proceso 
        de agregar a memoria termina
        ###########################Best Fit Extended#################################
        1.-
        El codigo recorre y guarda en la lista todos los espacios que se encuentran
        vacios ("0").
        2.-
        Se agregan a la lista Coordenadas para ser utilizados posteriormente
        2.1.-
        En caso de no encontrar el espacio necesario manda mensaje ("No se pudo
        almacenar, espacio insuficiente, se requiere vaciar memoria") y el proceso 
        de agregar a memoria termina
        ###########################Finalizando funcion###############################
        i.      Se hace el cambio de texto en la tabla
        ii.     Se reinician valores temporales
        iii.    Se vacian listas temporales
        iv.     se agrega el valor de proceso a la lista activos
        v.      se actualiza la lista de procesos que pueden ser terminados
        */
        ///////////////////////INSERTAR A MEMORIA INICIO////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            //VARIABLES LOCALES
            int estilo = 0;//estilo es el tipo de alojamiento de memoria ("0")
            int activando = 0; //activando es el proceso a activar
            bool notfound = true;
            int temp = 0;
            bool fin = false;
            error = false;
            coordenadas = new List<int>();//VACIADO DE ESPACIOS
            comboBox1.Items.Clear();//VACIADO DE LISTA
            while (!error && !fin)
            {
                int nodos=0;
                if (radioButton1.Checked) estilo = 1;//first fit
                if (radioButton2.Checked) estilo = 2;//Best fit
                if (radioButton3.Checked) estilo = 3;//Best fit extended
                //busca errores de usuario
                buscaerror();
                if (error)
                break;
                //busca el primer proceso mas pequeño
                activando = procesolibre();
                //captura el numero asignado por usuario
                nodos = int.Parse(textBox1.Text);
                switch (estilo)
                {
                    //ASIGNAR FIRST FIT
                    case 1 :  
                    {
                        int coordenada = 0,ii;
                        //Identifica la primera de la serie de coordenadas
                        //necesarias para llenar la cantidad de bloques 
                        //asignados por el usuario
                        while (coordenada <= (100 - nodos) && notfound)
                        {
                            temp = 0;
                            ii = coordenada;
                            while (ii <= coordenada + nodos  && ii<100)
                            {
                                temp += procesos[ii];
                                ii++;
                            }
                            if (temp == 0)
                            {
                                notfound = false;
                                break;
                            }
                            coordenada++;
                        }
                        if (error || notfound)
                        {
                            MessageBox.Show("No hay espacio disponible para la operacion, " +
                            "se requiere liberar espacio",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                        //ingresa el proceso a activar en memoria
                        for (int i = coordenada; i < nodos + coordenada; i++)
                        { procesos[i] = activando;}
                        activos.Add(activando);//ASIGNAR A LISTA DE PROCESOS
                    }
                    break;
                    //ASIGNAR BEST FIT
                    case 2:
                    {
                        coordenadas = new List<int>();
                        temp = 0;
                        while (temp<100 &&  notfound)
                        {
                            if (procesos[temp] == 0 )
                            {
                                coordenadas.Add(temp);
                                if (coordenadas.Count() == nodos)
                                {
                                    notfound = false;
                                }
                            }
                            temp++;
                        }
                        if (coordenadas.Count() < nodos)//REVISAR QUE HAYA ESPACIO NECESARIO
                        {
                            MessageBox.Show("No hay espacio disponible para la operacion, " +
                                        "se requiere liberar espacio",
                                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            error = true;
                                break;
                        }
                        foreach (var item in coordenadas)
                        {
                            procesos[item] = activando; 
                                
                        }
                            activos.Add(activando);//ASIGNAR A LISTA DE PROCESOS
                    }
                        break;
                    //ASIGNAR BEST FIT EXTENDED
                    case 3:
                    {
                        coordenadas = new List<int>();
                        //PASO I
                        //AGREGAR ESPACIOS VACIOS A COORDENADAS
                        while (temp < 100)
                        {
                            if (procesos[temp] == 0)
                            {

                                coordenadas.Add(temp);
                                if (coordenadas.Count() == nodos)
                                {
                                    notfound = false;
                                }
                            }
                            temp++;
                        }
                        if (notfound)//REVISAR QUE HAYA ESPACIOS VACIOS NECESARIOS
                        {
                            MessageBox.Show("No hay espacio disponible para la operacion, " +
                                            "se requiere liberar espacio",
                                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            error = true;
                        }
                        //ASIGNAR A MEMORIA
                        foreach (var item in coordenadas)
                        {
                            procesos[item] = activando;

                        }
                        activos.Add(activando);//ASIGNAR A LISTA DE PROCESOS
                    }
                    break;
                    default:
                        break;
                }
                fin = true;//FINALICACION DE PROCESO
            }
            //FINALIZACION DE PROCESO
            {
                //IMPRESION DE TABLA
                tabular();
                textBox1.Text = "";//BORRADO DE VARIABLE EN INTERFAZ
                //SE LIMPIA SELECCION
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                //LIMPIAR COMBOBOX
                foreach (var item in activos)
                {
                    //AGREGAR OPCIONES A VACIADO DE MEMORIA
                    comboBox1.Items.Add("Proceso #"+item);
                }
                button1.Enabled = false;//SE DESHABILITA BOTON 
            }
        }
        ///////////////////////INSERTAR A MEMORIA FIN//////////////////
        ///////////////////////VACIAR MEMORIA INICIO///////////////////
        private void button3_Click(object sender, EventArgs e)
        {
            //Se captura el numero de proceso a eliminar
            //La primer linea agarra el texto que esta en la 
            //seleccion y agarra el numero que esta tras el '#'
            int noproc = int.Parse(comboBox1.SelectedItem.ToString().Split('#').ElementAt(1));
            int temp = 0;
            coordenadas = new List<int>();//VACIADO DE ESPACIOS
            //El ciclo encuentra todas las coordenadas donde
            //el proceso especificado se encuentra.
            while (temp < 100)
            {
                if (procesos[temp] == noproc)
                {
                    coordenadas.Add(temp);
                }
                temp++;
            }
            //vacia todas las coordenadas =>("0")
            foreach (var item in coordenadas)
            {
                procesos[item] = 0;
            }
            //impresion de tabla
            tabular();
            //quita el proceso eliminado de la lista de los procesos activos
            activos.Remove(noproc);
            //actualiza las opciones que hay en el combobox
            comboBox1.Items.Clear();
            foreach (var item in activos)
            {
                comboBox1.Items.Add("Proceso #" + item);
            }
            button3.Enabled = false;//desactiva control de usuario
        }
        ///////////////////////VACIAR MEMORIA FIN//////////////////////
        ///////////////////////RESET INICIO////////////////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            activos.Clear();
            for (int i = 0; i < 100; i++)
            {
                procesos[i] = 0;
            }
            tabular();
        }
        ///////////////////////RESET FIN////////////////////////////
        ///////////////////////CHECK DE ERRORES INICIO/////////////
        public void buscaerror()
        {
            //En caso de que no se declaro la cantidad de bloques
            if (string.IsNullOrEmpty(textBox1.Text) || textBox1.Text == "0")
            {
                textBox1.Text = "0";
                MessageBox.Show("No se establecio la cantidad de Nodos," +
                    " o esta fue \"0\" el proceso no pudo ser iniciado",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                error = true;
            }
            int nodos = int.Parse(textBox1.Text);
            //En caso de que la cantidad de bloques es mayor a 100
            if (!(string.IsNullOrEmpty(textBox1.Text)))
            {
                if (nodos > 100)
                {
                    MessageBox.Show("Solo se cuentan con 100 nodos disponibles " +
                        "la cantidad puesta fue mayor a la disponible",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    error = true;
                }
            }
        }
        ///////////////////////CHECK DE ERRORES FIN///////////////////////
        ///////////////////////REVISION DE PROCESOS ACTIVOS INICIO////////
        public int procesolibre()
        {
            bool found = false;
            int no = 0;
            while (!found)
            {
                no++;
                if (!activos.Contains(no))
                {
                    found = true;
                }
            }
            return no;
        }
        ///////////////////////REVISION DE PROCESOS ACTIVOS FIN////////
        ///////////////////////IMPRESION DE TABLA FIN////////////////////
        
        public void tabular()
        {
            for (int i = 0; i < 10; i++)//IMPRESION DE TABLA
            {
                for (int ii = 0; ii < 10; ii++)
                {
                    string name = "lbl" + (ii + i * 10);

                    Control ctn = this.Controls[name];

                    ctn.Text = procesos[(ii + i * 10)].ToString();
                }
            }
        }
        ///////////////////////IMPRESION DE TABLA FIN////////////////////
    }
}
