using System.Windows;
using System.Windows.Input;

namespace ChatPrisma.Themes;

public partial class Stepper
{
    public Stepper()
    {
        this.InitializeComponent();
    }

    public static readonly DependencyProperty PreviousCommandProperty = DependencyProperty.Register(
        nameof(PreviousCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(default(ICommand)));

    public ICommand PreviousCommand
    {
        get { return (ICommand)GetValue(PreviousCommandProperty); }
        set { SetValue(PreviousCommandProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(int), typeof(Stepper), new PropertyMetadata(default(int)));

#pragma warning disable CA1721
    public int Value
    {
        get { return (int)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
#pragma warning restore CA1721

    public static readonly DependencyProperty NextCommandProperty = DependencyProperty.Register(
        nameof(NextCommand), typeof(ICommand), typeof(Stepper), new PropertyMetadata(default(ICommand)));

    public ICommand NextCommand
    {
        get { return (ICommand)GetValue(NextCommandProperty); }
        set { SetValue(NextCommandProperty, value); }
    }
}
