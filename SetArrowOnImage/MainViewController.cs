using CoreAnimation;

namespace SetArrowOnImage
{
    public class MainViewController : UIViewController, IUIScrollViewDelegate
    {
        UIImageView _imageView;
        CGPoint? _startPoint = null;
        CAShapeLayer? _startDotLayer = null;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var scrollView = new UIScrollView(View.Frame);
            scrollView.MinimumZoomScale = 1.0f;
            scrollView.MaximumZoomScale = 10.0f;
            scrollView.Delegate = this;

            View.AddSubview(scrollView);

            _imageView = new UIImageView(UIImage.FromFile("sample.png"));
            scrollView.ContentSize = _imageView.Frame.Size;
            scrollView.AddSubview(_imageView);

            var returnButton = new UIButton(UIButtonType.System);
            returnButton.Frame = new CGRect(20, View.Frame.Height - 60, 100, 40);
            returnButton.SetTitle("戻す", UIControlState.Normal);
            returnButton.TouchUpInside += (sender, args) =>
            {
                // 直近の線と番号を削除
                var layer = _imageView.Layer.Sublayers?.LastOrDefault();
                layer?.RemoveFromSuperLayer();
                layer = _imageView.Layer.Sublayers?.LastOrDefault();
                layer?.RemoveFromSuperLayer();
                layer = null;
            };
            View.AddSubview(returnButton);

            _imageView.UserInteractionEnabled = true;
            _imageView.AddGestureRecognizer(new UITapGestureRecognizer((recognizer) =>
            {
                var tapLocation = recognizer.LocationInView(_imageView);

                if (_startPoint == null)
                {
                    // 始点を設定、描画
                    _startPoint = tapLocation;
                    DrawStartDotAt(tapLocation);
                }
                else
                {
                    // 終点がタップされたら線を引く
                    var endPoint = tapLocation;
                    var arrowLayer = new ArrowLayer(_startPoint.Value, endPoint, UIColor.SystemGray.CGColor);
                    _imageView.Layer.AddSublayer(arrowLayer);

                    // 始点に番号を表示
                    var number = _imageView.Layer.Sublayers?.ToList().FindAll(x => x is ArrowLayer).Count;
                    var numberLayer = new CATextLayer
                    {
                        String = number.ToString(),
                        FontSize = 30,
                        ForegroundColor = UIColor.Red.CGColor,
                        BackgroundColor = UIColor.Clear.CGColor,
                        TextAlignmentMode = CATextLayerAlignmentMode.Center,
                        Frame = new CGRect(_startPoint.Value.X - 20, _startPoint.Value.Y - 20, 40, 40),
                        ContentsScale = UIScreen.MainScreen.Scale // Retina対応
                    };
                    _imageView.Layer.AddSublayer(numberLayer);

                    // 始点と終点をリセット
                    _startPoint = null;
                    RemoveStartDot();
                }
            }));
        }

        private void DrawStartDotAt(CGPoint point)
        {
            var dotRadius = 5.0f;
            _startDotLayer = new CAShapeLayer
            {
                FillColor = UIColor.SystemGray.CGColor
            };

            var dotPath = new CGPath();
            dotPath.AddEllipseInRect(new CGRect(point.X - dotRadius, point.Y - dotRadius, dotRadius * 2, dotRadius * 2));
            _startDotLayer.Path = dotPath;

            _imageView.Layer.AddSublayer(_startDotLayer);
        }

        private void RemoveStartDot()
        {
            _startDotLayer?.RemoveFromSuperLayer();
            _startDotLayer = null;
        }

        [Export("viewForZoomingInScrollView:")]
        public UIView ViewForZoomingInScrollView(UIScrollView scrollView)
        {
            return _imageView;
        }
    }
}
