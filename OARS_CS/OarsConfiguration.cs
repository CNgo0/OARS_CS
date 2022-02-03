namespace OARS
{
    public enum OarsApiEnv
    {
        Development,
        Production
    }

    public enum OarsDbEnv
    {
        Development,
        Production
    }

    public class OarsConfiguration
    {
        public string project;
        public string key;
        public OarsApiEnv apiEnv;
        public OarsDbEnv dbEnv;

        public OarsConfiguration(string project, string key)
        {
            this.project = project;
            this.key = key;
            this.apiEnv = OarsApiEnv.Production;
            this.dbEnv = OarsDbEnv.Production;
        }

        public void SetEnvironment(OarsApiEnv apiEnv, OarsDbEnv dbEnv)
        {
            this.apiEnv = apiEnv;
            this.dbEnv = dbEnv;
        }
    }
}
