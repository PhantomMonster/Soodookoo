
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

                int? NCellNumber = Cells[NCell].NumberIs;

                if (NCellNumber is not null)
                {
                    Cell.UpdateNumberIsNot((int)NCellNumber);
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

    public List<int> PrioritiseCells()
    {
        return Cells.Where(x => x.Value.NumberIs is null).OrderBy(x => x.Value.NumberPotential.Count).Select(x => x.Key).ToList();
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

    }


}