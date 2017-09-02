﻿using System;
using System.IO;
using System.Windows.Forms;
using Kuriimu.Compression;
using Kuriimu.IO;

namespace Kuriimu.UI
{
    public static class CompressionTools
    {
        public static void LoadCompressionTools(ToolStripMenuItem tsb)
        {
            ToolStripMenuItem tsb2;
            ToolStripMenuItem tsb3;
            ToolStripMenuItem tsb4;
            tsb.DropDownItems.Clear();

            //--------General---------
            tsb.DropDownItems.Add(new ToolStripMenuItem("General", null));
            tsb2 = (ToolStripMenuItem)tsb.DropDownItems[0];
            //  GZip
            tsb2.DropDownItems.Add(new ToolStripMenuItem("GZip", null));
            tsb3 = (ToolStripMenuItem)tsb2.DropDownItems[0];
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Compress", null, Compress));
            tsb3.DropDownItems[0].Tag = Compression.GZip;
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Decompress", null, Decompress));
            tsb3.DropDownItems[1].Tag = Compression.GZip;
            //  ZLib
            tsb2.DropDownItems.Add(new ToolStripMenuItem("ZLib", null));
            tsb3 = (ToolStripMenuItem)tsb2.DropDownItems[1];
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Compress", null, Compress));
            tsb3.DropDownItems[0].Tag = Compression.ZLib;
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Decompress", null, Decompress));
            tsb3.DropDownItems[1].Tag = Compression.ZLib;

            //-------Nintendo---------
            tsb.DropDownItems.Add(new ToolStripMenuItem("Nintendo", null));
            tsb2 = (ToolStripMenuItem)tsb.DropDownItems[1];

            //  Compress
            tsb2.DropDownItems.Add(new ToolStripMenuItem("Compress", null));
            tsb3 = (ToolStripMenuItem)tsb2.DropDownItems[0];
            //    LZ10
            tsb3.DropDownItems.Add(new ToolStripMenuItem("LZ10", null, Compress));
            tsb3.DropDownItems[0].Tag = Compression.NLZ10;
            //    LZ11
            tsb3.DropDownItems.Add(new ToolStripMenuItem("LZ11", null, Compress));
            tsb3.DropDownItems[1].Tag = Compression.NLZ11;
            //    LZ60
            tsb3.DropDownItems.Add(new ToolStripMenuItem("LZ60", null, Compress));
            tsb3.DropDownItems[2].Tag = Compression.NLZ60;
            //    Huffman
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Huffman", null));
            tsb4 = (ToolStripMenuItem)tsb3.DropDownItems[3];
            tsb4.DropDownItems.Add(new ToolStripMenuItem("4Bit", null, Compress));
            tsb4.DropDownItems[0].Tag = Compression.NHuff4;
            tsb4.DropDownItems.Add(new ToolStripMenuItem("8Bit", null, Compress));
            tsb4.DropDownItems[1].Tag = Compression.NHuff8;
            //    RLE
            tsb3.DropDownItems.Add(new ToolStripMenuItem("RLE", null, Compress));
            tsb3.DropDownItems[4].Tag = Compression.NRLE;
            //    LZ77
            tsb3.DropDownItems.Add(new ToolStripMenuItem("LZ77", null, Compress));
            tsb3.DropDownItems[5].Tag = Compression.LZ77;
            //    RevLZ77
            tsb3.DropDownItems.Add(new ToolStripMenuItem("RevLZ77", null, Compress));
            tsb3.DropDownItems[6].Tag = Compression.RevLZ77;
            //    LZOvl
            tsb3.DropDownItems.Add(new ToolStripMenuItem("LZOvl", null, Compress));
            tsb3.DropDownItems[7].Tag = Compression.LZOvl;
            //    Yaz0
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Yaz0", null, Compress));
            tsb3.DropDownItems[8].Tag = Compression.Yaz0;

            //  Decompress
            tsb2.DropDownItems.Add(new ToolStripMenuItem("Decompress", null));
            tsb3 = (ToolStripMenuItem)tsb2.DropDownItems[1];
            //    General
            tsb3.DropDownItems.Add(new ToolStripMenuItem("General", null, Decompress));
            tsb3.DropDownItems[0].Tag = Compression.Nintendo;
            //    LZ77
            tsb3.DropDownItems.Add(new ToolStripMenuItem("LZ77", null, Decompress));
            tsb3.DropDownItems[1].Tag = Compression.LZ77;
            //    RevLZ77
            tsb3.DropDownItems.Add(new ToolStripMenuItem("RevLZ77", null, Decompress));
            tsb3.DropDownItems[2].Tag = Compression.RevLZ77;
            //    LZOvl
            tsb3.DropDownItems.Add(new ToolStripMenuItem("LZOvl", null, Decompress));
            tsb3.DropDownItems[3].Tag = Compression.LZOvl;
            //    Yaz0
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Yaz0", null, Decompress));
            tsb3.DropDownItems[4].Tag = Compression.Yaz0;


            //-------Level 5---------
            tsb.DropDownItems.Add(new ToolStripMenuItem("Level 5", null));
            tsb2 = (ToolStripMenuItem)tsb.DropDownItems[2];

            //  Compress
            tsb2.DropDownItems.Add(new ToolStripMenuItem("Compress", null));
            tsb3 = (ToolStripMenuItem)tsb2.DropDownItems[0];
            //    LZ10
            tsb3.DropDownItems.Add(new ToolStripMenuItem("LZ10", null, Compress));
            tsb3.DropDownItems[0].Tag = Compression.L5LZ10;
            //    Huffman
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Huffman", null));
            tsb4 = (ToolStripMenuItem)tsb3.DropDownItems[1];
            tsb4.DropDownItems.Add(new ToolStripMenuItem("4Bit", null, Compress));
            tsb4.DropDownItems[0].Tag = Compression.L5Huff4;
            tsb4.DropDownItems.Add(new ToolStripMenuItem("8Bit", null, Compress));
            tsb4.DropDownItems[1].Tag = Compression.L5Huff8;
            //    RLE
            tsb3.DropDownItems.Add(new ToolStripMenuItem("RLE", null, Compress));
            tsb3.DropDownItems[2].Tag = Compression.L5RLE;

            //  Decompress
            tsb2.DropDownItems.Add(new ToolStripMenuItem("Decompress", null, Decompress));
            tsb2.DropDownItems[1].Tag = Compression.Level5;

            //-------Specific---------
            tsb.DropDownItems.Add(new ToolStripMenuItem("Specific", null));
            tsb2 = (ToolStripMenuItem)tsb.DropDownItems[3];

            //    LZECD
            tsb2.DropDownItems.Add(new ToolStripMenuItem("LZECD", null));
            tsb3 = (ToolStripMenuItem)tsb2.DropDownItems[0];
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Compress", null, Compress));
            tsb3.DropDownItems[0].Tag = Compression.LZECD;
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Decompress", null, Decompress));
            tsb3.DropDownItems[1].Tag = Compression.LZECD;
            //    LZ10VLE
            tsb2.DropDownItems.Add(new ToolStripMenuItem("LZ10VLE", null));
            tsb3 = (ToolStripMenuItem)tsb2.DropDownItems[1];
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Compress", null, Compress));
            tsb3.DropDownItems[0].Tag = Compression.LZ10VLE;
            tsb3.DropDownItems.Add(new ToolStripMenuItem("Decompress", null, Decompress));
            tsb3.DropDownItems[1].Tag = Compression.LZ10VLE;
        }

        public static bool PrepareFiles(string openCaption, string saveCaption, string saveExtension, out FileStream openFile, out FileStream saveFile)
        {
            openFile = null;
            saveFile = null;

            var ofd = new OpenFileDialog
            {
                Title = openCaption,
                Filter = "All Files (*.*)|*.*"
            };

            if (ofd.ShowDialog() != DialogResult.OK) return false;
            openFile = File.OpenRead(ofd.FileName);

            var sfd = new SaveFileDialog()
            {
                Title = saveCaption,
                FileName = Path.GetFileName(ofd.FileName) + saveExtension,
                Filter = "All Files (*.*)|*.*"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                openFile.Dispose();
                return false;
            }
            saveFile = File.Create(sfd.FileName);

            return true;
        }

        public static void Decompress(object sender, EventArgs e)
        {
            var tsi = sender as ToolStripMenuItem;

            if (!PrepareFiles("Open a " + tsi.Tag.ToString() + " compressed file...", "Save your decompressed file...", ".decomp", out FileStream openFile, out FileStream saveFile)) return;

            try
            {
                using (openFile)
                using (var outFs = new BinaryWriterX(saveFile))
                    switch (tsi.Tag)
                    {
                        case Compression.Level5:
                            outFs.Write(Level5.Decompress(openFile));
                            break;
                        case Compression.Nintendo:
                            outFs.Write(Nintendo.Decompress(openFile));
                            break;
                        case Compression.LZ77:
                            outFs.Write(LZ77.Decompress(openFile));
                            break;
                        case Compression.RevLZ77:
                            outFs.Write(RevLZ77.Decompress(openFile));
                            break;
                        case Compression.LZOvl:
                            outFs.Write(LZOvl.Decompress(openFile));
                            break;
                        case Compression.Yaz0:
                            outFs.Write(Yaz0.Decompress(openFile));
                            break;
                        case Compression.LZECD:
                            outFs.Write(LZECD.Decompress(openFile));
                            break;
                        case Compression.LZ10VLE:
                            outFs.Write(LZSSVLE.Decompress(openFile));
                            break;
                        case Compression.GZip:
                            outFs.Write(GZip.Decompress(openFile));
                            break;
                        case Compression.ZLib:
                            outFs.Write(ZLib.Decompress(openFile));
                            break;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), tsi?.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MessageBox.Show($"Successfully decompressed {Path.GetFileName(openFile.Name)}.", tsi?.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Compress(object sender, EventArgs e)
        {
            var tsi = sender as ToolStripMenuItem;

            if (!PrepareFiles("Open a decompressed " + tsi.Tag.ToString() + "file...", "Save your compressed file...", ".comp", out FileStream openFile, out FileStream saveFile)) return;

            try
            {
                using (openFile)
                using (var outFs = new BinaryWriterX(saveFile))
                    switch (tsi.Tag)
                    {
                        case Compression.L5LZ10:
                            outFs.Write(Level5.Compress(openFile, Level5.Method.LZ10));
                            break;
                        case Compression.L5Huff4:
                            outFs.Write(Level5.Compress(openFile, Level5.Method.Huffman4Bit));
                            break;
                        case Compression.L5Huff8:
                            outFs.Write(Level5.Compress(openFile, Level5.Method.Huffman8Bit));
                            break;
                        case Compression.L5RLE:
                            outFs.Write(Level5.Compress(openFile, Level5.Method.RLE));
                            break;
                        case Compression.NLZ10:
                            outFs.Write(Nintendo.Compress(openFile, Nintendo.Method.LZ10));
                            break;
                        case Compression.NLZ11:
                            outFs.Write(Nintendo.Compress(openFile, Nintendo.Method.LZ11));
                            break;
                        case Compression.NLZ60:
                            outFs.Write(Nintendo.Compress(openFile, Nintendo.Method.LZ60));
                            break;
                        case Compression.NHuff4:
                            outFs.Write(Nintendo.Compress(openFile, Nintendo.Method.Huff4));
                            break;
                        case Compression.NHuff8:
                            outFs.Write(Nintendo.Compress(openFile, Nintendo.Method.Huff8));
                            break;
                        case Compression.NRLE:
                            outFs.Write(Nintendo.Compress(openFile, Nintendo.Method.RLE));
                            break;
                        case Compression.LZ77:
                            outFs.Write(LZ77.Compress(openFile));
                            break;
                        case Compression.RevLZ77:
                            outFs.Write(RevLZ77.Compress(openFile));
                            break;
                        case Compression.LZOvl:
                            outFs.Write(LZOvl.Compress(openFile));
                            break;
                        case Compression.Yaz0:
                            outFs.Write(Yaz0.Compress(openFile));
                            break;
                        case Compression.GZip:
                            outFs.Write(GZip.Compress(openFile));
                            break;
                        case Compression.ZLib:
                            outFs.Write(ZLib.Compress(openFile));
                            break;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), tsi?.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MessageBox.Show($"Successfully compressed {Path.GetFileName(openFile.Name)}.", tsi?.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public enum Compression : short
        {
            ZLib,
            GZip,

            Level5,
            Nintendo,

            L5Huff4,
            L5Huff8,
            L5LZ10,
            L5RLE,

            NLZ10,
            NLZ11,
            NLZ60,
            NHuff4,
            NHuff8,
            NRLE,

            LZ77,
            RevLZ77,
            LZOvl,
            LZ10VLE,
            LZECD,

            Yaz0
        }
    }
}