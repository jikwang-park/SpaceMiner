

public class DataTableUtilities
{
    public static int[] SplitColumnToInt(string str)
    {
        var splited = str.Split("_");
        var splitedInt = new int[splited.Length];

        for (int i = 0; i < splitedInt.Length; ++i)
        {
            splitedInt[i] = int.Parse(splited[i]);
        }
        return splitedInt;
    }

    public static string[] SplitColumnToString(string str)
    {
        var splited = str.Split("_");
        return splited;
    }

    public static float[] SplitColumnToFloat(string str)
    {
        var splited = str.Split("_");
        var splitedFloat = new float[splited.Length];

        for (int i = 0; i < splitedFloat.Length; ++i)
        {
            splitedFloat[i] = float.Parse(splited[i]);
        }
        return splitedFloat;
    }

    public static BigNumber[] SplitColumnToBigNumber(string str)
    {
        var splited = str.Split("_");
        var splitedBigNumber = new BigNumber[splited.Length];

        for (int i = 0; i < splitedBigNumber.Length; ++i)
        {
            splitedBigNumber[i] = splited[i];
        }
        return splitedBigNumber;
    }
}