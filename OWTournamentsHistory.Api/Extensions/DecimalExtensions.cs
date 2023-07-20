namespace OWTournamentsHistory.Api.Proto
{
    public sealed partial class DecimalValue
    {
        private const decimal _nanoFactor = 1_000_000_000;
        public DecimalValue(long units, int nanos)
        {
            Units = units;
            Nanos = nanos;
        }

        public static implicit operator decimal(DecimalValue grpcDecimal)
            => grpcDecimal.Units + grpcDecimal.Nanos / _nanoFactor;

        public static implicit operator DecimalValue(decimal value)
        {
            var units = decimal.ToInt64(value);
            var nanos = decimal.ToInt32((value - units) * _nanoFactor);
            return new DecimalValue(units, nanos);
        }
    }
}