
int variables = 0;
int conditions = 0;
List<List<int>> x = new();
List<List<int>> dual = new();
List<int> b = new();

Console.WriteLine("Duál generáló");

ReadInput();
ShowConditions();

void ShowConditions()
{
    for (int i = 0; i < conditions; i++)
    {
        for (int j = 0; j < variables; j++)
        {
            string preseage = "";
            if (j > 0 || x[i][j] < 0)
            {
                preseage = x[i][j] >= 0 ? "+" : "-";
            }
            Console.Write($"{preseage}{Math.Abs(x[i][j])}");
        }

        Console.Write($"<{b[i]}");

        Console.WriteLine();
    }
}

void ReadInput()
{
    Console.Write("Adja meg a változók számát: ");

    if (!int.TryParse(Console.ReadLine().Trim(), out variables) || variables < 0)
    {
        Console.WriteLine("Hibás formátum!");
        return;
    }

    Console.Write("Adja meg a feltételek számát: ");

    if (!int.TryParse(Console.ReadLine().Trim(), out conditions) || conditions < 0)
    {
        Console.WriteLine("Hibás formátum!");
        return;
    }

    if (conditions < variables)
    {
        Console.WriteLine("Nincs elég feltétel!");
        return;
    }

    for (int i = 0; i < conditions; i++)
    {
        x.Add(new());
        for (int j = 0; j < variables; j++)
        {
            int xj;
            Console.Write($"x{j}: ");

            if (!int.TryParse(Console.ReadLine().Trim(), out xj))
            {
                Console.WriteLine("Hibás formátum!");
                return;
            }

            x[i].Add(xj);
        }

        Console.Write($"Condition: < ");
        int bi;
        if (!int.TryParse(Console.ReadLine().Trim(), out bi))
        {
            Console.WriteLine("Hibás formátum!");
            return;
        }

        b.Add(bi);
    }
}

void GenerateDual()
{
    for (int i = 0; i < conditions; i++)
    {

    }
}
