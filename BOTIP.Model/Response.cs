namespace BOTIP.Model
{
    public class Response
    {
       public Dictionary<string, int> suspiciousIps { get; set; }

        public string Error { get; set; }
    }
}