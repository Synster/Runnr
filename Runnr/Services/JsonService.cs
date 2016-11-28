using Newtonsoft.Json;
using Runnr.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runnr
{
    public static class JsonService
    {
        private const string NotesFileName = "notes.json";
        private const string AppsFileName = "apps.json";
        static JsonSerializer serializer;

        static JsonService()
        {
            serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
        }

        public async static void SaveNotes(List<Note> notes)
        {
            await Task.Run(() =>
            {
                using (StreamWriter sw = new StreamWriter(NotesFileName, false))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, notes);
                }
            });
        }

        public async static void Save(List<ApplicationDetail> apps)
        {
            await Task.Run(() =>
            {
                using (StreamWriter sw = new StreamWriter(AppsFileName, false))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, apps);
                }
            });
        }

        public static List<ApplicationDetail> LoadAllApps()
        {
            List<ApplicationDetail> apps;
            using (StreamReader sr = new StreamReader(AppsFileName))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                apps = serializer.Deserialize<List<ApplicationDetail>>(reader);
            }
            return apps;
        }

        public static List<Note> LoadAllNotes()
        {
            List<Note> note;
            using (StreamReader sr = new StreamReader(NotesFileName))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                note = serializer.Deserialize<List<Note>>(reader);
            }
            return note;
        }


    }
}
