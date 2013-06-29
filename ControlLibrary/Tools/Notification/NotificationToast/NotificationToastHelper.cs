using NotificationsExtensions.ToastContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ControlLibrary.Tools
{
    public class NotificationToastHelper
    {
        /// <summary>
        /// Post弹出消息对话框系统APP图片
        /// </summary>
        /// <param name="title">主题</param>
        /// <param name="description">内容</param>
        public static void DisplayTextTost(string title, string description, ToastAudioContent toastAudioContent = ToastAudioContent.Default)
        {
            //var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            //var textElements = toastXml.GetElementsByTagName("text");
            //for (uint i = 0; i < textElements.Length; i++)
            //{
            //    string text = null;
            //    if (i == 0) text = title;
            //    else if (i == 1) text = description;
            //    if (text != null)
            //        textElements.Item(i).AppendChild(toastXml.CreateTextNode(text));
            //}

            //var toast = new ToastNotification(toastXml);
            //ToastNotificationManager.CreateToastNotifier().Show(toast);

            IToastText02 toastContent = ToastContentFactory.CreateToastText02();
            toastContent.TextHeading.Text = title;
            toastContent.TextBodyWrap.Text = description;
            toastContent.Audio.Content = toastAudioContent;
            ToastNotification toast = toastContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        /// <summary>
        /// Post弹出消息对话框可设置WEB的图片
        /// </summary>
        /// <param name="toastImageSrc"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public static void DisplayWebImageToast(string toastImageSrc, string title, string description, ToastAudioContent toastAudioContent = ToastAudioContent.Default)
        {
            //var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

            //var imageElements = toastXml.GetElementsByTagName("image");
            //var imageElement = (XmlElement)imageElements.Item(0);
            //imageElement.SetAttribute("src", toastImageSrc == null ? string.Empty : toastImageSrc);
            //imageElement.SetAttribute("alt", "");

            //var textElements = toastXml.GetElementsByTagName("text");
            //for (uint i = 0; i < textElements.Length; i++)
            //{
            //    string text = null;
            //    if (i == 0) text = title;
            //    else if (i == 1) text = description;
            //    if (text != null)
            //        textElements.Item(i).AppendChild(toastXml.CreateTextNode(text));
            //}

            ////Debug.WriteLine(toastXml.GetXml());

            //var toast = new ToastNotification(toastXml);
            ////toast.Failed += toast_Failed;
            //ToastNotificationManager.CreateToastNotifier().Show(toast);

            IToastImageAndText02 toastContent = ToastContentFactory.CreateToastImageAndText02();
            toastContent.TextHeading.Text = title;
            toastContent.TextBodyWrap.Text = description;
            toastContent.Image.Src = toastImageSrc;
            toastContent.Image.Alt = toastImageSrc;
            toastContent.Audio.Content = toastAudioContent;
            ToastNotification toast = toastContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static void toast_Failed(ToastNotification sender, ToastFailedEventArgs args)
        {
            var errorCode = args.ErrorCode;
            return;
        }

        /// <summary>
        /// Post弹出消息对话框系统APP图片
        /// </summary>
        /// <param name="title">主题</param>
        /// <param name="description">内容</param>
        /// <param name="notificationSound">声音(默认是成功的声音)</param>
        public static void DisplayTextCustomTost(string title, string description, NotificationToastSound notificationSound = NotificationToastSound.Success)
        {
            IToastText02 toastContent = ToastContentFactory.CreateToastText02();
            toastContent.TextHeading.Text = title;
            toastContent.TextBodyWrap.Text = description;
            switch (notificationSound)
            {
                case NotificationToastSound.Success:
                    {
                        toastContent.Audio.Content = ToastAudioContent.Reminder;
                        break;
                    }
                case NotificationToastSound.Failure:
                    {
                        toastContent.Audio.Content = ToastAudioContent.Mail;
                        break;
                    }
                case NotificationToastSound.News:
                    {
                        toastContent.Audio.Content = ToastAudioContent.Default;
                        break;
                    }
                default:
                    {
                        toastContent.Audio.Content = ToastAudioContent.Default;
                        break;
                    }
            }
            ToastNotification toast = toastContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        /// <summary>
        /// Post弹出消息对话框可设置WEB的图片
        /// </summary>
        /// <param name="toastImageSrc">Web Uri</param>
        /// <param name="title">主题</param>
        /// <param name="description">内容</param>
        /// <param name="notificationSound">声音(默认是成功的声音)</param>
        public static void DisplayWebImageCustomToast(string toastImageSrc, string title, string description, NotificationToastSound notificationSound = NotificationToastSound.Success)
        {
            IToastImageAndText02 toastContent = ToastContentFactory.CreateToastImageAndText02();
            toastContent.TextHeading.Text = title;
            toastContent.TextBodyWrap.Text = description;
            toastContent.Image.Src = toastImageSrc;
            toastContent.Image.Alt = toastImageSrc;
            switch (notificationSound)
            {
                case NotificationToastSound.Success:
                    {
                        toastContent.Audio.Content = ToastAudioContent.Default;
                        break;
                    }
                case NotificationToastSound.Failure:
                    {
                        toastContent.Audio.Content = ToastAudioContent.Mail;
                        break;
                    }
                case NotificationToastSound.News:
                    {
                        toastContent.Audio.Content = ToastAudioContent.Reminder;
                        break;
                    }
                default:
                    {
                        toastContent.Audio.Content = ToastAudioContent.Default;
                        break;
                    }
            }
            ToastNotification toast = toastContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
