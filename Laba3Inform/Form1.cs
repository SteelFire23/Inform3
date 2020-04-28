using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Laba3Inform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
           
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog file = new FolderBrowserDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                treeView1.Nodes.Clear();                
                chart1.Series.Clear();
                chart1.Series.Add("first");
                treeView1.BeforeSelect += treeView1_BeforeSelect;
                treeView1.BeforeExpand += treeView1_BeforeExpand;
                FillDriveNodes();
                if (file.SelectedPath != @"D:\" && file.SelectedPath != @"C:\" && file.SelectedPath != @"E:\")
                {
                    file.SelectedPath = file.SelectedPath.Insert(2, @"\");
                    Find(treeView1.Nodes, file.SelectedPath);
                }
                else
                {
                    Find(treeView1.Nodes, file.SelectedPath);
                }
                void Find(TreeNodeCollection Nodes, String str)
                {
                    foreach (TreeNode i in Nodes)
                    {                        
                        if (i.FullPath == str)
                        {
                            treeView1.SelectedNode = i;
                            return;
                        }
                        Find(i.Nodes, str);
                    }                    
                }
                DirectoryInfo Dir = new DirectoryInfo(file.SelectedPath);
                MainPoint(ref treeView1, ref dataGridView1, ref chart1, Dir, ref statusStrip1);              
            }            
        }
        public static void MainPoint(ref TreeView treeView1, ref DataGridView dataGridView1, ref Chart chart1, DirectoryInfo Dir, ref StatusStrip statusStrip1)
        {
            dataGridView1.Rows.Clear();
            FileInfo[] files = Dir.GetFiles();
            if (files.Length != 0)
            {
                uint size = 0;
                int total, now;
                dataGridView1.Rows.Add(files.Length-1);
                for (int i = 0; i < files.Length; i++)
                {                    
                    if ( i >= dataGridView1.Rows.Count) break;                    
                    dataGridView1[1, i].Value = files[i].Name;
                    dataGridView1[2, i].Value = files[i].Length / 1024;
                    dataGridView1[3, i].Value = Type(files[i].Name);
                    dataGridView1[0, i].Value = true;
                    size += uint.Parse(dataGridView1[2, i].Value.ToString());
                }
                total = dataGridView1.Rows.Count;
                now = total;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        if (ColorTest(dataGridView1[3, i].Value.ToString()) == 1)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        }
                        if (ColorTest(dataGridView1[3, i].Value.ToString()) == 2)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightSalmon;
                        }
                        if (ColorTest(dataGridView1[3, i].Value.ToString()) == 3)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                        }
                        if (ColorTest(dataGridView1[3, i].Value.ToString()) == 4)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        }
                        if (ColorTest(dataGridView1[3, i].Value.ToString()) == 5)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Cyan;
                        }
                    }
                    catch(Exception exep) { }
                }
                int img = 0, doc = 0, arh = 0, ex = 0, dl = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    Numbers(ref img, ref doc, ref arh, ref ex, ref dl, dataGridView1[3, i].Value.ToString());
                }
                FillChart(ref img, ref doc, ref arh, ref ex, ref dl, ref chart1);
                statusStrip1.Items[0].Text = "Total: " + size + " ";
                statusStrip1.Items[1].Text = now + " of " + total + " items selected";
            }
        }
        public static void FillChart(ref int img, ref int doc, ref int arh, ref int ex, ref int dl,ref Chart chart1)
        {
            chart1.Series.Clear();
            chart1.Series.Add("first");
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 25;
            chart1.Series[0].Points.AddXY("Изображения", img);
            chart1.Series[0].Points.AddXY("Документы", doc);
            chart1.Series[0].Points.AddXY("Архивы", arh);
            chart1.Series[0].Points.AddXY("Установщики", ex);
            chart1.Series[0].Points.AddXY("Библиотеки", dl);
        }
        public static void Numbers(ref int img,ref int doc,ref int arh,ref int ex,ref int dl,string S)
        {
            string[] gf = new string[] { "png", "jpg", "bmp", "gif" };
            string[] Doc = new string[] { "docx", "xlsx", "pdf", "txt" };
            string[] dat = new string[] { "zip", "rar", "7z" };         
                if (S == "exe") ++ex;
                if (S == "dll") ++dl;
            for (int i = 0; i < Doc.Length; i++)
            {
                if (gf[i] == S) ++img;
                if (Doc[i] == S) ++doc;
                if (i < dat.Length)
                {
                    if (dat[i] == S) ++arh;
                }
            }  
        }
        public static int ColorTest(string S)
        {
            string[] gf = new string[] { "png", "jpg", "bmp", "gif" };
            string[] doc = new string[] { "docx", "xlsx", "pdf", "txt" };
            string[] dat = new string[] { "zip", "rar", "7z" };
            if (S == "exe") return 4;
            if (S == "dll") return 5;
            for(int i = 0; i < gf.Length; i++)
            {
                if (S == gf[i])
                {
                    return 1;
                }
                else if (S == doc[i])
                {
                    return 2;
                }
                if (i < dat.Length-1)
                {
                    if (S == dat[i])
                    {
                        return 3;
                    }
                }
            }
            return -1;
        }
        public static string Type(string S)
        {
            char[] A = S.ToCharArray();
            string tmp = "";
            for (int i = A.Length-1; i >=0; i--)
            {
                if (A[i] == '.')
                {                    
                    return Rev(tmp);
                }
                tmp += A[i];                                    
            }
            return null;              
        }
        public static String Rev(string S)
        {
            char[] A = S.ToCharArray();
            Array.Reverse(A);
            string tmp = "";
            for (int i = 0; i < A.Length; i++)
            {
                tmp += A[i];
            }
            return tmp;
        }
        private void FillDriveNodes()
        {
            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    TreeNode driveNode = new TreeNode { Text = drive.Name };
                    FillTreeNode(driveNode, drive.Name);
                    treeView1.Nodes.Add(driveNode);
                }
            }
            catch (Exception ex) { }
        }
        private void FillTreeNode(TreeNode driveNode, string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    TreeNode dirNode = new TreeNode();
                    dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                    driveNode.Nodes.Add(dirNode);
                }
            }
            catch (Exception ex) { }
        }
        void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length != 0)
                    {
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length != 0)
                    {
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Текстовые файлы (*.txt)|*.txt";
            file.FileName = "InformData";
            if (file.ShowDialog() == DialogResult.OK)
            {                
                if (!File.Exists(file.FileName))
                {
                    Save(file.FileName, dataGridView1);
                }
                else
                {
                    File.Delete(file.FileName);
                    Save(file.FileName, dataGridView1);
                }                
            }
        }
        public static void Save(string path,DataGridView dataGridView1)
        {
            StreamWriter first = new StreamWriter(File.Open(path, FileMode.OpenOrCreate));
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                first.Write(dataGridView1[1, i].Value.ToString() + " ");
                first.Write(dataGridView1[2, i].Value.ToString() + " ");
                first.Write(dataGridView1[3, i].Value.ToString()+" | ");
            }
            first.Close();
        }

        private void colorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            int i = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows[i].DefaultCellStyle.BackColor = colorDialog1.Color;
        }

        private void fontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            dataGridView1.DefaultCellStyle.Font = fontDialog1.Font;
            treeView1.Font = fontDialog1.Font;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DirectoryInfo Dir = new DirectoryInfo(treeView1.SelectedNode.FullPath);
            FileInfo[] files = Dir.GetFiles();
            if (files.Length != 0)
            {
                dataGridView1.Rows.Clear();
                MainPoint(ref treeView1, ref dataGridView1, ref chart1, Dir, ref statusStrip1);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int img = 0, doc = 0, arh = 0, ex = 0, dl = 0, total = dataGridView1.Rows.Count, now = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                now++;
                Numbers(ref img, ref doc, ref arh, ref ex, ref dl, dataGridView1[3, i].Value.ToString());
            }
            statusStrip1.Items[1].Text = now + " of " + total + " selected";
            FillChart(ref img, ref doc, ref arh, ref ex, ref dl, ref chart1);
            img = 0; doc = 0; arh = 0; ex = 0; dl = 0;now = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1[0, i].Value))
                {
                    now++;
                    Numbers(ref img, ref doc, ref arh, ref ex, ref dl, dataGridView1[3, i].Value.ToString());
                }
            }
            statusStrip1.Items[1].Text = now + " of " + total + " selected";
            FillChart(ref img, ref doc, ref arh, ref ex, ref dl, ref chart1);
        }
    }
}
