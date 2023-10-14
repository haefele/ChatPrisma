using System.Windows;

namespace ChatPrisma.Views.TextEnhancement;

public partial class TextEnhancementView
{
    public TextEnhancementView()
    {
        this.InitializeComponent();
    }

    private void TextEnhancementView_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.InstructionTextBox.Focus();
    }
}