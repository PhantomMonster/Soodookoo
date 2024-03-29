
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FormWithButton;

public class Grid
{
    public Dictionary<int, Cell> Cells = new();

    public Grid(string startingGrid = "")
    {

        try
        {
            if (startingGrid.Length != 81) { throw new Exception("startingGrid is not 81 characters"); }

            List<int> SourceArray = startingGrid.ToCharArray().ToList().ToList().ConvertAll(c => (int)Char.GetNumericValue(c));

            for (int i = 0; i < 81; i++)
            {
                Cells.Add(i, new Cell(i, SourceArray[i]));
            }

            PopulateN();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error with Grid: {0}", ex);

        }


    }




    public List<int> GetRow(int cellID)
    {
        return Cells.Where(x => x.Value.CellRow == cellID).Select(y => y.Value.CellID).ToList();
    }

    public List<int> GetColumn(int cellID)
    {
        return Cells.Where(x => x.Value.CellColumn == cellID).Select(y => y.Value.CellID).ToList();
    }

    public List<int> GetBlock(int cellID)
    {
        return Cells.Where(x => x.Value.CellBlock == cellID).Select(y => y.Value.CellID).ToList();
    }

    public void PopulateN()
    {
        for (int i = 0; i < 9; i++)
        {
            List<int> CellList = GetRow(i);

            foreach (int cell in CellList)
            {
                Cells[cell].N.Add("row", CellList.Except(new List<int> { cell }).ToList());
            }


            CellList = GetColumn(i);

            foreach (int cell in CellList)
            {
                Cells[cell].N.Add("column", CellList.Except(new List<int> { cell }).ToList());
            }

            CellList = GetBlock(i);

            foreach (int cell in CellList)
            {
                Cells[cell].N.Add("block", CellList.Except(new List<int> { cell }).ToList());
            }
        }

    }

    public void CheckCellNNumberIs(int CheckCell)
    {
        Cell Cell = Cells[CheckCell];

        Cell.GridColourSelectedCell();

        foreach (var NGroup in Cell.N)
        {
            foreach (int NCell in NGroup.Value)
            {
                Cells[NCell].GridColourCheckCell();

                int NCellNumber = Cells[NCell].NumberIs ?? 0;

                if (Cell.NumberPotential.Contains(NCellNumber))
                {
                    Cell.UpdateNumberIsNot(NCellNumber);
                }
            }
        }

        if (Cell.NumberIs is null && Cell.NumberPotential.Count == 1)
        {
            //UpdateCellinGrid(Cell, Cell.NumberPotential.FirstOrDefault());

            Cell.UpdateNumberIs(Cell.NumberPotential.FirstOrDefault());

            List<int> NtoCheck = new();

            Form1.ProcessList = PrioritiseCells();

            foreach (var nGroup in Cell.N)
            {
                NtoCheck.AddRange(nGroup.Value.Intersect(PrioritiseCells()));
            }

            CheckAllN(NtoCheck.Intersect(PrioritiseCells()).ToList());

        }

    }

    public void CheckCellNNumberIsNot(int CheckCell)
    {
        Cell Cell = Cells[CheckCell];

        Cell.GridColourSelectedCell();

        foreach (int Number in Cell.NumberPotential)
        {
            foreach (var NGroup in Cell.N)
            {
                bool ChangeNumber = true;

                foreach (int NCell in NGroup.Value)
                {
                    Cells[NCell].GridColourCheckCell();

                    List<int> NCellNumber = Cells[NCell].NumberIsNot;

                    int NumberInList = NCellNumber.Where(x => x == Number).Count();

                    if (NumberInList == 0)
                    {
                        ChangeNumber = false;
                    }

                }
                if (ChangeNumber == true)
                {
                    //UpdateCellinGrid(Cell, Number);

                    Cell.UpdateNumberIs(Number);

                    List<int> NtoCheck = new();

                    Form1.ProcessList = PrioritiseCells();

                    foreach (var nGroup in Cell.N)
                    {
                        NtoCheck.AddRange(nGroup.Value.Intersect(PrioritiseCells()));
                    }

                    CheckAllN(NtoCheck.Intersect(PrioritiseCells()).ToList());



                    break;
                }

            }

        }

    }

    public void CheckforDoubles(int CheckCell)
    {
        Cell Cell = Cells[CheckCell];

        int NumberOfPotential = Cell.NumberPotential.Count;

        if (NumberOfPotential != 2) { return; }

        Cell.GridColourSelectedCell();

        foreach (var NGroup in Cell.N)
        {
            List<int> NCellNumber = new() { CheckCell };

            foreach (int nCell in NGroup.Value)
            {
                Cell NCell = Cells[nCell];

                NCell.GridColourCheckCell();

                if (Cell.NumberPotential.SequenceEqual(NCell.NumberPotential))
                {
                    NCellNumber.Add(nCell);

                    Form1.GameGrid.Refresh();
                }

            }

            if (NCellNumber.Count == NumberOfPotential)
            {

                foreach (int cell in NCellNumber)
                {
                    Cells[cell].NumberPotential = Cell.NumberPotential;
                }

                foreach (int cell in NGroup.Value.Except(NCellNumber))
                {
                    Cells[cell].NumberPotential = Cells[cell].NumberPotential.Except(Cell.NumberPotential).ToList();
                }
            }

        }
    }

    public void UpdateCellinGrid(Cell Cell, int number)
    {
        Cell.UpdateNumberIs(number);

        Display(Form1.GameGrid);



        // foreach (var NGroup in Cell.N)
        // {
        //     CheckAllN(NGroup.Value);
        // }

    }

    // public void CheckAllN()
    // {
    //     for (int i = 0; i < 81; i++)
    //     {
    //         CheckCellNNumberIs(i);

    //         CheckCellNNumberIsNot(i);
    //     }
    // }

    public void CheckAllN(List<int>? order = null)
    {
        order ??= Cells.Select(x => x.Key).ToList();

        foreach (int item in order.Distinct())
        {
            CheckCellNNumberIs(item);

            Display(Form1.GameGrid);
        }
    }

    public List<int> PrioritiseCells(List<int>? startingCells = null)
    {
        List<int> PrioritisedCells = Cells.Where(x => x.Value.NumberIs is null).OrderBy(x => x.Value.NumberPotential.Count).Select(x => x.Key).ToList();

        if (startingCells is not null)
        {
            return PrioritisedCells.Intersect(startingCells).ToList();
        }
        
        return PrioritisedCells;
    }

    public List<int> PrioritiseDoubleCells(List<int>? startingCells = null)
    {
        List<int> PrioritisedCells = Cells.Where(x => x.Value.NumberIs is null).Where(x => x.Value.NumberPotential.Count == 2).Select(x => x.Key).ToList();

        if (startingCells is not null)
        {
            return PrioritisedCells.Intersect(startingCells).ToList();
        }

        return PrioritisedCells;
    }


    public void Display()
    {
        for (int i = 0; i < 9; i++)
        {
            List<int> GridRow = GetRow(i);

            List<string> CellList = new();

            foreach (int cell in GridRow)
            {
                CellList.Add(Cells[cell].NumberIs?.ToString() ?? "_");
            }

            Console.WriteLine(CellList.Aggregate((a, b) => a + "|" + b));
        }

    }

    public void Display(DataGridView DataGridView)
    {
        DataGridView.Rows.Clear();

        for (int i = 0; i < 9; i++)
        {

            List<int> GridRow = GetRow(i);

            string[] CellList = new string[9];

            foreach (int cell in GridRow)
            {
                CellList[cell % 9] = Cells[cell].NumberIs.ToString() ?? "";
            }

            DataGridView.Rows.Add(CellList);
            //DataGridView.Columns[i].DisplayIndex = i;

        }
        DataGridView.ClearSelection();

        DataGridView.DefaultCellStyle.Font = new Font("Tahoma", 16);

    }


    public void DisplayPotential(DataGridView DataGridView)
    {
        DataGridView.Rows.Clear();

        for (int i = 0; i < 9; i++)
        {

            List<int> GridRow = GetRow(i);

            string[] CellList = new string[9];

            foreach (int cell in GridRow)
            {
                //CellList[cell % 9] = Cells[cell].NumberPotential.ToString() ?? "";
                CellList[cell % 9] = String.Join(" ", Cells[cell].NumberPotential);
            }

            DataGridView.Rows.Add(CellList);
            //DataGridView.Columns[i].DisplayIndex = i;

        }
        DataGridView.ClearSelection();

        DataGridView.DefaultCellStyle.Font = new Font("Tahoma", 8);
        DataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
    }

}