using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorProcesos
{
    public class MPrioridad
    {
        DataGridView dataGridView;

        //----------------RoundRobin Class Constructor-------------------
        public MPrioridad(ref DataGridView temp_dataGridView)
        {
            dataGridView = temp_dataGridView;
        }

        //----------------Main Round Robin Algorithm Method-------------------
        public void runPrioridad(ref Proceso[] procesos, int quantum)
        {
            //Assign Each Process Its Execution Time
            foreach (var proceso in procesos)
            {
                proceso.TiempoRestante = proceso.Tiempo;
            }
            while (true)
            {
                bool executionFinished = true;
                bool PrioridadAlta = false;
                bool PrioridadMedia = false;
                bool PrioridadBaja = false;

                //Loop through all processes until the loop ends
                foreach (var proceso in procesos)
                {
                    if (proceso.Prioridad == 3)
                    {
                        proceso.Estado = "COMPLETED";
                        updateDataGridView(dataGridView, procesos);
                        proceso.TiempoRestante = 0;
                    }
                    if (proceso.Prioridad < 3)
                    {
                        proceso.Estado = "RUNNING";
                        updateDataGridView(dataGridView, procesos);
                        executionTimer(quantum);
                        proceso.Estado = "READY";
                        updateDataGridView(dataGridView, procesos);
                    }
                }
                foreach (var procesom in procesos)
                {
                    if (procesom.Prioridad == 2)
                    {
                        procesom.Estado = "COMPLETED";
                        updateDataGridView(dataGridView, procesos);
                        procesom.TiempoRestante = 0;
                    }
                    if (procesom.Prioridad < 2)
                    {
                        procesom.Estado = "RUNNING";
                        updateDataGridView(dataGridView, procesos);
                        executionTimer(quantum);
                        procesom.Estado = "READY";
                        updateDataGridView(dataGridView, procesos);
                    }
                }
                foreach (var procesob in procesos)
                {
                        procesob.Estado = "COMPLETED";
                        updateDataGridView(dataGridView, procesos);
                        procesob.TiempoRestante = 0;
                }
                if (executionFinished == true)
                {
                    break;
                }
            }
        }

        //----------------Update Data Grid View Method-------------------------------
        public void updateDataGridView(DataGridView dataGridView, Proceso[] procesos)
        {
            //Remove Current Data Grid Rows and Refresh it
            dataGridView.Rows.Clear();
            //dataGridView.Refresh();

            //Add Processes rows again to data grid view with updated values
            foreach (var proceso in procesos)
            {
                string[] row = { proceso.Id.ToString(), proceso.Nombre, proceso.Estado, proceso.TiempoRestante.ToString(), proceso.Prioridad.ToString() };
                dataGridView.Rows.Add(row);
            }
        }

        //----------------Process IO Execution Method
        public void ioExecution(Proceso[] procesos, int id, int interupt)
        {
            //Change Process State to Waiting when it goes to IO
            foreach (var proceso in procesos)
            {
                if (proceso.Id == id && proceso.Estado != "COMPLETED")
                {
                    proceso.Estado = "WAITING";
                }
            }
            updateDataGridView(dataGridView, procesos);

            //Execute the IO for 1 second
            executionTimer(1);

            //Change Process back to Ready State after IO has completed
            foreach (var proceso in procesos)
            {
                if (proceso.Id == id && proceso.Estado != "COMPLETED")
                {
                    proceso.Estado = "READY";
                }
            }
            updateDataGridView(dataGridView, procesos);
        }

        //----------------Process Execution Timer Method
        public void executionTimer(int tempTime)
        {
            int executionTime = tempTime * 500;
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            if (executionTime == 0 || executionTime < 0)
            {
                return;
            }
            timer1.Interval = executionTime;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };
            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

    }
}