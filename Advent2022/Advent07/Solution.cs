using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Advent2022.Advent07
{
    public class Solution : ISolution
    {
        
        public Folder root;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            root = new Folder(null, "");
            foreach (var line in lines) ParseLine(line);
        }


        private Folder current;
        private void ParseLine(string input)
        {
            if (input == "$ cd /")
            {
                current = root;
            }
            else if (input == "$ cd ..")
            {
                current = current.Parent;
            }
            else if (input.StartsWith("$ cd"))
            {
                var name = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last();

                current = current.GoToSubFolder(name);
            }
            else if (input == "$ ls")
            {

            }
            else if (input.StartsWith("dir "))
            {
                
            }
            else
            {
                var file = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var size = long.Parse(file[0]);
                var name = file[1];

                current.AddOrUpdateFile(name, size);
            }
        }

        public Solution() : this("Input.txt") { }

        public class Folder
        {
            public Folder Parent;
            public string Name;
            public string Path;

            public List<Folder> SubFolders;
            public List<File> Files;

            public long OwnSize => Files.Sum(f => f.Size);
            public long TotalSize => OwnSize + SubFolders.Sum(s => s.TotalSize);

            [ComplexParserConstructor]
            public Folder(Folder parent, string name)
            {
                Parent = parent;
                Name = name;
                Path = (parent?.Path ?? "") + "/" + name;

                SubFolders = new();
                Files = new();
            }

            public Folder GoToSubFolder(string name)
            {
                if (!SubFolders.Any(sf => sf.Name == name)) SubFolders.Add(new(this, name));

                return SubFolders.Single(sf => sf.Name == name);
            }

            public void AddOrUpdateFile(string name, long size)
            {
                if (!Files.Any(f => f.Name == name)) Files.Add(new(this, name, size));

                var file = Files.Single(f => f.Name == name);
                file.Size = size;
            }

            public List<Folder> GetFolderAndAllSubFolders() => GetFolderAndAllSubFolders(new());

            public List<Folder> GetFolderAndAllSubFolders(List<Folder> folders)
            {
                folders.Add(this);

                foreach (var subFolder in SubFolders) subFolder.GetFolderAndAllSubFolders(folders);

                return folders;
            }
        }

        public class File
        {
            public Folder Folder;

            public string Name;
            public long Size;

            public File(Folder folder, string name, long size)
            {
                Folder= folder;
                Name= name;
                Size = size;
            }
        }

        public object GetResult1()
        {
            return root.GetFolderAndAllSubFolders()
                .Where(f => f.TotalSize <= 100000)
                .Sum(f => f.TotalSize);
        }

        public object GetResult2()
        {
            long emptySpace = 70000000 - root.TotalSize;

            var requiredSpace = 30000000 - emptySpace;

            return root.GetFolderAndAllSubFolders()
                .Select(f => f.TotalSize)
                .Where(f => f >= requiredSpace)
                .Min();
        }
    }
}
