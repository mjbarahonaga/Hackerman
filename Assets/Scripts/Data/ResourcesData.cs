using System;

[Serializable]
public class Resources
{
    public int CodeLines;
    public int Bitcoin;

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
        res1.CodeLines += res2.CodeLines;
        res1.Bitcoin += res2.Bitcoin;
        return res1;
    }
    public static Resources operator -(Resources res1, Resources res2)
    {
        res1.CodeLines -= res2.CodeLines;
        res1.Bitcoin -= res2.Bitcoin;
        return res1;
    }
    public static Resources operator *(Resources res, int value)
    {
        res.CodeLines *= value;
        res.Bitcoin *= value;
        return res;
    }
    public static Resources operator *(Resources res, float value)
    {
        res.CodeLines = (int)(res.CodeLines * value); 
        res.Bitcoin = (int)(res.Bitcoin * value);
        return res;
    }

}

