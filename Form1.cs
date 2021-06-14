﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace PingPlotter
{

    public partial class Form1 : Form
    {
        System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public Timer sw = new Timer();
        public float elapsed = 0.0f;
        public bool download = false;
        public int counter;
        public long cumulativeRTT;
        public long averageRTT;

        System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series
        {
            Name = "Ping",
            Color = System.Drawing.Color.Green,
            IsVisibleInLegend = false,
            IsXValueIndexed = true,
            ChartType = SeriesChartType.Line
        };

        System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series
        {
            Name = "Download Speed",
            Color = System.Drawing.Color.Blue,
            IsVisibleInLegend = false,
            IsXValueIndexed = true,
            ChartType = SeriesChartType.Line
        };


        public Form1()
        {
            InitializeComponent();
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            SuspendLayout();

            // chart1
            chartArea1.Name = "chart1";
            chart1.ChartAreas.Add(chartArea1);
            chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new System.Drawing.Point(0, 50);
            chart1.Name = "chart1";
            //this.chart1.Size = new System.Drawing.Size(284, 212);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart1.Titles.Add("Ping (ms) / Time (s)");

            // Form1
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 600);
            Controls.Add(this.chart1);
            Name = "LineGraph";
            Text = "Ping Plotter";
            Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ResumeLayout(false);

            //Button
            button1.Text = "Download Speed";

            sw.Tick += sw_Tick;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            

            chart1.Series.Add(series1);            
           
            sw.Start();
            chart1.Invalidate();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void sw_Tick(object sender, EventArgs e)
        {
            elapsed += sw.Interval;          

            if (!download)
            {
                Ping pingSender = new Ping();
                PingReply pingReceiver = pingSender.Send("8.8.8.8");
                chart1.Series[0].Points.AddXY((elapsed / 1000) - 0.1, pingReceiver.RoundtripTime);

                cumulativeRTT += pingReceiver.RoundtripTime;
                counter++;
                averageRTT = cumulativeRTT / counter;
                label1.Text = "Average ping: " + averageRTT.ToString() + "ms";
            }
            else
            {
                chart1.Series[0].Points.AddXY((elapsed / 1000) - 0.1, CheckInternetSpeed());
            }

        }

        public double CheckInternetSpeed()
        {
            // Create Object Of WebClient
            System.Net.WebClient wc = new System.Net.WebClient();

            //DateTime Variable To Store Download Start Time.
            DateTime dt1 = DateTime.Now;

            //Number Of Bytes Downloaded Are Stored In ‘data’
            byte[] data = wc.DownloadData("http://google.com");

            //DateTime Variable To Store Download End Time.
            DateTime dt2 = DateTime.Now;

            //To Calculate Speed in Kb Divide Value Of data by 1024 And Then by End Time Subtract Start Time To Know Download Per Second.
            return Math.Round((data.Length / 1024) / (dt2 - dt1).TotalSeconds, 2);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!download)
            {
                chart1.Series.Clear();
                chart1.Series.Add(series2);
                elapsed = 0;
                download = true;
                button1.Text = "Ping";
                button1.Update();
                chart1.Titles.Clear();
                chart1.Titles.Add("Download Speed (kb/s) / Time (s)");
            }
            else
            {
                chart1.Series.Clear();
                chart1.Series.Add(series1);
                elapsed = 0;
                download = false;
                button1.Text = "Download Speed";
                button1.Update();
                chart1.Titles.Clear();
                chart1.Titles.Add(" Ping (ms) / Time (s)");
            }
        }
    }
}
