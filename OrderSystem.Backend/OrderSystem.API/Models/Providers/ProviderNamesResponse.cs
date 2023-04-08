namespace OrderSystem.API.Models.Providers
{
    public class ProvidersResponse
    {
        public ProviderModel[] Providers { get; set; }
        public string Error { get; set; }

        public class ProviderModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}