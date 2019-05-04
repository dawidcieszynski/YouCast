using System.Linq;
using SharpNetSH;

namespace YouCast.Helpers
{
    public class UrlReservations : NetShResult
    {
        public UrlReservations(IResponse response) : base(response.Response)
        {
            if (!(response.ResponseObject is Tree responseResponseObject))
                return;

            Reservations = responseResponseObject.Children.Select(
                c =>
                {
                    var urlReservation = new UrlReservation($"{c.Value}");
                    foreach (var child in c.Children)
                    {
                        urlReservation.Data[child.Title] = $"{child.Value}";
                        foreach (var child2 in child.Children)
                        {
                            urlReservation.Data[child2.Title] = $"{child2.Value}";
                        }
                    }
                    return urlReservation;
                }).ToArray();
        }

        public UrlReservation[] Reservations { get; } = new UrlReservation[0];
    }
}
