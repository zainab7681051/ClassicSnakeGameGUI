namespace ClassicSnakeGameGUI.src;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
public static class Images
{
    public readonly static ImageSource Empty = Loadimage("Empty.png");
    public readonly static ImageSource Body = Loadimage("Body.png");
    public readonly static ImageSource Head = Loadimage("Head.png");
    public readonly static ImageSource Food = Loadimage("Food.png");
    public readonly static ImageSource DeadBody = Loadimage("DeadBody.png");
    public readonly static ImageSource DeadHead = Loadimage("DeadHead.png");

    private static ImageSource Loadimage(string fileName)
    {
        return new BitmapImage(new Uri($"../assets/{fileName}", UriKind.Relative));

    }
}