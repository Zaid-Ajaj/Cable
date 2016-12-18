/**
 * @version 1.0.0.0
 * @copyright Copyright ©  2016
 * @compiler Bridge.NET 15.6.0
 */
Bridge.assembly("Cable.Bridge", function ($asm, globals) {
    "use strict";

    Bridge.define("Cable.Bridge.Client", {
        statics: {
            interfacesCache: null,
            config: {
                init: function () {
                    this.interfacesCache = new (System.Collections.Generic.Dictionary$2(String,Object))();
                }
            },
            toCamelCase: function (input) {
                if (!System.String.isNullOrEmpty(input)) {
                    var output = "";
                    output = System.String.concat(output, (String.fromCharCode(String.fromCharCode(input.charCodeAt(0)).toLowerCase().charCodeAt(0))));

                    for (var i = 1; i < input.length; i = (i + 1) | 0) {
                        output = System.String.concat(output, String.fromCharCode(input.charCodeAt(i)));
                    }

                    return output;
                }

                return "";
            },
            postJsonSync: function (url, data) {
                var xmlHttp = new XMLHttpRequest();
                xmlHttp.open("POST", url, false);

                var serialized = Cable.Bridge.Json.serialize(data);

                xmlHttp.send(serialized);

                if (xmlHttp.status === 200 || xmlHttp.status === 304) {
                    var json = JSON.parse(xmlHttp.responseText);

                    if (json.$exception) {
                        throw new System.Exception(json.$exceptionData.Message);
                    } else {
                        var result = Cable.Bridge.Json.deserialize(Object, xmlHttp.responseText);
                        return result;
                    }
                } else {
                    throw new System.Exception(System.String.concat("Error: ", xmlHttp.statusText, "\n", xmlHttp.responseText));
                }
            },
            postJsonAsync: function (url, data) {
                var $step = 0,
                    $task1, 
                    $taskResult1, 
                    $jumpFromFinally, 
                    $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                    $returnValue, 
                    tcs, 
                    xmlHttp, 
                    serialized, 
                    $async_e, 
                    $asyncBody = Bridge.fn.bind(this, function () {
                        try {
                            for (;;) {
                                $step = System.Array.min([0,1], $step);
                                switch ($step) {
                                    case 0: {
                                        tcs = new System.Threading.Tasks.TaskCompletionSource();
                                            xmlHttp = new XMLHttpRequest();
                                            xmlHttp.open("POST", url, true);
                                            xmlHttp.setRequestHeader("Content-Type", "application/json");
                                            xmlHttp.onreadystatechange = function () {
                                                if (xmlHttp.readyState === 4) {
                                                    if (xmlHttp.status === 200 || xmlHttp.status === 304) {
                                                        var json = JSON.parse(xmlHttp.responseText);

                                                        if (json.$exception) {
                                                            tcs.setException(new System.Exception(json.$exceptionData.Message));
                                                            return;
                                                        } else {
                                                            var result = Cable.Bridge.Json.deserialize(Object, xmlHttp.responseText);
                                                            tcs.setResult(result);
                                                        }

                                                    } else {
                                                        tcs.setException(new System.Exception(xmlHttp.statusText));
                                                    }
                                                }

                                            };

                                            serialized = Cable.Bridge.Json.serialize(data);
                                            xmlHttp.send(serialized);
                                            $task1 = tcs.task;
                                            $step = 1;
                                            $task1.continueWith($asyncBody);
                                            return;
                                    }
                                    case 1: {
                                        $taskResult1 = $task1.getAwaitedResult();
                                        $tcs.setResult($taskResult1);
                                            return;
                                    }
                                    default: {
                                        $tcs.setResult(null);
                                        return;
                                    }
                                }
                            }
                        } catch($async_e1) {
                            $async_e = System.Exception.create($async_e1);
                            $tcs.setException($async_e);
                        }
                    }, arguments);

                $asyncBody();
                return $tcs.task;
            },
            resolve: function (T) {
                var $t;
                var serviceType = T;

                var serviceName = Bridge.Reflection.getTypeName(serviceType);
                var serviceFullName = System.String.replaceAll(Bridge.Reflection.getTypeFullName(serviceType), ".", "$");

                if (Cable.Bridge.Client.interfacesCache.containsKey(serviceFullName)) {
                    return Cable.Bridge.Client.interfacesCache.get(serviceFullName);
                }

                if (!Bridge.Reflection.isInterface(serviceType)) {
                    throw new System.ArgumentException(System.String.format("Type {0} must be an interface", Bridge.Reflection.getTypeName(serviceType)));
                }

                var methods = Bridge.Reflection.getMembers(serviceType, 8, 28);

                if (methods.length === 0) {
                    throw new System.ArgumentException("The interface does not have any methods, if there are any methods then annotate the interface with the [Reflectable] attribute");
                }

                var service = {};

                $t = Bridge.getEnumerator(methods);
                while ($t.moveNext()) {
                    (function () {
                        var method = $t.getCurrent();
                        var methodName = method.n;
                        if (Bridge.referenceEquals(method.rt, System.Threading.Tasks.Task)) {
                            service[System.String.format("{0}${1}", serviceFullName, methodName)] = function () {
                                    var $step = 0,
                                        $task1, 
                                        $taskResult1, 
                                        $jumpFromFinally, 
                                        $tcs = new System.Threading.Tasks.TaskCompletionSource(), 
                                        $returnValue, 
                                        parameters, 
                                        url, 
                                        result, 
                                        $async_e, 
                                        $asyncBody = Bridge.fn.bind(this, function () {
                                            try {
                                                for (;;) {
                                                    $step = System.Array.min([0,1], $step);
                                                    switch ($step) {
                                                        case 0: {
                                                            parameters = System.Linq.Enumerable.from(arguments).toArray();
                                                                url = System.String.format("/{0}/{1}", serviceName, methodName);
                                                                $task1 = Cable.Bridge.Client.postJsonAsync(url, parameters);
                                                                $step = 1;
                                                                $task1.continueWith($asyncBody);
                                                                return;
                                                        }
                                                        case 1: {
                                                            $taskResult1 = $task1.getAwaitedResult();
                                                            result = $taskResult1;
                                                                $tcs.setResult(result);
                                                                return;
                                                        }
                                                        default: {
                                                            $tcs.setResult(null);
                                                            return;
                                                        }
                                                    }
                                                }
                                            } catch($async_e1) {
                                                $async_e = System.Exception.create($async_e1);
                                                $tcs.setException($async_e);
                                            }
                                        }, arguments);

                                    $asyncBody();
                                    return $tcs.task;
                                };
                        } else {
                            service[System.String.format("{0}${1}", serviceFullName, methodName)] = function () {
                                    var parameters = System.Linq.Enumerable.from(arguments).toArray();
                                    var url = System.String.format("/{0}/{1}", serviceName, methodName);
                                    var result = Cable.Bridge.Client.postJsonSync(url, parameters);
                                    return result;
                                };
                        }
                    }).call(this);
                }

                Cable.Bridge.Client.interfacesCache.add(serviceFullName, service);

                return service;
            }
        }
    });

    Bridge.define("Cable.Bridge.Converters", {
        statics: {
            isNumeric: function (value) {
                return Bridge.is(value, System.SByte) || Bridge.is(value, System.Byte) || Bridge.is(value, System.Int16) || Bridge.is(value, System.UInt16) || Bridge.is(value, System.Int32) || Bridge.is(value, System.UInt32) || Bridge.is(value, System.Int64) || Bridge.is(value, System.UInt64) || Bridge.is(value, System.Single) || Bridge.is(value, System.Double) || Bridge.is(value, System.Decimal);
            },
            isPrimitive: function (value) {
                return Bridge.is(value, String) || Bridge.is(value, System.Char) || Bridge.is(value, Boolean) || Bridge.is(value, Date) || Bridge.is(value, System.TimeSpan) || Cable.Bridge.Converters.isNumeric(value);
            },
            isArray: function (obj) {
                var type = Bridge.getType(obj);

                return Bridge.referenceEquals(Bridge.Reflection.getTypeName(type), "Array") || System.String.endsWith(Bridge.Reflection.getTypeFullName(type), "[]") || Bridge.referenceEquals(Bridge.Reflection.getTypeName(type), "ArrayEnumerable") || Bridge.isDefined(obj.toArray);
            },
            extactExistingProps: function (input) {
                var keys = Object.keys(input);
                var props = [];

                for (var i = 0; i < keys.length; i = (i + 1) | 0) {
                    if (System.String.startsWith(keys[i], "$")) {
                        continue;
                    }

                    var property = Bridge.merge(new Cable.Bridge.Property(), {
                        setName: keys[i],
                        setType: Bridge.getType(input[keys[i]]),
                        setValue: input[keys[i]]
                    } );


                    props.push(property);
                }

                return props;
            },
            encodeDateTime: function (time) {
                var result = {};
                result.IsPrimitive = true;
                result.IsArray = false;
                result.Type = "DateTime";
                result.IsNumeric = false;

                var value = {};
                value.Year = time.getFullYear();
                value.Month = (time.getMonth() + 1);
                value.Day = time.getDate();
                value.Hour = time.getHours();
                value.Minute = time.getMinutes();
                value.Second = time.getSeconds();
                value.Millisecond = time.getMilliseconds();

                result.Value = value;
                return result;
            },
            encodeNumeric: function (value) {
                var result = {};
                result.IsPrimitive = true;
                result.IsArray = false;
                result.IsNumeric = true;
                result.Type = Bridge.Reflection.getTypeName(Bridge.getType(value));
                result.Value = value.toString();
                return result;
            },
            encodeChar: function (value) {
                var result = {};
                result.IsPrimitive = true;
                result.IsArray = false;
                result.IsNumeric = false;
                result.Type = "Char";
                result.Value = value;
                return result;
            },
            encodeString: function (value) {
                var result = {};
                result.IsPrimitive = true;
                result.IsArray = false;
                result.IsNumeric = false;
                result.Type = "String";
                result.Value = value;
                return result;
            },
            encodeBoolean: function (value) {
                var result = {};
                result.IsPrimitive = true;
                result.IsArray = false;
                result.IsNumeric = false;
                result.Type = "Boolean";
                result.Value = value;
                return result;
            },
            encodeTimeSpan: function (timeSpan) {
                var result = {};
                result.IsPrimitive = true;
                result.IsArray = false;
                result.IsNumeric = false;
                result.Type = "TimeSpan";
                result.Value = timeSpan.getTicks().toString();
                return result;
            },
            encodeObject: function (T, value) {
                var $t;
                var type = Bridge.getType(value);

                if (Cable.Bridge.Converters.isPrimitive(value)) {
                    if (Bridge.referenceEquals(type, Date)) {
                        return Cable.Bridge.Converters.encodeDateTime(value);
                    } else if (Bridge.referenceEquals(type, System.Char)) {
                        return Cable.Bridge.Converters.encodeChar(value);
                    } else if (Cable.Bridge.Converters.isNumeric(value)) {
                        return Cable.Bridge.Converters.encodeNumeric(value);
                    } else if (Bridge.referenceEquals(type, String)) {
                        return Cable.Bridge.Converters.encodeString(value);
                    } else if (Bridge.referenceEquals(type, Boolean)) {
                        return Cable.Bridge.Converters.encodeBoolean(value);
                    } else if (Bridge.referenceEquals(type, System.TimeSpan)) {
                        return Cable.Bridge.Converters.encodeTimeSpan(value);
                    }

                    return null;


                } else if (Cable.Bridge.Converters.isArray(value)) {
                    var result = {};
                    result.IsPrimitive = true;
                    result.IsArray = true;
                    result.IsNumeric = false;

                    if (Bridge.referenceEquals(Bridge.Reflection.getTypeName(type), "Array")) {
                        result.Type = "Array";
                        var values = [];
                        for (var i = 0; i < value.length; i = (i + 1) | 0) {
                            var subValue = Cable.Bridge.Converters.encodeObject(Object, value[i]);
                            values.push(subValue);
                        }

                        result.Value = values;
                        return result;
                    } else if (System.String.startsWith(Bridge.Reflection.getTypeFullName(type), "System.Collections.Generic.List")) {
                        result.Type = "List";
                        var list = value.toArray();
                        var valuesFromList = System.Array.init(list.length, null);

                        for (var i1 = 0; i1 < list.length; i1 = (i1 + 1) | 0) {
                            valuesFromList[i1] = Cable.Bridge.Converters.encodeObject(Object, list[i1]);
                        }

                        result.Value = valuesFromList;
                        return result;
                    } else if (Bridge.referenceEquals(Bridge.Reflection.getTypeName(type), "ArrayEnumerable")) {
                        result.Type = "Array";
                        var list1 = System.Linq.Enumerable.from(value).toArray();
                        var valuesFromList1 = System.Array.init(list1.length, null);

                        for (var i2 = 0; i2 < list1.length; i2 = (i2 + 1) | 0) {
                            valuesFromList1[i2] = Cable.Bridge.Converters.encodeObject(Object, list1[i2]);
                        }

                        result.Value = valuesFromList1;
                        return result;
                    } else {
                        try {
                            result.Type = "Array";
                            var list2 = System.Linq.Enumerable.from(value).toArray();
                            var valuesFromList2 = System.Array.init(list2.length, null);

                            for (var i3 = 0; i3 < list2.length; i3 = (i3 + 1) | 0) {
                                valuesFromList2[i3] = Cable.Bridge.Converters.encodeObject(Object, list2[i3]);
                            }

                            result.Value = valuesFromList2;
                            return result;
                        }
                        catch ($e1) {
                            $e1 = System.Exception.create($e1);
                            return null;
                        }
                    }
                } else {
                    // then it is an object here
                    var result1 = {  };
                    result1.IsPrimitive = false;
                    result1.IsArray = false;
                    result1.IsNumeric = false;
                    result1.Type = Bridge.Reflection.getTypeFullName(Bridge.getType(value));

                    var props = {  };

                    $t = Bridge.getEnumerator(Cable.Bridge.Converters.extactExistingProps(value));
                    while ($t.moveNext()) {
                        var prop = $t.getCurrent();

                        props[prop.getName()] = Cable.Bridge.Converters.encodeObject(prop.getType(), prop.getValue());
                    }

                    result1.Value = props;

                    return result1;
                }
            },
            decodeObject: function (T, json) {
                var type = json.Type;

                if (json.IsPrimitive && !json.IsArray) {
                    if (Bridge.referenceEquals(type, "DateTime")) {
                        var year = json.Value.Year;
                        var month = json.Value.Month;
                        var day = json.Value.Day;
                        var hour = json.Value.Hour;
                        var minute = json.Value.Minute;
                        var second = json.Value.Second;
                        var milliseconds = json.Value.Millisecond;
                        return new Date(year, month - 1, day, hour, minute, second, milliseconds);
                    }

                    if (Bridge.referenceEquals(type, "String")) {
                        return json.Value;
                    }

                    if (Bridge.referenceEquals(type, "TimeSpan")) {
                        return System.TimeSpan.fromTicks(System.Int64.parse(json.Value));
                    }

                    if (Bridge.referenceEquals(type, "UInt16")) {
                        return System.UInt16.parse(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "UInt32")) {
                        return System.UInt32.parse(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "UInt64")) {
                        return System.UInt64.parse(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "Int16")) {
                        return System.Int16.parse(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "Int32")) {
                        return System.Int32.parse(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "Int64")) {
                        return System.Int64.parse(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "Double")) {
                        return System.Double.parse(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "Decimal")) {
                        return System.Decimal(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "Byte")) {
                        return System.Byte.parse(json.Value);
                    }

                    if (Bridge.referenceEquals(type, "Boolean")) {
                        return json.Value;
                    }

                    return null;

                } else if (json.IsArray) {
                    if (Bridge.referenceEquals(json.Type, "Array")) {

                        var encodedItems = json.Value;
                        var arr = System.Array.init(encodedItems.length, null);

                        for (var i = 0; i < encodedItems.length; i = (i + 1) | 0) {
                            arr[i] = Cable.Bridge.Converters.decodeObject(Object, encodedItems[i]);
                        }

                        return arr;
                    }

                    if (Bridge.referenceEquals(json.Type, "List")) {
                        var encodedItems1 = json.Value;
                        var list = new (System.Collections.Generic.List$1(Object))();

                        for (var i1 = 0; i1 < encodedItems1.length; i1 = (i1 + 1) | 0) {
                            list.add(Cable.Bridge.Converters.decodeObject(Object, encodedItems1[i1]));
                        }

                        return list;
                    }

                    return null;
                } else {
                    var typeName = json.Type;
                    console.log(typeName);
                    var instance = Bridge.createInstance(Bridge.Reflection.getType(typeName));
                    var propNames = Cable.Bridge.Converters.objectKeys(json.Value);
                    console.log(propNames);
                    for (var i2 = 0; i2 < propNames.length; i2 = (i2 + 1) | 0) {
                        var propValue = json.Value[propNames[i2]];
                        var decoded = Cable.Bridge.Converters.decodeObject(Object, propValue);
                        var setPropName = System.String.concat("set", propNames[i2]);
                        console.log(setPropName);
                        console.log(instance);
                        instance[setPropName](decoded);
                    }

                    return instance;
                }
            },
            objectKeys: function (obj) {
                return Object.keys(obj);
            }
        }
    });

    Bridge.define("Cable.Bridge.Json", {
        statics: {
            serialize: function (obj) {
                var encoded = Cable.Bridge.Converters.encodeObject(Object, obj);
                var stringified = JSON.stringify(encoded);
                return stringified;
            },
            deserialize: function (T, json) {
                var parsed = JSON.parse(json);
                var decoded = Cable.Bridge.Converters.decodeObject(T, parsed);
                return decoded;
            }
        }
    });

    Bridge.define("Cable.Bridge.Property", {
        config: {
            properties: {
                Name: null,
                Type: null,
                Value: null
            }
        }
    });
});
