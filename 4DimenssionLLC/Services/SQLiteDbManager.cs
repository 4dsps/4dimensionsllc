using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Globalization;
//using com.blazor.decs.ViewModels;
//using System.Data.SQLite;
//using com.blazor.decs.UTIL;
using Microsoft.AspNetCore.Components.Routing;
using System.Data.SQLite;
using _4DimenssionLLC.ViewModels;
using System.Collections;
//using com.blazor.util;
namespace _4DimenssionLLC.services
{
    public static class SQLiteDbManager
    {
        //Data Retrieval
        public static SQLiteConnection sqliteConn;

        /// <summary>
        /// SQLite Connection Intialization.
        /// </summary>
        public static SQLiteConnection GetSQLiteConnection()
        {
            if (sqliteConn == null)
            {
                sqliteConn = new SQLiteConnection(dimsConstant.SQLLiteConnectionString);
                try
                {
                    sqliteConn.Open();

                    SQLiteCommand sqlite_cmd;
                    try
                    {
                        sqlite_cmd = sqliteConn.CreateCommand();
                        //sqlite_cmd.CommandText = "drop table reservations";
                        //sqlite_cmd.ExecuteNonQuery();
                        //sqlite_cmd.CommandText = "drop table fields";
                        //sqlite_cmd.ExecuteNonQuery();
                        //sqlite_cmd = sqliteConn.CreateCommand();
                        //sqlite_cmd.CommandText = "drop table fields";
                        //sqlite_cmd.ExecuteNonQuery();
                        sqlite_cmd.CommandText = "PRAGMA read_uncommitted = 1;";
                        sqlite_cmd.ExecuteNonQuery();
                        string CreateFieldSQL = "CREATE TABLE IF NOT EXISTS product (id INTEGER PRIMARY KEY AUTOINCREMENT,Name VARCHAR(150), ShortCode VARCHAR(10),  Description VARCHAR(1000),Price INTEGER,Address VARCHAR(500),CategoryId INTEGER,BrandId INTEGER,CreatedBy INTEGER ,CreatedAt  datetime,LastUpdatedBy INTEGER ,LastUpdatedAt datetime,Status INTEGER,Contact INTEGER)";

                        sqlite_cmd.CommandText = CreateFieldSQL;
                        sqlite_cmd.ExecuteNonQuery();

                        string Createsql = "CREATE TABLE IF NOT EXISTS loginusers (id INTEGER PRIMARY KEY AUTOINCREMENT,  loginname VARCHAR(50), password VARCHAR(50), logouttime datetime, logintime datetime,status int)";
                        // string Createsql1 = "CREATE TABLE SampleTable1 (Col1 VARCHAR(20), Col2 INT)";

                        sqlite_cmd.CommandText = Createsql;
                        sqlite_cmd.ExecuteNonQuery();

                        

 	    string CreateMediaSQL = "CREATE TABLE IF NOT EXISTS MediaContent(id INTEGER PRIMARY KEY AUTOINCREMENT,Name VARCHAR(150), productId INTEGER,  Title VARCHAR(1000),Description VARCHAR(1000),SrNo INTEGER,MimeType VARCHAR(50),LastUpdatedBy INTEGER,LastUpdatedAt datetime,Image  VARCHAR(500),Status INTEGER ,RowVer INTEGER)";

                        sqlite_cmd.CommandText = CreateMediaSQL;
                        sqlite_cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Table already exists");
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                return sqliteConn;
            }
            else if (sqliteConn.State != System.Data.ConnectionState.Open)
            {
                sqliteConn.Open();
                return sqliteConn;
            }
            else
                return sqliteConn;
            // return new SQLiteConnection();
        }
        public static void SaveContactForm(ContactFormViewModel contact)
        {
			
            if (sqliteConn == null)
                sqliteConn = GetSQLiteConnection();

            try
            {
                // Open the connection if it's not already open
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                    sqliteConn.Open();

                using (var sqlitecmd = sqliteConn.CreateCommand())
                {
                    if (contact.Id <= 0)
                    {
                        // Insert new contact
                        sqlitecmd.CommandText = @"
                    INSERT INTO ContactForm (FirstName, LastName, Dob, Contact1, Contact2, Email, Address, Message, lovereaction,likereaction,month)
                    VALUES (@FirstName, @LastName, @Dob, @Contact1, @Contact2, @Email, @Address, @Message, @lovereaction,@likereaction,@month);
                ";

                        sqlitecmd.Parameters.AddWithValue("@FirstName", contact.FirstName ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@LastName", contact.LastName ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@Dob", contact.Dob);
                        sqlitecmd.Parameters.AddWithValue("@Contact1", contact.Contact1);
                        sqlitecmd.Parameters.AddWithValue("@Contact2", (object)contact.Contact2 ?? DBNull.Value); // Handle nullable int
                        sqlitecmd.Parameters.AddWithValue("@Email", contact.Email ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@Address", contact.Address ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@Message", contact.Message ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@lovereaction", contact.lovereaction); // Handle as int
                        sqlitecmd.Parameters.AddWithValue("@likereaction", contact.likereaction); // Handle as int
                        sqlitecmd.Parameters.AddWithValue("@month", System.DateTime.Now.Month); // Handle as int

                        sqlitecmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Update existing contact
                        sqlitecmd.CommandText = @" UPDATE ContactForm SET FirstName = @FirstName,LastName = @LastName,
                        Dob = @Dob,
                        Contact1 = @Contact1,
                        Contact2 = @Contact2,
                        Email = @Email,
                        Address = @Address,
                        Message = @Message,
                        likereaction = @likereaction
                        lovereaction = @lovereaction
                        month = @month
                    WHERE Id = @Id;
                ";

                        sqlitecmd.Parameters.AddWithValue("@FirstName", contact.FirstName ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@LastName", contact.LastName ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@Dob", contact.Dob);
                        sqlitecmd.Parameters.AddWithValue("@Contact1", contact.Contact1);
                        sqlitecmd.Parameters.AddWithValue("@Contact2", (object)contact.Contact2 ?? DBNull.Value); // Handle nullable int
                        sqlitecmd.Parameters.AddWithValue("@Email", contact.Email ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@Address", contact.Address ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@Message", contact.Message ?? string.Empty);
                        sqlitecmd.Parameters.AddWithValue("@likereaction", contact.likereaction); // Handle as int
                        sqlitecmd.Parameters.AddWithValue("@lovereaction", contact.lovereaction); // Handle as int
                        sqlitecmd.Parameters.AddWithValue("@month", System.DateTime.Now.Month); // Handle as int
                        sqlitecmd.Parameters.AddWithValue("@Id", contact.Id);

                        sqlitecmd.ExecuteNonQuery();
                    }
                }
            }			
			catch (Exception ex)
            {
                // Handle any exceptions that occur during the save operation
                Console.WriteLine("Error: " + ex.Message);
				throw ex;

			}			
			finally
            {
                // Ensure the connection is closed
                if (sqliteConn.State != System.Data.ConnectionState.Closed)
                    sqliteConn.Close();			

			}		
		}
		public static ContactsStatsViewModel GetContactStats()
		{
			ContactsStatsViewModel stats = null;

			if (sqliteConn == null)
				sqliteConn = GetSQLiteConnection();

			try
			{
				// Open the connection if it's not already open
				if (sqliteConn.State != System.Data.ConnectionState.Open)
					sqliteConn.Open();

				using (var sqlitecmd = sqliteConn.CreateCommand())
				{
					sqlitecmd.CommandText = @"
               select lovecount,likecount,case month when 1 then 'JAN'  when 2 then 'FEB' when 3 then 'MAR' when 4 then 'APR' when 5 then 'MAY'  when 6 then 'JUN' when 7 then 'JUL' when 8 then 'AUG' when 9 then 'SEP' when 10 then 'OCT' when 11 then 'NOV'  else 'DEC' END MonthName from ( 
select sum(lovereaction) lovecount, sum(likereaction) likecount, month from contactform
group by month
);";

					using (var reader = sqlitecmd.ExecuteReader())
					{
						if (reader.Read())
						{
							stats = new ContactsStatsViewModel
							{
								lovecount = reader.GetInt32(reader.GetOrdinal("lovecount")),
								likecount = reader.GetInt32(reader.GetOrdinal("likecount")),
								MonthName = reader.GetString(reader.GetOrdinal("MonthName"))
							};
						}
					}
				}
			}
			
			catch (Exception ex)
			{
				// Handle any exceptions that occur during the retrieval operation
				Console.WriteLine("Error: " + ex.Message);
			}
			finally
			{
				// Ensure the connection is closed
				if (sqliteConn.State != System.Data.ConnectionState.Closed)
					sqliteConn.Close();
			}

			return stats;
		}








		/// <summary>
		/// InsertProductData, Add & edit of complete product with images
		/// </summary>
		//public static Int64 InsertProductData(ProductViewModel product) {
		//    Int64 productId = 0;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();
		//    try {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();
		//        SQLiteCommand sqlitecmd;
		//        try {
		//            if (product.id <= 0)
		//            {
		//                sqlitecmd = sqliteConn.CreateCommand();
		//                // sqlitecmd.CommandText = "INSERT INTO reservations (reference,mode,arrivaldate,departuredate,receivetime,firstname,lastname,paymentamount,transactiondate,voucherno,roomno,syncstatus,email) VALUES('Ahmad',2,'2014-10-23 15:21:07','2014-10-23 15:21:07','2014-10-23 15:21:07','arshad','ali',120,'2014-10-23 15:21:07','20nm','10vm',12,'znawazch@gmail.com'); ";
		//                sqlitecmd.CommandText = "INSERT INTO product (Name,ShortCode,Description,Price,Address,CategoryId,BrandId,CreatedBy,CreatedAt,LastUpdatedBy,LastUpdatedAt,Status,Contact ) VALUES( @Name,@ShortCode,@Description,@Price,@Address,@CategoryId,@BrandId,@CreatedBy,@CreatedAt,@LastUpdatedBy,@LastUpdatedAt,@Status,@Contact); ";
		//                List<SQLiteParameter> parameter = new List<SQLiteParameter>();
		//                SQLiteParameter pName = new SQLiteParameter("@Name", "" + product.name);
		//                parameter.Add(pName);
		//                SQLiteParameter pShortCode = new SQLiteParameter("@ShortCode", "" + product.shortCode);
		//                parameter.Add(pShortCode);
		//                SQLiteParameter pDescription = new SQLiteParameter("@Description", "" + product.description);
		//                parameter.Add(pDescription);
		//                SQLiteParameter pPrice = new SQLiteParameter("@Price", System.Data.DbType.Int32);
		//                pPrice.Value = (product.price == null ? 0 : product.price);
		//                parameter.Add(pPrice);
		//                SQLiteParameter pAddress = new SQLiteParameter("@Address", "" + product.address);
		//                parameter.Add(pAddress);
		//                SQLiteParameter pContact = new SQLiteParameter("@Contact", "" + product.contact);
		//                parameter.Add(pContact);
		//                SQLiteParameter pCategoryId = new SQLiteParameter("@CategoryId", System.Data.DbType.Int32);
		//                pCategoryId.Value = (product.categoryId == null ? 0 : product.categoryId);
		//                parameter.Add(pCategoryId);
		//                SQLiteParameter pBrandId = new SQLiteParameter("@BrandId", System.Data.DbType.Int32);
		//                pBrandId.Value = (product.BrandId == null ? 0 : product.BrandId);
		//                parameter.Add(pBrandId);
		//                SQLiteParameter pCreatedBy = new SQLiteParameter("@CreatedBy", System.Data.DbType.Int32);
		//                pCreatedBy.Value = (product.CreatedBy == null ? 0 : product.CreatedBy);
		//                parameter.Add(pCreatedBy);
		//                SQLiteParameter pCreatedAt = new SQLiteParameter("@CreatedAt", System.Data.DbType.DateTime);
		//                pCreatedAt.Value = GlobalUTIL.CurrentDateTime;
		//                parameter.Add(pCreatedAt);
		//                SQLiteParameter pLastUpdatedBy = new SQLiteParameter("@LastUpdatedBy", System.Data.DbType.Int32);
		//                pLastUpdatedBy.Value = (product.CreatedBy == null ? 0 : product.CreatedBy);// product.lastUpdatedBy;
		//                parameter.Add(pLastUpdatedBy);
		//                SQLiteParameter pLastUpdatedAt = new SQLiteParameter("@LastUpdatedAt", System.Data.DbType.DateTime);
		//                pLastUpdatedAt.Value = GlobalUTIL.CurrentDateTime;
		//                parameter.Add(pLastUpdatedAt);
		//                SQLiteParameter pStatus = new SQLiteParameter("@Status", System.Data.DbType.Int32);
		//                pStatus.Value = (product.status == 0 ? 1 : product.status);
		//                parameter.Add(pStatus);
		//                //SQLiteParameter pEntity_name = new SQLiteParameter("@entity_name", model.entity_name);
		//                //parameter.Add(pEntity_name);
		//                sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//                int insertedCount = sqlitecmd.ExecuteNonQuery();

		//                sqlitecmd.CommandText = "select last_insert_rowid()";
		//                // The row ID is a 64-bit value - cast the Command result to an Int64.                        //
		//                productId = (Int64)sqlitecmd.ExecuteScalar();

		//                if (product.MediaContents != null && product.MediaContents.Any())
		//                {
		//                    foreach (MediaContentModel prd in product.MediaContents)
		//                    {
		//                        if (!string.IsNullOrWhiteSpace(prd.image) && prd.srNo != null)
		//                        {
		//                            sqlitecmd = sqliteConn.CreateCommand();
		//                            sqlitecmd.CommandText = "INSERT INTO MediaContent (Name,productId,Title,Description,SrNo,MimeType,LastUpdatedBy,LastUpdatedAt,Image,Status,RowVer ) VALUES(@Name,@productId,@Title,@Description,@SrNo,@MimeType,@LastUpdatedBy,@LastUpdatedAt,@Image,@Status,@RowVer); ";
		//                            parameter = new List<SQLiteParameter>();
		//                            SQLiteParameter pImageName = new SQLiteParameter("@Name", "" + prd.name);
		//                            parameter.Add(pImageName);
		//                            SQLiteParameter pTitle = new SQLiteParameter("@Title", "" + prd.name);
		//                            parameter.Add(pTitle);
		//                            SQLiteParameter pDesc = new SQLiteParameter("@Description", "" + prd.description);
		//                            parameter.Add(pDesc);
		//                            SQLiteParameter pProductId = new SQLiteParameter("@productId", System.Data.DbType.Int32);
		//                            pProductId.Value = productId;
		//                            parameter.Add(pProductId);
		//                            SQLiteParameter pSrNo = new SQLiteParameter("@SrNo", System.Data.DbType.Int32);
		//                            pSrNo.Value = prd.srNo;
		//                            parameter.Add(pSrNo);
		//                            SQLiteParameter MimeType = new SQLiteParameter("@MimeType", prd.mimetype);
		//                            parameter.Add(MimeType);
		//                            SQLiteParameter pImageLastUpdatedBy = new SQLiteParameter("@LastUpdatedBy", System.Data.DbType.Int32);
		//                            pImageLastUpdatedBy.Value = product.CreatedBy;
		//                            parameter.Add(pImageLastUpdatedBy);
		//                            SQLiteParameter pImageLastUpdatedAt = new SQLiteParameter("@LastUpdatedAt", System.Data.DbType.DateTime);
		//                            pImageLastUpdatedAt.Value = product.lastUpdatedAt;
		//                            parameter.Add(pImageLastUpdatedAt);
		//                            SQLiteParameter pImageFileName = new SQLiteParameter("@Image", prd.image);
		//                            parameter.Add(pImageFileName);
		//                            SQLiteParameter pImageStatus = new SQLiteParameter("@Status", System.Data.DbType.Int16);
		//                            pImageStatus.Value = 1;
		//                            parameter.Add(pImageStatus);
		//                            SQLiteParameter pImageRowVer = new SQLiteParameter("@RowVer", System.Data.DbType.Int16);
		//                            pImageRowVer.Value = 1;
		//                            parameter.Add(pImageRowVer);
		//                            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//                            insertedCount = sqlitecmd.ExecuteNonQuery();
		//                        }// Validation Check                      


		//                    }// For Each
		//                }
		//            }//New  Insert
		//            else
		//            {
		//                sqlitecmd = sqliteConn.CreateCommand();
		//                // sqlitecmd.CommandText = "INSERT INTO reservations (reference,mode,arrivaldate,departuredate,receivetime,firstname,lastname,paymentamount,transactiondate,voucherno,roomno,syncstatus,email) VALUES('Ahmad',2,'2014-10-23 15:21:07','2014-10-23 15:21:07','2014-10-23 15:21:07','arshad','ali',120,'2014-10-23 15:21:07','20nm','10vm',12,'znawazch@gmail.com'); ";
		//                sqlitecmd.CommandText = "update product set Name=@Name,ShortCode=@ShortCode,Description=@Description,Price=@Price,Address=@Address,CategoryId=@CategoryId,BrandId=@BrandId,CreatedBy=@CreatedBy,CreatedAt=@CreatedAt,LastUpdatedBy=@LastUpdatedBy,LastUpdatedAt=@LastUpdatedAt,Status=@Status,Contact=@Contact where id=@id; ";
		//                List<SQLiteParameter> parameter = new List<SQLiteParameter>();

		//                SQLiteParameter pId = new SQLiteParameter("@id", System.Data.DbType.Int32);
		//                pId.Value =  product.id;
		//                parameter.Add(pId);
		//                SQLiteParameter pName = new SQLiteParameter("@Name", "" + product.name);
		//                parameter.Add(pName);
		//                SQLiteParameter pShortCode = new SQLiteParameter("@ShortCode", "" + product.shortCode);
		//                parameter.Add(pShortCode);
		//                SQLiteParameter pDescription = new SQLiteParameter("@Description", "" + product.description);
		//                parameter.Add(pDescription);
		//                SQLiteParameter pPrice = new SQLiteParameter("@Price", System.Data.DbType.Int32);
		//                pPrice.Value = (product.price == null ? 0 : product.price);
		//                parameter.Add(pPrice);
		//                SQLiteParameter pAddress = new SQLiteParameter("@Address", "" + product.address);
		//                parameter.Add(pAddress);
		//                SQLiteParameter pContact = new SQLiteParameter("@Contact", "" + product.contact);
		//                parameter.Add(pContact);
		//                SQLiteParameter pCategoryId = new SQLiteParameter("@CategoryId", System.Data.DbType.Int32);
		//                pCategoryId.Value = (product.categoryId == null ? 0 : product.categoryId);
		//                parameter.Add(pCategoryId);
		//                SQLiteParameter pBrandId = new SQLiteParameter("@BrandId", System.Data.DbType.Int32);
		//                pBrandId.Value = (product.BrandId == null ? 0 : product.BrandId);
		//                parameter.Add(pBrandId);
		//                SQLiteParameter pCreatedBy = new SQLiteParameter("@CreatedBy", System.Data.DbType.Int32);
		//                pCreatedBy.Value = (product.CreatedBy == null ? 0 : product.CreatedBy);
		//                parameter.Add(pCreatedBy);
		//                SQLiteParameter pCreatedAt = new SQLiteParameter("@CreatedAt", System.Data.DbType.DateTime);
		//                pCreatedAt.Value = GlobalUTIL.CurrentDateTime;
		//                parameter.Add(pCreatedAt);
		//                SQLiteParameter pLastUpdatedBy = new SQLiteParameter("@LastUpdatedBy", System.Data.DbType.Int32);
		//                pLastUpdatedBy.Value = (product.CreatedBy == null ? 0 : product.CreatedBy);// product.lastUpdatedBy;
		//                parameter.Add(pLastUpdatedBy);
		//                SQLiteParameter pLastUpdatedAt = new SQLiteParameter("@LastUpdatedAt", System.Data.DbType.DateTime);
		//                pLastUpdatedAt.Value = GlobalUTIL.CurrentDateTime;
		//                parameter.Add(pLastUpdatedAt);
		//                SQLiteParameter pStatus = new SQLiteParameter("@Status", System.Data.DbType.Int32);
		//                pStatus.Value = (product.status == 0 ? 1 : product.status);
		//                parameter.Add(pStatus);                       
		//                sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//                int insertedCount = sqlitecmd.ExecuteNonQuery();
		//                if ( !string.IsNullOrEmpty( product.removedImgSrNo))
		//                {
		//                    sqlitecmd.CommandText = "delete from  MediaContent where productId=" + product.id + " AND srNo IN (" + (string.IsNullOrWhiteSpace(product.removedImgSrNo) ? "0" : product.removedImgSrNo) + ")  ;";
		//                    // The row ID is a 64-bit value - cast the Command result to an Int64.                        //
		//                    sqlitecmd.ExecuteScalar();
		//                }

		//                if (product.MediaContents != null && product.MediaContents.Any())
		//                {


		//                    // Insert Images
		//                    foreach (MediaContentModel prd in product.MediaContents)
		//                    {
		//                        if (!string.IsNullOrWhiteSpace(prd.image) && prd.srNo != null)
		//                        {
		//                            sqlitecmd = sqliteConn.CreateCommand();
		//                            sqlitecmd.CommandText = "INSERT INTO MediaContent (Name,productId,Title,Description,SrNo,MimeType,LastUpdatedBy,LastUpdatedAt,Image,Status,RowVer ) VALUES (@Name,@productId,@Title,@Description,@SrNo,@MimeType,@LastUpdatedBy,@LastUpdatedAt,@Image,@Status,@RowVer); ";
		//                            parameter = new List<SQLiteParameter>();
		//                            SQLiteParameter pImageName = new SQLiteParameter("@Name", "" + prd.name);
		//                            parameter.Add(pImageName);
		//                            SQLiteParameter pTitle = new SQLiteParameter("@Title", "" + prd.name);
		//                            parameter.Add(pTitle);
		//                            SQLiteParameter pDesc = new SQLiteParameter("@Description", "" + prd.description);
		//                            parameter.Add(pDesc);
		//                            SQLiteParameter pProductId = new SQLiteParameter("@productId", System.Data.DbType.Int32);
		//                            pProductId.Value = product.id;
		//                            parameter.Add(pProductId);
		//                            SQLiteParameter pSrNo = new SQLiteParameter("@SrNo", System.Data.DbType.Int32);
		//                            pSrNo.Value = prd.srNo;
		//                            parameter.Add(pSrNo);
		//                            SQLiteParameter MimeType = new SQLiteParameter("@MimeType", prd.mimetype);
		//                            parameter.Add(MimeType);
		//                            SQLiteParameter pImageLastUpdatedBy = new SQLiteParameter("@LastUpdatedBy", System.Data.DbType.Int32);
		//                            pImageLastUpdatedBy.Value = product.CreatedBy;
		//                            parameter.Add(pImageLastUpdatedBy);
		//                            SQLiteParameter pImageLastUpdatedAt = new SQLiteParameter("@LastUpdatedAt", System.Data.DbType.DateTime);
		//                            pImageLastUpdatedAt.Value = product.lastUpdatedAt;
		//                            parameter.Add(pImageLastUpdatedAt);
		//                            SQLiteParameter pImageFileName = new SQLiteParameter("@Image", prd.image);
		//                            parameter.Add(pImageFileName);
		//                            SQLiteParameter pImageStatus = new SQLiteParameter("@Status", System.Data.DbType.Int16);
		//                            pImageStatus.Value = 1;
		//                            parameter.Add(pImageStatus);
		//                            SQLiteParameter pImageRowVer = new SQLiteParameter("@RowVer", System.Data.DbType.Int16);
		//                            pImageRowVer.Value = 1;
		//                            parameter.Add(pImageRowVer);
		//                            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//                            insertedCount = sqlitecmd.ExecuteNonQuery();
		//                        }// Validation Check 
		//                    }// For Each
		//                }
		//            }
		//        }
		//        catch (Exception ex) {
		//            throw ex;
		//        }
		//    }
		//    catch (Exception ex) {
		//        throw new Exception(string.Format("Reservation data storage in SQlite failed, Error detail {0}", ex.Message));
		//        // throw new Exception("Reservation data storage in SQlite failed");
		//    }
		//    return productId;
		//    // return new SQLiteConnection();
		//}
		///// <summary>
		///// loginLogout, login & logout for admin
		///// </summary>
		//public static bool loginLogout(LoginViewModel model, bool force = false)
		//{
		//    Boolean retStatus = false;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();
		//    try
		//    {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();
		//        SQLiteCommand sqlitecmd;
		//        try
		//        {
		//            sqlitecmd = sqliteConn.CreateCommand();
		//            if (force)
		//            {
		//                sqlitecmd.CommandText = "delete from  loginusers where id=@id; ";
		//                List<SQLiteParameter> dParam = new List<SQLiteParameter>();
		//                SQLiteParameter pid = new SQLiteParameter("@id", System.Data.DbType.Int64);
		//                pid.Value = model.id;
		//                dParam.Add(pid);
		//                sqlitecmd.Parameters.AddRange(dParam.ToArray());
		//                int res = sqlitecmd.ExecuteNonQuery();                      
		//            }                


		//            // sqlitecmd.CommandText = "INSERT INTO reservations (reference,mode,arrivaldate,departuredate,receivetime,firstname,lastname,paymentamount,transactiondate,voucherno,roomno,syncstatus,email) VALUES('Ahmad',2,'2014-10-23 15:21:07','2014-10-23 15:21:07','2014-10-23 15:21:07','arshad','ali',120,'2014-10-23 15:21:07','20nm','10vm',12,'znawazch@gmail.com'); ";
		//            sqlitecmd.CommandText = "INSERT INTO loginusers(loginname,password,logouttime,logintime,status)VALUES(@loginname,@password,@logouttime,@logintime,@status); ";
		//            List<SQLiteParameter> parameter = new List<SQLiteParameter>();                   
		//            SQLiteParameter pLoginName = new SQLiteParameter("@loginname", model.loginname);
		//            parameter.Add(pLoginName);
		//            SQLiteParameter pPassword = new SQLiteParameter("@password", model.password);
		//            parameter.Add(pPassword);
		//            //SQLiteParameter pMode = new SQLiteParameter("@mode", System.Data.DbType.Int16);
		//            //pMode.Value = model..mode;
		//            //parameter.Add(pMode);
		//            SQLiteParameter plogouttime = new SQLiteParameter("@logouttime", System.Data.DbType.DateTime);
		//            plogouttime.Value = model.logouttime;
		//            parameter.Add(plogouttime);
		//            SQLiteParameter plogintime = new SQLiteParameter("@logintime", System.Data.DbType.DateTime);
		//            plogintime.Value = model.logintime;
		//            parameter.Add(plogintime);

		//            SQLiteParameter pStatus = new SQLiteParameter("@status", System.Data.DbType.Int16);
		//            pStatus.Value = model.status;
		//            parameter.Add(pStatus);
		//            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//            int insertedCount = sqlitecmd.ExecuteNonQuery();

		//            sqlitecmd.CommandText = "select last_insert_rowid()";
		//            // The row ID is a 64-bit value - cast the Command result to an Int64.                        //
		//            Int64 LastRowID64 = (Int64)sqlitecmd.ExecuteScalar();

		//            retStatus = true;
		//        }
		//        catch (Exception ex)
		//        {
		//            throw new Exception("Table already exists");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        throw new Exception("Reservation data persist in database failed");
		//    }
		//    return retStatus;
		//    // return new SQLiteConnection();
		//}
		///// <summary>
		///// loadSQLiteData, load and return collection of products' data
		///// </summary>
		//public static List<ProductViewModel> loadSQLiteData() {
		//    List<ProductViewModel> lst = new List<ProductViewModel>();
		//    // Boolean retStatus = false;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();           
		//    try {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();
		//        try {

		//            SQLiteDataReader sqlite_datareader;
		//            SQLiteCommand sqlitecmd = sqliteConn.CreateCommand();
		//            //if (currentEntity > 0)
		//            //    sqlitecmd.CommandText = "SELECT * FROM reservations where DATE(transactiondate) between @dtFrom and @dtTo and  entity_id=@entity_id and syncstatus=@syncstatus;";
		//            //else
		//                sqlitecmd.CommandText = "SELECT * FROM product where status=@status;";
		//            List<SQLiteParameter> parameter = new List<SQLiteParameter>();                   
		//            SQLiteParameter pStatus = new SQLiteParameter("@status", System.Data.DbType.Int16);
		//            pStatus.Value = 1;
		//            parameter.Add(pStatus);

		//            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//            sqlite_datareader = sqlitecmd.ExecuteReader();

		//            while (sqlite_datareader.Read() && lst != null) {
		//                ProductViewModel entity = new ProductViewModel();
		//                //id,Name,ShortCode,Description,Price,Address,CategoryId,BrandId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Status
		//                entity.id = sqlite_datareader.GetInt32(0);
		//                //model.ui = sqlite_datareader.GetInt64(0);
		//                entity.name = "" + sqlite_datareader.GetValue(1);
		//                entity.shortCode = "" + sqlite_datareader.GetValue(2);// sqlite_datareader.GetString(1);
		//                 entity.description = "" + sqlite_datareader.GetValue(3);// sqlite_datareader.GetInt16(2);
		//                entity.price =sqlite_datareader.GetInt32(4);
		//                if (sqlite_datareader.IsDBNull(5) || "" + sqlite_datareader.GetString(5) == "")
		//                {
		//                    entity.address = com.blazor.decs.UTIL.GlobalSettings.DECSAddress;
		//                }
		//                else
		//                {
		//                    entity.address = "" + sqlite_datareader.GetValue(5);
		//                } //sqlite_datareader.GetInt32(4);
		//                entity.categoryId = sqlite_datareader.GetInt32(6);
		//                entity.BrandId = sqlite_datareader.GetInt32(7);
		//                entity.CreatedBy = sqlite_datareader.GetInt32(8);
		//                entity.CreatedAt =sqlite_datareader.GetDateTime(9);
		//                entity.lastUpdatedBy = sqlite_datareader.GetInt32(10);
		//                entity.lastUpdatedAt = sqlite_datareader.GetDateTime(11);
		//                entity.status = sqlite_datareader.GetInt32(12);
		//                if (sqlite_datareader.IsDBNull(13) || "" + sqlite_datareader.GetString(13) == "")
		//                {
		//                    entity.contact = com.blazor.decs.UTIL.GlobalSettings.DECSContact;
		//                }
		//                else
		//                {
		//                    entity.contact =""+ sqlite_datareader.GetValue(13);
		//                }


		//                using (SQLiteCommand mediaCmd = sqliteConn.CreateCommand())
		//                {
		//                    mediaCmd.CommandText = "SELECT * FROM MediaContent WHERE status = @status AND productId = @productId;";
		//                    SQLiteParameter pProductId = new SQLiteParameter("@productId", System.Data.DbType.Int32);
		//                    pProductId.Value = entity.id;
		//                    mediaCmd.Parameters.Add(pStatus);
		//                    mediaCmd.Parameters.Add(pProductId);

		//                    using (SQLiteDataReader mediaDataReader = mediaCmd.ExecuteReader())
		//                    {
		//                        List<MediaContentModel> mediaContents = new List<MediaContentModel>();
		//                        while (mediaDataReader.Read())
		//                        {
		//                            MediaContentModel mediaContent = new MediaContentModel();
		//                            mediaContent.id = mediaDataReader.GetInt32(0);

		//                            mediaContent.productId = mediaDataReader.GetInt32(2);

		//                            mediaContent.image = mediaDataReader.GetString(9);
		//                            mediaContent.srNo = mediaDataReader.GetInt32(5);

		//                            mediaContent.status = mediaDataReader.GetInt32(10);

		//                            mediaContents.Add(mediaContent);
		//                        }
		//                        entity.MediaContents = mediaContents;
		//                    }
		//                }
		//                lst.Add(entity);
		//            }
		//            // sqliteConn.Close();
		//        }
		//        catch (Exception ex) {
		//            throw new Exception("Table already exists");
		//        }
		//    }
		//    catch (Exception ex) {
		//        Console.WriteLine(ex.Message);
		//        return null;
		//    }
		//    return lst;
		//    // return new SQLiteConnection();
		//}
		///// <summary>
		///// loadLoginData, load login data, with BOOL return
		///// </summary>
		//public static bool loadLoginData(LoginViewModel model)
		//{
		//    List<ProductViewModel> lst = new List<ProductViewModel>();
		//    // Boolean retStatus = false;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();

		//    try
		//    {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();

		//        SQLiteCommand sqlitecmd = sqliteConn.CreateCommand();
		//        sqlitecmd.CommandText = "SELECT COUNT(*) FROM loginusers WHERE loginname = @loginname AND password = @password;";

		//        sqlitecmd.Parameters.AddWithValue("@loginname", model.loginname);
		//        sqlitecmd.Parameters.AddWithValue("@password", model.password);

		//        int count = Convert.ToInt32(sqlitecmd.ExecuteScalar());

		//        if(count > 0)
		//        {
		//            return true;
		//        }
		//        else { return false; }
		//    }
		//    catch (Exception ex)
		//    {
		//        Console.WriteLine(ex.Message);
		//        return false;
		//    }

		//}
		// return new SQLiteConnection();

		//public static viewmodels.SQLiteMappingViewModel loadSQLiteDataDetailed(Int64 Uid)
		//{
		//    viewmodels.SQLiteMappingViewModel model = new viewmodels.SQLiteMappingViewModel();
		//    // Boolean retStatus = false;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();
		//    try
		//    {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();
		//        try
		//        {

		//            SQLiteDataReader sqlite_datareader;
		//            SQLiteCommand sqlitecmd = sqliteConn.CreateCommand();
		//            sqlitecmd.CommandText = "SELECT * FROM reservations where id=@id";
		//            List<SQLiteParameter> parameter = new List<SQLiteParameter>();
		//            SQLiteParameter pUid = new SQLiteParameter("@id", System.Data.DbType.Int64);
		//            pUid.Value = Uid;
		//            parameter.Add(pUid);
		//            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//            sqlite_datareader = sqlitecmd.ExecuteReader();

		//            if (sqlite_datareader.Read() && model != null)
		//            {


		//                model.id = sqlite_datareader.GetInt64(0);
		//                model.reference = "" + sqlite_datareader.GetValue(1);// sqlite_datareader.GetString(1);
		//                model.mode = sqlite_datareader.GetInt16(2);
		//                model.arrivaldate = sqlite_datareader.GetDateTime(3);
		//                model.departuredate = sqlite_datareader.GetDateTime(4);
		//                model.receivetime = sqlite_datareader.GetDateTime(5);
		//                model.firstname = "" + sqlite_datareader.GetValue(6);
		//                model.lastname = "" + sqlite_datareader.GetValue(7);
		//                model.paymentamount = Convert.ToDouble(sqlite_datareader.GetValue(8));
		//                model.transactiondate = sqlite_datareader.GetDateTime(9);
		//                model.voucherno = "" + sqlite_datareader.GetValue(10);
		//                model.roomno = "" + sqlite_datareader.GetValue(11);// sqlite_datareader.GetString(11);
		//                model.syncstatus = sqlite_datareader.GetInt32(12);
		//                model.email = "" + sqlite_datareader.GetValue(13); //sqlite_datareader.GetString(13);
		//                model.entity_id = sqlite_datareader.GetInt16(14);
		//                model.entity_name = "" + sqlite_datareader.GetValue(15); //sqlite_datareader.GetString(15);
		//                model.entity_type = sqlite_datareader.GetInt16(16);
		//                model.undo = sqlite_datareader.GetInt16(17);
		//                model.payableamount = Convert.ToDouble(sqlite_datareader.GetValue(18));// sqlite_datareader.GetInt16(18);

		//            }
		//            // sqliteConn.Close();
		//        }
		//        catch (Exception ex)
		//        {
		//            throw new Exception("Table already exists");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        return null;
		//    }
		//    return model;
		//    // return new SQLiteConnection();
		//}
		//public static viewmodels.MappingViewModel loadSQLiteFullData(Int64 id, int status = 1)
		//{
		//    viewmodels.MappingViewModel model = new viewmodels.MappingViewModel();
		//    List<string> lstFrms = new List<string>();
		//    Boolean retStatus = false;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();
		//    try
		//    {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();
		//        try
		//        {
		//            SQLiteDataReader sqlite_datareader;
		//            SQLiteCommand sqlitecmd = sqliteConn.CreateCommand();
		//            sqlitecmd.CommandText = "SELECT  * FROM reservations where id=@id;";
		//            List<SQLiteParameter> parameter = new List<SQLiteParameter>();
		//            SQLiteParameter pId = new SQLiteParameter("@id", System.Data.DbType.Int64);
		//            pId.Value = id;
		//            parameter.Add(pId);
		//            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//            sqlite_datareader = sqlitecmd.ExecuteReader();
		//            if (sqlite_datareader.Read() && model != null)
		//            {
		//                model.uid = sqlite_datareader.GetInt64(0);
		//                model.reference = "" + sqlite_datareader.GetValue(1);// sqlite_datareader.GetString(1);
		//                model.mode = sqlite_datareader.GetInt32(2);
		//                model.createdAt = sqlite_datareader.GetDateTime(9);
		//                model.roomno = "" + sqlite_datareader.GetValue(11);// sqlite_datareader.GetString(11);
		//                model.status = sqlite_datareader.GetInt32(12);
		//                model.entity_Id = sqlite_datareader.GetInt32(14);
		//                model.id = model.entity_Id;
		//                model.entity_name = "" + sqlite_datareader.GetValue(15);// sqlite_datareader.GetString(15);
		//                model.undo = sqlite_datareader.GetInt16(17);
		//                // model.pay = Convert.ToDouble(sqlite_datareader.GetValue(8)) sqlite_datareader.GetInt16(17);
		//                sqlite_datareader.Close();
		//                int fromId = 0;
		//                // Get field Data
		//                if (model.id > 0)
		//                {
		//                    //sqlitecmd = sqliteConn.CreateCommand();
		//                    sqlitecmd.CommandText = "SELECT sr,fuid,field_desc,parent_field_id,pms_field_name,pms_field_xpath,pms_field_expression,value,automation_mode,ocrFactor,ocrImage,default_value,format, control_type,mandatory,is_reference,is_unmapped,maxLength,scan,feed,data_type,action_type,form, formname,custom_tag FROM fields where idref=@refid";
		//                    parameter = new List<SQLiteParameter>();
		//                    SQLiteParameter refid = new SQLiteParameter("@refid", System.Data.DbType.Int64);
		//                    refid.Value = id;
		//                    parameter.Add(refid);
		//                    ////SQLiteParameter pstatus = new SQLiteParameter("@status", System.Data.DbType.Int16);
		//                    ////pstatus.Value = status;
		//                    ////parameter.Add(pstatus);
		//                    sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//                    SQLiteDataReader sqlitedrdr = sqlitecmd.ExecuteReader();
		//                    List<viewmodels.EntityFieldViewModel> flds = new List<viewmodels.EntityFieldViewModel>();
		//                    List<viewmodels.FormViewModel> frmls = new List<viewmodels.FormViewModel>();
		//                    while (sqlitedrdr.Read() && model != null)
		//                    {
		//                        viewmodels.EntityFieldViewModel fldModel = new viewmodels.EntityFieldViewModel();
		//                        //fldModel.id = id;
		//                        fldModel.sr = sqlitedrdr.GetInt32(0);
		//                        fldModel.fuid = "" + sqlitedrdr.GetValue(1);// sqlitedrdr.GetString(1);
		//                        fldModel.field_desc = "" + sqlitedrdr.GetValue(2);// sqlitedrdr.GetString(2);
		//                        fldModel.parent_field_id = "" + sqlitedrdr.GetValue(3);// sqlitedrdr.GetString(3);
		//                        fldModel.pms_field_name = "" + sqlitedrdr.GetValue(4);// sqlitedrdr.GetString(4);
		//                        fldModel.pms_field_xpath = "" + sqlitedrdr.GetValue(5);// sqlitedrdr.GetString(5);
		//                        fldModel.pms_field_expression = "" + sqlitedrdr.GetValue(6);// sqlitedrdr.GetString(6);
		//                        fldModel.value = "" + sqlitedrdr.GetValue(7);// sqlitedrdr.GetString(7);
		//                        fldModel.automation_mode = sqlitedrdr.GetInt32(8);
		//                        fldModel.ocrFactor = Convert.ToDouble(sqlitedrdr.GetValue(9));// sqlitedrdr.GetDouble(9);
		//                        fldModel.ocrImage = "" + sqlitedrdr.GetValue(10);// sqlitedrdr.GetString(10);
		//                        fldModel.default_value = "" + sqlitedrdr.GetValue(11);// sqlitedrdr.GetString(11);
		//                        fldModel.format = "" + sqlitedrdr.GetValue(12);// sqlitedrdr.GetString(12);
		//                        fldModel.control_type = sqlitedrdr.GetInt16(13);
		//                        fldModel.mandatory = sqlitedrdr.GetInt16(14);
		//                        fldModel.is_reference = sqlitedrdr.GetInt16(15);
		//                        fldModel.is_unmapped = sqlitedrdr.GetInt16(16);
		//                        fldModel.maxLength = sqlitedrdr.GetInt16(17);
		//                        fldModel.scan = sqlitedrdr.GetInt16(18);
		//                        fldModel.feed = sqlitedrdr.GetInt16(19);
		//                        fldModel.data_type = sqlitedrdr.GetInt16(20);
		//                        fldModel.action_type = sqlitedrdr.GetInt16(21);
		//                        fldModel.custom_tag = "" + sqlitedrdr.GetValue(24);// Custom tag
		//                        fldModel.entity_id = model.entity_Id;
		//                        flds.Add(fldModel);
		//                        if (frmls.Where(x => x.pmspageid.Contains("" + sqlitedrdr.GetValue(22))).FirstOrDefault() == null)
		//                        {
		//                            //form, formname
		//                            viewmodels.FormViewModel frm = new viewmodels.FormViewModel();
		//                            frm.id = fromId + 1;
		//                            frm.entityid = model.entity_Id;
		//                            frm.pmspageid = "" + sqlitedrdr.GetValue(22);// sqlitedrdr.GetString(22);
		//                            frm.pmspagename = "" + sqlitedrdr.GetValue(23);// sqlitedrdr.GetString(23);                                   
		//                            frm.fields = flds;
		//                            frmls.Add(frm);
		//                            if (frmls.Count > 1)
		//                                flds = new List<viewmodels.EntityFieldViewModel>();
		//                        }
		//                    }// while (sqlitedrdr.Read() && model != null)
		//                    if (frmls != null && frmls.Any())
		//                        model.forms = frmls;
		//                    sqlitedrdr.Close();
		//                }
		//            }// While
		//             // sqliteConn.Close();
		//        }
		//        catch (Exception ex)
		//        {
		//            throw new Exception("Table already exists");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        return null;
		//    }
		//    return model;
		//    // return new SQLiteConnection();
		//}
		//public static viewmodels.MappingViewModel loadSQLiteFullDataWithAllFields(Int64 id, int pEntityId)
		//{
		//    viewmodels.MappingViewModel model = new viewmodels.MappingViewModel();
		//    viewmodels.MappingViewModel SModel = API.PRESETS.mappings.Where(x => x.entity_Id == pEntityId).FirstOrDefault();

		//    List<string> lstFrms = new List<string>();
		//    Boolean retStatus = false;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();
		//    try
		//    {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();
		//        try
		//        {
		//            SQLiteDataReader sqlite_datareader;
		//            SQLiteCommand sqlitecmd = sqliteConn.CreateCommand();
		//            sqlitecmd.CommandText = "SELECT  * FROM reservations where id=@id;";
		//            List<SQLiteParameter> parameter = new List<SQLiteParameter>();
		//            SQLiteParameter pId = new SQLiteParameter("@id", System.Data.DbType.Int64);
		//            pId.Value = id;
		//            parameter.Add(pId);
		//            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//            sqlite_datareader = sqlitecmd.ExecuteReader();
		//            if (sqlite_datareader.Read() && model != null)
		//            {
		//                model.uid = sqlite_datareader.GetInt64(0);
		//                model.reference = "" + sqlite_datareader.GetValue(1);// sqlite_datareader.GetString(1);
		//                model.mode = sqlite_datareader.GetInt32(2);
		//                model.createdAt = sqlite_datareader.GetDateTime(9);
		//                model.roomno = "" + sqlite_datareader.GetValue(11);// sqlite_datareader.GetString(11);
		//                model.status = sqlite_datareader.GetInt32(12);
		//                model.entity_Id = sqlite_datareader.GetInt32(14);
		//                model.undo = sqlite_datareader.GetInt16(17);
		//                model.id = model.entity_Id;
		//                model.entity_name = "" + sqlite_datareader.GetValue(15);// sqlite_datareader.GetString(15);
		//                sqlite_datareader.Close();
		//                int fromId = 0;
		//                // Get field Data
		//                if (model.id > 0)
		//                {
		//                    //sqlitecmd = sqliteConn.CreateCommand();
		//                    sqlitecmd.CommandText = "SELECT sr,fuid,field_desc,parent_field_id,pms_field_name,pms_field_xpath,pms_field_expression,value,automation_mode,ocrFactor,ocrImage,default_value,format, control_type,mandatory,is_reference,is_unmapped,maxLength,scan,feed,data_type,action_type,form, formname,custom_tag FROM fields where idref=@refid";
		//                    parameter = new List<SQLiteParameter>();
		//                    SQLiteParameter refid = new SQLiteParameter("@refid", System.Data.DbType.Int64);
		//                    refid.Value = id;
		//                    parameter.Add(refid);
		//                    ////SQLiteParameter pstatus = new SQLiteParameter("@status", System.Data.DbType.Int16);
		//                    ////pstatus.Value = status;
		//                    ////parameter.Add(pstatus);
		//                    sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//                    SQLiteDataReader sqlitedrdr = sqlitecmd.ExecuteReader();
		//                    List<viewmodels.EntityFieldViewModel> flds = new List<viewmodels.EntityFieldViewModel>();
		//                    List<viewmodels.FormViewModel> frmls = new List<viewmodels.FormViewModel>();
		//                    while (sqlitedrdr.Read() && model != null)
		//                    {
		//                        viewmodels.EntityFieldViewModel fldModel = new viewmodels.EntityFieldViewModel();
		//                        //fldModel.id = id;
		//                        fldModel.sr = sqlitedrdr.GetInt32(0);
		//                        fldModel.fuid = "" + sqlitedrdr.GetValue(1);// sqlitedrdr.GetString(1);
		//                        fldModel.field_desc = "" + sqlitedrdr.GetValue(2);// sqlitedrdr.GetString(2);
		//                        fldModel.parent_field_id = "" + sqlitedrdr.GetValue(3);// sqlitedrdr.GetString(3);
		//                        fldModel.pms_field_name = "" + sqlitedrdr.GetValue(4);// sqlitedrdr.GetString(4);
		//                        fldModel.pms_field_xpath = "" + sqlitedrdr.GetValue(5);// sqlitedrdr.GetString(5);
		//                        fldModel.pms_field_expression = "" + sqlitedrdr.GetValue(6);// sqlitedrdr.GetString(6);
		//                        fldModel.value = "" + sqlitedrdr.GetValue(7);// sqlitedrdr.GetString(7);
		//                        fldModel.automation_mode = sqlitedrdr.GetInt32(8);
		//                        fldModel.ocrFactor = Convert.ToDouble(sqlitedrdr.GetValue(9));// sqlitedrdr.GetDouble(9);
		//                        fldModel.ocrImage = "" + sqlitedrdr.GetValue(10);// sqlitedrdr.GetString(10);
		//                        fldModel.default_value = "" + sqlitedrdr.GetValue(11);// sqlitedrdr.GetString(11);
		//                        fldModel.format = "" + sqlitedrdr.GetValue(12);// sqlitedrdr.GetString(12);
		//                        fldModel.control_type = sqlitedrdr.GetInt16(13);
		//                        fldModel.mandatory = sqlitedrdr.GetInt16(14);
		//                        fldModel.is_reference = sqlitedrdr.GetInt16(15);
		//                        fldModel.is_unmapped = sqlitedrdr.GetInt16(16);
		//                        fldModel.maxLength = sqlitedrdr.GetInt16(17);
		//                        fldModel.scan = sqlitedrdr.GetInt16(18);
		//                        fldModel.feed = sqlitedrdr.GetInt16(19);
		//                        fldModel.data_type = sqlitedrdr.GetInt16(20);
		//                        fldModel.action_type = sqlitedrdr.GetInt16(21);
		//                        fldModel.custom_tag = "" + sqlitedrdr.GetValue(24);
		//                        fldModel.entity_id = model.entity_Id;
		//                        flds.Add(fldModel);
		//                        if (frmls.Where(x => x.pmspageid.Contains("" + sqlitedrdr.GetValue(22))).FirstOrDefault() == null)
		//                        {
		//                            //form, formname
		//                            viewmodels.FormViewModel frm = new viewmodels.FormViewModel();
		//                            frm.id = fromId + 1;
		//                            frm.entityid = model.entity_Id;
		//                            frm.pmspageid = "" + sqlitedrdr.GetValue(22);// sqlitedrdr.GetString(22);
		//                            frm.pmspagename = "" + sqlitedrdr.GetValue(23);// sqlitedrdr.GetString(23);                                   
		//                            frm.fields = flds;
		//                            frmls.Add(frm);
		//                            if (frmls.Count > 1)
		//                                flds = new List<viewmodels.EntityFieldViewModel>();
		//                        }
		//                    }// while (sqlitedrdr.Read() && model != null)
		//                    if (frmls != null && frmls.Any())
		//                    {
		//                        foreach (viewmodels.FormViewModel vfrm in SModel.forms.Where(x => x.Status == 1))
		//                        {
		//                            viewmodels.FormViewModel nForm = frmls.Where(x => x.pmspageid.ToLower() == vfrm.pmspageid.ToLower() && x.pmspagename.ToLower() == vfrm.pmspagename.ToLower()).FirstOrDefault();
		//                            if (nForm != null)
		//                            {
		//                                foreach (viewmodels.EntityFieldViewModel fld in vfrm.fields.Where(x => x.status == 1))
		//                                {
		//                                    if (nForm.fields.Where(t => t.fuid == fld.fuid && fld.entity_id == model.entity_Id).FirstOrDefault() == null)
		//                                    {
		//                                        nForm.fields.Add(new viewmodels.EntityFieldViewModel { fuid = fld.fuid, feed = fld.feed, scan = fld.scan, entity_id = fld.entity_id, is_reference = fld.is_reference, is_unmapped = fld.is_unmapped, control_type = fld.control_type, field_desc = fld.field_desc, sr = fld.sr, status = fld.status, pms_field_expression = fld.pms_field_expression, maxLength = fld.maxLength, default_value = fld.default_value, parent_field_id = fld.parent_field_id, action_type = fld.action_type, data_type = fld.data_type, mandatory = fld.mandatory, format = fld.format, schema_field_name = fld.schema_field_name, pms_field_xpath = fld.pms_field_xpath, pms_field_name = fld.pms_field_name, ocrImage = fld.ocrImage, ocrFactor = fld.ocrFactor, automation_mode = fld.automation_mode, custom_tag = fld.custom_tag });
		//                                    }
		//                                }
		//                            }
		//                            else
		//                            {
		//                                frmls.Add(nForm.DeepClone());
		//                            }
		//                        }// Form Level
		//                        model.forms = frmls;
		//                    }
		//                    sqlitedrdr.Close();
		//                }
		//            }// While
		//             // sqliteConn.Close();
		//        }
		//        catch (Exception ex)
		//        {
		//            throw new Exception("Table already exists");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        return null;
		//    }
		//    return model;
		//    // return new SQLiteConnection();
		//}
		//public static int updateReservationStatus(Int64 id, int status)
		//{

		//    int retStatus = 0;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();
		//    try
		//    {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();
		//        try
		//        {

		//            SQLiteDataReader sqlite_datareader;
		//            SQLiteCommand sqlitecmd = sqliteConn.CreateCommand();

		//            // Purge Fields Data
		//            sqlitecmd.CommandText = "update reservations set syncstatus=@syncstatus, transactiondate=@transactiondate where id=@id;";
		//            List<SQLiteParameter> parameter = new List<SQLiteParameter>();
		//            SQLiteParameter pId = new SQLiteParameter("@id", System.Data.DbType.Int64);
		//            pId.Value = id;
		//            parameter.Add(pId);
		//            SQLiteParameter pSyncstatus = new SQLiteParameter("@syncstatus", System.Data.DbType.Int16);
		//            pSyncstatus.Value = status;
		//            parameter.Add(pSyncstatus);
		//            SQLiteParameter pTransactiondate = new SQLiteParameter("@transactiondate", System.Data.DbType.DateTime);
		//            pTransactiondate.Value = GlobalApp.CurrentLocalDateTime;
		//            parameter.Add(pTransactiondate);
		//            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//            retStatus = sqlitecmd.ExecuteNonQuery();
		//            // Fields
		//            if (status != (int)util.Enums.APPROVAL_STATUS.NEW_ISSUED)
		//            {
		//                sqlitecmd.CommandText = "delete from fields where idref=@idref";
		//                List<SQLiteParameter> fParams = new List<SQLiteParameter>();
		//                SQLiteParameter pidref = new SQLiteParameter("@idref", System.Data.DbType.Int64);
		//                pidref.Value = id;
		//                fParams.Add(pidref);
		//                sqlitecmd.Parameters.AddRange(fParams.ToArray());
		//                sqlitecmd.ExecuteNonQuery();
		//            }

		//            // sqliteConn.Close();
		//        }
		//        catch (Exception ex)
		//        {
		//            throw new Exception("Delete failed, try another time or contact vendor.");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        return retStatus;
		//    }
		//    return retStatus;
		//    // return new SQLiteConnection();
		//}
		//public static viewmodels.MappingViewModel updateSQLiteFullData(Int64 id, viewmodels.MappingViewModel model)
		//{
		//    int retStatus = 0;
		//    if (sqliteConn == null)
		//        sqliteConn = GetSQLiteConnection();
		//    try
		//    {
		//        if (sqliteConn.State != System.Data.ConnectionState.Open)
		//            sqliteConn.Open();
		//        try
		//        {

		//            SQLiteDataReader sqlite_datareader;
		//            SQLiteCommand sqlitecmd = sqliteConn.CreateCommand();
		//            sqlitecmd.CommandText = "update reservations set syncstatus=@syncstatus where id= @id; ";// where  syncstatus = @syncstatus";                   
		//            List<SQLiteParameter> parameter = new List<SQLiteParameter>();
		//            SQLiteParameter pId = new SQLiteParameter("@id", System.Data.DbType.Int64);
		//            pId.Value = model.uid;
		//            parameter.Add(pId);
		//            SQLiteParameter psyncstatus = new SQLiteParameter("@syncstatus", System.Data.DbType.Int16);
		//            psyncstatus.Value = model.saves_status;
		//            parameter.Add(psyncstatus);
		//            sqlitecmd.Parameters.AddRange(parameter.ToArray());
		//            // sqlitecmd.CommandText = CreateFieldSQL;
		//            retStatus = sqlitecmd.ExecuteNonQuery();
		//        }
		//        catch (Exception ex)
		//        {
		//            throw new Exception("Update failed");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        retStatus = 0;
		//    }
		//    return model;
		//    // return new SQLiteConnection();
		//}

		/// <summary>
		/// DeleteProductData, Delete product record entire
		/// </summary>
		public static Int32 DeleteProductData(Int64 id)
        {
            int retStatus = 0;
            if (sqliteConn == null)
                sqliteConn = GetSQLiteConnection();
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                    sqliteConn.Open();
                try
                {

                    SQLiteDataReader sqlite_datareader;
                    SQLiteCommand sqlitecmd = sqliteConn.CreateCommand();
                    sqlitecmd.CommandText = "update product set status=@syncstatus where id= @id; ";// where  syncstatus = @syncstatus";                   
                    List<SQLiteParameter> parameter = new List<SQLiteParameter>();
                    SQLiteParameter pId = new SQLiteParameter("@id", System.Data.DbType.Int64);
                    pId.Value = id;
                    parameter.Add(pId);
                    SQLiteParameter psyncstatus = new SQLiteParameter("@syncstatus", System.Data.DbType.Int16);
                    psyncstatus.Value = 2;//Deleted
                    parameter.Add(psyncstatus);
                    sqlitecmd.Parameters.AddRange(parameter.ToArray());
                    // sqlitecmd.CommandText = CreateFieldSQL;
                    retStatus = sqlitecmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Update failed");
                }
            }
            catch (Exception ex)
            {
                retStatus = 0;
            }
            return retStatus;
            // return new SQLiteConnection();
        }

    }
}
