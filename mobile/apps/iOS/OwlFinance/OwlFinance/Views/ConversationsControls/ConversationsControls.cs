using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using OwlFinance.Managers;
using OwlFinance.Models;
using OwlFinance.ViewControllers.Delegates;
using OwlFinance.ViewModels;
using Twilio.Video;
using UIKit;

namespace OwlFinance.Views.ConversationsControls
{
	public partial class ConversationsControls : SwipeShrinkView
	{
		private static readonly ConversationsControls instance = Create();
		private static AppDelegate AppDelegate => (AppDelegate)UIApplication.SharedApplication.Delegate;

		private readonly CallTimer timer = new CallTimer();
		private ConversationViewModel viewModel;

		// Video SDK components
		private VideoClient client;
		private Room room;
		private LocalMedia localMedia;
		private LocalAudioTrack localAudioTrack;
		private Participant participant;
		private VideoRoomDelegate roomDelegate;
		private VideoParticipantDelegate participantDelegate;

		public static ConversationsControls Instance => instance;
		public bool IsVisible => Superview != null;
		private ConversationsControls(IntPtr handle) : base(handle) { }
		
		public static ConversationsControls Create()
		{
			ConversationsControls view = null;

			AppDelegate.InvokeOnMainThread(() =>
			{
				var nib = NSBundle.MainBundle.LoadNib("ConversationsControls", null, null);
				view = Runtime.GetNSObject<ConversationsControls>(nib.ValueAt(0));
				view.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 172);
				view.SetSizeAndPosition();

				view.Alpha = 0;

				AddDropShadow(view);
			});

			return view;
		}

		public async Task ShowAsync()
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.Window.AddSubview(this);

			ToggleView();
			InitializeTimer();

			TalkingToLabel.Text = "Waiting for agent...";
			viewModel = new ConversationViewModel();

			RegisterVideoEvents();
			await ConnectToRoomAsync();
		}

		public async Task SendCallRequestAsync()
		{
			await SignalRManager.SendAsync("incomingcall");
		}

		public void Clear()
		{
			timer?.Stop();
			ElapsedTimeLabel.Text = "00:00";
		}

		public void HangUp()
		{
			room?.Disconnect();
			Reset();
		}

		private void Reset()
		{
			ToggleView();
			Clear();
		}

		private async Task ConnectToRoomAsync()
		{
			var token = await viewModel.GetCallToken(AppSettingsManager.UserNickName);
			client = VideoClient.ClientWithToken(token);

			// Prepare local media which we will share with Room Participants.
			PrepareLocalMedia();

			ConnectOptions connectOptions = ConnectOptions.OptionsWithBlock(builder =>
			{
				builder.LocalMedia = localMedia;
				builder.Name = "account" + AppSettingsManager.AccountId;
			});

			room = client.ConnectWithOptions(connectOptions, roomDelegate);
		}

		private void PrepareLocalMedia()
		{
			localMedia = new LocalMedia();
			localAudioTrack = localMedia.AddAudioTrack(true);
		}

		private void RegisterVideoEvents()
		{
			// Wire up event handlers for VideoRoomDelegate
			roomDelegate = new VideoRoomDelegate();
			roomDelegate.OnDidConnectToRoom += HandleOnDidConnectToRoom;
			roomDelegate.OnParticipantDidConnect += HandleOnParticipantDidConnect;
			roomDelegate.OnParticipantDisconnected += HandleOnParticipantDisconnected;

			// Wire up event handlers for VideoParticipantDelegate
			participantDelegate = new VideoParticipantDelegate();
		}

		private void HandleOnParticipantDidConnect(string msg, Participant remoteParticipant)
		{
			if (remoteParticipant != null)
			{
				participant = remoteParticipant;
				participant.Delegate = participantDelegate;
				TalkingToLabel.Text = remoteParticipant.Identity;
			}
		}

		private void HandleOnDidConnectToRoom(string msg, Room remoteRoom)
		{
			if (remoteRoom.Participants.Length > 0)
			{
				participant = remoteRoom.Participants[0];
				participant.Delegate = participantDelegate;
				TalkingToLabel.Text = participant.Identity;
			}
		}

		private void HandleOnParticipantDisconnected(string msg, Participant remoteParticipant)
		{
			Acr.UserDialogs.UserDialogs.Instance.Alert("The agent disconnected.");
			HangUp();
		}

		private static void AddDropShadow(UIView view)
		{
			view.Layer.MasksToBounds = false;
			view.Layer.ShadowOffset = new CGSize(-5, 5);
			view.Layer.ShadowRadius = 5;
			view.Layer.ShadowOpacity = 0.4f;
		}

		private void InitializeTimer()
		{
			timer.Start();
			timer.OnUpdate += () =>
			{
				InvokeOnMainThread(UpdateElapsedTimeLabel);
			};
		}

		private void UpdateElapsedTimeLabel()
		{
			ElapsedTimeLabel.Text = timer.Time;
		}

		private void ToggleView()
		{
			Animate(0.25, 0.0, UIViewAnimationOptions.CurveEaseInOut, () =>
			{
				Alpha = Alpha == 0 ? 1 : 0;
			}, null);
		}

		partial void MuteButton_TouchUpInside(UIButton sender)
		{
			if (localAudioTrack != null)
			{
				localAudioTrack.Enabled = !localAudioTrack.Enabled;
			}
		}

		partial void HangUpButton_TouchUpInside(UIButton sender)
		{
			HangUp();
		}

		partial void SpeakerButton_TouchUpInside(UIButton sender)
		{
			// do nothing
		}
	}
}