﻿using System;
using System.Drawing;
using System.IO;
using Kuriimu.Contract;
using Kuriimu.IO;

namespace image_level5.XF
{
    public sealed class XfAdapter : IImageAdapter
    {
        private Bitmap _xf = null;

        #region Properties

        public string Name => "XF";
        public string Description => "Level 5 Font";
        public string Extension => "*.xf";
        public string About => "This is the XF image adapter for Kukkii.";

        // Feature Support
        public bool FileHasExtendedProperties => false;
        public bool CanSave => false;

        public FileInfo FileInfo { get; set; }

        #endregion

        public bool Identify(string filename)
        {
            using (var br = new BinaryReaderX(File.OpenRead(filename)))
            {
                if (br.BaseStream.Length < 4) return false;
                return br.ReadString(4) == "XPCK" && Path.GetExtension(filename) == ".xf";
            }
        }

        public LoadResult Load(string filename)
        {
            LoadResult result = LoadResult.Success;

            FileInfo = new FileInfo(filename);

            if (FileInfo.Exists)
                _xf = new XF(File.OpenRead(FileInfo.FullName)).bmp;
            else
                result = LoadResult.FileNotFound;

            return result;
        }

        public SaveResult Save(string filename = "")
        {
            SaveResult result = SaveResult.Success;

            if (filename.Trim() != string.Empty)
                FileInfo = new FileInfo(filename);

            try
            {
                //not implemented
            }
            catch (Exception)
            {
                result = SaveResult.Failure;
            }

            return result;
        }

        // Bitmaps
        public Bitmap Bitmap
        {
            get
            {
                return _xf;
            }
            set
            {
                _xf = value;
            }
        }

        public bool ShowProperties(Icon icon) => false;
    }
}
