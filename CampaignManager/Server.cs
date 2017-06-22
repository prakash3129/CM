using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Data.SqlClient;
using System.Net.Sockets;
using RemoteViewing.Vnc;
using RemoteViewing.Vnc.Server;
using RemoteViewing.Windows.Forms.Server;

namespace GCC
{
    
    public static class Server
    {
        static string Password = "Pr@k@sH";
        static VncServerSession Session;

        static void SessionConnected(object sender, EventArgs e)
        {
            //Thread.Sleep(60000);
            ExecuteQuery("UPDATE c_machines set STATUS = 'Active Session',LastUpdatedDate = GETDATE() WHERE MachineID='" + GV.sMachineID + "';");
          //  Listen();
        }

        static void ConnectionFailed(object sender, EventArgs e)
        {
            Thread.Sleep(60000);
            Session = null;
            Listen();
        }

        static void ConnectionClosed(object sender, EventArgs e)
        {
            //Thread.Sleep(60000);
            Session = null;
            Listen();
        }

        static void HandlePasswordProvided(object sender, PasswordProvidedEventArgs e)
        {
            e.Accept(Password.ToCharArray());
        }     
        
        static void ExecuteQuery(string sQuery)
        {
            using (SqlConnection con = new SqlConnection(GV.sMSSQL1))
            {
                try
                {
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    using (SqlCommand cmd = new SqlCommand(sQuery, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch(Exception ex)
                {
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false, sQuery);
                }
            }
        }   

       public static void Listen()
        {
            if (GV.IP.StartsWith("172.27"))
            {
                Random Rand = new Random();
                TcpListener listener = null;
                while (true)
                {
                    try
                    {
                        int iPort = Rand.Next(10000, 60000);//Max 65535
                        listener = new TcpListener(IPAddress.Any, iPort);
                        listener.Start();
                        ExecuteQuery("UPDATE c_machines set STATUS = 'Online', SystemState = '' ,RDPPort='" + iPort + "',CMVersion = '" + GV.sSoftwareVersion + "',LastSession='" + GV.sSessionID + "',LastUpdatedDate = GETDATE(), LastLoggedProjectID='" + GV.sProjectID + "' WHERE MachineID='" + GV.sMachineID + "';");
                        break;
                    }
                    catch (Exception ex)
                    {
                        listener = null;
                        Thread.Sleep(30000);
                    }
                }

                try
                {
                    var client = listener.AcceptTcpClient();

                    // Set up a framebuffer and options.
                    var options = new VncServerSessionOptions();
                    options.AuthenticationMethod = AuthenticationMethod.Password;

                    // Create a session.
                    Session = new VncServerSession();
                    Session.Connected += SessionConnected;
                    Session.ConnectionFailed += ConnectionFailed;
                    Session.Closed += ConnectionClosed;
                    
                    Session.PasswordProvided += HandlePasswordProvided;
                    Session.SetFramebufferSource(new VncScreenFramebufferSource("Hello World", System.Windows.Forms.Screen.PrimaryScreen));
                    Session.Connect(client.GetStream(), options);
                }
                catch(Exception ex)
                {
                    ExecuteQuery("UPDATE c_machines set STATUS = 'Connection Error',RDPPort='',LastUpdatedDate = GETDATE() WHERE MachineID='" + GV.sMachineID + "';");
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(), ex, true, false);
                }
            }
        }

    }
}
