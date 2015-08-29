using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using M = Mass;
    using L = Length;
    using T = Commons.Time;

    /// <summary>
    /// 
    /// </summary>
    public class Pressure : DerivedDimension, IPressure
    {
        //TODO: these are here for what other purpose?
        private IMass Mass
        {
            get { return Dimensions.OfType<IMass>().SingleOrDefault(); }
        }

        private ILength Length
        {
            get { return Dimensions.OfType<ILength>().SingleOrDefault(); }
        }

        private ITime Time
        {
            get { return Dimensions.OfType<ITime>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Pascal_%28unit%29" />
        public static readonly IPressure Pascal = new Pressure("Pa", M.Kilogram, L.Meter, (ITime)T.Second.Invert().Squared());

        private Pressure(string abbreviation, IMass mass, ILength length, ITime time)
            : base(abbreviation, mass, length, time)
        {
            VerifyDimensions();
        }

        private Pressure(Pressure other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            var m = this.Mass;
            var l = this.Length;
            var t = this.Time;

            Debug.Assert(m != null && m.Exponent == 1);
            Debug.Assert(l != null && l.Exponent == 1);
            Debug.Assert(t != null && t.Exponent == -2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<Pressure, IPressure>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Pressure(this);
        }
    }
}
