using System;
using System.Collections.Generic;

namespace text_btxt
{
    public sealed class Header
    {
        public byte[] Identifier; // 0x00 00 00 00 24 10 12 FF
        public ushort NumberOfLabels;
        public ushort NumberOfStrings;
        internal uint Size;
        internal uint LabelsOffset;
        internal short HasLabels;
        internal short Version;
        internal short NumberOfEntries;
        internal short FileSize;
    }

    public sealed class Label
    {
        internal uint TextOffset;
        internal short Text;
        internal uint ExtraOffset;
        internal char[] Extra;
        internal short TextID;
        internal short ExtraID;
        internal uint NameOffset;

        public uint StringCount { get; set; }

        public string Name { get; set; }
        public List<String> Strings { get; set; }

        public Label()
        {
            Name = string.Empty;
            Strings = new List<String>();
        }
    }

    public sealed class String
    {
        public uint ID { get; set; }
        public string Text { get; set; }

        public String()
        {
            ID = 0;
            Text = string.Empty;
        }
    }

    public sealed class InvalidBTXTException : Exception
    {
        public InvalidBTXTException(string message) : base(message) { }
    }
}