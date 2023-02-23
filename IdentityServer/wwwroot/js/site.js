
$(function () {
    $(".Logout").click(function () {

        $("#modal").load("/Account/logoff", function () {
            $("#modal").modal("show");
        })
    });
    $(".CreateRole").click(function () {
        $("#modal").load("/ManageUser/CreateRoles", function () {
            $("#modal").modal("show");
        })

    });
    $(".editRole").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/ManageUser/EditRoles?id=" + id, function () {
            $("#modal").modal("show");
        })
    });
    $(".RolePermission").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/ManageUser/EditRolePermission?id=" + id, function () {
            $("#modal").modal("show");
        });

    });
    $(".addUser").click(function () {
        $("#modal").load("/ManageUser/CreateUser", function () {
            $("#modal").modal("show");
        })
    });

    $(".EditUser").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/ManageUser/EditUser?id=" + id, function () {
            $("#modal").modal("show");
        });

    });
    $(".DeleteUser").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/ManageUser/DeleteUser?id=" + id, function () {
            $("#modal").modal("show");
        });

    });

    $(".AddClient").click(function () {
        $("#modal").load("/IdentityServer/AddClients", function () {
            $("#modal").modal("show");

        });

    });

    $(".EditClient").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/IdentityServer/AddClients?id=" + id, function () {
            $("#modal").modal("show");
        });

    });

    //   $('.dropdown-toggle').dropdown();
});



$(document).ready(function () {
    $(".plus").click(function () {
        $(this).toggleClass("minus").siblings("ul").toggle();
    })

    /* $("input[type=checkbox]").click(function () {
           //alert($(this).attr("id"));
           //var sp = $(this).attr("id");
           //if (sp.substring(0, 4) === "c_bs" || sp.substring(0, 4) === "c_bf") {
           $(this).siblings("ul").find("input[type=checkbox]").prop('checked', $(this).prop('checked'));
           //}
       })*/
    /*
    $("input[type=checkbox]").change(function () {
        var sp = $(this).attr("id");
        if (sp.substring(0, 4) === "c_io") {
            var ff = $(this).parents("ul[id^=bf_l]").attr("id");
            if ($('#' + ff + ' > li input[type=checkbox]:checked').length == $('#' + ff + ' > li input[type=checkbox]').length) {
                $('#' + ff).siblings("input[type=checkbox]").prop('checked', true);
                check_fst_lvl(ff);
            }
            else {
                $('#' + ff).siblings("input[type=checkbox]").prop('checked', false);
                check_fst_lvl(ff);
            }
        }

        if (sp.substring(0, 4) === "c_bf") {
            var ss = $(this).parents("ul[id^=bs_l]").attr("id");
            if ($('#' + ss + ' > li input[type=checkbox]:checked').length == $('#' + ss + ' > li input[type=checkbox]').length) {
                $('#' + ss).siblings("input[type=checkbox]").prop('checked', true);
                check_fst_lvl(ss);
            }
            else {
                $('#' + ss).siblings("input[type=checkbox]").prop('checked', false);
                check_fst_lvl(ss);
            }
        }
    });

})

function check_fst_lvl(dd) {
    //var ss = $('#' + dd).parents("ul[id^=bs_l]").attr("id");
    var ss = $('#' + dd).parent().closest("ul").attr("id");
    if ($('#' + ss + ' > li input[type=checkbox]:checked').length == $('#' + ss + ' > li input[type=checkbox]').length) {
        //$('#' + ss).siblings("input[id^=c_bs]").prop('checked', true);
        $('#' + ss).siblings("input[type=checkbox]").prop('checked', true);
    }
    else {
        //$('#' + ss).siblings("input[id^=c_bs]").prop('checked', false);
        $('#' + ss).siblings("input[type=checkbox]").prop('checked', false);
    }

}*/


    function pageLoad() {
        $(".plus").click(function () {
            $(this).toggleClass("minus").siblings("ul").toggle();
        })
    }
})

       
    


$(function () {


    var s = $("#CallbackUriStartIndex").val();


    b = function (n, m) {
        console.log(f);
        var d;
        if (f == true) {
            d = "checked";
        } else { d = ""; }

        return "<tr>\r\n<td>\n\r" + n + '<\/td><td>\r\n<input hidden name="UriClient[' + m + '].Id" type="text" ><input type="checkbox" id="CallSing" name="UriClient[' + m + '].CallSing" ' + d + ' value="' + f + '"><input hidden type="text" name="UriClient[' + m + '].Uri" value="' + n + '" >\r\n<button type = "button" id="deleteCallbackUri" class="btn btn-danger btn-sm float-end deleteCallbackUri" > <i class="fa fa-trash"><\/i><\/button><\/td><\/tr>';


    };
    $("#AddNewCallbackUriButton").on("click", function (n) {

        var t, i;
        (n.preventDefault(), t = "", $("#SampleCallbackUri").val() !== "" ? (t = $("#SampleCallbackUri").val(), $("#SampleCallbackUri").val(""), f = true) : t = $("#SampleCallbackUri").val(), t != "") && ($("#" + t + ("SampleCallbackUri").length > 0 || (i = b(t, s++), $("#CallbackUri").append(i), f, $("#CallbackUri").show())))
        console.log(f);
    });

    $("#AddNewSignoutUriButton").on("click", function (n) {
        var t, i;
        (n.preventDefault(), t = "", $("#SampleSignoutUri").val() !== "" ? (t = $("#SampleSignoutUri").val(), $("#SampleSignoutUri").val(""), f = false) : t = $("#SampleSignoutUri").val(), t != "") && ($("#" + t + ("SampleSignoutUri").length > 0 || (i = b(t, s++), $("#CallSignoutUri").append(i), f, $("#CallSignoutUri").show())))
        console.log(f);
    });
    $(document).on("click", "#deleteCallbackUri", function (n) {

        n.preventDefault();
        var t = $(this).parent().parent(), i = t.find("input"); $(i).each(function () { $(this).val("") });
        t.remove();
        s--;

    });

});

$(function () {
            $(".otherResource").on("click",
                function (n) {
                    n.preventDefault();
                    var t = $(this).attr("id"), u = $(this).attr("data-name"), i = t.substring(0, t.length - 7), f = i + "InputId", e = i + "OwnedId", r = i + "GroupId"; $("#" + e).show();
                    $("#" + t).hide();
                    $("#" + f).val(u);
                    $("#" + r).show();
                    for (const n of $("#" + r + " option")) $(n).attr("selected", "selected")
                }); $(".ownedResource").on("click", function (n) {
                    n.preventDefault();
                    var t = $(this).attr("id"), i = t.substring(0, t.length - 7), u = i + "InputId", f = i + "OtherId", r = i + "GroupId";
                    $("#" + f).show();
                    $("#" + t).hide();
                    $("#" + u).val("");
                    $("#" + r).hide();
                    for (const n of $("#" + r + " option")) $(n).removeAttr("selected")
                });
            // $(".ownedResource").hide();
            var t = 0, i = 0, r = function (n, i, r, u) { return "<tr>\r\n<td>\r\n" + n + " <\/td><td>\r\n" + i + "\r\n <\/td> <td>                                        " + r + "<\/td> <td>\r\n                                       " + u + ' <\/td>\r\n<td hidden>\r\n                                        <input type="text" name="Client.Secrets[' + t + '].Type"/ value="' + n + '">\r\n                                        <input type="text" name="Client.Secrets[' + t + '].Value" value="' + i + '"/>\r\n                                        <input type="text" name="Client.Secrets[' + t + '].Description" value="' + r + '"/>\r\n                                        <input type="date" name="Client.Secrets[' + t + '].Expiration" value="' + u + '"/><\/td><td>                                       <button type="button" class="btn btn-outline-danger float-end deleteSecretButton">Delete<\/button><\/td><\/tr>' };


            var s = $("#GrantTypeStartIndex").val();

            $("#GrantTypeSelect").change(function () {
                $("#GrantTypeSelect").val() === "custom" ? $("#GrantTypeCustomInputGroup").show() :
                    ($("#GrantTypeCustomInput").val(""), $("#GrantTypeCustomInputGroup").hide())
            });
            a = function (n) {
                return "<tr>\r\n<td>\r\n" + n + ' <\/td><td>\r\n<input type="text" hidden name="Client_AllowedGrantTypes[' + s + '].GrandType"/ value="' + n + '" id="' + n + 'GrantTypeInput">\r\n<button type = "button" class="btn btn-danger btn-sm float-end deleteGrantTypeButton" > <i class="fa fa-trash"><\/i><\/button><\/td><\/tr>'
            };
            $("#AddNewGrantTypeButton").on("click", function (n) {
                var t, i; (n.preventDefault(), t = "", $("#GrantTypeCustomInput").val() !== "" ? (t = $("#GrantTypeCustomInput").val(), $("#GrantTypeCustomInput").val("")) : t = $("#GrantTypeSelect").val(), t != "") && ($("#" + t + "GrantTypeInput").length > 0 || (i = a(t), $("#GrantTypeTableBodyId").append(i), s++, $("#GrantTypeTableId").show()))
            });
            $(document).on("click", ".deleteGrantTypeButton", function (n) {
                n.preventDefault();
                var t = $(this).parent().parent(), i = t.find("input"); $(i).each(function () { $(this).val("") });
                t.remove();
                s--;
                //element.classList.remove("mystyle");
                //t.hide()
            });// t.hide()

});
//AddNewGrantTypeButton