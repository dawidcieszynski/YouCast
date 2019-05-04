using System.Linq;
using SharpNetSH;

namespace YouCast.Helpers
{
    public class FirewallRules : NetShResult
    {
        public FirewallRules(IResponse response) : base(response.Response)
        {
            if (!(response.ResponseObject is Tree responseResponseObject))
                return;

            Rules = responseResponseObject.Children.Select(
                c =>
                {
                    var urlReservation = new FirewallRule($"{c.Value}");
                    foreach (var child in c.Children)
                    {
                        urlReservation.Data[child.Title] = $"{child.Value}";
                        foreach (var child2 in child.Children)
                        {
                            urlReservation.Data[child2.Title] = $"{child2.Value}";
                        }
                    }

                    if (int.TryParse(c.Children[7].Value.ToString(), out var localPort))
                    {
                        urlReservation.LocalPort = localPort;
                    }

                    return urlReservation;
                }).ToArray();
        }

        public FirewallRule[] Rules { get; } = new FirewallRule[0];
    }
}