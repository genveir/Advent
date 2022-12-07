using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.ElfFileSystem
{
    public class Folder
    {
        public Folder Parent;
        public string Name;
        public string Path;

        public bool HasBeenListed;

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

        public void EnsureSubFolder(string name)
        {
            if (!SubFolders.Any(sf => sf.Name == name)) SubFolders.Add(new(this, name));
        }

        public Folder GoToSubFolder(string name, bool ensure)
        {
            if (ensure) EnsureSubFolder(name);

            return SubFolders.Single(sf => sf.Name == name);
        }

        public void AddOrUpdateFile(string name, long size)
        {
            if (!Files.Any(f => f.Name == name)) Files.Add(new(this, name, size));

            var file = Files.Single(f => f.Name == name);
            file.Size = size;
        }

        public void MoveFile(string name, Folder newFolder)
        {
            var file = Files.SingleOrDefault(f => f.Name == name);
            if (file != null)
            {
                Files.Remove(file);
                file.MoveTo(newFolder);
                newFolder.Files.Add(file);
            }
        }

        public List<Folder> GetFolderAndAllSubFolders() => GetFolderAndAllSubFolders(new());

        public List<Folder> GetFolderAndAllSubFolders(List<Folder> folders)
        {
            folders.Add(this);

            foreach (var subFolder in SubFolders) subFolder.GetFolderAndAllSubFolders(folders);

            return folders;
        }
    }
}
