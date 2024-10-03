using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;

namespace Etiquetas
{
    public partial class Form1 : Form
    {
        Paciente paciente;
        String error;
        private const int DesiredLength = 12; // Desired length of the text
        int status;

        public Form1()
        {
            InitializeComponent();
            status = 0;
            error = "";
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom2", 197, 118);
            printDialog1.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom2", 197, 118);
            this.txbRh.Focus();
            txbRh.TabIndex = 0;

            txbVolume.TabIndex = 1;
            txbLote.TabIndex = 2;
            txbFabricacao.TabIndex = 3;
            txbValidade.TabIndex = 4;
            btImprimir.TabIndex = 5;
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            btImprimir.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            //btImprimir.Enabled = true;

        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btImprimir.Enabled = true;
            if (status == 1)
                lblError.Text = error;
            else
                lblError.ResetText();
            this.txbRh.ResetText();
            this.txbRh.Enabled = true;
            this.txbRh.Focus();


            ClearAllTextBoxes(this);

            ClearAllMaskedTextBoxes(this);

        }
        public class Paciente
        {

            public string nm_nome { get; set; }



        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {




            try
            {




                PrintDialog printDialog1 = new PrintDialog();
                printDialog1.Document = printDocument1;
                DialogResult result = printDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {

                    printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom2", 197, 118);
                    printDialog1.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom2", 197, 118);
                    printDocument1.Print();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                status = 1;

            }
        }



        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            DateTime data = DateTime.Now;
            string rh = txbRh.Text;
            //detiq = dados.getDados(be);
            string text = txbVolume.Text;
            if (text.Length < DesiredLength)
            {
                text = text.PadRight(DesiredLength); // Pad the text with spaces
            }
            else if (text.Length > DesiredLength)
            {
                text = text.Substring(0, DesiredLength); // Truncate the text to the desired length
            }
            string url = "http://10.48.21.64:5003/hspmsgh-api/pacientes/paciente/" + rh;
            WebRequest request = WebRequest.Create(url);
            try
            {
                using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                    {
                        JsonSerializer json = new JsonSerializer();
                        var objText = reader.ReadToEnd();
                        paciente = JsonConvert.DeserializeObject<Paciente>(objText);

                    }
                }

                string[] nameParts = paciente.nm_nome.Split(' ');
                for (int i = 1; i < nameParts.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(nameParts[i]))
                    {
                        nameParts[i] = nameParts[i][0] + ".";
                    }
                }
                // Join the parts back together
                string abbreviatedName = string.Join(" ", nameParts);

                e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom2", 197, 118);//900 é a largura da página
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom2", 197, 118);
                printDialog1.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom2", 197, 118);
                using (Graphics g = e.Graphics)
                {
                    using (Font fnt = new Font("Arial", 12))
                    {

                        int startXEsquerda = 2;
                        int starty = 2;//distancia das linhas
                        // Create two images.

                        Image image1 = Image.FromFile("./Files/HSPM_LOGO.jpg");
                        var newImage = ResizeImage(image1, 35, 20);
                        // Get a PropertyItem from image1.
                        //PropertyItem propItem = image1.GetPropertyItem(20624);

                        // Change the ID of the PropertyItem.
                        //propItem.Id = 20625;

                        // Set the PropertyItem for image2.
                        //image1.SetPropertyItem(propItem);
                        // RectangleF srcRect = new RectangleF(100.0F, 100.0F, 100.0F, 100.0F);
                        // GraphicsUnit units = GraphicsUnit.Millimeter;
                        // Draw the image.
                        e.Graphics.DrawImage(newImage, 5.0F, 0.0F);



                        // Create image.
                        // Image newImage = Image.FromFile("C://Users/h013026/Downloads/HSPM_LOGO.jpg");

                        // Create coordinates for upper-left corner of image.
                        // float x = 0.0F;
                        /// float y = 0.0F;

                        // Create rectangle for source image.
                        // RectangleF srcRect = new RectangleF(100.0F, 100.0F, 100.0F, 100.0F);
                        // GraphicsUnit units = GraphicsUnit.Pixel;
                        //         
                        // Draw image to screen.
                        //e.Graphics.DrawImage(newImage, x, y, srcRect, units);


                        //g.DrawImage(image1, new Rectangle(10, 10, 200, 200));




                        //newImage.Save("c:\\newImage.png", ImageFormat.Png);

                        Pen blackPen = new Pen(Color.Black, 1);

                        // Create location and size of rectangle.
                        int x = 4;
                        int y = starty + 40;
                        int width = 7;
                        int height = 7;

                        // Draw rectangle to screen.
                        //e.Graphics.DrawRectangle(blackPen, x, y, width, height);


                        g.DrawString("Clínica de Alergia", new Font("Sans Serif", 8, FontStyle.Bold), System.Drawing.Brushes.Black, startXEsquerda + 45, starty);
                        g.DrawString("Paciente: " + abbreviatedName, new Font("Sans Serif", 7, FontStyle.Bold), System.Drawing.Brushes.Black, startXEsquerda, starty + 24);


                        g.DrawString("Candida + Corynebacterium Paruum", new Font("Sans Serif", 6, FontStyle.Bold), System.Drawing.Brushes.Black, startXEsquerda, starty + 40);


                        x = 4;
                        y = starty + 56;
                        width = 7;
                        height = 7;

                        // Draw rectangle to screen.
                        // e.Graphics.DrawRectangle(blackPen, x, y, width, height);
                        // g.DrawString("Insetos            " + "Diluição:........................", new Font("Sans Serif", 6, FontStyle.Bold), System.Drawing.Brushes.Black, startXEsquerda + 10, starty + 56);
                        g.DrawString("Volume: " + text + "Lote: " + txbLote.Text, new Font("Sans Serif", 6, FontStyle.Bold), System.Drawing.Brushes.Black, startXEsquerda, starty + 72);
                        g.DrawString("Fab.: " + txbFabricacao.Text + "    Val.: " + txbValidade.Text, new Font("Sans Serif", 7, FontStyle.Bold), System.Drawing.Brushes.Black, startXEsquerda, starty + 98);

                        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                        drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                        g.DrawString("Conservar 2º C - 8º C ", new Font("Sans Serif", 7, FontStyle.Bold), System.Drawing.Brushes.Black, 175, 5, drawFormat);



                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                status = 1;

            }
        }
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }




        private void ClearAllTextBoxes(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = string.Empty;
                }
                else
                {
                    ClearAllTextBoxes(c); // Recursively call for nested controls
                }

            }
        }
        private void ClearAllMaskedTextBoxes(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is MaskedTextBox)
                {
                    ((MaskedTextBox)c).Text = string.Empty;
                }
                else
                {
                    ClearAllMaskedTextBoxes(c); // Recursively call for nested controls
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }

}