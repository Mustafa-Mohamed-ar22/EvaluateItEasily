namespace EvaluateItEasily.Infrastructure.Helpers
{
    public class EmailBodyBuilder
    {
      
        public static string GenerateEmailBody(string rootPath, string template,
            Dictionary<string, string> templateModel)
        {
            var templatePath = Path.Combine(rootPath, "Templates",$"{template}.html" );

            var streamReader = new StreamReader(templatePath);

            var body = streamReader.ReadToEnd();

            streamReader.Close();

            foreach (var item in templateModel)

                body = body.Replace(item.Key, item.Value);

            return body;
        }
    }
}
