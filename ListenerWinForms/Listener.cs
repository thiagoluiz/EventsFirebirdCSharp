using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListenerWinForms
{
    public static class Listener
    {
        public static FbRemoteEvent events { get; set; }

        public static void IniciarEscuta()
        {
            events = new FbRemoteEvent("database = localhost:C:\\Aplicacoes\\BD\\BASE.FDB; user = sysdba; password = masterkey");
            events.QueueEvents("ATUALIZAPROGRESSO", "PROCESSOCONCLUIDO");
        }

    }
}
