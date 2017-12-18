using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace EventListener
{
    class Program
    {
        static FbConnection connection;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                FbConnectionStringBuilder cs = new FbConnectionStringBuilder();
                cs.DataSource = "localhost";
                cs.Database = @"E:\Samples\Tempo2012MVVM\Tempo2012\Tempo2012.UI.WPF\bin\Debug\data\TEMPO2012.fdb";
                cs.UserID = "SYSDBA";
                cs.Password = "masterkey";
                cs.Charset = "UTF8";

                connection = new FbConnection(cs.ToString());
                connection.Open();

                FbRemoteEvent revent = new FbRemoteEvent(connection);
                revent.AddEvents(new string[] { "acc_insert", "acc_update", });

                // Add callback to the Firebird events
                revent.RemoteEventCounts += new FbRemoteEventEventHandler(EventCounts);

                // Queue events
                revent.QueueEvents();

                Console.ReadLine();
                connection.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        static void EventCounts(object sender, FbRemoteEventEventArgs args)
        {
            if (args.Counts > 0)
            {
                string selectFromAccountsWhereModifyU="";
                switch (args.Name)
                {
                    case "acc_insert":
                        selectFromAccountsWhereModifyU = "Select * from \"accounts\" where MODIFY='I'";
                        break;
                    case "acc_update":
                        selectFromAccountsWhereModifyU = "Select * from \"accounts\" where MODIFY='U'";
                        break;
                }
                try
                {

                    FbTransaction ft = connection.BeginTransaction();
                    FbCommand fb = new FbCommand(selectFromAccountsWhereModifyU, connection, ft);
                    FbDataReader reader = fb.ExecuteReader();
                    while (reader.Read())
                    {

                        string row = "Record ->";
                        for (var i = 0; i < reader.FieldCount; i++)
                        {


                            row +=string.Format(" {0} ",reader.GetValue(i));
                        }
                        switch (args.Name)
                        {
                            case "acc_insert":
                                row += " Inserted";
                                break;
                            case "acc_update":
                                row += " Updated";
                                ;
                                break;
                        }
                        Console.WriteLine(row);
                    }
                    reader.Close();
                    switch (args.Name)
                    {
                        case "acc_insert":
                            selectFromAccountsWhereModifyU = "Update \"accounts\" set MODIFY='N' where MODIFY='I'";
                            break;
                        case "acc_update":
                            selectFromAccountsWhereModifyU = "Update \"accounts\" set MODIFY='G' where MODIFY='U'";
                            break;
                    }
                    fb.CommandText = selectFromAccountsWhereModifyU;
                    fb.ExecuteNonQuery();
                    ft.Commit();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }
    }
    
}
