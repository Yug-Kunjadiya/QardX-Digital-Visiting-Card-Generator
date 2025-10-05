using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace QardX.Services
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string data);
        string GenerateVCardData(string fullName, string email, string phone, string company, string linkedIn);
        string GenerateEnhancedVCardData(Models.VisitingCardViewModel card);
        string GeneratePublicCardUrl(int cardId, string baseUrl);
    }

    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQRCode(string data)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        public string GenerateVCardData(string fullName, string email, string phone, string company, string linkedIn)
        {
            var vCard = $@"BEGIN:VCARD
VERSION:3.0
FN:{fullName}
EMAIL:{email}
TEL:{phone}
ORG:{company}
URL:{linkedIn}
END:VCARD";
            return vCard;
        }

        public string GenerateEnhancedVCardData(Models.VisitingCardViewModel card)
        {
            var vCard = "BEGIN:VCARD\nVERSION:3.0\n";
            
            // Name fields
            if (!string.IsNullOrEmpty(card.FirstName) && !string.IsNullOrEmpty(card.LastName))
            {
                vCard += $"N:{card.LastName};{card.FirstName};;;\n";
                vCard += $"FN:{card.FirstName} {card.LastName}\n";
            }
            else if (!string.IsNullOrEmpty(card.FullName))
            {
                vCard += $"FN:{card.FullName}\n";
            }

            // Organization and title
            if (!string.IsNullOrEmpty(card.Company))
            {
                vCard += $"ORG:{card.Company}\n";
            }
            
            if (!string.IsNullOrEmpty(card.JobTitle))
            {
                vCard += $"TITLE:{card.JobTitle}\n";
            }

            // Contact information
            if (!string.IsNullOrEmpty(card.Email))
            {
                vCard += $"EMAIL;TYPE=WORK:{card.Email}\n";
            }
            
            if (!string.IsNullOrEmpty(card.Phone))
            {
                vCard += $"TEL;TYPE=WORK,VOICE:{card.Phone}\n";
            }

            // Address
            if (!string.IsNullOrEmpty(card.Address))
            {
                vCard += $"ADR;TYPE=WORK:;;{card.Address};;;\n";
            }

            // Social media and web URLs
            if (!string.IsNullOrEmpty(card.Website))
            {
                vCard += $"URL;TYPE=WORK:{card.Website}\n";
            }
            
            if (!string.IsNullOrEmpty(card.LinkedIn))
            {
                vCard += $"URL;TYPE=LinkedIn:{card.LinkedIn}\n";
            }
            
            if (!string.IsNullOrEmpty(card.Instagram))
            {
                vCard += $"URL;TYPE=Instagram:https://instagram.com/{card.Instagram.Replace("@", "")}\n";
            }
            
            if (!string.IsNullOrEmpty(card.Twitter))
            {
                vCard += $"URL;TYPE=Twitter:https://twitter.com/{card.Twitter.Replace("@", "")}\n";
            }
            
            if (!string.IsNullOrEmpty(card.Facebook))
            {
                vCard += $"URL;TYPE=Facebook:{card.Facebook}\n";
            }

            // Professional information as notes
            var notes = new List<string>();
            
            if (!string.IsNullOrEmpty(card.Skills))
            {
                notes.Add($"Skills: {card.Skills}");
            }
            
            if (!string.IsNullOrEmpty(card.Languages))
            {
                notes.Add($"Languages: {card.Languages}");
            }
            
            if (!string.IsNullOrEmpty(card.AvailabilityStatus))
            {
                notes.Add($"Availability: {card.AvailabilityStatus}");
            }

            if (notes.Any())
            {
                vCard += $"NOTE:{string.Join(" | ", notes)}\n";
            }

            vCard += "END:VCARD";
            return vCard;
        }

        public string GeneratePublicCardUrl(int cardId, string baseUrl)
        {
            return $"{baseUrl}/card/{cardId}";
        }
    }
}