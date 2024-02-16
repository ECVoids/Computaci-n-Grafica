using System.Threading.Channels;
using SkiaSharp;

SKBitmap bmp = new(480, 480); 
using SKCanvas canvas = new(bmp);
canvas.Clear(SKColor.Parse("#000000"));

SKPoint3[] points3D = new SKPoint3[]
{
    new SKPoint3(100, 100, 100),  // A
    new SKPoint3(300, 100, 100),  // B
    new SKPoint3(300, 300, 100),  // C
    new SKPoint3(100, 300, 100),  // D
    new SKPoint3(100, 100, 300),  // E
    new SKPoint3(300, 100, 300),  // F
    new SKPoint3(300, 300, 300),  // G
    new SKPoint3(100, 300, 300)   // H
};


SKPoint3 observer = new SKPoint3(200, 200, -400); 

SKPoint[] points2D = new SKPoint[points3D.Length];
for (int i = 0; i < points3D.Length; i++)
{
    points2D[i] = ProjectPoint(points3D[i], observer);
}

SKPaint paint = new SKPaint { Color = SKColors.Peru, StrokeWidth = 2 };

    canvas.DrawLine(points2D[0], points2D[1], paint); // AB
    canvas.DrawLine(points2D[1], points2D[2], paint); // BC
    canvas.DrawLine(points2D[2], points2D[3], paint); // CD
    canvas.DrawLine(points2D[3], points2D[0], paint); // DA
    canvas.DrawLine(points2D[0], points2D[4], paint); // AE
    canvas.DrawLine(points2D[1], points2D[5], paint); // BF
    canvas.DrawLine(points2D[2], points2D[6], paint); // CG
    canvas.DrawLine(points2D[3], points2D[7], paint); // DH
    canvas.DrawLine(points2D[4], points2D[5], paint); // EF
    canvas.DrawLine(points2D[5], points2D[6], paint); // FG
    canvas.DrawLine(points2D[6], points2D[7], paint); // GH
    canvas.DrawLine(points2D[7], points2D[4], paint); // HE

using (SKImage img = SKImage.FromBitmap(bmp))
using (SKData data = img.Encode(SKEncodedImageFormat.Png, 100))
using (Stream stream = File.OpenWrite("cubo.png"))
{
    data.SaveTo(stream);
}


static SKPoint ProjectPoint(SKPoint3 point3D, SKPoint3 observer)
{
    float depth = point3D.Z - observer.Z;
    float factor = observer.Z / depth;
    float projectedX = observer.X + factor * (point3D.X - observer.X);
    float projectedY = observer.Y + factor * (point3D.Y - observer.Y);
    return new SKPoint(projectedX, projectedY);
}