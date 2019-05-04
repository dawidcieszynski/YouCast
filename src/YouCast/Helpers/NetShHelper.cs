using System;
using SharpNetSH;
using SharpNetSH.ADVFIREWALL.FIREWALL.Enums;
using Action = SharpNetSH.ADVFIREWALL.FIREWALL.Enums.Action;

namespace YouCast.Helpers
{
    public class NetShHelper
    {
        private readonly NetSH _netSh;

        public NetShHelper()
        {
            _netSh = new NetSH(new CommandLineHarness());
        }

        public FirewallRules GetFirewallRule(string applicationName)
        {
            var response = _netSh.AdvFirewall.Firewall.Show.Rule(applicationName);
            return new FirewallRules(response);
        }

        public bool CreateFirewallRule(string name, int port)
        {
            var response = _netSh.AdvFirewall.Firewall.Add.Rule(name, Direction.In, Action.Allow, protocol: Protocol.Tcp, localport: port);
            return response.IsNormalExit;
        }

        public bool DeleteFirewallRule(string name)
        {
            var response = _netSh.AdvFirewall.Firewall.Delete.Rule(name);
            return response.IsNormalExit;
        }

        public bool UpdateFirewallRule(string name, int port)
        {
            var response = _netSh.AdvFirewall.Firewall.Set.Rule(name, localport: port);
            return response.IsNormalExit;
        }

        public UrlReservations GetUrlAcl(string url)
        {
            var response = _netSh.Http.Show.UrlAcl(url);
            return new UrlReservations(response);
        }

        public bool CreateUrlAcl(string url)
        {
            var user = $"{Environment.UserDomainName}\\{Environment.UserName}";
            var response = _netSh.Http.Add.UrlAcl(url, user, null);
            return response.IsNormalExit;
        }

        public bool DeleteUrlAcl(string url)
        {
            var response = _netSh.Http.Delete.UrlAcl(url);
            return response.IsNormalExit;
        }
    }
}