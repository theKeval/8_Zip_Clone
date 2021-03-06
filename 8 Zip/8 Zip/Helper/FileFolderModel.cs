﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace _8_Zip.Helper
{
    public class FileFolderModel
    {
        public string name { get; set; }
        public bool isFolder { get; set; }
        public long size { get; set; }
        public string ratio { get; set; }
        public IStorageItem File { get; set; }
        public bool IsRoot { get; set; }
        public string Ratio { get; set; }
        public string Size { get; set; }

        public long compressedSize { get; set; }
        
    }
}
