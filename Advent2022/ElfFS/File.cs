using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.ElfFS
{
    public class File
    {
        public Folder Folder { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }

        public File(Folder folder, string name, long size)
        {
            Folder = folder;
            Name = name;
            Size = size;
        }

        public void MoveTo(Folder folder)
        {
            Folder = folder;
        }
    }
}
