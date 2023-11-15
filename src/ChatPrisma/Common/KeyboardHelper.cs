using System.Windows.Input;

namespace ChatPrisma.Common;

public static class KeyboardHelper
{
    private static readonly Key[] s_allKeys = Enum.GetValues<Key>();

    public static bool AnyKeyPressed()
    {
        foreach (var key in s_allKeys)
        {
            // Skip the None key
            if (key == Key.None)
                continue;

            if (Keyboard.IsKeyDown(key))
                return true;
        }

        return false;
    }
}
