using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PharmacyTool.Web.Properties;


namespace PharmacyTool.Web.Service.DAO.PharmacyTool.ユーザー管理
{
    // メモ: ここでクラス名 "UserManagement" を変更する場合は、Web.config で "UserManagement" への参照も更新する必要があります。
    public class UserManagement : IUserManagement
    {
        public CreateNewUser結果 CreateNewUser実行(string UserID, string Confidential, int アクセス権限)
        {
            CreateNewUser結果 結果 = new CreateNewUser結果();

            if (string.IsNullOrEmpty(UserID) || string.IsNullOrEmpty(Confidential))
            {
                結果.ユーザー作成成功かどうか = false;
                結果.エラーメッセージ = "UserID、パスワード、アクセスレベルを入力して下さい。";

                return 結果;
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
            command.CommandText = "dbo.PTユーザー作成";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@入力ユーザーID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@入力コンフィデンシャル", SqlDbType.NVarChar, 256, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@アクセス権限", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            command.Parameters[1].Value = UserID;
            command.Parameters[2].Value = Confidential;
            command.Parameters[3].Value = アクセス権限;

            SqlDataReader sdr;
            try
            {
                command.Connection.Open();

                using (sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows == false)
                    {
                        結果.ユーザー作成成功かどうか = true;
                        結果.エラーメッセージ = "新規ユーザーの作成に失敗しました。";

                        return 結果;
                    }

                    while (sdr.Read())
                    {
                        if ((bool)sdr["ユーザー作成成功かどうか"] == true)
                        {
                            結果.ユーザー作成成功かどうか = (bool)sdr["ユーザー作成成功かどうか"];
                            結果.エラーメッセージ = (string)sdr["エラーメッセージ"];
                        }
                        else
                        {
                            結果.ユーザー作成成功かどうか = (bool)sdr["ユーザー作成成功かどうか"];
                            結果.エラーメッセージ = (string)sdr["エラーメッセージ"];
                        }

                    }

                    return 結果;

                }

            }
            catch (Exception e)
            {

                結果.ユーザー作成成功かどうか = false;
                結果.エラーメッセージ = "新規ユーザーの作成に失敗しました。" + e.Message + e.StackTrace;

                return 結果;

            }
            finally
            {
                command.Connection.Close();
            }


        }

        public DeleteUser結果 DeleteUser実行(string UserID)
        {
            DeleteUser結果 結果 = new DeleteUser結果();


            if (string.IsNullOrEmpty(UserID))
            {
                結果.ユーザー削除成功かどうか = false;
                結果.エラーメッセージ = "削除するUserIDを選択して下さい。";

                return 結果;
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
            command.CommandText = "dbo.PTユーザー削除";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@入力ユーザーID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            command.Parameters[1].Value = UserID;

            SqlDataReader sdr;
            try
            {
                command.Connection.Open();

                using (sdr = command.ExecuteReader())
                {
                    if (sdr.HasRows == false)
                    {
                        結果.ユーザー削除成功かどうか = true;
                        結果.エラーメッセージ = "このユーザーの削除に失敗しました。";

                        return 結果;
                    }

                    while (sdr.Read())
                    {
                        if ((bool)sdr["ユーザー削除成功かどうか"] == true)
                        {
                            結果.ユーザー削除成功かどうか = (bool)sdr["ユーザー削除成功かどうか"];
                            結果.エラーメッセージ = (string)sdr["エラーメッセージ"];
                        }
                        else
                        {
                            結果.ユーザー削除成功かどうか = (bool)sdr["ユーザー削除成功かどうか"];
                            結果.エラーメッセージ = (string)sdr["エラーメッセージ"];
                        }

                    }

                    return 結果;

                }

            }
            catch (Exception e)
            {

                結果.ユーザー削除成功かどうか = false;
                結果.エラーメッセージ = "このユーザーの削除に失敗しました。" + e.Message + e.StackTrace;

                return 結果;

            }
            finally
            {
                command.Connection.Close();
            }



        }


        public List<AllUser情報取得結果> AllUser情報取得実行()
        {

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
            command.CommandText = "dbo.PTユーザー全件取得";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));


            SqlDataReader sdr;
            List<AllUser情報取得結果> l結果 = new List<AllUser情報取得結果>();

            try
            {
                command.Connection.Open();

                using (sdr = command.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        AllUser情報取得結果 結果 = new AllUser情報取得結果();

                        結果.UserID = (string)sdr["ユーザーID"];
                        結果.アクセス権限 = (int)sdr["アクセス権限"];
                        結果.削除フラグ = (bool)sdr["削除フラグ"];

                        l結果.Add(結果);
                    }

                    return l結果;

                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message + e.StackTrace);
            }
            finally
            {
                command.Connection.Close();
            }

        }


        public string AllUser情報更新実行(List<AllUser情報取得結果> list)
        {
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
            command.CommandText = "dbo.PTユーザー情報更新";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@入力ユーザーID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@アクセス権限", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@削除フラグ", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@コンフィデンシャル", SqlDbType.NVarChar, 256, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@変更前ユーザーID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));


            foreach (var l in list)
            {
                command.Connection.Open();

                command.Parameters[1].Value = l.UserID;
                command.Parameters[2].Value = l.アクセス権限;
                command.Parameters[3].Value = l.削除フラグ;
                command.Parameters[4].Value = l.Password;
                command.Parameters[5].Value = l.変更前のUserID;

                SqlDataReader sdr;

                try
                {

                    using (sdr = command.ExecuteReader())
                    {
                        while (sdr.Read())
                        {

                            if ((bool)sdr["ユーザー情報更新成功かどうか"])
                            {
                                break;
                            }
                            else
                            {
                                return (string)sdr["エラーメッセージ"];
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + e.StackTrace);
                }
                finally
                {
                    command.Connection.Close();
                }

            }

            return "更新しました。";

        }

        public string LoginUser情報更新実行(AllUser情報取得結果 lu)
        {
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
            command.CommandText = "dbo.PTログインユーザー情報更新";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@入力ユーザーID", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@入力コンフィデンシャル", SqlDbType.NVarChar, 256, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            command.Parameters[1].Value = lu.UserID;
            command.Parameters[2].Value = lu.Password;


            SqlDataReader sdr;

            try
            {
                command.Connection.Open();

                using (sdr = command.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        if ((bool)sdr["ユーザー情報更新成功かどうか"])
                        {
                            return "更新しました。";
                        }
                        else
                        {
                            return "更新に失敗しました。";
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message + e.StackTrace);
            }
            finally
            {
                command.Connection.Close();
            }


            return "更新しました。";

        }


    }

    public class CreateNewUser結果
    {
        private bool _ユーザー作成成功かどうか;

        public bool ユーザー作成成功かどうか
        {
            get { return _ユーザー作成成功かどうか; }
            set { _ユーザー作成成功かどうか = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }
    }

    public class DeleteUser結果
    {
        private bool _ユーザー削除成功かどうか;

        public bool ユーザー削除成功かどうか
        {
            get { return _ユーザー削除成功かどうか; }
            set { _ユーザー削除成功かどうか = value; }
        }

        private string _エラーメッセージ;

        public string エラーメッセージ
        {
            get { return _エラーメッセージ; }
            set { _エラーメッセージ = value; }
        }
    }

    public class AllUser情報取得結果
    {
        private string _UserID;

        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private int _アクセス権限;

        public int アクセス権限
        {
            get { return _アクセス権限; }
            set { _アクセス権限 = value; }
        }

        private bool _削除フラグ;

        public bool 削除フラグ
        {
            get { return _削除フラグ; }
            set { _削除フラグ = value; }
        }

        private string _変更前のUserID;

        public string 変更前のUserID
        {
            get { return _変更前のUserID; }
            set { _変更前のUserID = value; }
        }
    }


}
