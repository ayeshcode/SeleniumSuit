using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;

namespace SeleniumSuit
{
    class DatabaceSet
    {
    private static string applicationName = string.Empty;
        public static SqlConnection connection;

        public static SqlConnection GetConnection()
        {
            if (DatabaceSet.connection != null)
            {
                if (DatabaceSet.connection.State == ConnectionState.Open)
                    return DatabaceSet.connection;
                if (DatabaceSet.connection != null)
                    DatabaceSet.connection.Dispose();
                DatabaceSet.connection = (SqlConnection)null;
            }
            DatabaceSet.connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            DatabaceSet.connection.Open();
            return DatabaceSet.connection;
        }

       
        public static void StoreInDB(
          string Scenario,
          string PageName,
          string ElementName,
          string TestDescription,
          TestResult TestStatus)
        {
            SeleniumStatus seleniumStatus = new SeleniumStatus();
            seleniumStatus.Scenario = Scenario + " - Dataset " + (object)Common.RowNumber;
            seleniumStatus.PageName = PageName;
            seleniumStatus.ElementName = ElementName;
            seleniumStatus.TestDescription = TestDescription;
            seleniumStatus.TestStatus = TestStatus.ToString();
            seleniumStatus.UserName = WindowsIdentity.GetCurrent().Name;
            seleniumStatus.TestID = ExecuteTest.TestID;
            seleniumStatus.ParentID = "";
            if (Common.Screenshot == null || TestStatus.ToString() == "Success")
            {
                seleniumStatus.Screenshot = " ";
                seleniumStatus.FailureURL = " ";
            }
            else
            {
                seleniumStatus.Screenshot = Common.Screenshot;
                seleniumStatus.FailureURL = Common.FailureURL;
            }
            Guid guid = TestRun.GetAll().Where<TestRun>((Func<TestRun, bool>)(run => run.ExecutedTime.ToString() == ExecuteTest.scenarioTime.ToString())).Select<TestRun, Guid>((Func<TestRun, Guid>)(run => run.TestRunID)).FirstOrDefault<Guid>();
            seleniumStatus.TestRunID = guid;
            Persistance.Create((object)seleniumStatus);
        }

       

       

        

        private static DataTable GetDatabases(SqlConnection conn)
        {
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_databases";
            DataTable dataTable = new DataTable();
            dataTable.Load((IDataReader)command.ExecuteReader());
            return dataTable;
        }
    }
}

    

