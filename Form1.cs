using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicWand
{
    public partial class Form1 : Form
    {
        const int AmountOfForms = 20;
        public List<Form2> Forms { get; set; }
        private System.Windows.Forms.Timer _timerInitialNewForm;


        public Form1()
        {
            InitializeComponent();
            Forms = new List<Form2>();

            _timerInitialNewForm = new System.Windows.Forms.Timer();
            _timerInitialNewForm.Interval = 100;
            _timerInitialNewForm.Tick += new EventHandler(InitialChildren);
            _timerInitialNewForm.Start();
        }
        void InitialChildren(Object myObject, EventArgs myEventArgs)
        {
            Form2 tmpForm = new Form2(this);
            tmpForm.Show();
            Forms.Add(tmpForm);
            if (Forms.Count == AmountOfForms)
            {
                _timerInitialNewForm.Stop();
                this.Focus();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Forms.Count > 1)
            {
                Forms[Forms.Count - 1].Close();
                Forms.RemoveAt(Forms.Count - 1);
                e.Cancel = true;
            }
        }
    }
}
