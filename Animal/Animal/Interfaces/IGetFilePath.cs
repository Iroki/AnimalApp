using System;
using System.Collections.Generic;
using System.Text;

namespace Animal
{
   public interface IGetFilePath
    {
        string GetFullPath(string relativeFilePath);
    }
}
