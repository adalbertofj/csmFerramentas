using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsmFerramentas
{
    public partial class FrmEstornarNotaFiscal : Form

    {
        
        public FrmEstornarNotaFiscal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            EstornarNotaFiscal est = new EstornarNotaFiscal(txtNotaFiscal.Text);
            MessageBox.Show("Executado com sucesso!");
        }



    }
}
