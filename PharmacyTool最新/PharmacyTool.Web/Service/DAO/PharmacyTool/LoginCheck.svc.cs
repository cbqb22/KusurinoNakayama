using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PharmacyTool.Web.Properties;


namespace PharmacyTool.Web.Service.DAO.PharmacyTool
{
    // メモ: ここでクラス名 "LoginCheck" を変更する場合は、Web.config で "LoginCheck" への参照も更新する必要があります。
    public class LoginCheck : ILoginCheck
    {
        public List<ログインチェック結果> LoginCheck実行(string 入力ユーザーID, string 入力コンフィデンシャル)
        {
            List<ログインチェック結果> List結果 = new List<ログインチェック結果>();
            ログインチェック結果 結果 = new ログインチェック結果();

            if (入力ユーザーID == null || 入力コンフィデンシャル == null)
            {
                結果.チェック成功か = false;
                結果.アクセス権限 = -1;
                結果.エラーメッセージ = "ユーザーIDまたは、パスワードが空です。";

                List結果.Add(結果);

                return List結果;
            }

#if DEBUG
            string St = Settings.Default.DataSourceDEBUG;

#elif NAKAYAMA

            string St = Settings.Default.DataSourceNAKAYAMA;
#else

            string St = Settings.Default.DataSource;

#endif

            SqlConnection Cn = new SqlConnection();




            Cn.ConnectionString = St;

            var command = new SqlCommand();
            command.Connection = new SqlConnection(St);
            command.CommandText = "dbo.PTログインチェック";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@入力ユーザーID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@入力コンフィデンシャル", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            command.Parameters[1].Value = 入力ユーザーID;
            command.Parameters[2].Value = 入力コンフィデンシャル;

            SqlDataReader sdr;

            try
            {
                command.Connection.Open();
                using (sdr = command.ExecuteReader())
                {

                    if (sdr.HasRows == false)
                    {
                        結果.チェック成功か = false;
                        結果.アクセス権限 = -1;
                        結果.エラーメッセージ = "ログインに失敗しました。";

                        List結果.Add(結果);

                        return List結果;
                    }

                    while (sdr.Read())
                    {
                        if ((bool)sdr["チェック成功かどうか"] == true)
                        {
                            結果.チェック成功か = true;
                            結果.アクセス権限 = (int)sdr["アクセス権限"];
                            結果.エラーメッセージ = "";
                            結果.表示名称 = (string)sdr["表示名称"];
                            結果.ユーザーID = (string)sdr["ユーザーID"];

                            List結果.Add(結果);

                            return List結果;

                        }
                        else
                        {
                            結果.チェック成功か = false;
                            結果.アクセス権限 = -1;
                            結果.エラーメッセージ = "ログインに失敗しました。";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                結果.チェック成功か = false;
                結果.アクセス権限 = -1;
                結果.エラーメッセージ = e.Message + e.StackTrace;

                List結果.Add(結果);

                return List結果;

            }
            finally
            {
                command.Connection.Close();
            }

            return List結果;
        }
    }

    public class ログインチェック結果
    {
        private bool _チェック成功か;

        public bool チェック成功か
        {
            get { return _チェック成功か; }
            set { _チェック成功か = value; }
        }

        private int _アクセス権限;

        public int アクセス権限
        {
            get { return _アクセス権限; }
            set { _アクセス権限 = value; }
        }

        private string _ユーザーID;

        public string ユーザーID
        {
            get { return _ユーザーID; }
            set { _ユーザーID = value; }
        }

        private string _表示名称;

        public string 表示名称
        {
            get { return _表示名称; }
            set { _表示名称 = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }
    }
}
