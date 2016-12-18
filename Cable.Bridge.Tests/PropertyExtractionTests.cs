using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.QUnit;


namespace Cable.Bridge.Tests
{

    public class PropertyExtractionTests
    {
        class PropDataExample
        {
            public DateTime Time { get; set; }
            public long Long { get; set; }
            public decimal Decimal { get; set; }
            public int Int32 { get; set; }
            public double Double { get; set; }
        }

        public static void Run()
        {
            QUnit.Module("Property");

            var time = new DateTime(2016, 12, 10, 20, 30, 0, 100);

            var instance = new PropDataExample
            {
                Time = time,
                Long = 10,
                Decimal = 10m,
                Double = 2.5,
                Int32 = 20
            };

            var propData = Converters.ExtactExistingProps(instance);

            var timeProp = propData.FirstOrDefault(prop => prop.Name == "Time");
            var longProp = propData.FirstOrDefault(prop => prop.Name == "Long");
            var decimalProp = propData.FirstOrDefault(prop => prop.Name == "Decimal");
            var doubleProp = propData.FirstOrDefault(prop => prop.Name == "Double");
            var intProp = propData.FirstOrDefault(prop => prop.Name == "Int32");


            QUnit.Test("Number of properties extracted is correct", assert =>
            {
                assert.Equal(propData.Length, 5);
            });

            QUnit.Test("Properties extracted are not null", assert =>
            {
                assert.Equal(timeProp == null, false);
                assert.Equal(longProp == null, false);
                assert.Equal(decimalProp == null, false);
                assert.Equal(doubleProp == null, false);
                assert.Equal(intProp == null, false);
            });

            QUnit.Test("Property types are extracted correctly", assert => 
            {
                assert.Equal(timeProp.Type, typeof(DateTime));
                assert.Equal(longProp.Type, typeof(long));
                assert.Equal(decimalProp.Type, typeof(decimal));
                assert.Equal(doubleProp.Type, typeof(double));
                assert.Equal(intProp.Type, typeof(int));
            });


            QUnit.Test("Property values are extracted correctly", assert =>
            {
                assert.Equal(timeProp.Value, time);
                assert.Equal((long)longProp.Value == (long)10, true);
                assert.Equal((decimal)decimalProp.Value == 10m, true);
                assert.Equal(doubleProp.Value, 2.5);
                assert.Equal(intProp.Value, 20);
            });
        }
    }
}
