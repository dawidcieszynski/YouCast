using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using YouCast.Helpers;

namespace YouCast.Tests.Helpers
{
    public class NetShHelperUrlAclTests : IDisposable
    {
        public void Dispose()
        {
            if (!PermissionsHelper.IsRunAsAdministrator())
                return;

            var sut = new NetShHelper();

            foreach (var testUrl in TestUrls)
                sut.DeleteUrlAcl(testUrl);
        }

        private static readonly string[] TestUrls = { "http://+:10000/", "http://+:10001/", "http://+:10002/", "http://+:10003/" };

        [SkippableFact]
        public void ShouldBeAbleToCreateUrlAcl()
        {
            Skip.If(!PermissionsHelper.IsRunAsAdministrator(), "require administrator permissions");
            var sut = new NetShHelper();

            var result = sut.CreateUrlAcl(TestUrls[0]);

            result.Should().BeTrue();
        }

        [SkippableFact]
        public void ShouldBeAbleToGetUrlAcl()
        {
            Skip.If(!PermissionsHelper.IsRunAsAdministrator(), "require administrator permissions");
            var sut = new NetShHelper();
            PrepareTestUrlAcl(sut, TestUrls[1]);

            var result = sut.GetUrlAcl(TestUrls[1]);

            result.Should().NotBeNull();
            result.Reservations.First().Url.Should().Be(TestUrls[1]);
            result.Reservations.First().Data["User"].Should().Be($"{Environment.UserDomainName}\\{Environment.UserName}");
        }

        private void PrepareTestUrlAcl(NetShHelper sut, string testUrl)
        {
            var createResult = sut.CreateUrlAcl(testUrl);
            if (createResult == false)
            {
                sut.DeleteUrlAcl(testUrl).Should().BeTrue();
                createResult = sut.CreateUrlAcl(testUrl);
            }

            createResult.Should().BeTrue();
        }

        [SkippableFact]
        public void ShouldBeAbleToDeleteUrlAcl()
        {
            Skip.If(!PermissionsHelper.IsRunAsAdministrator(), "require administrator permissions");
            var sut = new NetShHelper();
            PrepareTestUrlAcl(sut, TestUrls[2]);

            var result = sut.DeleteUrlAcl(TestUrls[2]);

            result.Should().BeTrue();
            var getResult = sut.GetUrlAcl(TestUrls[2]);
            getResult.Should().NotBeNull();
            getResult.Reservations.Should().BeEmpty();
        }

        [SkippableFact]
        public void ShouldFailIfPermissionsRequired()
        {
            Skip.If(PermissionsHelper.IsRunAsAdministrator(), "require non-administrator permissions");

            var sut = new NetShHelper();

            var result = sut.CreateUrlAcl(TestUrls[3]);

            result.Should().BeFalse();
        }
    }
}