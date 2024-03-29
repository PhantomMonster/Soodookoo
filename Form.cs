using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FormWithButton
{
    public class Form1 : Form
    {
        private static Grid Grid;
        private Panel buttonPanel = new Panel();
        private DataGridView songsDataGridView = new DataGridView();
        public static DataGridView GameGrid = new DataGridView();
        private Button StepButton = new Button();
        private Button StepCupletsButton = new();
        private Button ChangeViewButton = new Button();
        private string CurrentView = "default";
        public static List<int> ProcessList = new();

        public Form1()
        {
            this.Load += new EventHandler(Form1_Load!);
        }

        private void Form1_Load(System.Object sender, System.EventArgs e)
        {
            SetupLayout();
            SetupGameGrid();
            PopulateGameGrid();
        }

        // private void songsDataGridView_CellFormatting(object sender,
        //     System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        // {
        //     if (e != null)
        //     {
        //         if (this.songsDataGridView.Columns[e.ColumnIndex].Name == "Release Date")
        //         {
        //             if (e.Value != null)
        //             {
        //                 try
        //                 {
        //                     e.Value = DateTime.Parse(e.Value.ToString() ?? "")
        //                         .ToLongDateString();
        //                     e.FormattingApplied = true;
        //                 }
        //                 catch (FormatException)
        //                 {
        //                     Console.WriteLine("{0} is not a valid date.", e.Value.ToString());
        //                 }
        //             }
        //         }
        //     }
        // }


        private void StepButton_Click(object sender, EventArgs e)
        {
            bool ExThrown = true;

            if (ProcessList.Count == 0) {ProcessList = Grid.PrioritiseCells();}

            do
            {
                try
                {
                    //GameGrid.ClearSelection();

                    int CelltoProcess = ProcessList[0];

                    //Grid.CheckAllN();

                    //CelltoProcess = 32;

                    //Cell Cell = Grid.Cells[CelltoProcess];

                    //GameGrid.RowsDefaultCellStyle.BackColor = Color.LightGray;

                    //GameGrid.CellStyle.BackColor = Color.LightGray;

                    //GameGrid.Refresh();

                    Grid.CheckCellNNumberIs(CelltoProcess);

                    Grid.CheckCellNNumberIsNot(CelltoProcess);

                    ProcessList.RemoveAt(0);

                    Grid.Display(GameGrid);

                    ProcessList = Grid.PrioritiseCells(ProcessList);

                    //GameGrid.Rows[Cell.CellRow].Cells[Cell.CellColumn].Style.BackColor = Color.Red;

                    //GameGrid.Refresh();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error with Step: {0}", ex);

                    ExThrown = false;

                    //ProcessList = Grid.PrioritiseDoubleCells();
                }
           } while (ExThrown);



           


        }

        private void StepCupletsButton_Click(object sender, EventArgs e)
        {
            if (ProcessList.Count == 0) {ProcessList = Grid.PrioritiseDoubleCells();}

            bool ExThrown = true;

            do
            {
                try
                {
                    
                    int CelltoProcess = ProcessList[0];

                    Grid.CheckforDoubles(CelltoProcess);

                    ProcessList.RemoveAt(0);

                    Grid.Display(GameGrid);

                    ProcessList = Grid.PrioritiseDoubleCells(ProcessList);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error with Step: {0}", ex);

                    

                    ExThrown = false;


                    //ProcessList = Grid.PrioritiseCells();
                }
            } while (ExThrown);


        }

        private void ChangeViewButton_Click(object sender, EventArgs e)
        {
            switch (CurrentView)
            {
                case "potential":
                    Grid.Display(GameGrid);
                    CurrentView = "default";
                    break;

                case "default":
                    Grid.DisplayPotential(GameGrid);
                    CurrentView = "potential";
                    break;


                default:
                    Grid.Display(GameGrid);
                    CurrentView = "default";
                    break;
            }



            GameGrid.Refresh();

        }

        private void SetupLayout()
        {
            this.Size = new Size(9 * 50 + 20, 9 * 50 + 75);
            songsDataGridView.AllowUserToResizeColumns = false;
            songsDataGridView.AllowUserToResizeColumns = false;

            StepButton.Text = "Step";
            StepButton.Location = new Point(8, 4);
            StepButton.Click += new EventHandler(StepButton_Click!);


            StepCupletsButton.Text = "Step Cuplets";
            StepCupletsButton.Location = new Point(100, 4);
            StepCupletsButton.Click += new EventHandler(StepCupletsButton_Click!);
            StepCupletsButton.Width = 100;

            ChangeViewButton.Text = "Change View";
            ChangeViewButton.Location = new Point(200, 4);
            ChangeViewButton.Width = 100;
            ChangeViewButton.Click += new EventHandler(ChangeViewButton_Click!);


            buttonPanel.Controls.Add(StepButton);
            buttonPanel.Controls.Add(ChangeViewButton);
            buttonPanel.Controls.Add(StepCupletsButton);

            buttonPanel.Height = 30;
            buttonPanel.Dock = DockStyle.Bottom;

            this.Controls.Add(this.buttonPanel);
        }

        private void SetupGameGrid()
        {
            this.Controls.Add(GameGrid);
            GameGrid.ColumnCount = 9;
            GameGrid.RowCount = 9;

            GameGrid.Name = "GameGrid";
            GameGrid.Location = new Point(8, 8);
            GameGrid.Size = new Size(50, 50);

            GameGrid.RowTemplate.Height = 50;

            for (int i = 0; i < 9; i++)
            {
                GameGrid.Columns[i].Name = $"{i}";
                GameGrid.Columns[i].Width = 50;

            }

            GameGrid.ColumnHeadersVisible = false;
            GameGrid.RowHeadersVisible = false;

            GameGrid.AllowUserToAddRows = false;
            GameGrid.AllowUserToResizeColumns = false;
            GameGrid.AllowUserToResizeRows = false;

            GameGrid.DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;


            GameGrid.MultiSelect = false;
            GameGrid.Dock = DockStyle.Fill;


        }

        private void PopulateGameGrid()
        {
            //PopulateTestGrid("900000037004700000000038905420000800170903456690841023009074060300000500806302000");

            //Grid = new("900000037004700000000038905420000800170903456690841023009074060300000500806302000");

            Grid = new("080903002304000050079250600058070120000100500001000700007800060000006090902000000");

            Grid.Display(GameGrid);

            //PopulateTestGrid("080903002304000050079250600058070120000100500001000700007800060000006090902000000");

            //PopulateTestGridExpert();

            //Grid.Display();

            //Grid.CheckAllN();

            //Grid.Display();

            ProcessList = Grid.PrioritiseCells();

            //GameGrid.Rows.Clear();
            Grid.Display(GameGrid);

        }

        private void PopulateTestGrid(string SourceString)
        {
            var SourceArray = SourceString.ToCharArray();

            int i = 0;
            foreach (int Char in SourceArray.ToList().ConvertAll(c => (int)Char.GetNumericValue(c)))
            {
                if (Char != 0)
                {
                    Grid.Cells[i].UpdateNumberIs(Char);
                }

                i++;
            }

        }

        private void PopulateTestGridExpert()
        {

            Grid.Cells[0].UpdateNumberIs(4);

            Grid.Cells[5].UpdateNumberIs(1);

            Grid.Cells[10].UpdateNumberIs(6);

            Grid.Cells[12].UpdateNumberIs(2);

            Grid.Cells[21].UpdateNumberIs(9);

            Grid.Cells[24].UpdateNumberIs(8);

            Grid.Cells[26].UpdateNumberIs(4);

            Grid.Cells[31].UpdateNumberIs(5);

            Grid.Cells[32].UpdateNumberIs(7);

            Grid.Cells[35].UpdateNumberIs(1);

            Grid.Cells[43].UpdateNumberIs(7);

            Grid.Cells[45].UpdateNumberIs(2);

            Grid.Cells[49].UpdateNumberIs(6);

            Grid.Cells[52].UpdateNumberIs(5);

            Grid.Cells[54].UpdateNumberIs(6);

            Grid.Cells[55].UpdateNumberIs(7);

            Grid.Cells[56].UpdateNumberIs(5);

            Grid.Cells[65].UpdateNumberIs(2);

            Grid.Cells[69].UpdateNumberIs(1);

            Grid.Cells[70].UpdateNumberIs(9);

            Grid.Cells[74].UpdateNumberIs(1);

            Grid.Cells[78].UpdateNumberIs(3);
        }
        [STAThread]
        static void Main()
        {



            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}