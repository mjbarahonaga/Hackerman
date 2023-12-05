using System;

[Serializable]
public struct Resources
{
    public int CodeLines;
    public int Bitcoin;

    public Resources(int cl, int b)
    {
        CodeLines = cl;
        Bitcoin = b;
    }

    public static Resources operator +(Resources res1, Resources res2) => new Resources(res1.CodeLines + res2.CodeLines, res1.Bitcoin + res2.Bitcoin);
    public static Resources operator *(Resources res, int value) => new Resources(res.CodeLines * value, res.Bitcoin * value);
    public static Resources operator *(Resources res, float value) => new Resources( (int)(res.CodeLines * value), (int)(res.Bitcoin * value));

}

