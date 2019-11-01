using System;

namespace MediaRemoteMe
{
    partial class MainPage
    {
        //Register application page events
        void ApplicationEventsRegister()
        {
            try
            {
                //Register application activated event
                App.OnApplicationActivated += OnApplicationActivatedEvent;
            }
            catch { }
        }

        //Disable application page events
        void ApplicationEventsDisable()
        {
            try
            {
                //Disable application activated event
                App.OnApplicationActivated -= OnApplicationActivatedEvent;
            }
            catch { }
        }

        //Handle Application Activated Event
        async void OnApplicationActivatedEvent(object sender, EventArgs e) { await ApplicationNavigation(); }
    }
}