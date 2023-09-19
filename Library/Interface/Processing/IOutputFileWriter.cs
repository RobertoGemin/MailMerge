﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interface.Processing
{
    public interface IOutputFileWriter
    {
        void WriteOutputFile(string outputDir, string orderId, string content);
    }
}