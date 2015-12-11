using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BilDestek
{
    public delegate void Callback();
    class DB
    {
        public event Callback disconnected;
        public DB() {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                disconnected();
            }
        }
        private SqlConnection connect()
        {
            SqlConnection con = new SqlConnection("Server=127.0.0.1;Database=UGUR;User Id=sa;Password=1234;Connection Timeout=5;");
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                disconnected();
                throw ex;
            }
            return con;
        }
        public int register(String mac,String ipAddress,String username) {
            SqlCommand com; SqlDataReader dr;
            SqlConnection con;
            int id = -1;
            try
            {
                con = connect();
                com = new SqlCommand("SELECT * FROM Computers WHERE mac=@mac AND username=@username", con);
                com.Parameters.AddWithValue("@mac", mac);
                com.Parameters.AddWithValue("@username", username);
                dr = com.ExecuteReader();
            }
            catch (Exception ex)
            {
                disconnected();
                throw ex;
            }

            

            
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    id = Convert.ToInt32(dr[0]);
                }
                dr.Close();
            }
            else
            {
                dr.Close();

                SqlCommand com2;

                try
                {
                    com2 = new SqlCommand("INSERT INTO Computers (mac,ip,username) output INSERTED.id VALUES (@mac,@ip,@username)", con);
                    com2.Parameters.AddWithValue("@mac", mac);
                    com2.Parameters.AddWithValue("@ip", ipAddress);
                    com2.Parameters.AddWithValue("@username", username);
                    id = (int)com2.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    disconnected();
                    throw ex;
                }
            }
            con.Close();
            return id;
        }


        public void endTask(Computer comp,Task t)
        {
            SqlCommand com;
            SqlConnection con;
            try
            {
                con = connect();
                com = new SqlCommand("INSERT INTO task_user (cid,tid,result) VALUES (@cid,@tid,@result)", con);
                com.Parameters.AddWithValue("@cid", comp.id);
                com.Parameters.AddWithValue("@tid", t.id);
                com.Parameters.AddWithValue("@result", t.result);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                disconnected();
                throw ex;
            }

            con.Close();
        }

        public Task getTask(Computer comp)
        {
            SqlCommand com; SqlDataReader dr;
            SqlConnection con;
            Task t = new Task(comp, -1, "", 1, "", "","",false);
            try
            {
                con = connect();
                com = new SqlCommand("SELECT * FROM Tasks WHERE DATEDIFF(minute, inserted_at, GETDATE())<15 AND id NOT IN (SELECT tid FROM task_user WHERE cid=@cid)", con);
                com.Parameters.AddWithValue("@cid", comp.id);
                dr = com.ExecuteReader();
            }
            catch (Exception ex)
            {
                disconnected();
                throw ex;
            }



            if (dr.HasRows)
            {
                dr.Read();
                t.id = Convert.ToInt32(dr[0]);
                t.taskname = dr[1].ToString();
                t.param1 = dr[2].ToString();
                t.param2 = dr[3].ToString();
                t.delay = Convert.ToInt32(dr[4]);
                t.message = dr[5].ToString();
                t.keep = Convert.ToInt32(dr[7]) == 1 ? true : false;
            }
            dr.Close();
            con.Close();
            return t;
        }

    }
}
