using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calcolo_hash
{
    public partial class Form1 : Form
	{
		private static readonly SHA256 Sha256 = SHA256.Create(); // Istanzio gli oggetti SHA256 e MD5
		private static readonly MD5 Md5 = MD5.Create();
		private static string output = string.Empty;
			 
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			OpenFileDialog selez_file = new OpenFileDialog();

			if (selez_file.ShowDialog() == DialogResult.OK)
			{
				textBox2.Text = selez_file.FileName; // Imposto il solo nome del file selezionato alla prima textbox
				textBox1.Text = selez_file.SafeFileName; // imposto tutto il percorso!
			}
		}

		private void label2_Click(object sender, EventArgs e)
		{ }


		/// <summary>
		/// Esegui in maniera asincrona il calcolo del Hash SHA256
		/// </summary>
		/// <param name="pathCompletaFile"></param>
		/// <returns></returns>
		private static async Task CalcolaHashSHA256(string pathCompletaFile)
		{
			await Task.Run(() =>
			{
				byte[] arrayByte = null;
				string result = string.Empty;
				using (FileStream stream = File.OpenRead(pathCompletaFile))
				{
					arrayByte = Sha256.ComputeHash(stream);
				}
				foreach (byte b in arrayByte)
				{
					result += b.ToString("x2");
				}
				output = result;
			});
		}


		/// <summary>
		/// Esegui in maniera asincrona il calcolo del HASH MD5
		/// </summary>
		/// <param name="pathCompletaFile"></param>
		/// <returns></returns>
		public static async Task CalcolaHashMD5(string pathCompletaFile)
		{
			await Task.Run(() =>
			{
				byte[] arrayByte = null; 
				string result = string.Empty;
				using (FileStream stream = File.OpenRead(pathCompletaFile))
				{
					arrayByte = Md5.ComputeHash(stream);
				}	
				foreach (byte b in arrayByte)
				{
					result += b.ToString("x2");
				}
				output = result;
			});
		}

		private async void button2_Click(object sender, EventArgs e)
		{
			if (textBox2.Text.ToLower() == "")
			{
				System.Windows.Forms.MessageBox.Show("E' necessario selezionare un file!", "ATTENZIONE", MessageBoxButtons.OK);
			}
			else
			{
				if (File.Exists(textBox2.Text)) // Eseguo un controllo sulla validità del file scelto in input!
				{
					Task primoTask = CalcolaHashMD5(textBox2.Text);
					await primoTask;
					textBox3.Text = output;
					Task secondoTask = CalcolaHashSHA256(textBox2.Text);
					await secondoTask;
					textBox4.Text = output;
					SystemSounds.Hand.Play();
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("Il file selezionato non è valido!", "ERRORE", MessageBoxButtons.OK);
				}
			}
		}

        private void buttonInfo_Click(object sender, EventArgs e)
        {
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fvi.FileVersion;

			MessageBox.Show($"Versione applicativo: {version}", "Info versione", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
    }
}
