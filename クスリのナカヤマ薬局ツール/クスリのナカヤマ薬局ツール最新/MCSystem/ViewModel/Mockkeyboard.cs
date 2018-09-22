using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MCSystem.ViewModel
{
  static class MockKeyboard
  {
    static Dictionary<char, Key> KeyPresses;
    static MockKeyboard()
    {
      KeyPresses = new Dictionary<char, Key>();

      KeyPresses.Add('\b', Key.Backspace);
      KeyPresses.Add('\t', Key.Tab);
      KeyPresses.Add('\n', Key.Return);
      KeyPresses.Add('\x7F', Key.Delete);
      
      KeyPresses.Add(' ', Key.Space);
      KeyPresses.Add('!', Key.Exclamation);
      KeyPresses.Add('\"', Key.Quotes);
      KeyPresses.Add('#', Key.Pound);
      KeyPresses.Add('$', Key.Dollar);
      KeyPresses.Add('%', Key.Percent);
      KeyPresses.Add('&', Key.Ampersand);
      KeyPresses.Add('\'', Key.Apostrophe);

      KeyPresses.Add('(', Key.LeftParen);
      KeyPresses.Add(')', Key.RightParen);
      KeyPresses.Add('*', Key.Asterisk);
      KeyPresses.Add('+', Key.Plus);
      KeyPresses.Add(',', Key.Comma);
      KeyPresses.Add('-', Key.Hyphen);
      KeyPresses.Add('.', Key.Period);
      KeyPresses.Add('/', Key.Slash);

      KeyPresses.Add('0', Key.D0);
      // The rest of the digits
      KeyPresses.Add('9', Key.D9);

      KeyPresses.Add(':', Key.Colon);
      KeyPresses.Add(';', Key.Semicolon);
      KeyPresses.Add('<', Key.LessThan);
      KeyPresses.Add('=', Key.Equal);
      KeyPresses.Add('>', Key.GreaterThan);
      KeyPresses.Add('?', Key.Question);
      KeyPresses.Add('@', Key.At);

      KeyPresses.Add('A', Key.A);
      // The rest of the capital letters
      KeyPresses.Add('Z', Key.Z);

      KeyPresses.Add('[', Key.LeftBracket);
      KeyPresses.Add('\\', Key.BackSlash);
      KeyPresses.Add(']', Key.RightBracket);
      KeyPresses.Add('^', Key.Caret);
      KeyPresses.Add('_', Key.Underscore);
      KeyPresses.Add('`', Key.Backquote);
      
      KeyPresses.Add('a', Key.a);
      // The rest of the lower-case letters
      KeyPresses.Add('z', Key.z);

      KeyPresses.Add('{', Key.LeftBrace);
      KeyPresses.Add('|', Key.Pipe);
      KeyPresses.Add('}', Key.RightBrace);
      KeyPresses.Add('~', Key.Tilde);
    }

    public static void Press(char c)
    {
      if (KeyPresses.ContainsKey(c))
        Press(KeyPresses[c]);
    }

    public static void Type(string s)
    {
      foreach (char c in s.ToCharArray())
        Press(c);
    }

    public static void Press(Key key)
    {
      if (key.Modifiers == Modifiers.None && key.vKey != 0)
      {
        keybd_event((byte)key.vKey, 0, KEYEVENTF_KEYDOWN, 0);
        keybd_event((byte)key.vKey, 0, KEYEVENTF_KEYUP, 0);
      }
      else if (0 != (Modifiers.Shift & key.Modifiers))
      {
        ShiftPress(key);
      }
      // Other modifier logic
    }

    public static void ShiftPress(Key key)
    {
      keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYDOWN, 0);
      Press(new Key(key.vKey, key.Modifiers & ~Modifiers.Shift));
      keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, 0);
    }

    public class Key
    {
      public Key(byte vKey, Modifiers modifiers)
      {
        this.vKey = vKey;
        Modifiers = modifiers;
      }

      public byte vKey;
      public Modifiers Modifiers;

      public static Key None { get { return new Key((byte)0, Modifiers.None); } }

      public static Key Backspace { get { return new Key(VK_BACK, Modifiers.None); } }
      public static Key Tab { get { return new Key(VK_TAB, Modifiers.None); } }
      public static Key Return { get { return new Key(VK_RETURN, Modifiers.None); } }
      public static Key Delete { get { return new Key(VK_DELETE, Modifiers.None); } }

      public static Key Space { get { return new Key(VK_SPACE, Modifiers.None); } }
      public static Key Exclamation { get { return new Key(VK_1, Modifiers.Shift); } }
      public static Key Quotes { get { return new Key(VK_APOSTROPHE, Modifiers.Shift); } }
      public static Key Pound { get { return new Key(VK_3, Modifiers.Shift); } }
      public static Key Dollar { get { return new Key(VK_4, Modifiers.Shift); } }
      public static Key Percent { get { return new Key(VK_5, Modifiers.Shift); } }
      public static Key Ampersand { get { return new Key(VK_7, Modifiers.Shift); } }
      public static Key Apostrophe { get { return new Key(VK_APOSTROPHE, Modifiers.None); } }

      public static Key LeftParen { get { return new Key(VK_9, Modifiers.Shift); } }
      public static Key RightParen { get { return new Key(VK_0, Modifiers.Shift); } }
      public static Key Asterisk { get { return new Key(VK_8, Modifiers.Shift); } }
      public static Key Plus { get { return new Key(VK_EQUAL, Modifiers.Shift); } }
      public static Key Comma { get { return new Key(VK_COMMA, Modifiers.None); } }
      public static Key Hyphen { get { return new Key(VK_HYPHEN, Modifiers.None); } }
      public static Key Period { get { return new Key(VK_PERIOD, Modifiers.None); } }
      public static Key Slash { get { return new Key(VK_SLASH, Modifiers.None); } }

      public static Key D0 { get { return new Key(VK_0, Modifiers.None); } }
      // The rest of the digits
      public static Key D9 { get { return new Key(VK_9, Modifiers.None); } }

      public static Key Colon { get { return new Key(VK_SEMICOLON, Modifiers.Shift); } }
      public static Key Semicolon { get { return new Key(VK_SEMICOLON, Modifiers.None); } }
      public static Key LessThan { get { return new Key(VK_COMMA, Modifiers.Shift); } }
      public static Key Equal { get { return new Key(VK_EQUAL, Modifiers.None); } }
      public static Key GreaterThan { get { return new Key(VK_PERIOD, Modifiers.Shift); } }
      public static Key Question { get { return new Key(VK_SLASH, Modifiers.Shift); } }
      public static Key At { get { return new Key(VK_2, Modifiers.Shift); } }

      public static Key A { get { return new Key(VK_A, Modifiers.None); } }
      // The rest of the capital letters
      public static Key Z { get { return new Key(VK_Z, Modifiers.None); } }

      public static Key LeftBracket { get { return new Key(VK_LBRACKET, Modifiers.None); } }
      public static Key BackSlash { get { return new Key(VK_BACKSLASH, Modifiers.None); } }
      public static Key RightBracket { get { return new Key(VK_RBRACKET, Modifiers.None); } }
      public static Key Caret { get { return new Key(VK_6, Modifiers.Shift); } }
      public static Key Underscore { get { return new Key(VK_HYPHEN, Modifiers.Shift); } }
      public static Key Backquote { get { return new Key(VK_BACKQUOTE, Modifiers.None); } }

      public static Key a { get { return new Key(VK_A, Modifiers.Shift); } }
      // The rest of the lower-case letters
      public static Key z { get { return new Key(VK_Z, Modifiers.Shift); } }

      public static Key LeftBrace { get { return new Key(VK_LBRACKET, Modifiers.Shift); } }
      public static Key Pipe { get { return new Key(VK_BACKSLASH, Modifiers.Shift); } }
      public static Key RightBrace { get { return new Key(VK_RBRACKET, Modifiers.Shift); } }
      public static Key Tilde { get { return new Key(VK_BACKQUOTE, Modifiers.Shift); } }

    }

    //[Flags]
    public enum Modifiers : uint
    {
      None = 0,
      Alt = MOD_ALT,
      Control = MOD_CONTROL,
      Shift = MOD_SHIFT,
      Windows = MOD_WIN
    }

    public const int MOD_ALT = 0x0100;
    public const int MOD_CONTROL = 0x0200;
    public const int MOD_SHIFT = 0x0400;
    public const int MOD_WIN = 0x0800;

    public const byte VK_BACK = (byte)0x08;
    public const byte VK_TAB = (byte)0x09;
    public const byte VK_RETURN = (byte)0x0D;
    public const byte VK_SHIFT = (byte)0x10;
    public const byte VK_SPACE = (byte)0x20;
    public const byte VK_DELETE = (byte)0x2E;
    public const byte VK_ESCAPE = (byte)0x1B;

    public const byte VK_0 = (byte)0x30;
    public const byte VK_1 = (byte)0x31;
    public const byte VK_2 = (byte)0x32;
    public const byte VK_3 = (byte)0x33;
    public const byte VK_4 = (byte)0x34;
    public const byte VK_5 = (byte)0x35;
    public const byte VK_6 = (byte)0x36;
    public const byte VK_7 = (byte)0x37;
    public const byte VK_8 = (byte)0x38;
    public const byte VK_9 = (byte)0x39;

    public const byte VK_A = (byte)0x41;
    // The rest of the letters
    public const byte VK_Z = (byte)0x5A;

    public const byte VK_SEMICOLON = (byte)0xBA;
    public const byte VK_EQUAL = (byte)0xBB;
    public const byte VK_COMMA = (byte)0xBC;
    public const byte VK_HYPHEN = (byte)0xBD;
    public const byte VK_PERIOD = (byte)0xBE;
    public const byte VK_SLASH = (byte)0xBF;
    public const byte VK_BACKQUOTE = (byte)0xC0;
    public const byte VK_LBRACKET = (byte)0xD9;
    public const byte VK_BACKSLASH = (byte)0xDA;
    public const byte VK_RBRACKET = (byte)0xDB;
    public const byte VK_APOSTROPHE = (byte)0xDC;

    public const int KEYEVENTF_KEYUP = 0x2;
    public const int KEYEVENTF_KEYDOWN = 0x0;

    [DllImport("coredll.dll", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
  }
}