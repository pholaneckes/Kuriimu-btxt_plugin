﻿using System;
using System.Drawing;
using System.IO;
using Kuriimu.Contract;
using Kuriimu.IO;

namespace image_nintendo.BXLIM
{
    public class BxlimAdapter : IImageAdapter
    {
        private Cetera.Image.BXLIM _bxlim = null;

        #region Properties

        // Information
        public string Name => "BXLIM";
        public string Description => "Binary Layout Image";
        public string Extension => "*.bclim;*.bflim";
        public string About => "This is the BCLIM and BFLIM image adapter for Kukkii.";

        // Feature Support
        public bool FileHasExtendedProperties => false;
        public bool CanSave => true;

        public FileInfo FileInfo { get; set; }

        #endregion

        public bool Identify(string filename)
        {
            using (var br = new BinaryReaderX(File.OpenRead(filename)))
            {
                if (br.BaseStream.Length < 40) return false;
                br.BaseStream.Seek((int)br.BaseStream.Length - 40, SeekOrigin.Begin);
                string magic = br.ReadString(4);
                return magic == "CLIM" || magic == "FLIM";
            }
        }

        public LoadResult Load(string filename)
        {
            LoadResult result = LoadResult.Success;

            FileInfo = new FileInfo(filename);

            if (FileInfo.Exists)
                _bxlim = new Cetera.Image.BXLIM(FileInfo.OpenRead());
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
                _bxlim.Save(FileInfo.Create());
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
                return _bxlim.Image;
            }
            set
            {
                _bxlim.Image = value;
            }
        }

        public bool ShowProperties(Icon icon) => false;
    }
}
