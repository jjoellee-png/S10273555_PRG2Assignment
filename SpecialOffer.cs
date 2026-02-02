// Student Number: S10273555
// Student Name: Joelle Heng
// Partner Name: Tiara

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10273555_PRG2Assignment
{
    public class SpecialOffer
    {
        private string offerCode;
        public string OfferCode { get; private set; }

        private string offerDesc;
        public string OfferDesc { get; private set; }
        private double discount;
        public double Discount { get; private set; }

        public SpecialOffer(string offerCode, string offerDesc, double discount)
        {
            if (offerCode == null || offerCode == "")
                throw new ArgumentException("Offer code cannot be empty");

            if (offerDesc == null || offerDesc == "")
                throw new ArgumentException("Offer description cannot be empty");

            // allow 0 discount if needed, but not negative or > 100
            if (discount < 0 || discount > 100)
                throw new ArgumentException("Discount must be between 0 and 100");

            OfferCode = offerCode;
            OfferDesc = offerDesc;
            Discount = discount;
        }

        public override string ToString()
        {
            if (Discount == 0)
                return OfferCode + " - " + OfferDesc + " (No discount)";

            return OfferCode + " - " + OfferDesc + " (" + Discount.ToString("0.##") + "% off)";
        }
    }
}
