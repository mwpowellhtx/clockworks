﻿namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    /// <summary>
    /// 
    /// </summary>
    public class Mass : BaseDimension, IMass
    {
        /// <summary>
        /// SlugPerKilogram: 14.5939029d
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Slug_%28mass%29" >Slug (mass)</a>
        private const double SlugPerKilogram = 14.5939029d;

        private const double PoundPerKilogram = 2.20462262185d;
        private const double OuncesPerKilogram = PoundPerKilogram*16d;
        private const double StonePerKilogram = PoundPerKilogram/14d;

        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=ounce+to+kilogram" >Ounce to kilogram conversion</a>
        internal const double KilogramsPerOunce = 1d/OuncesPerKilogram;

        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=pound+to+kilogram" >Pound to kilogram conversion</a>
        internal const double KilogramsPerPound = 1d/PoundPerKilogram;

        internal const double KilogramsPerStone = 1d/StonePerKilogram;

        /// <summary>
        /// KilogramsPerSlug: 1/14.5939029d
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Slug_%28mass%29" >Slug (mass)</a>
        /// <a href="!:https://www.google.com/?gws_rd=ssl#q=slug+to+kilogram" >Slug to kilogram conversion</a>
        internal const double KilogramPerSlug = 1d/SlugPerKilogram;
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly IMass Slug = new Mass("slug", KilogramPerSlug, SlugPerKilogram);

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMass Ounce = new Mass("oz", KilogramsPerOunce, OuncesPerKilogram);

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMass Pound = new Mass("lb", KilogramsPerPound, PoundPerKilogram);

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMass Stone = new Mass("st", KilogramsPerStone, StonePerKilogram);

        private Mass(string abbreviation, double toBaseFactor, double fromBaseFactor)
            : base(abbreviation,
                new BaseDimensionUnitConversion(toBaseFactor),
                new BaseDimensionUnitConversion(fromBaseFactor))
        {
        }

        private Mass(Mass other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Mass, IMass>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Mass(this);
        }
    }
}
