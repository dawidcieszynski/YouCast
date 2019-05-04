using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using YouCast.Helpers;

namespace YouCast.Tests.Helpers
{
    public class NetShHelperFirewallTests : IDisposable
    {
        private string TestRuleName(string name) => $"NetShHelperTests_{name}";
        private const int TestPorts = 2345;

        [SkippableFact]
        public void ShouldBeAbleToCreateFirewallRule()
        {
            Skip.If(!PermissionsHelper.IsRunAsAdministrator(), "require administrator permissions");
            var sut = new NetShHelper();

            var result = sut.CreateFirewallRule(TestRuleName(nameof(ShouldBeAbleToCreateFirewallRule)), TestPorts + 1);

            result.Should().BeTrue();
        }

        [SkippableFact]
        public void ShouldBeAbleToGetFirewallRule()
        {
            Skip.If(!PermissionsHelper.IsRunAsAdministrator(), "require administrator permissions");
            var sut = new NetShHelper();
            PrepareTestRule(sut, TestRuleName(nameof(ShouldBeAbleToGetFirewallRule)), TestPorts + 4);

            var result = sut.GetFirewallRule(TestRuleName(nameof(ShouldBeAbleToGetFirewallRule)));

            result.Should().NotBeNull();
            result.Rules.First().RuleName.Should().Be(TestRuleName(nameof(ShouldBeAbleToGetFirewallRule)));
            result.Rules.First().LocalPort.Should().Be(TestPorts + 4);
        }

        private void PrepareTestRule(NetShHelper sut, string name, int port)
        {
            var createResult = sut.CreateFirewallRule(name, port);
            if (createResult == false)
            {
                sut.DeleteFirewallRule(name).Should().BeTrue();
                createResult = sut.CreateFirewallRule(name, port);
            }

            createResult.Should().BeTrue();

            var getResult = sut.GetFirewallRule(name);
            getResult.Should().NotBeNull();
            getResult.Rules.First().LocalPort.Should().Be(port);
        }

        [SkippableFact]
        public void ShouldBeAbleToDeleteFirewallRule()
        {
            Skip.If(!PermissionsHelper.IsRunAsAdministrator(), "require administrator permissions");
            var sut = new NetShHelper();
            PrepareTestRule(sut, TestRuleName(nameof(ShouldBeAbleToDeleteFirewallRule)), TestPorts + 3);

            var result = sut.DeleteFirewallRule(TestRuleName(nameof(ShouldBeAbleToDeleteFirewallRule)));

            result.Should().BeTrue();
            var getResult = sut.GetFirewallRule(TestRuleName(nameof(ShouldBeAbleToDeleteFirewallRule)));
            getResult.Should().NotBeNull();
            getResult.Rules.Should().BeEmpty();
        }

        [SkippableFact]
        public void ShouldBeAbleToUpdateFirewallRule()
        {
            Skip.If(!PermissionsHelper.IsRunAsAdministrator(), "require administrator permissions");
            var sut = new NetShHelper();
            PrepareTestRule(sut, TestRuleName(nameof(ShouldBeAbleToUpdateFirewallRule)), TestPorts + 4);

            var result = sut.UpdateFirewallRule(TestRuleName(nameof(ShouldBeAbleToUpdateFirewallRule)), TestPorts + 5);

            result.Should().BeTrue();
            var getResult2 = sut.GetFirewallRule(TestRuleName(nameof(ShouldBeAbleToUpdateFirewallRule)));
            getResult2.Should().NotBeNull();
            getResult2.Rules.First().LocalPort.Should().Be(TestPorts + 5);
        }

        public void Dispose()
        {
            if (!PermissionsHelper.IsRunAsAdministrator())
                return;

            var sut = new NetShHelper();
            sut.DeleteFirewallRule(TestRuleName(nameof(ShouldBeAbleToCreateFirewallRule)));
            sut.DeleteFirewallRule(TestRuleName(nameof(ShouldBeAbleToDeleteFirewallRule)));
            sut.DeleteFirewallRule(TestRuleName(nameof(ShouldBeAbleToUpdateFirewallRule)));
            sut.DeleteFirewallRule(TestRuleName(nameof(ShouldBeAbleToDeleteFirewallRule)));
        }
    }
}
