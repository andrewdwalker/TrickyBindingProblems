
using Library.EnumDefinitions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace TrickyBindingProblems
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (ICar car in (DataContext as MainWindowVM).Cars)
            {
                Console.WriteLine(car.Color.ToString()); // NO CHANGES in Colors unfortunately!
            }

            foreach (IWeirdCar car in (DataContext as MainWindowVM).WeirdCars)
            {
                Console.WriteLine(car.Color.ToString()); // NO CHANGES in Colors unfortunately!
            }
        }
    }

    public class MainWindowVM
    {
        public MainWindowVM()
        {
            Cars = new ObservableCollection<ICar>();
            Cars.Add(new Car(MyColor.Green, "Honda"));
            Cars.Add(new Car(MyColor.Blue, "GM"));
            Cars.Add(new Car());

            WeirdCars = new ObservableCollection<IWeirdCar>();
            WeirdCars.Add(new WeirdCar("Green", "Honda"));
            WeirdCars.Add(new WeirdCar("Blue", "GM"));
            WeirdCars.Add(new WeirdCar());
        }
        public ObservableCollection<ICar> Cars { get; set; }
        public ObservableCollection<IWeirdCar> WeirdCars { get; set; }

    }

    public class Car : ICar
    {
        public Car()
        {
            Color = MyColor.Red;
            Make = "Ford";
        }
        public Car(MyColor color, string make)
        {
            Color = color;
            Make = make;
        }
        public MyColor Color { get; set; }
        public string Make { get; set; }
    }

    public interface ICar
    {
        MyColor Color { get; set; }
        string Make { get; set; }
    }

    public class WeirdCar : IWeirdCar
    {
        public WeirdCar()
        {
            Color = "Red";
            Make = "Ford";
        }
        public WeirdCar(string color, string make)
        {
            Color = color;
            Make = make;
        }
        public string Color { get; set; }
        public string Make { get; set; }
    }

    public interface IWeirdCar
    {
        string Color { get; set; }
        string Make { get; set; }
    }

    /// <summary>
    /// See: https://stackoverflow.com/questions/20290842/converter-to-show-description-of-an-enum-and-convert-back-to-enum-value-on-sele
    /// </summary>
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;

            return GetDescription((Enum)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }
    }

    public class StringToColorEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;

            MyColor color = (MyColor)Enum.Parse(typeof(MyColor), value as string, true);
            return color;
            //return GetDescription((Enum)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

}

