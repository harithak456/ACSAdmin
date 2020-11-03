$(document).ready(function () {
    $("#btnLogin").click(function () {
        var Username = $("#Username").val();
        var Password = $("#Password").val();
      
        //var GivenDate = '2020-11-15';
        //var eDate = new Date(GivenDate);
        //var sDate = new Date();

        //eDate.setHours(0, 0, 0, 0);
        //sDate.setHours(0, 0, 0, 0);
        //if (sDate > eDate) {
        //    alert('Product Expired. Please Contact Customer Support');
        //}
        //else {
            $.ajax({
                type: "POST",
                url: "/Admin/CreateLogin",
                data: "{'Username':'" + Username + "','Password':'" + Password + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //beforeSend: function () { $("#loader").css("display", "block"); },
                //complete: function () { $("#loader").css("display", "none"); },
                success: OnSuccessCall,
                error: OnErrorCall
            });
       // }
        function OnSuccessCall(data) {
            if (data == "1") {
                location.href = "/Admin/Dashboard";
            } else if (data == "-1") {
                alert("Please Check Your Username and Password");
            } else if (data == "0") {
                alert("Please Fill");
            }
        }
        function OnErrorCall() {
            alert("error");
        }
    });

    $("#btnLogout").click(function () {
        $.ajax({
            type: "POST",
            url: "/Admin/CreateLogout",
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            beforeSend: function () { $("#loader").css("display", "block"); },
            complete: function () { $("#loader").css("display", "none"); },
            success: OnSuccessLogoutCall,
            error: OnErrorLogoutCall
        });
        function OnSuccessLogoutCall(data) {
            if (data == "1") {
                location.href = "/Admin/Login";
            } else {
                alert("Try Agine");
            }
        }
        function OnErrorLogoutCall() {
            alert("error");
        }
    });
});