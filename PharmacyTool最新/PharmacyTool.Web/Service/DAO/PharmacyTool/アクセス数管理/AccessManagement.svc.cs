using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PharmacyTool.Web.Properties;



namespace PharmacyTool.Web.Service.DAO.PharmacyTool.アクセス数管理
{
    //[ServiceContract(Namespace = "")]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AccessManagement : IAccessManagement
    {
        public アクセス数取得結果 Doアクセス数カウントアップ取得()
        {


#if DEBUG
            string St = Settings.Default.DataSourceDEBUG;

#elif NAKAYAMA
            string St = Settings.Default.DataSourceNAKAYAMA;

#else

            string St = Settings.Default.DataSource;

#endif

            SqlConnection Cn = new SqlConnection();


            アクセス数取得結果 結果 = new アクセス数取得結果();

            Cn.ConnectionString = St;

            var command = new SqlCommand();
            command.Connection = new SqlConnection(St);
            command.CommandText = "dbo.PTアクセス数カウントアップ取得";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@総数のみアップか", SqlDbType.Bit, 1, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));


            command.Parameters[1].Value = true;


            SqlDataReader sdr;

            try
            {
                command.Connection.Open();
                using (sdr = command.ExecuteReader())
                {

                    if (sdr.HasRows == false)
                    {
                        結果.取得成功か = false;
                        結果.アクセス数 = 0;
                        結果.エラーメッセージ = "アクセス数取得に失敗しました。";


                        return 結果;
                    }

                    while (sdr.Read())
                    {
                        if ((bool)sdr["取得成功か"] == true)
                        {
                            結果.取得成功か = true;
                            結果.アクセス数 = (int)sdr["アクセス数"];
                            結果.エラーメッセージ = "";


                            return 結果;

                        }
                        else
                        {
                            結果.取得成功か = false;
                            結果.アクセス数 = 0;
                            結果.エラーメッセージ = "アクセス数取得に失敗しました。";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                結果.取得成功か = false;
                結果.アクセス数 = 0;
                結果.エラーメッセージ = e.Message + e.StackTrace;

                return 結果;
            }
            finally
            {
                command.Connection.Close();
            }

            return 結果;

        }

    }

    public class アクセス数取得結果
    {
        private bool _取得成功か;

        public bool 取得成功か
        {
            get { return _取得成功か; }
            set { _取得成功か = value; }
        }

        private int _アクセス数;

        public int アクセス数
        {
            get { return _アクセス数; }
            set { _アクセス数 = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }
    }
}
