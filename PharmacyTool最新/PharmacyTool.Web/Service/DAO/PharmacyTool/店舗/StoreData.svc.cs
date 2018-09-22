using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using PharmacyTool.Web.Properties;

namespace PharmacyTool.Web.Service.DAO.PharmacyTool.店舗
{
    // メモ: ここでクラス名 "StoreData" を変更する場合は、Web.config で "StoreData" への参照も更新する必要があります。
    public class StoreData : IStoreData
    {
        public List<PT店舗名リターンデータセット> 店舗名取得()
        {

#if DEBUG
            string St = Settings.Default.DataSourceDEBUG;

#elif NAKAYAMA

            string St = Settings.Default.DataSourceNAKAYAMA;
#else

            string St = Settings.Default.DataSource;

#endif

            //List<string> result = new List<string>();
            List<PT店舗名リターンデータセット> result = new List<PT店舗名リターンデータセット>();
            SqlConnection Cn = new SqlConnection();




            Cn.ConnectionString = St;

            var command = new SqlCommand();
            command.Connection = new SqlConnection(St);
            command.CommandText = "dbo.PT店舗名取得";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));


            SqlDataReader sdr;

            try
            {
                command.Connection.Open();
                using (sdr = command.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        PT店舗名リターンデータセット dset = new PT店舗名リターンデータセット();

                        dset.店舗名 = (string)sdr["店舗名"];
                        dset.ID = (int)sdr["ID"];

                        result.Add(dset);

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

            return result;
        }


        public string 新規店舗名作成(string 作成店舗名)
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
            command.CommandText = "dbo.PT店舗名作成";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@新規店舗名", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            command.Parameters[1].Value = 作成店舗名;

            SqlDataReader sdr;

            try
            {
                command.Connection.Open();
                using (sdr = command.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        if ((bool)sdr["新規店舗作成成功かどうか"] == true)
                        {
                            //// ここに新店舗フォルダ作成処理を追加する。
                            //string 現在庫folderpath = Path.Combine(Path.Combine(Settings.Default.在庫データUploadFileRootPathNAKAYAMA, "現在庫"), 作成店舗名);
                            //string 使用量folderpath = Path.Combine(Path.Combine(Settings.Default.在庫データUploadFileRootPathNAKAYAMA, "使用量"), 作成店舗名);
                            //DateTime fromStartDate = new DateTime(2009, 01, 01); // 2009年度からスタート
                            //DateTime toEndDate = DateTime.Now;
                            //string 不動品folderpath = Path.Combine(Path.Combine(Settings.Default.在庫データUploadFileRootPathNAKAYAMA, "不動品"), 作成店舗名);
                            //// 現在庫フォルダ作成
                            //if (!Directory.Exists(現在庫folderpath))
                            //{
                            //    Directory.CreateDirectory(現在庫folderpath);
                            //}
                            //// 使用量フォルダ作成
                            //if (!Directory.Exists(使用量folderpath))
                            //{
                            //    Directory.CreateDirectory(使用量folderpath);
                            //}

                            //// 使用量の年度フォルダを作成
                            //while (fromStartDate.Year <= toEndDate.Year)
                            //{
                            //    string createname = Path.Combine(使用量folderpath, string.Format("{0}年", fromStartDate.Year.ToString()));
                            //    if (!Directory.Exists(createname))
                            //    {
                            //        Directory.CreateDirectory(createname);
                            //    }

                            //    fromStartDate = fromStartDate.AddYears(1);
                                
                            //}
                            //// 不動品フォルダ作成
                            //if (!Directory.Exists(不動品folderpath))
                            //{
                            //    Directory.CreateDirectory(不動品folderpath);
                            //}




                            return (string)sdr["エラーメッセージ"];
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


            return "新規店舗名作成に失敗しました。";
        }

        public string 店舗名削除(string 削除店舗名)
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
            command.CommandText = "dbo.PT店舗名削除";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            command.Parameters.Add(new SqlParameter("@削除店舗名", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            command.Parameters[1].Value = 削除店舗名;

            SqlDataReader sdr;

            try
            {
                command.Connection.Open();
                using (sdr = command.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        if ((bool)sdr["店舗削除成功かどうか"] == true)
                        {
                            return (string)sdr["エラーメッセージ"];
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


            return "店舗の削除に失敗しました。";
        }

    }


    public class PT店舗名リターンデータセット
    {
        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _店舗名;

        public string 店舗名
        {
            get { return _店舗名; }
            set { _店舗名 = value; }
        }
    }
}
