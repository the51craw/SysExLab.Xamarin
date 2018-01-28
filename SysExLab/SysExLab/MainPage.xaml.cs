using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SysExLab
{
    public partial class MainPage : ContentPage
    {
        public UIHandler uIHandler;
        private static MainPage mainPage;

        public interface IEventHandler
        {
            void GlobalHandler(object sender, EventArgs e);
        }
        
        public MainPage()
        {
            InitializeComponent();
            mainPage = this;
            Init();
        }

        public void Init()
        {
            //Button myButton = this.FindByName<Button>("MyButton");
            //myButton.Text = "Changed!";
            StackLayout mainStackLayout = this.FindByName<StackLayout>("MainStackLayout");
            uIHandler = new UIHandler(mainStackLayout);
            //uIHandler.Clear();
            //Button newButton = new Button();
            //newButton.Text = "New button!";
            //mainGrid.Children.Add(newButton);
        }

        public static MainPage GetMainPage()
        {
            return mainPage;
        }
    }
}
