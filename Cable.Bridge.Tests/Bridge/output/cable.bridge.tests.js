/**
 * @version 1.0.0.0
 * @copyright Copyright ©  2016
 * @compiler Bridge.NET 15.6.0
 */
Bridge.assembly("Cable.Bridge.Tests", function ($asm, globals) {
    "use strict";

    Bridge.define("Cable.Bridge.Tests.ArrayTests", {
        statics: {
            arrayIsEncodedCorrectly: function () {
                var arr = [1, 2, 3];
                var encoded = Cable.Bridge.Converters.encodeObject(Array, arr);

                QUnit.test("Array of ints is encoded correctly", function (assert) {
                    assert.equal(encoded.IsPrimitive, true);
                    assert.equal(encoded.IsArray, true);
                    assert.equal(encoded.IsNumeric, false);
                    assert.equal(encoded.Type, "Array");
                    assert.equal(encoded.Value.length, arr.length);

                    var values = encoded.Value;

                    for (var i = 0; i < values.length; i = (i + 1) | 0) {
                        var intJson = values[i];
                        assert.equal(intJson.IsPrimitive, true);
                        assert.equal(intJson.IsNumeric, true);
                        assert.equal(intJson.IsArray, false);
                        assert.equal(intJson.Type, "Int32");
                        assert.equal(intJson.Value, ((i + 1) | 0));
                    }

                });


            },
            run: function () {
                QUnit.module("Array Tests");
                Cable.Bridge.Tests.ArrayTests.arrayIsEncodedCorrectly();

                QUnit.test("Serialization and deserialization of int array works", $asm.$.Cable.Bridge.Tests.ArrayTests.f1);


                QUnit.test("Serialization and deserialization of string array works", $asm.$.Cable.Bridge.Tests.ArrayTests.f2);

                QUnit.test("Serialization and deserialization of long array works", $asm.$.Cable.Bridge.Tests.ArrayTests.f3);

                QUnit.test("Serialization and deserialization of DateTime array works", $asm.$.Cable.Bridge.Tests.ArrayTests.f4);


            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.ArrayTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.ArrayTests, {
        f1: function (assert) {
            var ints = [1, 2, 3];
            var serialized = Cable.Bridge.Json.serialize(ints);
            var deserialized = Cable.Bridge.Json.deserialize(Array, serialized);
            assert.deepEqual(deserialized, ints);
        },
        f2: function (assert) {
            var strings = ["one", "two"];
            var serialized = Cable.Bridge.Json.serialize(strings);
            var deserialized = Cable.Bridge.Json.deserialize(Array, serialized);
            assert.deepEqual(deserialized, strings);
        },
        f3: function (assert) {
            var longs = [System.Int64(10), System.Int64(20), System.Int64(30)];
            var serialized = Cable.Bridge.Json.serialize(longs);
            var deserialized = Cable.Bridge.Json.deserialize(Array, serialized);

            assert.equal(longs[0].equals(deserialized[0]), true);
            assert.equal(longs[1].equals(deserialized[1]), true);
            assert.equal(longs[2].equals(deserialized[2]), true);

        },
        f4: function (assert) {
            var dates = [new Date(), Bridge.Date.utcNow(), new Date(2016, 12 - 1, 10, 20, 30, 0, 15)];
            var serialized = Cable.Bridge.Json.serialize(dates);
            var deserialized = Cable.Bridge.Json.deserialize(Array, serialized);
            assert.deepEqual(deserialized, dates);
        }
    });

    Bridge.define("Cable.Bridge.Tests.Base", {
        config: {
            properties: {
                Id: 0,
                Long: System.Int64(0)
            }
        }
    });

    Bridge.define("Cable.Bridge.Tests.BaseGeneric$1", function (T) { return {
        config: {
            properties: {
                Value: Bridge.getDefaultValue(T)
            }
        }
    }; });

    Bridge.define("Cable.Bridge.Tests.DateTimeTests", {
        statics: {
            dateTimeIsEncodedCorrectly: function () {
                var time = new Date(2016, 12 - 1, 10, 20, 30, 0, 235);

                var encoded = Cable.Bridge.Converters.encodeDateTime(time);


                QUnit.test("Encoding DateTime works", function (assert) {
                    assert.equal(encoded.IsPrimitive, true);
                    assert.equal(encoded.IsArray, false);
                    assert.equal(encoded.IsNumeric, false);
                    assert.equal(encoded.Type, "DateTime");
                    assert.equal(encoded.Value.Year, 2016);
                    assert.equal(encoded.Value.Month, 12);
                    assert.equal(encoded.Value.Day, 10);
                    assert.equal(encoded.Value.Hour, 20);
                    assert.equal(encoded.Value.Minute, 30);
                    assert.equal(encoded.Value.Second, 0);
                    assert.equal(encoded.Value.Millisecond, 235);
                });

                QUnit.test("Encoding boxed DateTime works", function (assert) {
                    var boxed = time;

                    var boxedEncoded = Cable.Bridge.Converters.encodeDateTime(boxed);

                    assert.deepEqual(encoded, boxedEncoded);
                });

                QUnit.test("Encode -> Decode DateTime works", $asm.$.Cable.Bridge.Tests.DateTimeTests.f1);

            },
            run: function () {
                QUnit.module("DateTime Tests");
                Cable.Bridge.Tests.DateTimeTests.dateTimeIsEncodedCorrectly();
            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.DateTimeTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.DateTimeTests, {
        f1: function (assert) {
            var now = new Date();

            var serialized = Cable.Bridge.Json.serialize(now);

            var deserialized = Cable.Bridge.Json.deserialize(Date, serialized);

            assert.equal(now.getFullYear(), deserialized.getFullYear());
            assert.equal((now.getMonth() + 1), (deserialized.getMonth() + 1));
            assert.equal(now.getDate(), deserialized.getDate());
            assert.equal(now.getHours(), deserialized.getHours());
            assert.equal(now.getMinutes(), deserialized.getMinutes());
            assert.equal(now.getSeconds(), deserialized.getSeconds());
            assert.equal(now.getMilliseconds(), deserialized.getMilliseconds());

        }
    });

    Bridge.define("Cable.Bridge.Tests.EnumerableTests", {
        statics: {
            getNumber: function (x) {
                var $yield = [];
                $yield.push(x);
                return System.Array.toEnumerable($yield);
            },
            enumerableIsConvertedCorrectly: function () {
                var sample = Cable.Bridge.Tests.EnumerableTests.getNumber(10);

                var encoded = Cable.Bridge.Converters.encodeObject(System.Collections.Generic.IEnumerable$1(System.Int32), sample);

                QUnit.test("IEnumerable with yield return is converted corretly to array", function (assert) {
                    assert.equal(encoded == null, false);
                    assert.equal(encoded.IsPrimitive, true);
                    assert.equal(encoded.IsArray, true);
                    assert.equal(encoded.IsNumeric, false);
                    assert.equal(encoded.Type, "Array");
                    assert.equal(encoded.Value.length, 1);

                    var number = encoded.Value[0];
                    assert.equal(number.IsPrimitive, true);
                    assert.equal(number.IsNumeric, true);
                    assert.equal(number.IsArray, false);
                    assert.equal(number.Type, "Int32");
                    assert.equal(number.Value, 10);
                });

                QUnit.test("IEnumerable from range is converted correctly", $asm.$.Cable.Bridge.Tests.EnumerableTests.f1);

                QUnit.test("IEnumerable of int is serialized and deserialized correctly", $asm.$.Cable.Bridge.Tests.EnumerableTests.f2);

            },
            run: function () {
                QUnit.module("Enumerable tests");
                Cable.Bridge.Tests.EnumerableTests.enumerableIsConvertedCorrectly();
            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.EnumerableTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.EnumerableTests, {
        f1: function (assert) {
            var range = System.Linq.Enumerable.range(1, 1);
            var converted = Cable.Bridge.Converters.encodeObject(System.Linq.EnumerableInstance$1, range);
            assert.equal(converted == null, false);
            assert.equal(converted.IsPrimitive, true);
            assert.equal(converted.IsArray, true);
            assert.equal(converted.IsNumeric, false);
            assert.equal(converted.Type, "Array");
            assert.equal(converted.Value.length, 1);

            var number = converted.Value[0];
            assert.equal(number.IsPrimitive, true);
            assert.equal(number.IsNumeric, true);
            assert.equal(number.IsArray, false);
            assert.equal(number.Type, "Int32");
            assert.equal(number.Value, 1);
        },
        f2: function (assert) {
            var enu = System.Linq.Enumerable.range(1, 10);
            var serialized = Cable.Bridge.Json.serialize(enu);
            var deserialized = Cable.Bridge.Json.deserialize(System.Collections.Generic.IEnumerable$1(System.Int32), serialized);
            assert.equal(System.Linq.Enumerable.from(deserialized).count(), System.Linq.Enumerable.from(enu).count());

            var enuArr = System.Linq.Enumerable.from(enu).toArray();
            var deserializedArr = System.Linq.Enumerable.from(deserialized).toArray();

            for (var i = 0; i < enuArr.length; i = (i + 1) | 0) {
                assert.equal(enuArr[i], deserializedArr[i]);
            }
        }
    });

    Bridge.define("Cable.Bridge.Tests.InheritanceTests", {
        statics: {
            run: function () {
                QUnit.module("Inheritance Tests");

                QUnit.test("Inherited class from abstract class is converted correctly", $asm.$.Cable.Bridge.Tests.InheritanceTests.f1);

                QUnit.test("Inherited class from generic abstract class is converted correctly", $asm.$.Cable.Bridge.Tests.InheritanceTests.f2);

                QUnit.test("Inherited generic class from generic abstract class is converted correctly", $asm.$.Cable.Bridge.Tests.InheritanceTests.f3);
            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.InheritanceTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.InheritanceTests, {
        f1: function (assert) {
            var sample = Bridge.merge(new Cable.Bridge.Tests.Derived(), {
                setId: 5,
                setLong: System.Int64(10),
                setDecimal: System.Decimal(10.0),
                setDateTime: new Date()
            } );

            var serialized = Cable.Bridge.Json.serialize(sample);

            var deserialized = Cable.Bridge.Json.deserialize(Cable.Bridge.Tests.Derived, serialized);

            assert.equal(deserialized.getId(), sample.getId());
            assert.equal(Bridge.equals(deserialized.getDateTime(), sample.getDateTime()), true);
            assert.equal(deserialized.getDecimal().equalsT(sample.getDecimal()), true);
            assert.equal(deserialized.getLong().equals(sample.getLong()), true);

        },
        f2: function (assert) {
            var sample = Bridge.merge(new Cable.Bridge.Tests.DerivedFromGeneric(), {
                setValue: 20
            } );

            var serialized = Cable.Bridge.Json.serialize(sample);

            var deserialized = Cable.Bridge.Json.deserialize(Cable.Bridge.Tests.DerivedFromGeneric, serialized);

            assert.equal(deserialized.getValue(), 20);
        },
        f3: function (assert) {
            var sample = Bridge.merge(new (Cable.Bridge.Tests.GenericDerived$2(System.Int32,String))(), {
                setValue: "MyValue",
                setOtherValue: 10
            } );

            var serialized = Cable.Bridge.Json.serialize(sample);

            var deserialized = Cable.Bridge.Json.deserialize(Cable.Bridge.Tests.GenericDerived$2(System.Int32,String), serialized);

            assert.equal(deserialized.getValue(), sample.getValue());

            assert.equal(deserialized.getOtherValue(), sample.getOtherValue());

        }
    });

    Bridge.define("Cable.Bridge.Tests.ListTests", {
        statics: {
            run: function () {
                QUnit.module("List Tests");


                QUnit.test("List is encoded correctly", $asm.$.Cable.Bridge.Tests.ListTests.f1);


                QUnit.test("Serilization and deserialization of List<int> works", $asm.$.Cable.Bridge.Tests.ListTests.f2);



            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.ListTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.ListTests, {
        f1: function (assert) {
            var list = new (System.Collections.Generic.List$1(System.Int32))();
            list.add(1);
            list.add(2);
            list.add(3);

            var encoded = Cable.Bridge.Converters.encodeObject(System.Collections.Generic.List$1(System.Int32), list);

            assert.equal(encoded.IsPrimitive, true);
            assert.equal(encoded.IsArray, true);
            assert.equal(encoded.IsNumeric, false);
            assert.equal(encoded.Type, "List");
            assert.equal(encoded.Value.length, list.getCount());
        },
        f2: function (assert) {
            var sample = new (System.Collections.Generic.List$1(System.Int32))();
            sample.add(10);
            sample.add(20);

            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.Collections.Generic.List$1(System.Int32), serialized);

            assert.equal(deserialized == null, false);
            assert.equal(sample.getCount(), deserialized.getCount());

            for (var i = 0; i < sample.getCount(); i = (i + 1) | 0) {
                assert.equal(sample.getItem(i), deserialized.getItem(i));
            }


            deserialized.add(30);
            assert.equal(System.Linq.Enumerable.from(deserialized).sum(), 60);

        }
    });

    Bridge.define("Cable.Bridge.Tests.Nested$1", function (T) { return {
        config: {
            properties: {
                InnerInner: Bridge.getDefaultValue(T)
            }
        }
    }; });

    Bridge.define("Cable.Bridge.Tests.NestedObjectTests", {
        statics: {
            run: function () {
                QUnit.module("Nested object tests");

                var sample = Bridge.merge(new Cable.Bridge.Tests.NestedObjectTests.WrappedInt(), {
                    setId: 5,
                    setName: "Zaid",
                    setHasValue: true,
                    setLarge: System.Int64([91579395,55523763])
                } );

                QUnit.test("Nested object is encoded correctly", function (assert) {
                    var encoded = Cable.Bridge.Converters.encodeObject(Cable.Bridge.Tests.NestedObjectTests.WrappedInt, sample);
                    assert.equal(encoded.IsPrimitive, false);
                    assert.equal(encoded.IsArray, false);
                    assert.equal(encoded.IsNumeric, false);
                    assert.equal(encoded.Type, "Cable.Bridge.Tests.NestedObjectTests.WrappedInt");
                    assert.equal(encoded.Value.Id == null, false);
                    assert.equal(encoded.Value.Id.IsPrimitive, true);
                    assert.equal(encoded.Value.Id.IsArray, false);
                    assert.equal(encoded.Value.Id.IsNumeric, true);
                    assert.equal(encoded.Value.Id.Type, "Int32");
                    assert.equal(encoded.Value.Id.Value, "5");
                    assert.equal(encoded.Value.Name == null, false);
                    assert.equal(encoded.Value.Name.IsPrimitive, true);
                    assert.equal(encoded.Value.Name.IsArray, false);
                    assert.equal(encoded.Value.Name.IsNumeric, false);
                    assert.equal(encoded.Value.Name.Type, "String");
                    assert.equal(encoded.Value.Name.Value, "Zaid");
                    assert.equal(encoded.Value.HasValue == null, false);
                    assert.equal(encoded.Value.HasValue.IsPrimitive, true);
                    assert.equal(encoded.Value.HasValue.IsArray, false);
                    assert.equal(encoded.Value.HasValue.IsNumeric, false);
                    assert.equal(encoded.Value.HasValue.Type, "Boolean");
                    assert.equal(encoded.Value.HasValue.Value, true);
                    assert.equal(encoded.Value.Large == null, false);
                    assert.equal(encoded.Value.Large.IsPrimitive, true);
                    assert.equal(encoded.Value.Large.IsArray, false);
                    assert.equal(encoded.Value.Large.IsNumeric, true);
                    assert.equal(encoded.Value.Large.Type, "Int64");
                    assert.equal(encoded.Value.Large.Value, "238472746327434243");
                });

                QUnit.test("Serialization and deserialization of nested object works", function (assert) {
                    var serialized = Cable.Bridge.Json.serialize(sample);

                    var deserialized = Cable.Bridge.Json.deserialize(Cable.Bridge.Tests.NestedObjectTests.WrappedInt, serialized);

                    assert.equal(sample.getId(), deserialized.getId());
                    assert.equal(sample.getName(), deserialized.getName());
                    assert.equal(sample.getHasValue(), deserialized.getHasValue());
                    assert.equal(sample.getLarge().equals(deserialized.getLarge()), true);
                });

                var generic = Bridge.merge(new (Cable.Bridge.Tests.WeirdGenericClass$1(Cable.Bridge.Tests.WeirdGenericClass$1(System.Int32)))(), {
                    setInner: Bridge.merge(new (Cable.Bridge.Tests.WeirdGenericClass$1(System.Int32))(), {
                        setInner: 20,
                        setNested: Bridge.merge(new (Cable.Bridge.Tests.Nested$1(System.Int32))(), {
                            setInnerInner: 5
                        } )
                    } ),
                    setNested: Bridge.merge(new (Cable.Bridge.Tests.Nested$1(Cable.Bridge.Tests.WeirdGenericClass$1(System.Int32)))(), {
                        setInnerInner: Bridge.merge(new (Cable.Bridge.Tests.WeirdGenericClass$1(System.Int32))(), {
                            setInner: 30,
                            setNested: Bridge.merge(new (Cable.Bridge.Tests.Nested$1(System.Int32))(), {
                                setInnerInner: 40
                            } )
                        } )
                    } )
                } );



                QUnit.test("Weird generic class in converted correctly", function (assert) {


                    var serialized = Cable.Bridge.Json.serialize(generic);

                    var deserialized = Cable.Bridge.Json.deserialize(Cable.Bridge.Tests.WeirdGenericClass$1(Cable.Bridge.Tests.WeirdGenericClass$1(System.Int32)), serialized);


                    assert.equal(generic.getInner().getInner(), deserialized.getInner().getInner());
                    assert.equal(generic.getInner().getNested().getInnerInner(), deserialized.getInner().getNested().getInnerInner());
                    assert.equal(generic.getNested().getInnerInner().getInner(), deserialized.getNested().getInnerInner().getInner());
                    assert.equal(generic.getNested().getInnerInner().getNested().getInnerInner(), deserialized.getNested().getInnerInner().getNested().getInnerInner());

                });
            }
        }
    });

    Bridge.define("Cable.Bridge.Tests.NestedObjectTests.WrappedInt", {
        config: {
            properties: {
                Id: 0,
                Large: System.Int64(0),
                Name: null,
                HasValue: false
            }
        }
    });

    Bridge.define("Cable.Bridge.Tests.NumericTypesTests", {
        statics: {
            run: function () {
                QUnit.module("Numeric types Tests");

                QUnit.test("UInt16 is converted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f1);

                QUnit.test("UInt32 is converted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f2);

                QUnit.test("UInt64 is converted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f3);

                QUnit.test("Int16 is converted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f4);

                QUnit.test("Int32 is coverted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f5);

                QUnit.test("Int64 is converted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f6);

                QUnit.test("Double is converted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f7);

                QUnit.test("Decimal is converted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f8);

                QUnit.test("Byte is converted correctly", $asm.$.Cable.Bridge.Tests.NumericTypesTests.f9);
            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.NumericTypesTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.NumericTypesTests, {
        f1: function (assert) {
            var sample = 10;
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.UInt16, serialized);
            assert.equal(sample === deserialized, true);
        },
        f2: function (assert) {
            var sample = 10;
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.UInt32, serialized);
            assert.equal(sample === deserialized, true);
        },
        f3: function (assert) {
            var sample = System.UInt64(10);
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.UInt64, serialized);
            assert.equal(sample.equals(deserialized), true);
        },
        f4: function (assert) {
            var sample = 2;
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.Int16, serialized);
            assert.equal(sample, deserialized);
        },
        f5: function (assert) {
            var sample = 7;
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.Int32, serialized);
            assert.equal(sample, deserialized);
        },
        f6: function (assert) {
            var sample = System.Int64(10);
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.Int64, serialized);
            assert.equal(sample.equals(deserialized), true);
        },
        f7: function (assert) {
            var sample = 2.0;
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.Double, serialized);
            assert.equal(sample, deserialized);
        },
        f8: function (assert) {
            var sample = System.Decimal(2.0);
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.Decimal, serialized);
            assert.equal(sample.equalsT(deserialized), true);
        },
        f9: function (assert) {
            var sample = 200;
            var serialized = Cable.Bridge.Json.serialize(sample);
            var deserialized = Cable.Bridge.Json.deserialize(System.Byte, serialized);
            assert.equal(sample, deserialized);
        }
    });

    Bridge.define("Cable.Bridge.Tests.Program", {
        $main: function () {
            Cable.Bridge.Tests.PropertyExtractionTests.run();
            Cable.Bridge.Tests.DateTimeTests.run();
            Cable.Bridge.Tests.ArrayTests.run();
            Cable.Bridge.Tests.ListTests.run();
            Cable.Bridge.Tests.EnumerableTests.run();
            Cable.Bridge.Tests.NestedObjectTests.run();
            Cable.Bridge.Tests.StringTests.run();
            Cable.Bridge.Tests.NumericTypesTests.run();
            Cable.Bridge.Tests.InheritanceTests.run();
            Cable.Bridge.Tests.ServiceRegistrationTests.run();
            Cable.Bridge.Tests.TimeSpanTests.run();
        }
    });

    Bridge.define("Cable.Bridge.Tests.PropertyExtractionTests", {
        statics: {
            run: function () {
                QUnit.module("Property");

                var time = new Date(2016, 12 - 1, 10, 20, 30, 0, 100);

                var instance = Bridge.merge(new Cable.Bridge.Tests.PropertyExtractionTests.PropDataExample(), {
                    setTime: time,
                    setLong: System.Int64(10),
                    setDecimal: System.Decimal(10.0),
                    setDouble: 2.5,
                    setInt32: 20
                } );

                var propData = Cable.Bridge.Converters.extactExistingProps(instance);

                var timeProp = System.Linq.Enumerable.from(propData).firstOrDefault($asm.$.Cable.Bridge.Tests.PropertyExtractionTests.f1, null);
                var longProp = System.Linq.Enumerable.from(propData).firstOrDefault($asm.$.Cable.Bridge.Tests.PropertyExtractionTests.f2, null);
                var decimalProp = System.Linq.Enumerable.from(propData).firstOrDefault($asm.$.Cable.Bridge.Tests.PropertyExtractionTests.f3, null);
                var doubleProp = System.Linq.Enumerable.from(propData).firstOrDefault($asm.$.Cable.Bridge.Tests.PropertyExtractionTests.f4, null);
                var intProp = System.Linq.Enumerable.from(propData).firstOrDefault($asm.$.Cable.Bridge.Tests.PropertyExtractionTests.f5, null);


                QUnit.test("Number of properties extracted is correct", function (assert) {
                    assert.equal(propData.length, 5);
                });

                QUnit.test("Properties extracted are not null", function (assert) {
                    assert.equal(timeProp == null, false);
                    assert.equal(longProp == null, false);
                    assert.equal(decimalProp == null, false);
                    assert.equal(doubleProp == null, false);
                    assert.equal(intProp == null, false);
                });

                QUnit.test("Property types are extracted correctly", function (assert) {
                    assert.equal(timeProp.getType(), Date);
                    assert.equal(longProp.getType(), System.Int64);
                    assert.equal(decimalProp.getType(), System.Decimal);
                    assert.equal(doubleProp.getType(), System.Double);
                    assert.equal(intProp.getType(), System.Int32);
                });


                QUnit.test("Property values are extracted correctly", function (assert) {
                    assert.equal(timeProp.getValue(), time);
                    assert.equal(System.Nullable.getValue(Bridge.cast(longProp.getValue(), System.Int64)).equals(System.Int64(10)), true);
                    assert.equal(System.Nullable.getValue(Bridge.cast(decimalProp.getValue(), System.Decimal)).equalsT(System.Decimal(10.0)), true);
                    assert.equal(doubleProp.getValue(), 2.5);
                    assert.equal(intProp.getValue(), 20);
                });
            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.PropertyExtractionTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.PropertyExtractionTests, {
        f1: function (prop) {
            return Bridge.referenceEquals(prop.getName(), "Time");
        },
        f2: function (prop) {
            return Bridge.referenceEquals(prop.getName(), "Long");
        },
        f3: function (prop) {
            return Bridge.referenceEquals(prop.getName(), "Decimal");
        },
        f4: function (prop) {
            return Bridge.referenceEquals(prop.getName(), "Double");
        },
        f5: function (prop) {
            return Bridge.referenceEquals(prop.getName(), "Int32");
        }
    });

    Bridge.define("Cable.Bridge.Tests.PropertyExtractionTests.PropDataExample", {
        config: {
            properties: {
                Time: null,
                Long: System.Int64(0),
                Decimal: System.Decimal(0.0),
                Int32: 0,
                Double: 0
            },
            init: function () {
                this.Time = new Date(-864e13);
            }
        }
    });

    Bridge.define("Cable.Bridge.Tests.ServiceRegistrationTests", {
        statics: {
            run: function () {
                QUnit.module("Service Registration Tests");

                QUnit.test("Cable.Resolve throws exception if interface not reflectable", function (assert) {
                    try {
                        var service = Cable.Bridge.Client.resolve(Cable.Bridge.Tests.ServiceRegistrationTests.INonReflectable);
                    }
                    catch ($e1) {
                        $e1 = System.Exception.create($e1);
                        var , ex;
                        if (Bridge.is($e1, System.ArgumentException)) {
                            ex = $e1;
                            assert.equal(ex.getMessage(), "The interface does not have any methods, if there are any methods then annotate the interface with the [Reflectable] attribute");
                        } else {
                            throw $e1;
                        }
                    }
                });

                QUnit.test("Cable.Resolve throws an exception if type is not an interface", function (assert) {
                    try {
                        var service = Cable.Bridge.Client.resolve(System.Int32);
                    }
                    catch ($e1) {
                        $e1 = System.Exception.create($e1);
                        var , ex;
                        if (Bridge.is($e1, System.ArgumentException)) {
                            ex = $e1;
                            var msg = ex.getMessage();
                            assert.equal(msg, "Type Int32 must be an interface");
                        } else {
                            throw $e1;
                        }
                    }
                });

                QUnit.test("Cable.Resolve throws exception if reflectable interface does not have methods", function (assert) {
                    try {
                        var service = Cable.Bridge.Client.resolve(Cable.Bridge.Tests.ServiceRegistrationTests.IReflectable);
                    }
                    catch ($e1) {
                        $e1 = System.Exception.create($e1);
                        var , ex;
                        if (Bridge.is($e1, System.ArgumentException)) {
                            ex = $e1;
                            var msg = ex.getMessage();
                            assert.equal(msg, "The interface does not have any methods, if there are any methods then annotate the interface with the [Reflectable] attribute");
                        } else {
                            throw $e1;
                        }
                    }
                });

                QUnit.test("Cable.Resolve doesn't throw exception on reflectable interface where all methods have return type of Task", $asm.$.Cable.Bridge.Tests.ServiceRegistrationTests.f1);
            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.ServiceRegistrationTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.ServiceRegistrationTests, {
        f1: function (assert) {
            var service = Cable.Bridge.Client.resolve(Cable.Bridge.Tests.ServiceRegistrationTests.IService);
            assert.equal(true, Bridge.isDefined(service));
        }
    });

    Bridge.define("Cable.Bridge.Tests.ServiceRegistrationTests.INonReflectable", {
        $kind: "interface"
    });

    Bridge.define("Cable.Bridge.Tests.ServiceRegistrationTests.IReflectable", {
        $kind: "interface"
    });

    Bridge.define("Cable.Bridge.Tests.ServiceRegistrationTests.IService", {
        $kind: "interface"
    });

    Bridge.define("Cable.Bridge.Tests.StringTests", {
        statics: {
            run: function () {
                QUnit.module("String tests");

                var sample = "my string";

                QUnit.test("Strings is serialized and deserialized correctly", function (assert) {
                    var serialized = Cable.Bridge.Json.serialize(sample);

                    var deserialized = Cable.Bridge.Json.deserialize(String, serialized);

                    assert.equal(deserialized, sample);
                });
            }
        }
    });

    Bridge.define("Cable.Bridge.Tests.TimeSpanTests", {
        statics: {
            run: function () {
                QUnit.module("TimeSpan Tests");

                QUnit.test("TimeSpan is converted correctly", $asm.$.Cable.Bridge.Tests.TimeSpanTests.f1);
            }
        }
    });

    Bridge.ns("Cable.Bridge.Tests.TimeSpanTests", $asm.$);

    Bridge.apply($asm.$.Cable.Bridge.Tests.TimeSpanTests, {
        f1: function (assert) {
            var timeSpan = System.TimeSpan.fromHours(5);

            var serialized = Cable.Bridge.Json.serialize(timeSpan);

            var deserialized = Cable.Bridge.Json.deserialize(System.TimeSpan, serialized);

            assert.equal(true, timeSpan.getTicks().equals(deserialized.getTicks()));

        }
    });

    Bridge.define("Cable.Bridge.Tests.WeirdGenericClass$1", function (T) { return {
        config: {
            properties: {
                Nested: null,
                Inner: Bridge.getDefaultValue(T)
            }
        }
    }; });

    Bridge.define("Cable.Bridge.Tests.Derived", {
        inherits: [Cable.Bridge.Tests.Base],
        config: {
            properties: {
                DateTime: null,
                Decimal: System.Decimal(0.0)
            },
            init: function () {
                this.DateTime = new Date(-864e13);
            }
        }
    });

    Bridge.define("Cable.Bridge.Tests.DerivedFromGeneric", {
        inherits: [Cable.Bridge.Tests.BaseGeneric$1(System.Int32)]
    });

    Bridge.define("Cable.Bridge.Tests.GenericDerived$2", function (T, U) { return {
        inherits: [Cable.Bridge.Tests.BaseGeneric$1(U)],
        config: {
            properties: {
                OtherValue: Bridge.getDefaultValue(T)
            }
        }
    }; });

    var $m = Bridge.setMetadata,
        $n = [System,System.Threading.Tasks,Cable.Bridge.Tests];
    $m($n[2].ServiceRegistrationTests.IService, function () { return {"m":[{"ab":true,"a":2,"n":"Add","t":8,"pi":[{"n":"x","pt":$n[0].Int32,"ps":0},{"n":"y","pt":$n[0].Int32,"ps":1}],"sn":"Cable$Bridge$Tests$ServiceRegistrationTests$IService$add","rt":$n[1].Task,"p":[$n[0].Int32,$n[0].Int32]}]}; });
});
