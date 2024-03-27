namespace RP_DotNetCore_DevApp.AppCode
{
    public class LdapOptions
    {
        public LdapServer Server { get; set; }
        public string BindDn { get; set; }
        public string BindCredentials { get; set; }
        public string SearchBase { get; set; }
        public string SearchFilter { get; set; }
    }

    public class LdapServer
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
