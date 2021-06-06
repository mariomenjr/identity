namespace Identity.DAL.Mongo.Settings
{
    public interface IMongoSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}