using System;
using CoreGraphics;
using UIKit;

namespace OwlFinance.Views.ConversationsControls
{
    public class SwipeShrinkView : UIView
    {
		public bool IsShrinking => Center != _initialCenter;
		public bool Shrunk => Center == _finalCenter;

        public event Action OnInitialSize;
        public event Action OnShrinking;
        public event Action OnShrunk;

        private const double AspectRatio = 0.5625;
		private const int MagicLeftOffset = 50;
		private CGPoint _finalCenter;
        private CGSize _initialSize;
        private CGPoint _initialCenter;
        private CGSize _finalSize;
        private nfloat _rangeTotal;
        private nfloat _widthRage;
        private nfloat _centerXRange;

        public SwipeShrinkView(IntPtr handle) 
            : base (handle) { }

        public SwipeShrinkView(CGRect frame) 
            : base(frame)
        {
            InitializeGestures();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            InitializeGestures();
        }

        protected void SetSizeAndPosition()
        {
            _initialSize = Frame.Size;
            _initialCenter = Center;
            _finalSize = new CGSize(
                UIScreen.MainScreen.Bounds.Width - UIScreen.MainScreen.Bounds.Width / 4,
                (UIScreen.MainScreen.Bounds.Width - UIScreen.MainScreen.Bounds.Width / 4) * AspectRatio);
            _finalCenter = new CGPoint(
				(UIScreen.MainScreen.Bounds.Width - UIScreen.MainScreen.Bounds.Width / 4) - MagicLeftOffset,
				(UIScreen.MainScreen.Bounds.Height - UIScreen.MainScreen.Bounds.Height / 4));
            _rangeTotal = _finalCenter.Y - _initialCenter.Y;
            _widthRage = _initialSize.Width - _finalSize.Width;
            _centerXRange = _finalCenter.X - _initialCenter.X;
        }

        private void InitializeGestures()
        {
            var panGesture = new UIPanGestureRecognizer(Panning);
            var tapGesture = new UITapGestureRecognizer(Tapped);

            tapGesture.NumberOfTapsRequired = 1;
            tapGesture.ShouldRecognizeSimultaneously += (recognizer, gestureRecognizer) => true;

            AddGestureRecognizer(panGesture);
            AddGestureRecognizer(tapGesture);
        }

        private void Panning(UIPanGestureRecognizer panGesture)
        {
            var translatedPoint = panGesture.TranslationInView(Superview);
            var gestureState = panGesture.State;
            var yDelta = panGesture.View.Center.Y + translatedPoint.Y;

            if (yDelta < _initialCenter.Y) gestureState = UIGestureRecognizerState.Ended;
            if (yDelta >= _finalCenter.Y) gestureState = UIGestureRecognizerState.Ended;

            if (gestureState == UIGestureRecognizerState.Began ||
                gestureState == UIGestureRecognizerState.Changed)
            {
                var progress = (panGesture.View.Center.Y - _initialCenter.Y) / _rangeTotal;
                var invertedProgress = 1 - progress;
                var newWidth = _finalSize.Width + _widthRage * invertedProgress;

                var tempFrame = panGesture.View.Frame;
				tempFrame.Size = new CGSize(newWidth, newWidth * AspectRatio);
                panGesture.View.Frame = tempFrame;
                
                var finalX = _initialCenter.X + _centerXRange * progress;

                panGesture.View.Center = new CGPoint(finalX, panGesture.View.Center.Y + translatedPoint.Y);
                panGesture.SetTranslation(CGPoint.Empty, Superview);

				// Shrinking state
				var evt = OnShrinking;
				evt?.Invoke();
            }
            else if (gestureState == UIGestureRecognizerState.Ended)
            {
                var topDistance = yDelta - _initialCenter.Y;
                var bottomDistance = _finalCenter.Y - yDelta;
                var chosenCenter = CGPoint.Empty;
                var chosenSize = CGSize.Empty;

                UserInteractionEnabled = false;

                if (topDistance > bottomDistance)
                {
                    // Set for bottom of screen animation
                    chosenCenter = _finalCenter;
                    chosenSize = _finalSize;
                }
                else
                {
                    // Set for top of screen Animation
                    chosenCenter = _initialCenter;
                    chosenSize = _initialSize;
                }

                if (panGesture.View.Center != chosenCenter)
                {
                    Animate(0.4, () =>
                    {
                        var tempFrame = panGesture.View.Frame;
                        tempFrame.Size = chosenSize;
                        panGesture.View.Frame = tempFrame;
                        panGesture.View.Center = chosenCenter;
                    }, () =>
                    {
                        UserInteractionEnabled = true;
                    });
                }
                else
                {
                    UserInteractionEnabled = true;
                }
            }

			if (Shrunk)
			{
				var evt = OnShrunk;
				evt?.Invoke();
			}
			else
			{
				var evt = OnInitialSize;
				evt?.Invoke();
			}

        }

        private void Tapped(UITapGestureRecognizer tapGesture)
        {
            if (tapGesture.View.Center != _finalCenter) return;

            UserInteractionEnabled = false;
            Animate(0.4, () =>
            {
                var tempFrame = tapGesture.View.Frame;
                tempFrame.Size = _initialSize;
                tapGesture.View.Frame = tempFrame;
                tapGesture.View.Center = _initialCenter;
            }, () =>
            {
                UserInteractionEnabled = true;
				// Fire action when view is initial size
				var evt = OnInitialSize;
				evt?.Invoke();
            });
        }
    }
}