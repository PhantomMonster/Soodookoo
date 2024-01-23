

using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using FormWithButton;


public class Cell
{
    public int CellID;
    public bool HasChanged;
    public int? NumberIs { get; set; } = null;
    public List<int> NumberIsNot = new();
    public List<int> NumberPotential = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public Dictionary<string, List<int>> N = new();
    public int CellRow { get { return CellID / 9; } }
    public int CellColumn { get { return CellID % 9; } }
    public int CellBlock { get { return 3 * (CellRow / 3) + (CellColumn / 3); } }

    public void UpdateNumberIs(int Number)
    {
        NumberIs = Number;

        NumberIsNot = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        NumberIsNot.Remove(Number);

        NumberPotential = new() { Number };

        GridColourChangedCell();
    }

    public void UpdateNumberIsNot(int Number)
    {

        NumberPotential.Remove(Number);

        NumberIsNot.Add(Number);

        NumberIsNot = NumberIsNot.Distinct().ToList();

    }

    public string GetPotentialNumbers()
    {
        return NumberPotential.ToString() ?? "";

    }

    public void GridColourSelectedCell()
    {
        Form1.GameGrid.Rows[CellRow].Cells[CellColumn].Style.BackColor = Color.LightBlue;

        Form1.GameGrid.Refresh();
    }

    public void GridColourSelectedGroup()
    {
        Form1.GameGrid.Rows[CellRow].Cells[CellColumn].Style.BackColor = Color.LightSalmon;

        Form1.GameGrid.Refresh();
    }

    public void GridColourCheckCell()
    {
        Form1.GameGrid.Rows[CellRow].Cells[CellColumn].Style.BackColor = Color.LightPink;

        //Form1.GameGrid.Refresh();
    }

    public void GridColourChangedCell()
    {
        Form1.GameGrid.Rows[CellRow].Cells[CellColumn].Style.BackColor = Color.LightGreen;

        Form1.GameGrid.Refresh();
    }


    public Cell(int cellID, int numberIs = 0)
    {
        CellID = cellID;

        if (numberIs != 0)
        {
            NumberIs = numberIs;

            NumberIsNot = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            NumberIsNot.Remove(numberIs);

            NumberPotential = new() { numberIs };

        }

    }

}