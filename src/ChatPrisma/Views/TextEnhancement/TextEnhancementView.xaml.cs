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

    private void TextEnhancementView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is TextEnhancementViewModel oldViewModel)
        {
            oldViewModel.ApplyInstructionCancelled -= this.ViewModelOnApplyInstructionCancelled;
        }

        if (e.NewValue is TextEnhancementViewModel newViewModel)
        {
            newViewModel.ApplyInstructionCancelled += this.ViewModelOnApplyInstructionCancelled;
        }
    }

    private void ViewModelOnApplyInstructionCancelled(object? sender, EventArgs e)
    {
        // Set cursor at the end of the instruction textbox
        this.InstructionTextBox.Select(this.InstructionTextBox.Text.Length, 0);
    }
}