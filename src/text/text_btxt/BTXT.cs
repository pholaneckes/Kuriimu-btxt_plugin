using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Kontract.IO;

namespace text_btxt
{
    public class BTXT
    {
        public Header Header = new Header();
        public List<uint> StringCount = new List<uint>();
        public List<uint> StringCountID = new List<uint>();
        public List<uint> Offsets = new List<uint>();
        public List<byte> Lab = new List<byte>();
        public List<Label> Labels = new List<Label>();
        public Encoding FileEncoding = Encoding.Unicode;

        //读取
        public BTXT(string filename)
        {
            using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryReaderX br = new BinaryReaderX(fs);

                // Header 文件头
                Header.Identifier = br.ReadBytes(8);
                //System.Diagnostics.Process.Start("explorer.exe","https://www." + BitConverter.ToString(Header.Identifier));
                if (!Header.Identifier.SequenceEqual(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x24, 0x10, 0x12, 0xFF }))
                    throw new InvalidBTXTException("The file provided is not a valid BTXT file.");
                Header.NumberOfLabels = br.ReadUInt16();
                Header.NumberOfStrings = br.ReadUInt16();

                // Create labels 创建K1的栏
                for (int i = 0; i < Header.NumberOfLabels; i++)
                    Labels.Add(new Label());

                // Attributes 属性
                for (int i = 0; i < Header.NumberOfLabels; i++)
                {
                    Label label = Labels[i];
                    label.StringCount = br.ReadUInt32();
                    StringCount.Add(label.StringCount);

                    for (int j = 0; j < label.StringCount; j++)
                    {
                        String str = new String();
                        str.ID = br.ReadUInt32();
                        label.Strings.Add(str);
                        StringCountID.Add(str.ID);
                    }
                }

                // Offsets 偏移
                for (int i = 0; i < Header.NumberOfLabels + Header.NumberOfStrings; i++)
                    Offsets.Add(br.ReadUInt32());

                // Set the offset start position
                uint offsetStart = (uint)br.BaseStream.Position;
                Offsets.Add((uint)br.BaseStream.Length - offsetStart); // Add an extra offset at the end

                // Labels 文本段名
                for (int i = 0; i < Header.NumberOfLabels; i++)
                {
                    Label label = Labels[i];
                    byte[] lab = br.ReadBytes((int)(Offsets[i + 1] - Offsets[i]));
                    label.Name = Encoding.ASCII.GetString(lab).TrimEnd('\0');
                    for(int j = 0; j < lab.Length; j++)
                    {
                        Lab.Add(lab[j]);
                    }
                }

                // Text 文本记录
                int index = 0;
                for (int i = 0; i < Header.NumberOfLabels; i++)
                {
                    Label label = Labels[i];

                    for (int j = 0; j < label.StringCount; j++)
                    {
                        String str = label.Strings[j];
                        str.Text = FileEncoding.GetString(br.ReadBytes((int)(Offsets[Header.NumberOfLabels + index + 1] - Offsets[Header.NumberOfLabels + index]))).TrimEnd('\0');
                        index++;
                    }
                }

                br.Close();
            }
        }

        //保存（写入）
        public bool Save(string filename)
        {
        	bool result = false;

        	try
        	{
        		using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
        		{
        			BinaryWriterX bw = new BinaryWriterX(fs);

                    //	Header  文件头
                    bw.Write(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x24, 0x10, 0x12, 0xFF });
                    bw.Write(Header.NumberOfLabels);             
                    bw.Write(Header.NumberOfStrings);

                    // Attribute  属性
                    byte kon = 0x0;
                    for (int i = 0; i < Header.NumberOfLabels; i++)
                    {
                        bw.Write(StringCount[i]);
                        for (int j = 0; j < StringCount[i]; j++)
                        {
                            Label label = Labels[i];
                            List<string> listLine = new List<string>();
                            listLine = label.Strings[j].Text.Split('\u00A0').ToList();
                            byte singleLine = (byte)TheLongestName(listLine).Length;
                            int num = Regex.Matches(label.Strings[j].Text, "\u00A0").Count;
                            byte lines = (byte)(num+1);
                            byte strs = (byte)label.Strings[j].Text.Length;
                            if(strs % 2 == 1)
                            {
                                strs++;
                            }
                            bw.Write(new byte[] { singleLine, lines, strs, kon });
                            kon++;
                        }
                    }
                    // Offsets  偏移
                    uint last = Offsets[Header.NumberOfLabels];
                    for (int i = 0; i < Header.NumberOfLabels; i++)
                    {
                        bw.Write(Offsets[i]);
                    }
                    // Labels  文本段名
                    for (int i = 0; i < Header.NumberOfStrings; i++)
                    {
                        for (int j = 0; j < StringCount[i]; j++)
                        {
                            Label label = Labels[i];
                            byte strs = (byte)label.Strings[j].Text.Length;
                            if (strs % 2 == 1)
                            {
                                strs++;
                            }
                            
                            bw.Write(last);
                            last = (uint)(last + strs * 2 + 4);
                        }
                    }
                    // Text 文本记录
                    for (int i = 0; i < Lab.Count; i++)
                    {
                        bw.Write(Lab[i]);
                    }
                    for (int i = 0; i < Header.NumberOfLabels; i++)
                    {
                        Label label = Labels[i];
                        for (int j = 0; j < label.StringCount; j++)
                        {
                            bw.Write(Encoding.Unicode.GetBytes(label.Strings[j].Text));
                            if (label.Strings[j].Text.Length % 2 == 0)
                            {
                                bw.Write(new byte[] { 0x0, 0x0, 0x0, 0x0 });
                            }
                            else
                            {
                                bw.Write(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                            }
                        }
                    }














                                                                                                                                                                //bw.WriteASCII(Header.Identifier);
                                                                                                                                                                // 		uint fileSizeOffset = (uint)bw.BaseStream.Position;
                                                                                                                                                                //bw.Write(Header.FileSize);
                                                                                                                                                                //bw.Write(Header.NumberOfEntries);
                                                                                                                                                                //bw.Write(Header.Version);
                                                                                                                                                                //bw.Write(Header.HasLabels);
                                                                                                                                                                //uint labelsOffset = (uint)bw.BaseStream.Position;
                                                                                                                                                                //bw.Write(Header.LabelsOffset - Header.Size);

                                                                                                                                                                //uint entryStart = Header.Size;
                                                                                                                                                                //uint textStart = (uint)bw.BaseStream.Position + (uint)(Labels.Count * 2 * 8);

                                                                                                                                                                //			Text
                                                                                                                                                                //bw.BaseStream.Seek(textStart, SeekOrigin.Begin);
                                                                                                                                                                //for (int i = 0; i < Header.NumberOfEntries; i++)
                                                                                                                                                                //{
                                                                                                                                                                //	Label label = Labels[i];
                                                                                                                                                                //	label.TextOffset = (uint)bw.BaseStream.Position - Header.Size;
                                                                                                                                                                //	bw.Write(label.Text);
                                                                                                                                                                //	bw.Write(new byte[] { 0x0, 0x0 });
                                                                                                                                                                //}

                                                                                                                                                                //			Extra
                                                                                                                                                                //for (int i = 0; i < Header.NumberOfEntries; i++)
                                                                                                                                                                //{
                                                                                                                                                                //	Label label = Labels[i];
                                                                                                                                                                //	label.ExtraOffset = (uint)bw.BaseStream.Position - Header.Size;
                                                                                                                                                                //	bw.Write(FileEncoding.GetBytes(label.Extra));
                                                                                                                                                                //	bw.Write(new byte[] { 0x0, 0x0 });
                                                                                                                                                                //}

                                                                                                                                                                //			Pad to the nearest 8 bytes
                                                                                                                                                                //PaddingWrite(bw);

                                                                                                                                                                //			Set label offset variables
                                                                                                                                                                //uint labelsOffsets = (uint)bw.BaseStream.Position;
                                                                                                                                                                //uint labelsStart = (uint)bw.BaseStream.Position + (uint)(Labels.Count * 4);

                                                                                                                                                                //			Grab the new LabelsOffset
                                                                                                                                                                //if (Header.HasLabels == 0x0101)
                                                                                                                                                                //	Header.LabelsOffset = (uint)bw.BaseStream.Position - Header.Size;
                                                                                                                                                                //else
                                                                                                                                                                //	Header.LabelsOffset = 0;

                                                                                                                                                                //			Text Offsets
                                                                                                                                                                //bw.BaseStream.Seek(entryStart, SeekOrigin.Begin);
                                                                                                                                                                //   for (int i = 0; i < Header.NumberOfEntries; i++)
                                                                                                                                                                //{
                                                                                                                                                                //	Label label = Labels[i];
                                                                                                                                                                //	bw.Write(label.TextID);
                                                                                                                                                                //	bw.Write(label.TextOffset);
                                                                                                                                                                //}
                                                                                                                                                                //			Extra Offsets
                                                                                                                                                                //for (int i = 0; i < Header.NumberOfEntries; i++)
                                                                                                                                                                //{
                                                                                                                                                                //	Label label = Labels[i];
                                                                                                                                                                //	bw.Write(label.ExtraID);
                                                                                                                                                                //	bw.Write(label.ExtraOffset);
                                                                                                                                                                //}

                                                                                                                                                                //			Labels
                                                                                                                                                                //if (Header.HasLabels == 0x0101)
                                                                                                                                                                //{
                                                                                                                                                                //				Label Names
                                                                                                                                                                //bw.BaseStream.Seek(labelsStart, SeekOrigin.Begin);
                                                                                                                                                                //for (int i = 0; i < Header.NumberOfEntries; i++)
                                                                                                                                                                //{
                                                                                                                                                                //	Label label = Labels[i];
                                                                                                                                                                //	label.NameOffset = (uint)bw.BaseStream.Position - Header.Size;
                                                                                                                                                                //	bw.WriteASCII(label.Name);
                                                                                                                                                                //	bw.Write((byte)0x0);
                                                                                                                                                                //}

                                                                                                                                                                //				Pad to the nearest 8 bytes
                                                                                                                                                                //PaddingWrite(bw);
                                                                                                                                                                //				Grab the new filesize
                                                                                                                                                                //Header.FileSize = (short)(uint)bw.BaseStream.Position;

                                                                                                                                                                //				Label Offsets
                                                                                                                                                                //bw.BaseStream.Seek(labelsOffsets, SeekOrigin.Begin);
                                                                                                                                                                //for (int i = 0; i < Header.NumberOfEntries; i++)
                                                                                                                                                                //	bw.Write(Labels[i].NameOffset);
                                                                                                                                                                //}

                                                                                                                                                                //			Update LabelsOffset
                                                                                                                                                                //bw.BaseStream.Seek(labelsOffset, SeekOrigin.Begin);
                                                                                                                                                                //bw.Write(Header.LabelsOffset);

                                                                                                                                                                //			Update FileSize
                                                                                                                                                                //bw.BaseStream.Seek(fileSizeOffset, SeekOrigin.Begin);
                                                                                                                                                                //bw.Write(Header.FileSize);

                    bw.Close();
        		}

        		result = true;
        	}
        	catch (Exception)
        	{ }

        	return result;
        }

        private static string TheLongestName(List<string> array)
        {
            string longest = array[0];
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].Length > longest.Length)
                {
                    longest = array[i];
                }
            }
            return longest;
        }

        private void PaddingWrite(BinaryWriterX bw, int alignment = 8)
        {
            long remainder = bw.BaseStream.Position % alignment;
            if (remainder > 0)
                for (int i = 0; i < alignment - remainder; i++)
                    bw.Write((byte)0x0);
        }
    }
}