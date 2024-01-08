

[System.Serializable]
public class ColorData
{
    public float colorR;
    public float colorG;
    public float colorB;

    public ColorData()
    {
    }

    public ColorData(int colorR, int colorG, int colorB)
    {
        this.colorR = colorR;
        this.colorG = colorG;
        this.colorB = colorB;
    }
}