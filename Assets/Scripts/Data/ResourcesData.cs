using System;

[Serializable]
public class Resources
{
    public float CodeLines;
    public float Bitcoin;

    public Resources()
    {
        CodeLines = 0;
        Bitcoin = 0;
    }

    public Resources(int cl, int b)
    {
        CodeLines = cl;
        Bitcoin = b;
    }

    public static Resources operator +(Resources res1, Resources res2) 
    {
        Resources tmp = new Resources();
        tmp.CodeLines = res1.CodeLines + res2.CodeLines;
        tmp.Bitcoin = res1.Bitcoin + res2.Bitcoin;
        return tmp;
    }
    public static Resources operator -(Resources res1, Resources res2)
    {
        Resources tmp = new Resources();
        tmp.CodeLines = res1.CodeLines - res2.CodeLines;
        tmp.Bitcoin = res1.Bitcoin - res2.Bitcoin;
        return tmp;
    }
    public static Resources operator *(Resources res, int value)
    {
        Resources tmp = new Resources();
        tmp.CodeLines = res.CodeLines * value;
        tmp.Bitcoin = res.Bitcoin * value;
        return tmp;
    }
    public static Resources operator *(Resources res, float value)
    {
        Resources tmp = new Resources();
        tmp.CodeLines = res.CodeLines * value;
        tmp.Bitcoin = res.Bitcoin * value;
        return tmp;
    }

}

