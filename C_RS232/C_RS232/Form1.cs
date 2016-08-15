using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace C_RS232
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //ex : 點了視窗上方OPEN選項...將使用者選擇的RS232設定填入
        {
            bool life = true;
		    if(comboBox1.Text == "None")
				serialPort1.Parity = Parity.None;
			else if(comboBox1.Text == "Odd")
				serialPort1.Parity = Parity.Odd;
			else if(comboBox1.Text == "Even")
				serialPort1.Parity = Parity.Even;
			else if(comboBox1.Text == "Mark")
				serialPort1.Parity = Parity.Mark;
			else if(comboBox1.Text == "Space")
				serialPort1.Parity = Parity.Space;
			else
			{
				MessageBox.Show("請重新選擇Parity");
				life = false;
			}

			if(life)
			{
				if(comboBox2.Text == "One")
					serialPort1.StopBits = StopBits.One;
				else if(comboBox2.Text == "OnePointFive")
					serialPort1.StopBits = StopBits.OnePointFive;
				else if(comboBox2.Text == "Two")
					serialPort1.StopBits = StopBits.Two;
				else
				{
					MessageBox.Show("請重新選擇StopBits");
					life = false;
				}
			}

			if(life)
			{
				serialPort1.PortName = textBox1.Text;
				serialPort1.BaudRate = Convert.ToInt32(textBox4.Text);
				serialPort1.DataBits = Convert.ToInt32(textBox3.Text);
				try
				{
					serialPort1.Open();
					button3.Enabled = true;
					button1.Enabled = false;
				}
                catch (Exception pError)
				{
                    MessageBox.Show(pError.Message.ToString());
				}
			}
        }

        private void button3_Click(object sender, EventArgs e)  //ex : 第二個RS232連線
        {
            serialPort2.PortName = textBox2.Text;
            serialPort2.BaudRate = serialPort1.BaudRate;
            serialPort2.Parity = serialPort1.Parity;
            serialPort2.DataBits = serialPort1.DataBits;
            serialPort2.StopBits = serialPort1.StopBits;
            serialPort2.Open();
            button3.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)  //ex : 清除第一個RS232的接收資料區
        {
            listBox1.Items.Clear();
            listBox3.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)  //ex : 清除第二個RS232的接收資料區
        {
            listBox2.Items.Clear();
            listBox4.Items.Clear();
        }

        delegate void SetTextCallback(string text , ListBox list_box);  //ex : 跨執行序
        private void tt(string s , ListBox list_box)                    //ex : 要做跨執行序的內容
        {
            list_box.Items.Add(s);
        }


        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)     //ex : 接收第一個RS232的資料
        {
             String input = serialPort1.ReadExisting();
			 serialPort2.Write(input);
             this.Invoke(new SetTextCallback(tt) , input , listBox3);

             String ss = "";
             int ii, j = 0;
             for (int i = 0; i < input.Length; i++)
             {
                 ii = input[i];
                 if (ii < 16)
                 {
                     ss = ss + "0";
                 }

                 String KK = String.Format("{0:X}", ii);    //ex : 將收到的ASCII轉成16進位
                 ss = ss + KK + " ";

                 if (++j == 50)                         //ex : 當行資料若是超過50個字元則換行
                 {
                     j = 0;
                     this.Invoke(new SetTextCallback(tt), ss, listBox1);
                     ss = "";
                 }
             }
             this.Invoke(new SetTextCallback(tt), ss, listBox1);
        }

        private void serialPort2_DataReceived(object sender, SerialDataReceivedEventArgs e) //ex : 接收第二個RS232的資料
        {
            String input = serialPort2.ReadExisting();
            serialPort1.Write(input);
            this.Invoke(new SetTextCallback(tt), input, listBox4);

            String ss = "";
            int ii, j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                ii = input[i];
                if (ii < 16)
                {
                    ss = ss + "0";
                }

                String KK = String.Format("{0:X}", ii);     //ex : 將收到的ASCII轉成16進位
                ss = ss + KK + " ";

                j++;
                if (j == 50)                                //ex : 當行資料若是超過50個字元則換行
                {
                    j = 0;
                    this.Invoke(new SetTextCallback(tt), ss, listBox2);
                    ss = "";
                }
            }
            this.Invoke(new SetTextCallback(tt), ss, listBox2);
        }

        private void button5_Click(object sender, EventArgs e)  //ex : 將接收的資料儲存至L1.txt
        {
            StreamWriter save_text = new StreamWriter("L1.txt");
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                save_text.WriteLine(listBox1.Items[i].ToString());
            }
            save_text.Close();

        }

        private void button6_Click(object sender, EventArgs e)  //ex : 將接收的資料儲存至L3.txt
        {
            StreamWriter save_text = new StreamWriter("L3.txt");
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                save_text.WriteLine(listBox3.Items[i].ToString());
            }
            save_text.Close();
        }

        private void button7_Click(object sender, EventArgs e)  //ex : 將接收的資料儲存至L2.txt
        {
            StreamWriter save_text = new StreamWriter("L2.txt");
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                save_text.WriteLine(listBox2.Items[i].ToString());
            }
            save_text.Close();
        }

        private void button8_Click(object sender, EventArgs e)  //ex : 將接收的資料儲存至L4.txt
        {
            StreamWriter save_text = new StreamWriter("L4.txt");
            for (int i = 0; i < listBox4.Items.Count; i++)
            {
                save_text.WriteLine(listBox4.Items[i].ToString());
            }
            save_text.Close();
        }
    }
}