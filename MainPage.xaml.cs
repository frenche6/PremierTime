using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Movies_Portable;
using Windows.UI.Notifications;
using Windows.ApplicationModel.Appointments;
using Windows.System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieList
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Movies_Portable.Controller mController = new Movies_Portable.Controller();
        
        ViewModel mViewModel = new ViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.DataContext = mViewModel;
            //getNext();
            AppState.Controller.userInputTitle = string.Empty;
            AppState.Controller.userInputYear = string.Empty;
            //mController.userInputTitle = string.Empty;
            //mController.userInputYear = string.Empty;
        }

        private async void getNext()
        {
            var lResults = await AppState.Controller.getNextPage();
            bool doFocus = mViewModel.Movies.Count > 0;
            foreach (var item in lResults)
            {
                mViewModel.Movies.Add(item);
            }

            if (doFocus)
            {
                uMovieListView.ScrollIntoView(mViewModel.Movies.LastOrDefault());
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            getNext();
        }

        private void uFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            var lButton = (Button)sender;
            var lDetail = (MovieDetails)lButton.DataContext;

            addReminder(lDetail);
            AddToCalendar(lDetail, lButton);

        }

        private void addReminder(MovieDetails aMovieDetails)
        {
            var lTemplate = ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText04);
            var lTextNodes = lTemplate.GetElementsByTagName("text");
            lTextNodes[0].AppendChild(lTemplate.CreateTextNode(aMovieDetails.title)); 
            lTextNodes[1].AppendChild(lTemplate.CreateTextNode(" has started playing in theaters."));
            //lTextNodes[2].AppendChild(lTemplate.CreateTextNode(""));

            DateTime lMovieDate = DateTime.Parse(aMovieDetails.release_date);
            if (lMovieDate > DateTime.Now)
            {
                ScheduledToastNotification lToast = new ScheduledToastNotification(lTemplate, lMovieDate);

                ToastNotificationManager.CreateToastNotifier().AddToSchedule(lToast);
            }
        }

        private async void AddToCalendar(MovieDetails aMovieDetails, FrameworkElement aElement)
        {
            var lAppointment = new Appointment();
            lAppointment.StartTime = DateTime.Parse(aMovieDetails.release_date);
            lAppointment.AllDay = true;
            lAppointment.Subject = aMovieDetails.title;
            var lRect = getElementRect(aElement);
            await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(lAppointment, lRect);
        }

        private Windows.Foundation.Rect getElementRect(FrameworkElement aElement)
        {
            var lTransform = aElement.TransformToVisual(null);
            var lPoint = lTransform.TransformPoint(new Windows.Foundation.Point());
            return new Rect(lPoint, new Size(aElement.ActualWidth, aElement.ActualHeight));
        }

        private void uFilterResults_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.Movies.Clear();
            AppState.Controller.resetPageCount();
            this.Frame.Navigate(typeof(FilterPage));
        }

        private void uLoadMoreResults_Click(object sender, RoutedEventArgs e)
        {
            getNext();
        }

        private void uRemoveFilter_Click(object sender, RoutedEventArgs e)
        {
            mViewModel.Movies.Clear();
            AppState.Controller.resetPageCount();
            AppState.Controller.userInputTitle = string.Empty;
            AppState.Controller.userInputYear = string.Empty;
            getNext();
        }

        private void uRateAndReview_Click(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + "4befc1da-7393-4e6e-b524-0456d24a38e6"));
        }

        /*
       

    Private Async Sub uAddToCalendarButton_Tapped(sender As Object, e As TappedRoutedEventArgs)
        Dim lViewModel = DirectCast(DataContext, ScheduleDetailViewModel)

        Dim lAppointment = New Appointment()
        lAppointment.StartTime = lViewModel.ScheduleDetail.AirDate
        lAppointment.Duration = New TimeSpan(0, lViewModel.ScheduleDetail.RuntimeInMinutes, 0)
        lAppointment.Location = lViewModel.ScheduleDetail.Series.Network.Name
        lAppointment.Reminder = New TimeSpan(0, 15, 0)
        lAppointment.Subject = lViewModel.ScheduleDetail.Series.Name
        Dim lDetails As String = lViewModel.ScheduleDetail.Name
        If Not String.IsNullOrEmpty(lViewModel.ScheduleDetail.Summary) Then
            lDetails += vbCrLf + StripTagsRegex(lViewModel.ScheduleDetail.Summary)
        End If
        lAppointment.Details = lDetails

        Dim lRect = GetElementRect(DirectCast(sender, FrameworkElement))

        Dim lId = Await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(lAppointment, lRect)
    End Sub

        */


    }
}
