/**
 * @version 1.0.0.0
 * @copyright Copyright ©  2016
 * @compiler Bridge.NET 15.6.0
 */
Bridge.assembly("Cable.Nancy.ClientTests", function ($asm, globals) {
    "use strict";

    Bridge.define("Cable.Nancy.ClientTests.Program", {
        $main: function () {
            QUnit.test("Tests are working", $asm.$.Cable.Nancy.ClientTests.Program.f1);
        }
    });

    Bridge.ns("Cable.Nancy.ClientTests.Program", $asm.$);

    Bridge.apply($asm.$.Cable.Nancy.ClientTests.Program, {
        f1: function (assert) {
            assert.equal(true, true);
        }
    });
});
