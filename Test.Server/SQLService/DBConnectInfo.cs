namespace Test.Server.SQLService
{
    public class DBConnectInfo
    {
        public string IP;
        public string Port;
        public string UserName;
        public string Password;
        public string Database;

        public DBConnectInfo()
        {
            SetDefaultData();
            Database = string.Empty;
        }

        public DBConnectInfo(string database)
        {
            SetDefaultData();
            Database = database;
        }

        private void SetDefaultData()
        {
            IP = "localhost";
            Port = "778";
            UserName = "postgres";
            Password = "aaa";
        }

        public string GetConnectString()
        {
            return $"Server={IP};Port={Port};Username={UserName};Password={Password}" + (string.IsNullOrEmpty(Database) ? string.Empty : $";Database={Database}");
        }
    }
}
