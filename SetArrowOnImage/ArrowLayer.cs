using CoreAnimation;

namespace SetArrowOnImage
{
    public class ArrowLayer : CAShapeLayer
    {
        public ArrowLayer(CGPoint startPoint, CGPoint endPoint, CGColor color)
        {
            StrokeColor = color;
            LineWidth = 5.0f;
            FillColor = UIColor.Clear.CGColor;

            // 矢印のパスを作成
            var path = new CGPath();
            path.MoveToPoint(startPoint);
            path.AddLineToPoint(endPoint);

            // 矢印の先端を描画 (簡易的に三角形で表現)
            var arrowHeadLength = 20.0f;
            var angle = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X);

            var arrowPoint1 = new CGPoint(
                endPoint.X - arrowHeadLength * Math.Cos(angle - Math.PI / 6),
                endPoint.Y - arrowHeadLength * Math.Sin(angle - Math.PI / 6)
            );

            var arrowPoint2 = new CGPoint(
                endPoint.X - arrowHeadLength * Math.Cos(angle + Math.PI / 6),
                endPoint.Y - arrowHeadLength * Math.Sin(angle + Math.PI / 6)
            );

            path.MoveToPoint(endPoint);
            path.AddLineToPoint(arrowPoint1);
            path.MoveToPoint(endPoint);
            path.AddLineToPoint(arrowPoint2);

            // 作成したパスをレイヤーに設定
            Path = path;
        }
    }
}

