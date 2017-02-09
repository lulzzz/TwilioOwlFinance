using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using JSQMessagesViewController;
using Newtonsoft.Json;
using OwlFinance.Managers;
using OwlFinance.ViewControllers.Delegates;
using OwlFinance.ViewModels;
using Twilio.Common;
using Twilio.IPMessaging;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Domain.Model.Security;
using UIKit;
using JMessage = JSQMessagesViewController.Message;
using JMessagesViewController = JSQMessagesViewController.MessagesViewController;

namespace OwlFinance.ViewControllers
{
	public partial class MessageDetailViewController : JMessagesViewController, ITwilioIPMessagingClientDelegate
	{
		public int SelectedTransactionId { get; set; }
		public int SelectedCaseId { get; set; }
		public string InviteeIdentity { get; set; }

		public UIStoryboard MainStoryboard => UIStoryboard.FromName("Main", NSBundle.MainBundle);

		private MessageDetailViewModel viewModel;
		private CaseExistenceModel caseModel;
		
		private List<JMessage> messages = new List<JMessage>();
		private MessagesBubbleImage outgoingBubbleImageData;
		private MessagesBubbleImage incomingBubbleImageData;
		private MessagesBubbleImageFactory bubbleFactory = new MessagesBubbleImageFactory();

		// Twilio IP Messaging
		// AgentID is currently arbitrary
		private const string AgentId = "2";
		private int caseId;

		private Channel caseChannel;
		private TwilioIPMessagingClient client;
		private AccessManagerDelegate accessManagerDelegate;
		private IPMessagingDelegate ipMessagingDelegate;

		public MessageDetailViewController(IntPtr handle) : base(handle) 
		{
			
		}

		public UIViewController GetViewController(UIStoryboard storyboard, string viewControllerName)
		{
			return storyboard.InstantiateViewController(viewControllerName);
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			var newCaseCreated = false;

			Title = "Chatting With Agent";
			EdgesForExtendedLayout = UIRectEdge.None;
			viewModel = new MessageDetailViewModel();

			// You must set your SenderId and display name
			SenderId = AppSettingsManager.ClientId;
			SenderDisplayName = AppSettingsManager.UserNickName;

			// These MessagesBubbleImages will be used in the GetMessageBubbleImageData override
			outgoingBubbleImageData = bubbleFactory.CreateOutgoingMessagesBubbleImage(UIColorExtensions.MessageBubbleLightGrayColor);
			incomingBubbleImageData = bubbleFactory.CreateIncomingMessagesBubbleImage(UIColorExtensions.MessageBubbleBlueColor);

			// Springy bubbles
			CollectionView.CollectionViewLayout.SpringinessEnabled = true;

			if (SelectedTransactionId != 0)
			{
				caseModel = await viewModel.DoesCaseExist(SelectedTransactionId);

				if (caseModel.DoesExist)
				{
					caseId = caseModel.Id;
					Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Getting your message history.");
				}
				else 
				{
					Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Creating case.");
					var newCase = await viewModel.CreateCaseCommand(SelectedTransactionId);

					caseId = newCase.Id;
					caseModel.Id = newCase.Id;
					caseModel.DoesExist = newCase.IsCreated;
					newCaseCreated = true;
				}

				// get ip messaging client
				client = await GetTwilioIpMessagingClient();

				if (newCaseCreated)
				{
					Acr.UserDialogs.UserDialogs.Instance.ShowSuccess("Please send your message when ready.");
				}
			}
		}

		public override void PressedSendButton(UIButton button, string text, string senderId, string senderDisplayName, NSDate date)
		{
			if (caseChannel == null) return;

			var message = caseChannel.Messages.CreateMessageWithBody(text);

			caseChannel.Messages.SendMessage(message, (r) =>
			{
				SystemSoundPlayer.PlayMessageSentSound();
				FinishSendingMessage(true);
			});
		}

		#region Cell Methods

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = base.GetCell(collectionView, indexPath) as MessagesCollectionViewCell;
			var message = messages[indexPath.Row];

			if (message.SenderId == AppSettingsManager.ClientId)
			{
				cell.TextView.TextColor = UIColor.Black;
			}

			return cell;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return messages.Count;
		}

		public override IMessageData GetMessageData(MessagesCollectionView collectionView, NSIndexPath indexPath)
		{
			return messages[indexPath.Row];
		}

		public override IMessageBubbleImageDataSource GetMessageBubbleImageData(MessagesCollectionView collectionView, NSIndexPath indexPath)
		{
			var message = messages[indexPath.Row];

			if (message.SenderId == AppSettingsManager.ClientId)
			{
				return outgoingBubbleImageData;
			}

			return incomingBubbleImageData;
		}

		public override IMessageAvatarImageDataSource GetAvatarImageData(MessagesCollectionView collectionView, NSIndexPath indexPath)
		{
			UIImage image = null;
			var index = (int)indexPath.Item;

			if (messages[index].SenderId == AppSettingsManager.ClientId)
			{
				image = GravatarManager.GetImage(GravatarUser.Customer);
			}
			else 
			{
				image = GravatarManager.GetImage(GravatarUser.Agent);
			}

			// Default is (34, 34) for diameter
			if (image != null) return MessagesAvatarImageFactory.CreateAvatarImage(image, 34);

			return null;
		}

		// For the date attribute header
		public override nfloat GetMessageBubbleTopLabelHeight(MessagesCollectionView collectionView, MessagesCollectionViewFlowLayout collectionViewLayout, NSIndexPath indexPath)
		{
			// Return the date on every 4th cell
			return indexPath.Item % 4 == 0 ? 20f : 0f;
		}

		// For the date attribute header
		public override NSAttributedString GetMessageBubbleTopLabelAttributedText(MessagesCollectionView collectionView, NSIndexPath indexPath)
		{
			var index = (int)indexPath.Item;
			var message = messages[index];

			return MessagesTimestampFormatter.SharedFormatter.GetAttributedTimestamp(message.Date);
		}

		#endregion

		private void AddMessageToDataSource(Twilio.IPMessaging.Message message)
		{
			var identity = JsonConvert.DeserializeObject<IdentityToken>(message.Author);

			// Get Agent Avatar
			if (identity.identityId != AppSettingsManager.ClientId && GravatarManager.GetImage(GravatarUser.Agent) == null)
			{
				GravatarManager.SetImage(GravatarUser.Agent, identity.picture);
			}

			// Get Customer Avatar
			if (identity.identityId == AppSettingsManager.ClientId && GravatarManager.GetImage(GravatarUser.Customer) == null)
			{
				GravatarManager.SetImage(GravatarUser.Customer, identity.picture);
			}

			var jmsg = new JMessage
				(
					identity.identityId,
					identity.name,
					message.DateUpdatedAsDate,
					message.Body
				);

			messages.Add(jmsg);

			SystemSoundPlayer.PlayMessageReceivedSound();
			FinishReceivingMessage(true);
			ScrollToBottom(true);
		}

		private async Task<TwilioIPMessagingClient> GetTwilioIpMessagingClient()
		{
			accessManagerDelegate = new AccessManagerDelegate();
			ipMessagingDelegate = new IPMessagingDelegate();

			ipMessagingDelegate.OnMessageAdded += AddMessageToDataSource;
			ipMessagingDelegate.OnClientReady += TwilioClientReady;

			var token = await viewModel.GetToken(AppSettingsManager.LoggedInUserEmail, AppSettingsManager.ClientId, AppSettingsManager.UserNickName, AppSettingsManager.UserPictureUrl);
			var accessManager = TwilioAccessManager.AccessManagerWithToken(token, accessManagerDelegate);

			client = TwilioIPMessagingClient.IpMessagingClientWithAccessManager(accessManager, new TwilioIPMessagingClientProperties(), ipMessagingDelegate);

			return client;
		}

		private void TwilioClientReady(object sender, EventArgs e)
		{
			caseChannel = client.ChannelsList.ChannelWithUniqueName("case" + caseId);

			caseChannel.JoinWithCompletion(c =>
			{
				LoadChannelHistory();
				Acr.UserDialogs.UserDialogs.Instance.HideLoading();
			});
		}

		private void LoadChannelHistory()
		{
			var msgs = caseChannel.Messages?.AllObjects?.OrderBy(m => m.Timestamp);
			if (msgs != null)
			{
				foreach (var msg in msgs)
				{
					AddMessageToDataSource(msg);
				}
			}
		}

		partial void DoneBarButton_Activated(UIBarButtonItem sender)
		{
			DismissModalViewController(true);
		}
	}
}