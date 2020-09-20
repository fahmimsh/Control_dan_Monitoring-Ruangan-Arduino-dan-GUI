using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;


namespace GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label26.Text = folderDlg.SelectedPath;
            chart1.Name = ("DATA TEMPEREATURE");
            chart1.Series.Add("1");
            chart1.Series[0].Name = ("Temperature 1");
            chart1.Series[1].Name = ("Temperature 2");
            chart1.Series.Add("1");
            chart1.Series[2].Name = ("Temperature 3");
            chart1.Series.Add("1");
            chart1.Series[3].Name = ("Temperature 4");
            chart1.Series[0].BorderWidth = 3;
            chart1.Series[1].BorderWidth = 3;
            chart1.Series[2].BorderWidth = 3;
            chart1.Series[3].BorderWidth = 3;
            chart1.Series[0].Color = Color.Gray;
            chart1.Series[1].Color = Color.DarkBlue;
            chart1.Series[2].Color = Color.DarkOrange;
            chart1.Series[3].Color = Color.Green;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[3].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.ChartAreas[0].AxisY.Title = ("Suhu (℃)");
            chart1.ChartAreas[0].AxisX.Title = ("Incoming Data (n)");
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
        }
        public string code;
        private void Form1_Load(object sender, EventArgs e)
        {
            String[] portList = System.IO.Ports.SerialPort.GetPortNames();
            foreach (String portName in portList)
                comboBox1.Items.Add(portName);

            comboBox1.Text = comboBox1.Items[comboBox1.Items.Count - 1].ToString();

            String[] baudrate = { "9600", "14400", "19200", "38400", "56000", "57600", "76800", "115200" };
            foreach (String baudrates in baudrate)
                comboBox2.Items.Add(baudrates);
        }
        //connect serial
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Int32.Parse(comboBox2.Text);
                serialPort1.NewLine = ("\r\n");
                serialPort1.Open();
                toolStripStatusLabel1.Text = serialPort1.PortName + (" is connected.");
            }
               
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ("ERROR: ") + ex.Message.ToString();
            }
        }
        //close serial
        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            toolStripStatusLabel1.Text = serialPort1.PortName + (" is Disconnected.");
        }
        //SET RELAY
        private void button3_Click(object sender, EventArgs e)
        {
            code = ("relay");
            try
            {
                serialPort1.Write("@,0,1,C,SR,1,*");
                listBox2.Items.Add("@,0,1,C,SR,1,*");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
                textWriter.WriteLine(listBox2.Text);
                textWriter.Close();
            }
            catch (Exception) { }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            code = ("relay");
            try
            {
                serialPort1.Write("@,0,1,C,SR,2,*");
                listBox2.Items.Add("@,0,1,C,SR,2,*");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
                textWriter.WriteLine(listBox2.Text);
                textWriter.Close();
            }
            catch (Exception) { }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            code = ("relay");
            try
            {
                serialPort1.Write("@,0,2,C,SR,1,*");
                listBox2.Items.Add("@,0,2,C,SR,1,*");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
                textWriter.WriteLine(listBox2.Text);
                textWriter.Close();
            }
            catch (Exception) { }   
        }
        private void button6_Click(object sender, EventArgs e)
        {
            code = ("relay");
            try
            {
                serialPort1.Write("@,0,2,C,SR,2,*");
                listBox2.Items.Add("@,0,2,C,SR,2,*");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
                textWriter.WriteLine(listBox2.Text);
                textWriter.Close();
            }
            catch (Exception) { }
        }
        //CLEAR RELAY
        private void button11_Click(object sender, EventArgs e)
        {
            code = ("relay");
            try
            {
                serialPort1.Write("@,0,1,C,CR,1,*");
                listBox2.Items.Add("@,0,1,C,CR,1,*");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
                textWriter.WriteLine(listBox2.Text);
                textWriter.Close();
            }
            catch (Exception) { }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            code = ("relay");
            try
            {
                serialPort1.Write("@,0,1,C,CR,2,*");
                listBox2.Items.Add("@,0,1,C,CR,2,*");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
                textWriter.WriteLine(listBox2.Text);
                textWriter.Close();
            }
            catch (Exception) { }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            code = ("relay");
            try
            {
                serialPort1.Write("@,0,2,C,CR,1,*");
                listBox2.Items.Add("@,0,2,C,CR,1,*");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
                textWriter.WriteLine(listBox2.Text);
                textWriter.Close();
            }
            catch (Exception) { }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            code = ("relay");
            try
            {
                serialPort1.Write("@,0,2,C,CR,2,*");
                listBox2.Items.Add("@,0,2,C,CR,2,*");
                listBox2.SelectedIndex = listBox2.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
                textWriter.WriteLine(listBox2.Text);
                textWriter.Close();
            }
            catch (Exception) { }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            code = ("temp1");
            serialPort1.Write("@,0,1,C,GT,S,*");
            listBox2.Items.Add("@,0,1,C,GT,S,*");
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
            TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
            textWriter.WriteLine(listBox2.Text);
            textWriter.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            code = ("temp2");
            serialPort1.Write("@,0,2,C,GT,S,*");
            listBox2.Items.Add("@,0,2,C,GT,S,*");
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
            TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
            textWriter.WriteLine(listBox2.Text);
            textWriter.Close();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            code = ("alldata1");
            serialPort1.Write("@,0,1,C,GA,A,*");
            listBox2.Items.Add("@,0,1,C,GA,A,*");
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
            TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
            textWriter.WriteLine(listBox2.Text);
            textWriter.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            code = ("alldata2");
            serialPort1.Write("@,0,2,C,GA,A,*");
            listBox2.Items.Add("@,0,1,C,GA,A,*");
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
            TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Output.txt", true);
            textWriter.WriteLine(listBox2.Text);
            textWriter.Close();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }

        public String receiveMsg;
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                receiveMsg = serialPort1.ReadLine();
                //INSIKASI SLAVE
                if (receiveMsg == "@,Dev,1,*")
                {
                    Dev1.BackColor = Color.Green;
                }
                if (receiveMsg == "@,Dev,2,*")
                {
                    Dev2.BackColor = Color.Green;
                }
                /* DATA RELAY */
                if (code == ("relay"))
                {
                    if (receiveMsg == "@,1,0,R,RR,1,1,*")
                    {
                        R1.BackColor = Color.Yellow;
                    }
                    else if (receiveMsg == "@,1,0,R,RR,1,0,*")
                    {
                        R1.BackColor = Color.Red;
                    }
                    else if (receiveMsg == "@,1,0,R,RR,2,1,*")
                    {
                        R2.BackColor = Color.Yellow;
                    }
                    else if (receiveMsg == "@,1,0,R,RR,2,0,*")
                    {
                        R2.BackColor = Color.Red;
                    }
                    else if (receiveMsg == "@,2,0,R,RR,1,1,*")
                    {
                        R3.BackColor = Color.Yellow;
                    }
                    else if (receiveMsg == "@,2,0,R,RR,1,0,*")
                    {
                        R3.BackColor = Color.Red;
                    }
                    else if (receiveMsg == "@,2,0,R,RR,2,1,*")
                    {
                        R4.BackColor = Color.Yellow;
                    }
                    else if (receiveMsg == "@,2,0,R,RR,2,0,*")
                    {
                        R4.BackColor = Color.Red;
                    }
                }
                Tampilkan(receiveMsg);
            }
            catch { }
        }

        private delegate void TampilkanDelegate(object item);

        private void Tampilkan(object item)
        {
            if (InvokeRequired)
                listBox1.Invoke(new TampilkanDelegate(Tampilkan), item);
            else
            {
                listBox1.Items.Add(item);

                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\Data_Input.txt", true);
                textWriter.WriteLine(listBox1.Text);
                textWriter.Close();

                splitData(item);
            }
        }

        private void splitData(object item)
        {
            String[] data = item.ToString().Split(',');
            if (data[0] == ("@") && data[1] == ("1") && data[2] == ("0") && data[3] == ("A") && data[4] == ("PB"))
            {
                Dev1.BackColor = Color.Blue;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\DataPanicButton.txt", true);
                textWriter.WriteLine(listBox1.Text);
                textWriter.Close();
            }
            if (data[0] == ("@") && data[1] == ("2") && data[2] == ("0") && data[3] == ("A") && data[4] == ("PB"))
            {
                Dev2.BackColor = Color.Blue;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                TextWriter textWriter = new StreamWriter(label26.Text + "\\DataPanicButton.txt", true);
                textWriter.WriteLine(listBox1.Text);
                textWriter.Close();
            }
            if (code == ("temp1"))
            {
                textBox1.Text = data[5];
                textBox3.Text = data[6];
                chart1.Series[0].Points.AddY(data[5]);
                chart1.Series[1].Points.AddY(data[6]);
            }
            if(code == ("temp2"))
            {
                textBox2.Text = data[5];
                textBox4.Text = data[6];
                chart1.Series[2].Points.AddY(data[5]);
                chart1.Series[3].Points.AddY(data[6]);
            }
            if(code == ("alldata1"))
            {
                textBox6.Text = data[5];
                textBox5.Text = data[6];
                textBox7.Text = data[7];
                textBox8.Text = data[8];
                chart1.Series[0].Points.AddY(data[5]);
                chart1.Series[1].Points.AddY(data[6]);
            }
            if(code == ("alldata2"))
            {
                textBox12.Text = data[5];
                textBox11.Text = data[6];
                textBox10.Text = data[7];
                textBox9.Text = data[8];
                chart1.Series[2].Points.AddY(data[5]);
                chart1.Series[3].Points.AddY(data[6]);
            }
            if (chart1.Series[0].Points.Count > 50)
                chart1.Series[0].Points.RemoveAt(0);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            folderDlg.ShowNewFolderButton = true;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
                label26.Text = folderDlg.SelectedPath;
        }
        private FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        private string pilihFile = "";
        private void button17_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pilihFile = openFileDialog1.FileName;
                label28.Text = pilihFile;
            }
        }
        private void button18_Click(object sender, EventArgs e)
        {
            string baris;
            int counter = 0;
            listBox3.Items.Clear();
            TextReader txt = new StreamReader(pilihFile);
            while ((baris = txt.ReadLine()) != null)
            {
                listBox3.Items.Add(baris);
                counter++;
                serialPort1.WriteLine(baris);
            }
            toolStripStatusLabel1.Text = "Sending " + counter.ToString() + "line(s)";
            txt.Close();
        }
    }
}
