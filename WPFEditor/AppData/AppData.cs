﻿using MegaMan.Editor.Bll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MegaMan.Editor.AppData
{
    [Serializable]
    public class StoredAppData
    {
        public List<RecentProject> RecentProjects { get; set; }

        public StoredAppData()
        {
            RecentProjects = new List<RecentProject>();
        }

        public void AddRecentProject(ProjectDocument project)
        {
            var path = project.Project.GameFile.Absolute;
            var existing = RecentProjects.FirstOrDefault(p => p.AbsolutePath == path);

            if (existing != null)
            {
                RecentProjects.Remove(existing);
                RecentProjects.Insert(0, existing);
            }
            else
            {
                RecentProjects.Insert(0, new RecentProject() { Name = project.Name, AbsolutePath = path });
            }
        }

        public static StoredAppData Load()
        {
            var file = GetFilePath();

            if (!File.Exists(file))
                return new StoredAppData();

            var serializer = new XmlSerializer(typeof(StoredAppData));

            using (var reader = XmlReader.Create(file))
            {
                var data = serializer.Deserialize(reader);
                return (StoredAppData)data;
            }
        }

        public void Save()
        {
            var file = GetFilePath();
            var settings = new XmlWriterSettings()
            {
                Indent = true,
                NewLineChars = "\t",
                OmitXmlDeclaration = true
            };
            var serializer = new XmlSerializer(typeof(StoredAppData));

            using (var writer = XmlWriter.Create(file, settings))
            {
                serializer.Serialize(writer, this);
            }
        }

        private static string GetFilePath()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var file = Path.Combine(directory, "settings.xml");
            return file;
        }
    }
}
