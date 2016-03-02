using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Template10.Controls
{
    [Obsolete("Use RingSegment instead")]
    public class PieSlice : Path
    {
        private bool _loaded = false;
        public PieSlice()
        {
            Loaded += (s, e) =>
            {
                _loaded = true;
                UpdatePath();
            };
        }

        // StartAngle
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(PieSlice),
            new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PieSlice); }));
        public double StartAngle { get { return (double)GetValue(StartAngleProperty); } set { SetValue(StartAngleProperty, value); } }

        // Angle
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(nameof(Angle), typeof(double), typeof(PieSlice),
            new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PieSlice); }));
        public double Angle { get { return (double)GetValue(AngleProperty); } set { SetValue(AngleProperty, value); } }

        // Radius
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(PieSlice),
            new PropertyMetadata(DependencyProperty.UnsetValue, (s, e) => { Changed(s as PieSlice); }));
        public double Radius { get { return (double)GetValue(RadiusProperty); } set { SetValue(RadiusProperty, value); } }

        private static void Changed(PieSlice pieSlice)
        {
            if (pieSlice._loaded)
                pieSlice.UpdatePath();
        }

        public void UpdatePath()
        {
            // ensure variables
            if (GetValue(StartAngleProperty) == DependencyProperty.UnsetValue)
                throw new ArgumentNullException("Start Angle is required");
            if (GetValue(RadiusProperty) == DependencyProperty.UnsetValue)
                throw new ArgumentNullException("Radius is required");
            if (GetValue(AngleProperty) == DependencyProperty.UnsetValue)
                throw new ArgumentNullException("Angle is required");

            Width = Height = 2 * (Radius + StrokeThickness);
            var endAngle = StartAngle + Angle;

            // path container
            var figureP = new Point(Radius, Radius);
            var figure = new PathFigure
            {
                StartPoint = figureP,
                IsClosed = true,
            };

            //  start angle line
            var lineX = Radius + Math.Sin(StartAngle * Math.PI / 180) * Radius;
            var lineY = Radius - Math.Cos(StartAngle * Math.PI / 180) * Radius;
            var lineP = new Point(lineX, lineY);
            var line = new LineSegment { Point = lineP };
            figure.Segments.Add(line);

            // outer arc
            var arcX = Radius + Math.Sin(endAngle * Math.PI / 180) * Radius;
            var arcY = Radius - Math.Cos(endAngle * Math.PI / 180) * Radius;
            var arcS = new Size(Radius, Radius);
            var arcP = new Point(arcX, arcY);
            var arc = new ArcSegment
            {
                IsLargeArc = Angle >= 180.0,
                Point = arcP,
                Size = arcS,
                SweepDirection = SweepDirection.Clockwise,
            };
            figure.Segments.Add(arc);

            // finalé
            Data = new PathGeometry { Figures = { figure } };
            InvalidateArrange();
        }
    }
}
