using Android.OS;
using System.Xml.Serialization;

namespace BatteryApp;

public partial class MainPage : ContentPage
{
	int counter = 0;
    bool pause = false;
    static int counterLimit = 25000;
    int current = 0;
    //int[] currentArray = new int[counterLimit];

    [Android.Runtime.Register("android/os/BatteryManager", DoNotGenerateAcw = true)]

    BatteryManager bm;
    BatteryProperty bp;

    //https://stackoverflow.com/questions/64532112/batterymanagers-battery-property-current-now-returning-0-or-incorrect-current-v
    //https://github.com/dotnet/maui/discussions/8681
    public MainPage()
	{
        bm = new BatteryManager();
        bp = new BatteryProperty();

		InitializeComponent();

        Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;

        var current = bm.GetIntProperty(2);
        //if (Math.Abs(current / 1000) < 1.0) 
        //{
        //    current = current * 1000;
        //}
        var status = bm.GetIntProperty(6);
        BatteryInfo.Text = (Battery.ChargeLevel * 100).ToString() + "%\nCurrent: " + current + " mAh\nStatus: " + Battery.State.ToString();

        if (pause == false)
        {
            //meaning currently running.
            CounterBtn.Text = "Pause";
        }
        else
        {
            CounterBtn.Text = "Unpause";
        }
        UpdateBatteryInfo();
    }

    private async void UpdateBatteryInfo()
    {
        while (!pause) {
            //currentArray[counter - 1] = current;
            if (counter >= counterLimit)
            {
                current = bm.GetIntProperty(2);
                //int currentAvg = Convert.ToInt32(currentArray.Sum() / counterLimit);
                var status = bm.GetIntProperty(6);
                BatteryInfo.Text = (Battery.ChargeLevel * 100).ToString() + "%\nCurrent: " + current + " mAh\nStatus: " + Battery.State.ToString();
                counter = 0;
            }
            counter++;
            await Task.Yield();
        }
    }

    private void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
    {
        current = bm.GetIntProperty(2);
        //int currentAvg = Convert.ToInt32(currentArray.Sum() / counterLimit);
        var status = bm.GetIntProperty(6);
        BatteryInfo.Text = (Battery.ChargeLevel * 100).ToString() + "%\nCurrent: " + current + " mAh\nStatus: " + Battery.State.ToString();
        counter = 0;
    }

    private void OnCounterClicked(object sender, EventArgs e)
	{
        if (pause)
        {
            pause = false;
            CounterBtn.Text = "Pause";
            UpdateBatteryInfo();
        }
        else
        {
            pause = true;
            CounterBtn.Text = "Unpause";
            UpdateBatteryInfo();
        }

    }
}

